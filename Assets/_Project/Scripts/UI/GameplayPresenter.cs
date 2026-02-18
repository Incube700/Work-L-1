public sealed class GameplayPresenter
{
    private readonly GameplayTargetPresenter _target;
    private readonly GameplayTypedPresenter _typed;
    private readonly GameplayStatusPresenter _status;
    private readonly GameplayInputPresenter _input;

    private readonly CurrencyListPresenter _currencyList;

    public GameplayPresenter(
        GameplayTargetPresenter target,
        GameplayTypedPresenter typed,
        GameplayStatusPresenter status,
        GameplayInputPresenter input,
        CurrencyListPresenter currencyList)
    {
        _target = target;
        _typed = typed;
        _status = status;
        _input = input;
        _currencyList = currencyList;
    }

    public void Initialize()
    {
        _target.Initialize();
        _typed.Initialize();
        _status.Initialize();

        // Кошелек показываем универсально (без hardcode Gold)
        _currencyList.Initialize();

        _input.Initialize();
    }

    public void Dispose()
    {
        _input.Dispose();

        _currencyList.Dispose();

        _status.Dispose();
        _typed.Dispose();
        _target.Dispose();
    }
}