using UnityEngine;

public sealed class PlayerProgressService
{
    private readonly SaveService _save;
    private readonly ConfigService _configs;
    private readonly GameStatsService _stats;
    private readonly WalletService _wallet;

    private EconomyConfig _economy;
    
    public int Wins => _stats.Wins;
    public int Losses => _stats.Losses;
    public int Gold => _wallet.Gold;
    
 public PlayerProgressService(SaveService save, ConfigService configs, 
        GameStatsService stats, WalletService wallet)
    {
        _save = save;
        _configs = configs;
        _stats = stats;
        _wallet = wallet;

        LoadOrCreate();
    }

    public void RegisterWin()
    {
        _stats.AddWin();
        _wallet.Add(_economy.WinGold);
        
        Save();
    }

    public void RegisterLoss()
    {
        _stats.AddLoss();
        _wallet.SubtractClamped(_economy.LoseGold);
        
        Save();
    }

    public void Print()
    {
       Debug.Log($"Stats: W:{Wins}/L:{Losses} {Gold}"); 
    }
   
    public void TryResetProgress()
    {
        int cost = _economy.ResetCost;

        if (_wallet.TrySpend(cost) == false)
        {
            Debug.Log($"Reset: Нехватает денег: надо  {cost}, ecть {Gold}");
            return;
        }
        
        _stats.Reset();
        Save();
        Debug.Log($"Reset: Статистика сброшена. Заплатил {cost}., Осталось {Gold}." );
    }

    private void LoadOrCreate()
    {
        _economy = _configs.Load<EconomyConfig>();

        if (_save.TryLoad(out SaveData data))
        {
            _stats.Set(data.wins, data.losses);
            _wallet.SetGold(data.gold);
            return;
        }
        
        _stats.Set(0 ,0);
        
        int startGold = _economy.StartGold;
        if (startGold < 0) throw new System.InvalidOperationException("Gold < 0!");
        
        _wallet.SetGold(startGold);
        Save();
    }

    private void Save()
    {
        SaveData data = new SaveData
        {
            wins = _stats.Wins,
            losses = _stats.Losses,
            gold = _wallet.Gold
        };
        
        _save.Save(data);
    }

    public void WipeToDefaults()
    {
        _save.Delete();
        
        _stats.Set(0 ,0);
        _wallet.SetGold(_economy.StartGold);
        
        Save();
        
        Debug.Log("All Save Deleted");
    }
 
 

}
   

