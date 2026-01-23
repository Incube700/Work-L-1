public interface IReadOnlyContainer
{
    T Resolve<T>();
    bool TryResolve<T>(out T instance);
}