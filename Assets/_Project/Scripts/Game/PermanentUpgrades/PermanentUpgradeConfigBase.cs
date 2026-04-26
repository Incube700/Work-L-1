using UnityEngine;

public abstract class PermanentUpgradeConfigBase : ScriptableObject
{
    [SerializeField] private PermanentUpgradeType _type;
    [SerializeField] private string _title = string.Empty;
    [SerializeField, TextArea] private string _description = string.Empty;
    [SerializeField, Min(1)] private int _costDiamonds = 1;

    public PermanentUpgradeType Type => _type;
    public string Title => _title;
    public string Description => _description;
    public int CostDiamonds => _costDiamonds;
}