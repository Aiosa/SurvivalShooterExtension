using System.Collections;
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
    [SerializeField]
    private Button exit;

    void Start()
    {
        original.onClick.AddListener(() => { load("Level 01"); });
        health.onClick.AddListener(() => { load("LevelHU"); });
        monster.onClick.AddListener(() => { load("Level02"); });
        lego.onClick.AddListener(() => { load("Legoslative"); });
        exit.onClick.AddListener(() => { Application.Quit(); });
    }

    void load(string level)
    {
        SceneManager.LoadScene(level, LoadSceneMode.Single);
    }
}

