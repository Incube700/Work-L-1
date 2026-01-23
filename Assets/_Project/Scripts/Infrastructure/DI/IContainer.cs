using System;

public interface IContainer : IReadOnlyContainer
{
    void BindInstance<T>(T instance);
    void BindLazy<T>(Func<IReadOnlyContainer, T> factory);
    void BindTransient<T>(Func<IReadOnlyContainer, T> factory);

    bool IsRegistered<T>();
    IContainer CreateChild();
}