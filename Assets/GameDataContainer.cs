using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataContainer : MonoBehaviour
{
    public static GameDataContainer Instance => _instance;
    private static GameDataContainer _instance;

    public float RemainingGhostTime { get; set; } = 10f;
    void Awake()
    {
        _instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
