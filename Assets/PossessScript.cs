using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;
using UnityEngine.InputSystem;

public class PossessScript : MonoBehaviour
{
    private GameObject _possessionTarget = null;
    private HashSet<GameObject> _possessablePawns = new HashSet<GameObject>();
    private PlayerInput _playerInput;
    private static readonly int PossessTrigger = Animator.StringToHash("Possess");

    private void Awake()
    {
        _playerInput = transform.parent.GetComponent<PlayerInput>();
    }
    
    // Update is called once per frame
    void OnTriggerEnter2D(Collider2D collision)
    {
        _possessablePawns.Add(collision.gameObject);
    }

    // Update is called once per frame
    void OnTriggerExit2D(Collider2D collision)
    {
        _possessablePawns.Remove(collision.gameObject);
    }

    private void Update()
    {
        if (_playerInput && _playerInput.actions["fire"].IsPressed())
        {
            BeginPossessionOnClosest();
        }
    }

    public void BeginPossessionOnClosest()
    {
        GameObject closest = null;
        float minDistance = 0;
        foreach (var possessablePawn in _possessablePawns)
        {
            var healthManager = possessablePawn.GetComponent<HealthManager>();
            if (!closest && possessablePawn.CompareTag("Enemy") && healthManager && !healthManager.IsDead)
            {
                closest = possessablePawn;
                minDistance = Vector3.SqrMagnitude(possessablePawn.transform.position - transform.position);
                continue;
            }
            float curDistance = Vector3.SqrMagnitude(possessablePawn.transform.position - transform.position);
            if (curDistance < minDistance && possessablePawn.CompareTag("Enemy") && healthManager && !healthManager.IsDead)
            {
                closest = possessablePawn;
                minDistance = curDistance;
            }
        }

        if (closest)
        {
            _possessionTarget = closest;
            var parent = transform.parent;
            parent.parent = closest.transform;
            parent.GetComponent<Animator>().SetTrigger(PossessTrigger);
            Destroy(parent.GetComponent<Movement2D>());
            Destroy(parent.GetComponent<Rigidbody2D>());
            parent.localPosition = new Vector3(0, 0, -4);
            parent.localRotation = Quaternion.identity;
        }
    }

    public void Possess()
    {
        if (_possessionTarget)
        {
            CameraFollow2D.Instance.SetTarget(_possessionTarget);
            FollowingAudioListener2D.Instance.Target = _possessionTarget;
            _possessionTarget.tag = "Player";
            _possessionTarget.name = "Player";
            _possessionTarget.GetComponent<PlayerInput>().enabled = true;
            Destroy(_possessionTarget.GetComponent<AIBase>());
            _possessionTarget.GetComponent<AimManager>().EnableCrosshair();
            _possessionTarget.GetComponent<WeaponManager>().RefreshEffectLibrary();
            Destroy();
        }
    }

    public void Destroy()
    {
        Destroy(transform.parent.gameObject);
    }
}
