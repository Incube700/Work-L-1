using System;
using UnityEngine;

[CreateAssetMenu(
    menuName = "Configs/Defend Game/Presentation Feedback Config",
    fileName = "DefendPresentationFeedbackConfig")]
public sealed class DefendPresentationFeedbackConfig : ScriptableObject
{
    [SerializeField] private SfxCueEntry[] _sfxCues = Array.Empty<SfxCueEntry>();
    [SerializeField] private VfxCueEntry[] _vfxCues = Array.Empty<VfxCueEntry>();

    public bool TryGetSfx(DefendSfxCue cue, out AudioClip clip)
    {
        if (_sfxCues == null)
        {
            clip = null;
            return false;
        }

        for (int i = 0; i < _sfxCues.Length; i++)
        {
            SfxCueEntry entry = _sfxCues[i];

            if (entry.Cue == cue)
            {
                clip = entry.Clip;
                return clip != null;
            }
        }

        clip = null;
        return false;
    }

    public bool TryGetVfx(DefendVfxCue cue, out GameObject prefab)
    {
        if (_vfxCues == null)
        {
            prefab = null;
            return false;
        }

        for (int i = 0; i < _vfxCues.Length; i++)
        {
            VfxCueEntry entry = _vfxCues[i];

            if (entry.Cue == cue)
            {
                prefab = entry.Prefab;
                return prefab != null;
            }
        }

        prefab = null;
        return false;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (_sfxCues == null)
        {
            _sfxCues = Array.Empty<SfxCueEntry>();
        }

        if (_vfxCues == null)
        {
            _vfxCues = Array.Empty<VfxCueEntry>();
        }
    }
#endif

    [Serializable]
    public struct SfxCueEntry
    {
        [SerializeField] private DefendSfxCue _cue;
        [SerializeField] private AudioClip _clip;

        public DefendSfxCue Cue => _cue;
        public AudioClip Clip => _clip;
    }

    [Serializable]
    public struct VfxCueEntry
    {
        [SerializeField] private DefendVfxCue _cue;
        [SerializeField] private GameObject _prefab;

        public DefendVfxCue Cue => _cue;
        public GameObject Prefab => _prefab;
    }
}

public enum DefendSfxCue
{
    ButtonClick = 0,
    Purchase = 1,
    Denied = 2,
    WaveStart = 3,
    Victory = 4,
    Defeat = 5,
    PlaceableSelected = 6,
    PlaceableConfirmed = 7,
    TowerAttack = 8,
    BaseHit = 9,
    EnemyDeath = 10
}

public enum DefendVfxCue
{
    PlacementPreview = 0,
    PlaceableConfirmed = 1,
    WaveStart = 2,
    TowerAttack = 3,
    BaseHit = 4,
    EnemyDeath = 5,
    PlayerExplosion = 6
}
