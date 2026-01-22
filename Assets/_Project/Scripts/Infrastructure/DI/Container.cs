using System;
using System.Collections.Generic;

public sealed class Container : IContainer
{
    private readonly Dictionary<Type, object> instances = new Dictionary<Type, object>();
    private readonly Container parent;

    public Container() { }

    private Container(Container parent)
    {
        this.parent = parent;
    }

    public void Bind<T>(T instance)
    {
        if (instance == null)
        {
            throw new ArgumentNullException(nameof(instance));
        }

        instances[typeof(T)] = instance;
    }

    public T Resolve<T>()
    {
        Type type = typeof(T);

        if (instances.TryGetValue(type, out object obj))
        {
            return (T)obj;
        }

        if (parent != null)
        {
            return parent.Resolve<T>();
        }

        throw new InvalidOperationException($"Type is not registered: {type.Name}");
    }

    public IContainer CreateChild()
    {
        return new Container(this);
    }
}