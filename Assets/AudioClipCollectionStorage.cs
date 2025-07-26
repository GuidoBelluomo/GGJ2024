using System.Linq;
using UnityEngine;

public class AudioClipCollectionStorage : MonoBehaviourSingleton<AudioClipCollectionStorage>
{
    [SerializeField] private AudioClipCollection[] audioClipCollections;

    public AudioClipCollection FindCollectionByName(string collectionName)
    {
        return audioClipCollections.FirstOrDefault(x => x.name == collectionName);
    }
}