using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioListener))]
public class FollowingAudioListener2D : MonoBehaviourSingleton<FollowingAudioListener2D>
{
    public GameObject Target
    {
        get => target;
        set => target = value;
    }
    [SerializeField] private GameObject target;

    void Update()
    {
        if (target != null)
        {
            transform.position = (Vector2)target.transform.position;
        }
        else
        {
            var transformPosition = transform.position;
            if (transformPosition.z != 0)
            {
                transform.position = (Vector2)transformPosition;
            }
        }
    }
}
