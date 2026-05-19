using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class MainMenuView : MonoBehaviour
{
    public event Action PlayClicked;
    public event Action ResetClicked;
    public event Action UpgradesClicked;

    [SerializeField] private TMP_Text _statusText;
    [SerializeField] private TMP_Text _resetButtonText;
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _resetButton;
    [SerializeField] private Button _upgradesButton;
    [SerializeField] private PermanentUpgradesMenuView _upgradesMenuView;

    [Header("Optional Generated Skin")]
    [SerializeField] private GameObject _optionalSkinRoot;
    [SerializeField] private Image _backgroundImage;
    [SerializeField] private Image _frameImage;
    [SerializeField] private Image _buttonImage;
    [SerializeField] private TMP_Text _label;

    public PermanentUpgradesMenuView UpgradesMenuView => _upgradesMenuView;

    private void Awake()
    {
        ValidateSerializedReferences();
    }

    private void OnValidate()
    {
        ValidateSerializedReferences();
    }

    private void OnEnable()
    {
        if (_playButton != null)
        {
            _playButton.onClick.AddListener(OnPlayClicked);
        }

        if (_resetButton != null)
        {
            _resetButton.onClick.AddListener(OnResetClicked);
        }

        if (_upgradesButton != null)
        {
            _upgradesButton.onClick.AddListener(OnUpgradesButtonClicked);
        }
    }

    private void OnDisable()
    {
        if (_playButton != null)
        {
            _playButton.onClick.RemoveListener(OnPlayClicked);
        }

        if (_resetButton != null)
        {
            _resetButton.onClick.RemoveListener(OnResetClicked);
        }

        if (_upgradesButton != null)
        {
            _upgradesButton.onClick.RemoveListener(OnUpgradesButtonClicked);
        }
    }

    public void SetResetCost(int cost)
    {
        if (_resetButtonText != null)
        {
            _resetButtonText.text = $"Reset ({cost} gold)";
        }
    }

    public void SetStatus(string message)
    {
        if (_statusText != null)
        {
            _statusText.text = message ?? string.Empty;
        }
    }

    private void OnPlayClicked()
    {
        PlayClicked?.Invoke();
    }

    private void OnResetClicked()
    {
        ResetClicked?.Invoke();
    }

    private void OnUpgradesButtonClicked()
    {
        UpgradesClicked?.Invoke();
    }

    private void ValidateSerializedReferences()
    {
        LogMissingReference(_statusText, nameof(_statusText));
        LogMissingReference(_resetButtonText, nameof(_resetButtonText));
        LogMissingReference(_playButton, nameof(_playButton));
        LogMissingReference(_resetButton, nameof(_resetButton));
        LogMissingReference(_upgradesButton, nameof(_upgradesButton));
        LogMissingReference(_upgradesMenuView, nameof(_upgradesMenuView));
    }

    private void LogMissingReference(UnityEngine.Object reference, string fieldName)
    {
        if (reference == null)
        {
            Debug.LogError($"{name}: {nameof(MainMenuView)} missing serialized reference '{fieldName}'.", this);
        }
    }
}
