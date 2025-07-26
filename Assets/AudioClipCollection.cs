using UnityEngine;

[CreateAssetMenu(fileName = "NewAudioClipCollection", menuName = "AudioClip Collection")]
public class AudioClipCollection : ScriptableObject
{
    public string collectionName;
    public AudioClip[] audioClips;

    public AudioClip GetRandomClip()
    {
        return GetClipAtIndex(Random.Range(0, audioClips.Length));
    }

    public AudioClip GetClipAtIndex(int index)
    {
        return audioClips[index];
    }

    public AudioClip GetRandomClipOrDefault(AudioClip defaultClip = null)
    {
        return GetClipAtIndexOrDefault(Random.Range(0, audioClips.Length), defaultClip);
    }

    public AudioClip GetClipAtIndexOrDefault(int index, AudioClip defaultClip = null)
    {
        if (audioClips.Length <= index) return defaultClip;
        return GetClipAtIndex(index);
    }
}