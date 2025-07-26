using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;
using UnityEngine.InputSystem;

public class DeathHandler : MonoBehaviour
{
    [SerializeField] private GameObject ghostPrefab;
    public void HandleDeath()
    {
        var aiBase = GetComponent<AIBase>();
        var playerInput = GetComponent<PlayerInput>();
        
        if (aiBase)
        {
            aiBase.enabled = false;
        }
        
        if (playerInput)
        {
            playerInput.enabled = false;
        }
        
        if (CompareTag("Player"))
        {
            var currentTransform = transform;
            var currentTransformPosition = currentTransform.position;
            currentTransformPosition.z = -4;
            GameObject go = Instantiate(ghostPrefab, currentTransformPosition, currentTransform.rotation);
            
            CameraFollow2D.Instance.SetTarget(go);
            FollowingAudioListener2D.Instance.Target = go;
            gameObject.tag = "Enemy";
        }
        
        Destroy(this);
    }
}
