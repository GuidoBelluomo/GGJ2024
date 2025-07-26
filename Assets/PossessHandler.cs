using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PossessHandler : MonoBehaviour
{
    [SerializeField] private PossessScript _possessScript;

    public void Possess()
    {
        _possessScript.Possess();
    }
}
