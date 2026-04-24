using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class DefendHudView : MonoBehaviour
{
    [SerializeField] private TMP_Text _waveText;
    [SerializeField] private TMP_Text _phaseText;
    [SerializeField] private TMP_Text _restTimerText;
    [SerializeField] private TMP_Text _buildingHpText;
    [SerializeField] private Slider _buildingHpSlider;

    private void Awake()
    {
        ConfigureBuildingHpSlider();
    }

    private void OnValidate()
    {
        ConfigureBuildingHpSlider();
    }

    public void SetWave(int currentWave, int totalWaves)
    {
        if (_waveText != null)
        {
            _waveText.text = $"Wave {currentWave}/{totalWaves}";
        }
    }

    public void SetPhase(string phase)
    {
        if (_phaseText != null)
        {
            _phaseText.text = phase;
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

        _restTimerText.text = $"Next wave in: {Mathf.CeilToInt(remainingSeconds)}s";
    }

    public void SetBuildingHealth(float current, float max)
    {
        if (_buildingHpText != null)
        {
            _buildingHpText.text = $"Base HP: {Mathf.CeilToInt(current)}/{Mathf.CeilToInt(max)}";
        }

        if (_buildingHpSlider != null)
        {
            _buildingHpSlider.maxValue = max;
            _buildingHpSlider.value = current;
        }
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
}