using System.Collections.Generic;
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
        StartCoroutine(DisplayStartText());
        previousNumOfEnemies = manager.GetTotalNumOfEnemies(manager.currentWave);
        totalNumOfEnemies = previousNumOfEnemies;
    }

    void Update()
    {
        int currentNumOfEnemies = manager.GetCurrentNumOfEnemies();
        
        if (currentNumOfEnemies != previousNumOfEnemies)
        {
            infoText.text = "WAVE " + (manager.currentWave + 1) +": " + currentNumOfEnemies + "/" + totalNumOfEnemies;
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

        float time = 3f;
        while (time > 0f)
        {
            infoText.text = time.ToString();
            yield return new WaitForSeconds(1);
            time--;
        }
        infoText.text = "WAVE " + (manager.currentWave + 1) +": " + totalNumOfEnemies + "/" + totalNumOfEnemies;
        manager.gameStarted = true;
    }
}
