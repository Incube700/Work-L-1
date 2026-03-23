using UnityEngine;

public sealed class PlayerClickEffectService
{
    private readonly GameObject _effectPrefab;
    private readonly float _heightOffset;

    public PlayerClickEffectService(GameObject effectPrefab, float heightOffset = 0.1f)
    {
        _effectPrefab = effectPrefab;
        _heightOffset = heightOffset;
    }

    public void Play(Vector3 position)
    {
        if (_effectPrefab == null)
        {
            return;
        }

        Vector3 effectPosition = position + Vector3.up * _heightOffset;
        Object.Instantiate(_effectPrefab, effectPosition, Quaternion.identity);
    }
}