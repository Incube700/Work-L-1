using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class DefendHudView : MonoBehaviour
{
    private static readonly Color CriticalHealthColor = new Color32(231, 76, 60, 255);
    private static readonly Color WarningHealthColor = new Color32(244, 190, 79, 255);
    private static readonly Color HealthyHealthColor = new Color32(86, 203, 127, 255);

    [SerializeField] private TMP_Text _waveText;
    [SerializeField] private TMP_Text _phaseText;
    [SerializeField] private TMP_Text _restTimerText;
    [SerializeField] private TMP_Text _buildingHpText;
    [SerializeField] private Slider _buildingHpSlider;

    private void Awake()
    {
        ValidateSerializedReferences();
        ConfigureTextRaycasts();
        ConfigureBuildingHpSlider();
    }

    private void OnValidate()
    {
        ValidateSerializedReferences();
        ConfigureTextRaycasts();
        ConfigureBuildingHpSlider();
    }

    public void SetWave(int currentWave, int totalWaves)
    {
        if (_waveText != null)
        {
            if (totalWaves <= 0)
            {
                _waveText.text = "Wave -/-";
                return;
            }

            _waveText.text = $"Wave {currentWave}/{totalWaves}";
        }
    }

    public void SetPhase(string phase)
    {
        if (_phaseText != null)
        {
            _phaseText.text = $"Status: {phase ?? string.Empty}";
        }
    }

    public void SetRestTimer(bool isVisible, float remainingSeconds)
    {
        if (_restTimerText == null)
        {
            return;
        }

        _restTimerText.gameObject.SetActive(isVisible);

        if (isVisible == false)
        {
            return;
        }

        _restTimerText.text = $"Build time: {Mathf.CeilToInt(remainingSeconds)}s";
    }

    public void SetBuildingHealth(float current, float max)
    {
        if (_buildingHpText != null)
        {
            _buildingHpText.text = $"Base {Mathf.CeilToInt(current)}/{Mathf.CeilToInt(max)}";
        }

        if (_buildingHpSlider != null)
        {
            _buildingHpSlider.minValue = 0f;
            _buildingHpSlider.maxValue = Mathf.Max(0f, max);
            _buildingHpSlider.value = Mathf.Clamp(current, 0f, _buildingHpSlider.maxValue);
        }

        UpdateBuildingHealthFill(current, max);
    }

    private void ConfigureBuildingHpSlider()
    {
        if (_buildingHpSlider == null)
        {
            return;
        }

        _buildingHpSlider.interactable = false;
        _buildingHpSlider.transition = Selectable.Transition.None;

        Navigation navigation = _buildingHpSlider.navigation;
        navigation.mode = Navigation.Mode.None;
        _buildingHpSlider.navigation = navigation;

        if (_buildingHpSlider.targetGraphic != null)
        {
            _buildingHpSlider.targetGraphic.raycastTarget = false;
        }

        if (_buildingHpSlider.fillRect != null)
        {
            Graphic fillGraphic = _buildingHpSlider.fillRect.GetComponent<Graphic>();

            if (fillGraphic != null)
            {
                fillGraphic.raycastTarget = false;
            }
        }

        if (_buildingHpSlider.handleRect != null)
        {
            Graphic handleGraphic = _buildingHpSlider.handleRect.GetComponent<Graphic>();

            if (handleGraphic != null)
            {
                handleGraphic.raycastTarget = false;
            }
        }
    }

    private void ConfigureTextRaycasts()
    {
        SetRaycastTarget(_waveText, false);
        SetRaycastTarget(_phaseText, false);
        SetRaycastTarget(_restTimerText, false);
        SetRaycastTarget(_buildingHpText, false);
    }

    private void UpdateBuildingHealthFill(float current, float max)
    {
        Graphic fillGraphic = GetBuildingHpFillGraphic();

        if (fillGraphic == null)
        {
            return;
        }

        float ratio = max <= 0f ? 0f : Mathf.Clamp01(current / max);
        fillGraphic.color = GetHealthColor(ratio);
    }

    private Graphic GetBuildingHpFillGraphic()
    {
        if (_buildingHpSlider == null || _buildingHpSlider.fillRect == null)
        {
            return null;
        }

        return _buildingHpSlider.fillRect.GetComponent<Graphic>();
    }

    private Color GetHealthColor(float ratio)
    {
        if (ratio <= 0.3f)
        {
            return CriticalHealthColor;
        }

        if (ratio <= 0.6f)
        {
            return WarningHealthColor;
        }

        return HealthyHealthColor;
    }

    private void SetRaycastTarget(Graphic graphic, bool isEnabled)
    {
        if (graphic != null)
        {
            graphic.raycastTarget = isEnabled;
        }
    }

    private void ValidateSerializedReferences()
    {
        LogMissingReference(_waveText, nameof(_waveText));
        LogMissingReference(_phaseText, nameof(_phaseText));
        LogMissingReference(_restTimerText, nameof(_restTimerText));
        LogMissingReference(_buildingHpText, nameof(_buildingHpText));
        LogMissingReference(_buildingHpSlider, nameof(_buildingHpSlider));
    }

    private void LogMissingReference(UnityEngine.Object reference, string fieldName)
    {
        if (reference == null)
        {
            Debug.LogError($"{name}: {nameof(DefendHudView)} missing serialized reference '{fieldName}'.", this);
        }
    }
}
