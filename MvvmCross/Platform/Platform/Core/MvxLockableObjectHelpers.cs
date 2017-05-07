// MvxLockableObjectHelpers.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Platform.Core
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public static class MvxLockableObjectHelpers
    {
        public static void RunSyncWithLock(object lockObject, Action action)
        {
            lock (lockObject)
            {
                action();
            }
        }

        public static void RunAsyncWithLock(object lockObject, Action action)
        {
            Task.Run(() =>
                {
                    lock (lockObject)
                    {
                        action();
                    }
                });
        }

        public static void RunSyncOrAsyncWithLock(object lockObject, Action action, Action whenComplete = null)
        {
            if (Monitor.TryEnter(lockObject))
            {
                try
                {
                    action();
                }
                finally
                {
                    Monitor.Exit(lockObject);
                }

                whenComplete?.Invoke();
            }
            else
            {
                Task.Run(() =>
                    {
                        lock (lockObject)
                        {
                            action();
                        }

                        whenComplete?.Invoke();
                    });
            }
        }
    }
}