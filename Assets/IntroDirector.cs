using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;
using Weapons;

public class IntroDirector : MonoBehaviour
{
    [SerializeField] private GameObject[] enemies;
    [SerializeField] private GameObject player;
    void Awake()
    {
        foreach (var enemy in enemies)
        {
            enemy.GetComponent<AIBase>().enabled = false;
            enemy.transform.GetChild(1).gameObject.SetActive(false);
        }
    }

    private void Start()
    {
        MusicDirector.Instance.State = MusicDirector.MusicState.Intro;
    }

    void WakeEnemies()
    {
        foreach (var enemy in enemies)
        {
            enemy.GetComponent<AIBase>().enabled = true;
            enemy.transform.GetChild(1).gameObject.SetActive(true);
        }
    }

    public void StartGame()
    {
        WakeEnemies();
        WakePlayer();
        MusicDirector.Instance.State = MusicDirector.MusicState.Game;
    }

    private void WakePlayer()
    {
        player.gameObject.SetActive(true);
    }
}
