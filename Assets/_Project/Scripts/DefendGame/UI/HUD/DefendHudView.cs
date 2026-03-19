using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class DefendHudView : MonoBehaviour
{
    [SerializeField] private TMP_Text _goldText;
    [SerializeField] private TMP_Text _waveText;
    [SerializeField] private TMP_Text _phaseText;
    [SerializeField] private TMP_Text _buildingHpText;
    [SerializeField] private Slider _buildingHpSlider;

    public void SetGold(int gold)
    {
        _goldText.text = $"Gold: {gold}";
    }

    public void SetWave(int currentWave, int totalWaves)
    {
        _waveText.text = $"Wave {currentWave}/{totalWaves}";
    }

    public void SetPhase(string phase)
    {
        _phaseText.text = phase;
    }

    public void SetBuildingHealth(float current, float max)
    {
        _buildingHpText.text = $"Base HP: {Mathf.CeilToInt(current)}/{Mathf.CeilToInt(max)}";

        if (_buildingHpSlider != null)
        {
            _buildingHpSlider.maxValue = max;
            _buildingHpSlider.value = current;
        }
    }
}