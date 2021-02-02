using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public enum AudioClips
    {
        PlayerHeal,
        PlayerTakeDamage,
        PlayerFootStep,    //Handled by firstperson thingy, probably not needed

        PickupItem,

        CrossbowWindup,
        CrossbowFire,

        DoorOpen,
        DoorLocked,

        EnemyIdle,
        EnemySpottedPlayer,
        EnemyBodyshot,
        EnemyHeadshot,
        EnemyMoving,
        EnemyAttacking,
        EnemyDeath,

        SIZE
    }
    [System.Serializable]
    public struct TaggedClip
    {
        public AudioClips tag;
        public AudioClip clip;
        public float volume;
    }

    static AudioManager m_singleton;

    public AudioSource PlayerAudioSource;
    public TaggedClip[] Clips;
    Dictionary<AudioClips, TaggedClip> m_clipDictionary;

    private void Awake()
    {
        m_singleton = this;
        m_clipDictionary = new Dictionary<AudioClips, TaggedClip>();
        for (int i = 0; i < Clips.Length; ++i)
            m_clipDictionary.Add(Clips[i].tag, Clips[i]);
    }

    public static TaggedClip GetAudioClip(AudioClips cliptag)
    {
        return m_singleton.m_clipDictionary[cliptag];
    }
    public static void Play(AudioClips cliptag, AudioSource source)
    {
        var clip = m_singleton.m_clipDictionary[cliptag];
        source.PlayOneShot(clip.clip, clip.volume);
    }
    public static void PlayOnPlayer(AudioClips cliptag)
    {
        Play(cliptag, m_singleton.PlayerAudioSource);
    }

}
