using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;
using Weapons;
using Random = Unity.Mathematics.Random;

public class RandomWeapon : MonoBehaviour
{
    [SerializeField] private GameObject[] weapons;
    void Start()
    {
        var parent = transform.parent;
        GameObject go = Instantiate(weapons[UnityEngine.Random.Range(0, weapons.Length)], parent);
        go.transform.localPosition = Vector3.zero;
        go.transform.localRotation = Quaternion.identity;
        
        parent.parent.GetComponent<WeaponManager>().SetCurrentWeapon(go.GetComponent<WeaponBase>());
        Destroy(gameObject);
    }
}
