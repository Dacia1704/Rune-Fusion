using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "AudioPropsSO", menuName = "AudioPropsSO")]
public class AudioPropsSO: ScriptableObject
{
        [field: SerializeField] public AudioMixerGroup MasterAudioMixerGroup { get; private set; }
        [field: SerializeField] public AudioMixerGroup BGMAudioMixerGroup { get; private set; }
        [field: SerializeField] public AudioMixerGroup SFXAudioMixerGroup { get; private set; }
        
        [field: Header("UI")]
        [field: SerializeField] public AudioClip BGMMain { get; private set; }
        [field: SerializeField] public AudioClip BGMCombat { get; private set; }
        [field: SerializeField] public AudioClip ButtonClickSound { get; private set; }
        [field: SerializeField] public AudioClip ClickSound { get; private set; }
        [field: SerializeField] public AudioClip ProtectBreakSound { get; private set; }
        [field: SerializeField] public AudioClip RuneBreakSound { get; private set; }
        [field: SerializeField] public AudioClip RuneFallSound { get; private set; }
        [field: SerializeField] public AudioClip RuneMatchesSound { get; private set; }
        [field: SerializeField] public AudioClip StartGameSound { get; private set; }
        
        
        [field: Header("Character")]
        [field: SerializeField] public AudioClip HurtSound { get; private set; }
        [field: SerializeField] public AudioClip SprintSound { get; private set; }
        [field: SerializeField] public AudioClip SwordSound { get; private set; }
        [field: SerializeField] public AudioClip ArrowFlySound { get; private set; }
        [field: SerializeField] public AudioClip BuffSound { get; private set; }
        [field: SerializeField] public AudioClip DebuffSound { get; private set; }
        [field: SerializeField] public AudioClip FireBallFlySound { get; private set; }
        [field: SerializeField] public AudioClip FireBallBustSound { get; private set; }
        [field: SerializeField] public AudioClip FrozenBustSound { get; private set; }
        [field: SerializeField] public AudioClip FrozenCreateSound { get; private set; }
}