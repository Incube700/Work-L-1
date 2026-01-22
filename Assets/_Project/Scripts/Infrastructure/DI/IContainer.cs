public interface IContainer
{
    void Bind<T>(T instance);
    T Resolve<T>();
    IContainer CreateChild();
}