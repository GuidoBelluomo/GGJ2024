using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;
using Random = Unity.Mathematics.Random;

public class TestAI : AIBase
{
    private float _moveTimer;
    private float _aimTimer;
    private float _shootTimer;
    private bool _holdingFire;
    // Update is called once per frame
    void Start()
    {
        _moveTimer = UnityEngine.Random.Range(1f, 3f);
        _aimTimer = UnityEngine.Random.Range(3f, 5f);
        _shootTimer = UnityEngine.Random.Range(4f, 7f);
    }

    private void Update()
    {
        _moveTimer -= Time.deltaTime;
        if (_moveTimer <= 0)
        {
            GetComponent<Movement2D>().Move(new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)).normalized * .75f);
            _moveTimer = UnityEngine.Random.Range(1f, 3f);
        }
        _aimTimer -= Time.deltaTime;
        if (_aimTimer <= 0)
        {
            GetComponent<AimManager>().Aim(new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)).normalized, AimManager.AimMode.Stick);
            _aimTimer = UnityEngine.Random.Range(3f, 5f);
        }
        
        _shootTimer -= Time.deltaTime;
        var weaponManager = GetComponent<WeaponManager>();
        if (_shootTimer <= 0)
        {
            if (!_holdingFire)
            {
                weaponManager.PrimaryFire();

                if (weaponManager.IsPrimaryFireAutomatic())
                {
                    _holdingFire = true;
                    _shootTimer = UnityEngine.Random.Range(0.5f, 1.5f);
                }
                else
                {
                    weaponManager.PrimaryFireReleased();
                    _shootTimer = UnityEngine.Random.Range(4f, 7f);
                }
            }
            else
            {
                _holdingFire = false;
                weaponManager.PrimaryFireReleased();
                _shootTimer = UnityEngine.Random.Range(4f, 7f);
            }
        }
        else if (_holdingFire)
        {
            weaponManager.PrimaryFire();
        }
    }

    private void OnDisable()
    {
        GetComponent<Movement2D>().Move(Vector2.zero);
    }
}
