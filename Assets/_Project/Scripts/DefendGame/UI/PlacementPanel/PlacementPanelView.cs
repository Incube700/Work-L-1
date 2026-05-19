using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class PlacementPanelView : MonoBehaviour
{
    public event Action MineSelected;
    public event Action TurretSelected;
    public event Action PuddleSelected;

    [SerializeField] private GameObject _root;

    [SerializeField] private Button _mineButton;
    [SerializeField] private Button _turretButton;
    [SerializeField] private Button _puddleButton;

    [SerializeField] private TMP_Text _mineCostText;
    [SerializeField] private TMP_Text _turretCostText;
    [SerializeField] private TMP_Text _puddleCostText;

    private PlaceableType _selectedType = PlaceableType.Mine;
    private bool _canAffordMine = true;
    private bool _canAffordTurret = true;
    private bool _canAffordPuddle = true;

    private void Awake()
    {
        if (_root == null)
        {
            _root = gameObject;
        }

        ValidateSerializedReferences();
    }

    private void OnValidate()
    {
        ValidateSerializedReferences();
    }

    private void OnEnable()
    {
        if (_mineButton != null)
        {
            _mineButton.onClick.AddListener(OnMineButtonClicked);
        }

        if (_turretButton != null)
        {
            _turretButton.onClick.AddListener(OnTurretButtonClicked);
        }

        if (_puddleButton != null)
        {
            _puddleButton.onClick.AddListener(OnPuddleButtonClicked);
        }
    }

    private void OnDisable()
    {
        if (_mineButton != null)
        {
            _mineButton.onClick.RemoveListener(OnMineButtonClicked);
        }

        if (_turretButton != null)
        {
            _turretButton.onClick.RemoveListener(OnTurretButtonClicked);
        }

        if (_puddleButton != null)
        {
            _puddleButton.onClick.RemoveListener(OnPuddleButtonClicked);
        }
    }

    public void SetVisible(bool isVisible)
    {
        if (_root != null)
        {
            _root.SetActive(isVisible);
        }
    }

    public void SetSelected(PlaceableType selectedType)
    {
        _selectedType = selectedType;
        RefreshButtons();
    }

    public void SetAffordable(bool canAffordMine, bool canAffordTurret, bool canAffordPuddle)
    {
        _canAffordMine = canAffordMine;
        _canAffordTurret = canAffordTurret;
        _canAffordPuddle = canAffordPuddle;

        RefreshButtons();
    }

    public void SetCosts(int mineCostGold, int turretCostGold, int puddleCostGold)
    {
        SetCostText(_mineCostText, mineCostGold);
        SetCostText(_turretCostText, turretCostGold);
        SetCostText(_puddleCostText, puddleCostGold);
    }

    private void SetCostText(TMP_Text text, int costGold)
    {
        if (text == null)
        {
            return;
        }

        text.text = $"{costGold} Gold";
    }

    private void RefreshButtons()
    {
        SetButtonInteractable(_mineButton, _selectedType != PlaceableType.Mine && _canAffordMine);
        SetButtonInteractable(_turretButton, _selectedType != PlaceableType.Turret && _canAffordTurret);
        SetButtonInteractable(_puddleButton, _selectedType != PlaceableType.Puddle && _canAffordPuddle);
    }

    private void SetButtonInteractable(Button button, bool isInteractable)
    {
        if (button == null)
        {
            return;
        }

        button.interactable = isInteractable;
    }

    private void OnMineButtonClicked()
    {
        MineSelected?.Invoke();
    }

    private void OnTurretButtonClicked()
    {
        TurretSelected?.Invoke();
    }

    private void OnPuddleButtonClicked()
    {
        PuddleSelected?.Invoke();
    }

    private void ValidateSerializedReferences()
    {
        LogMissingReference(_mineButton, nameof(_mineButton));
        LogMissingReference(_turretButton, nameof(_turretButton));
        LogMissingReference(_puddleButton, nameof(_puddleButton));
        LogMissingReference(_mineCostText, nameof(_mineCostText));
        LogMissingReference(_turretCostText, nameof(_turretCostText));
        LogMissingReference(_puddleCostText, nameof(_puddleCostText));
    }

    private void LogMissingReference(UnityEngine.Object reference, string fieldName)
    {
        if (reference == null)
        {
            Debug.LogError($"{name}: {nameof(PlacementPanelView)} missing serialized reference '{fieldName}'.", this);
        }
    }
}
