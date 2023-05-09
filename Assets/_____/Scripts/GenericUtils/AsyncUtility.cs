using System;
using System.Threading;
using System.Threading.Tasks;

public static class AsyncUtility
{
    public static async void DelayTask(float delayTime, Action action)
    {
        await Task.Run(() =>
        {
            Thread.Sleep((int)(delayTime * 1000));
        });
        action?.Invoke();
    }
}
