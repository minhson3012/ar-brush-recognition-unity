﻿using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // public NavMeshSurface surface;
    public GameObject playZone;
    public GameObject infoCanvas;
    public GameObject enemyManager;
    List<GameObject> items = new List<GameObject>();
    GameObject anchor;
    GameObject plane;
    EnemyManager manager;
    Text infoText;
    int previousNumOfEnemies;
    int totalNumOfEnemies;
    GameObject goal;
    GoalHealth goalHealth;
    void Awake()
    {
        //Find anchor
        anchor = GameObject.FindGameObjectWithTag("Anchor");

        //Attach playZone to anchor
        playZone.transform.parent = anchor.transform;
        playZone.transform.position = anchor.transform.position;
        playZone.transform.rotation = anchor.transform.rotation;

        //Attach infoCanvas to anchor
        infoCanvas.GetComponent<Canvas>().worldCamera = Camera.main;
        infoText = infoCanvas.GetComponentInChildren<Text>();

        manager = enemyManager.GetComponent<EnemyManager>();
    }

    void Start()
    {
        goal = GameObject.FindGameObjectWithTag("Goal");
        goalHealth = goal.GetComponent<GoalHealth>();

        StartCoroutine(DisplayStartText());
        previousNumOfEnemies = manager.GetTotalNumOfEnemies(manager.currentWave);
        totalNumOfEnemies = previousNumOfEnemies;
    }

    void Update()
    {
        int currentNumOfEnemies = manager.GetCurrentNumOfEnemies();
        if (goalHealth.currentHealth <= 0) infoText.text = "GAME OVER"; //Game over
        else if (currentNumOfEnemies == 0)
        {
            if (manager.currentWave <= manager.GetNumOfWaves() - 1)
            {
                infoText.text = "WAVE COMPLETE!\nGET READY FOR THE NEXT WAVE"; //Wave ended
                StartCoroutine(DisplayWaveText(10));
            }
            else infoText.text = "YOU WIN!"; //Game ended
        }
        else if (currentNumOfEnemies != previousNumOfEnemies)
        {
            //Game is still going
            infoText.text = "WAVE " + (manager.currentWave + 1) + ": " + currentNumOfEnemies + "/" + totalNumOfEnemies;
            previousNumOfEnemies = currentNumOfEnemies;
        }
    }

    IEnumerator DisplayStartText()
    {
        Debug.Log(infoText.text);
        infoText.text = "PROTECT THE CHICKEN";
        yield return new WaitForSeconds(2);

        infoText.text = "GET READY";

        yield return new WaitForSeconds(1);
        StartCoroutine(DisplayWaveText(3));
    }

    IEnumerator DisplayWaveText(float time)
    {
        // float time = 3f;
        while (time > 0f)
        {
            infoText.text = time.ToString();
            yield return new WaitForSeconds(1);
            time--;
        }
        infoText.text = "WAVE " + (manager.currentWave + 1) + ": " + totalNumOfEnemies + "/" + totalNumOfEnemies;
        manager.gameStarted = true;
    }
}
