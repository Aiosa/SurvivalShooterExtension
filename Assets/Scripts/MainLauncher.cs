﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MainLauncher : MonoBehaviour
{
    [SerializeField]
    private Button original;
    [SerializeField]
    private Button health;
    [SerializeField]
    private Button monster;
    [SerializeField]
    private Button lego;

    void Start()
    {
        original.onClick.AddListener(() => { load("Level 01"); });
        health.onClick.AddListener(() => { Debug.Log("You have clicked the button!"); });
        monster.onClick.AddListener(() => { load("Level02"); });
        lego.onClick.AddListener(() => { load("Legoslative"); });

    }

    void load(string level)
    {
        SceneManager.LoadScene(level, LoadSceneMode.Single);
    }
}
