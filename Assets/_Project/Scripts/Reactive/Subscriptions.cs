using System;

public sealed class Subscription : IDisposable
{
    private Action _dispose;

    public Subscription(Action dispose)
    {
        _dispose = dispose;
    }

    public void Dispose()
    {
        _dispose?.Invoke();
        _dispose = null;
    }
}

public static class ReactiveExtensions
{
    public static IDisposable Subscribe(this IReadOnlySimpleEvent evt, Action handler)
    {
        if (evt == null)
            throw new ArgumentNullException(nameof(evt));

        if (handler == null)
            throw new ArgumentNullException(nameof(handler));

        evt.Invoked += handler;
        return new Subscription(() => evt.Invoked -= handler);
    }

    public static IDisposable Subscribe<T>(this IReadOnlySimpleEvent<T> evt, Action<T> handler)
    {
        if (evt == null)
            throw new ArgumentNullException(nameof(evt));

        if (handler == null)
            throw new ArgumentNullException(nameof(handler));

        evt.Invoked += handler;
        return new Subscription(() => evt.Invoked -= handler);
    }
}
