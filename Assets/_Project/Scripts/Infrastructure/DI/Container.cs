using System;
using System.Collections.Generic;

public sealed class Container : IContainer
{
    private readonly Dictionary<Type, object> _instances = new Dictionary<Type, object>();
    private readonly Dictionary<Type, Func<IReadOnlyContainer, object>> _lazyFactories =
        new Dictionary<Type, Func<IReadOnlyContainer, object>>();
    private readonly Dictionary<Type, Func<IReadOnlyContainer, object>> _transientFactories =
        new Dictionary<Type, Func<IReadOnlyContainer, object>>();

    private readonly Container _parent;

    public Container() { }

    private Container(Container parent)
    {
        _parent = parent;
    }

    public void BindInstance<T>(T instance)
    {
        if (instance == null)
        {
            throw new ArgumentNullException(nameof(instance));
        }

        Type type = typeof(T);
        EnsureNotRegistered(type);

        _instances.Add(type, instance);
    }

    public void BindLazy<T>(Func<IReadOnlyContainer, T> factory)
    {
        if (factory == null)
        {
            throw new ArgumentNullException(nameof(factory));
        }

        Type type = typeof(T);
        EnsureNotRegistered(type);

        _lazyFactories.Add(type, c => factory(c));
    }

    public void BindTransient<T>(Func<IReadOnlyContainer, T> factory)
    {
        if (factory == null)
        {
            throw new ArgumentNullException(nameof(factory));
        }

        Type type = typeof(T);
        EnsureNotRegistered(type);

        _transientFactories.Add(type, c => factory(c));
    }

    public bool IsRegistered<T>()
    {
        return IsRegistered(typeof(T));
    }

    public IContainer CreateChild()
    {
        return new Container(this);
    }

    public T Resolve<T>()
    {
        object obj = Resolve(typeof(T), new Stack<Type>());
        return (T)obj;
    }

    public bool TryResolve<T>(out T instance)
    {
        if (TryResolve(typeof(T), new Stack<Type>(), out object obj))
        {
            instance = (T)obj;
            return true;
        }

        instance = default;
        return false;
    }

    private object Resolve(Type type, Stack<Type> path)
    {
        if (TryResolve(type, path, out object obj))
        {
            return obj;
        }

        throw new InvalidOperationException($"Type is not registered: {type.Name}");
    }

    private bool TryResolve(Type type, Stack<Type> path, out object obj)
    {
        if (_instances.TryGetValue(type, out obj))
        {
            return true;
        }

        if (_lazyFactories.TryGetValue(type, out Func<IReadOnlyContainer, object> lazyFactory))
        {
            obj = CreateWithCycleCheck(type, path, lazyFactory);
            _instances.Add(type, obj);
            return true;
        }

        if (_transientFactories.TryGetValue(type, out Func<IReadOnlyContainer, object> transientFactory))
        {
            obj = CreateWithCycleCheck(type, path, transientFactory);
            return true;
        }
        
        if (_parent != null)
        {
            return _parent.TryResolve(type, path, out obj);
        }

        obj = null;
        return false;
    }

    private object CreateWithCycleCheck(Type type, Stack<Type> path, Func<IReadOnlyContainer, object> factory)
    {
        if (path.Contains(type))
        {
            throw new InvalidOperationException($"Cyclic dependency detected: {BuildCycleMessage(path, type)}");
        }

        path.Push(type);

        try
        {
            return factory(this);
        }
        finally
        {
            path.Pop();
        }
    }

    private bool IsRegistered(Type type)
    {
        if (_instances.ContainsKey(type) || _lazyFactories.ContainsKey(type) || _transientFactories.ContainsKey(type))
        {
            return true;
        }

        return _parent != null && _parent.IsRegistered(type);
    }

    private void EnsureNotRegistered(Type type)
    {
        if (IsRegistered(type))
        {
            throw new InvalidOperationException($"Type already registered: {type.Name}");
        }
    }

    private string BuildCycleMessage(Stack<Type> path, Type requested)
    {
        Type[] types = path.ToArray(); // top->bottom
        string message = requested.Name;

        for (int i = 0; i < types.Length; i++)
        {
            message = types[i].Name + " -> " + message;
        }

        return message + " -> " + requested.Name;
    }
}
