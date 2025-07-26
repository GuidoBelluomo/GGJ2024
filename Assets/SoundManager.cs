using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public static class SoundManager
{
    public enum SoundType
    {
        Sfx,
        Music,
        Voice
    }
    
    public enum SoundConfigType
    {
        Sfx = SoundType.Sfx,
        Music = SoundType.Music,
        Voice = SoundType.Voice,
        Master
    }

    public static readonly float[] SoundConfig = new float[4] {1, 1, 1, 1};
    
    
    public static AudioSource Play2DSpatialSound(Vector2 position, AudioClip audioClip, SoundType soundType = SoundType.Sfx, bool destroyAfterPlaying = true, bool loop = false, GameObject parentObject = null)
    {
        var audioSource = Play3DSpatialSound(position, audioClip, soundType, destroyAfterPlaying, loop, parentObject);
        return audioSource;
    }
    
    public static AudioSource Play3DSpatialSound(Vector3 position, AudioClip audioClip, SoundType soundType = SoundType.Sfx, bool destroyAfterPlaying = true, bool loop = false, GameObject parentObject = null)
    {
        if (audioClip is null) {
            Debug.LogWarning("Tried to play null AudioClip");
            return null;
        }
        
        GameObject audioObject = new GameObject("AudioObject - "  + audioClip.name)
        {
            transform =
            {
                position = position
            }
        };

        var audioSource = audioObject.AddComponent<AudioSource>();
        audioSource.clip = audioClip;
        audioSource.volume = GetSoundVolume(soundType);
        audioSource.spatialBlend = 1;
        audioSource.loop = loop;
        audioSource.Play();
        
        if (destroyAfterPlaying) {
            audioObject.AddComponent<DestroySoundAfterPlaying>();
        }

        if (parentObject != null)
        {
            audioObject.transform.parent = parentObject.transform;
            audioObject.transform.localPosition = Vector3.zero;
        }
        
        return audioSource;
    }

    public static float GetSoundVolume(SoundType soundType)
    {
        return Mathf.Clamp01(SoundConfig[(int)soundType]) * Mathf.Clamp01(SoundConfig[(int)SoundConfigType.Master]);
    }

    public static AudioSource PlayUISound(AudioClip audioClip, SoundType soundType = SoundType.Sfx, bool destroyAfterPlaying = true, bool loop = false)
    {
        if (audioClip is null) {
            Debug.LogWarning("Tried to play null AudioClip");
            return null;
        }
        
        GameObject audioObject = new GameObject("AudioObject - "  + audioClip.name);

        var audioSource = audioObject.AddComponent<AudioSource>();
        audioSource.clip = audioClip;
        audioSource.volume = GetSoundVolume(soundType);
        audioSource.spatialBlend = 0;
        audioSource.loop = loop;
        audioSource.Play();
        
        if (destroyAfterPlaying) {
            audioObject.AddComponent<DestroySoundAfterPlaying>();
        }
        
        return audioSource;
    }
}
