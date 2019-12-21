using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InkScript : MonoBehaviour
{
    public GameObject InkUI;
    // public GameObject inkUI;
    public float startingInk = 5f;
    public float currentInk;
    float cooldown = 2f;
    Image[] inkImages = new Image[5];

    void Start()
    {
        currentInk = 5f;
        int numOfChild = InkUI.transform.childCount;
        for (int i = 0; i < numOfChild; i++)
        {
            inkImages[i] = InkUI.transform.GetChild(i).GetChild(0).GetComponent<Image>();
        }
    }

    public void ReduceInk(float inkAmount)
    {
        currentInk -= inkAmount;
    }

    void UpdateInkIndicator()
    {
        //Check if currentInk is equal or greater than startingInk
        if (currentInk >= startingInk) currentInk = startingInk;

        //Update images
        float floor = Mathf.Floor(currentInk);
        float currentFillAmount = currentInk - floor;
        for (int i = 0; i < inkImages.Length; i++)
        {
            if (i > floor) inkImages[i].fillAmount = 0f;
            else if (i == floor) inkImages[i].fillAmount = currentFillAmount;
        }
    }

    void Update()
    {
        if (currentInk < startingInk)
        {
            currentInk += 1 / cooldown * Time.deltaTime;
            UpdateInkIndicator();
        }
    }
}
