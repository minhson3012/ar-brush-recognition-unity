using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InkScript : MonoBehaviour
{
    public GameObject InkUI;
    // public GameObject inkUI;
    [SerializeField]
    float startingInk = 5f;
    [SerializeField]
    float currentInk;
    Image[] inkImages = new Image[5];

    void Start()
    {
        currentInk = 5f;
        int numOfChild = InkUI.transform.childCount;
        for(int i = 0; i < numOfChild; i++)
        {
            inkImages[i] = InkUI.transform.GetChild(i).GetChild(0).GetComponent<Image>();
        }
        Test();
    }

    void Test()
    {
        for(int i = 0; i < 5; i++)
        {
            inkImages[i].fillAmount = 0f;
        }
    }
}
