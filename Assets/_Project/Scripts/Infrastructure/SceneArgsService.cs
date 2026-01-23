public sealed class SceneArgsService
{
    private IInputArgs _args;

    public void Set(IInputArgs args)
    {
        this._args = args;
    }

    public bool TryGet<TArgs>(out TArgs result) where TArgs : class, IInputArgs
    {
        result = _args as TArgs;
        return result != null;
    }

    public void Clear()
    {
        _args = null;
    }
}