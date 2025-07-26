using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "NewMusicAsset", menuName = "MusicAsset")]
public class MusicAsset : ScriptableObject
{
    public AudioClip clip;
    public float bpm = 60;
    public float beats = 4;
    public float note = 4;
    [FormerlySerializedAs("overlayedLoop")] public bool managedLoop;
    [FormerlySerializedAs("overlayedLoopBar")] public float managedLoopBar;

    public float GetMeasureDuration()
    {
        return 60f / bpm * beats * 4f / note;
    }
    
    public float GetManagedLoopPoint()
    {
        return GetMeasureDuration() * (managedLoopBar - 1);
    }
}