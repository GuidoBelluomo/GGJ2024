using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnedEffect : MonoBehaviour
{
    public void Destroy()
    {
        Destroy(gameObject);
    }
}
