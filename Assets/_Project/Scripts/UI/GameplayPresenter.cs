public sealed class GameplayPresenter
{
    private readonly GameplayLoop _loop;

    private readonly GameplayTargetPresenter _target;
    private readonly GameplayTypedPresenter _typed;
    private readonly GameplayStatusPresenter _status;
    private readonly GameplayInputPresenter _input;

    private readonly CurrencyListPresenter _currencyList;

    public GameplayPresenter(
        GameplayLoop loop,
        GameplayTargetPresenter target,
        GameplayTypedPresenter typed,
        GameplayStatusPresenter status,
        GameplayInputPresenter input,
        CurrencyListPresenter currencyList)
    {
        _loop = loop;
        _target = target;
        _typed = typed;
        _status = status;
        _input = input;
        _currencyList = currencyList;
    }

    public void Start(GameMode mode)
    {
        // Подписки на события ДО loop.Start — чтобы поймать TargetChanged/TypedChanged
        _target.Initialize();
        _typed.Initialize();
        _status.Initialize();

        // Кошелек показываем универсально (без hardcode Gold)
        _currencyList.Initialize();

        _loop.Start(mode);

        // Инпут включаем после Start — чтобы _checker точно был создан
        _input.Initialize();
    }

    public void Stop()
    {
        _input.Dispose();

        _loop.Stop();

        _currencyList.Dispose();

        _status.Dispose();
        _typed.Dispose();
        _target.Dispose();
    }
}