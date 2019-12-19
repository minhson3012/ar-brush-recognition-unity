using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // public string sceneToLoad;

    // Start is called before the first frame update
    public void LoadScene(string sceneToLoad)
    {
        //GameObject[] objectsToKeep = GameObject.FindGameObjectsWithTag("DontDestroy");
        // foreach (GameObject gameObject in objectsToKeep)
        //     DontDestroyOnLoad(gameObject);
        SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive);
    }
}
