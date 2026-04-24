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

    private void Awake()
    {
        if (_root == null)
        {
            _root = gameObject;
        }
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
        SetButtonSelected(_mineButton, selectedType == PlaceableType.Mine);
        SetButtonSelected(_turretButton, selectedType == PlaceableType.Turret);
        SetButtonSelected(_puddleButton, selectedType == PlaceableType.Puddle);
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

        text.text = $"{costGold} G";
    }

    private void SetButtonSelected(Button button, bool isSelected)
    {
        if (button == null)
        {
            return;
        }

        button.interactable = isSelected == false;
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
}