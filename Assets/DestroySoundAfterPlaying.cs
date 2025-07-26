using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySoundAfterPlaying : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroyAfterPlay(GetComponent<AudioSource>()));
    }

    IEnumerator DestroyAfterPlay(AudioSource audioSource)
    {
        while (audioSource.isPlaying || !Application.isPlaying || !Application.isFocused)
        {
            yield return null;
        }
        
        Destroy(gameObject);
    }
}
