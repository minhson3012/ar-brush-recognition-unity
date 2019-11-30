using System.Collections.Generic;
using GoogleARCore;
using GoogleARCore.Examples.Common;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BrushGestures
{
    public class BrushPowers : MonoBehaviour
    {
        public Camera FirstPersonCamera;
        private bool powerActive = false;
        public GameObject Reticle;
        public GameObject DrawButton;
        public void InvokePower(string power)
        {
            switch (power)
            {
                case "thunder":
                    ShowPowerUI();
                    return;
                case "rain":
                    return;
                case "fire":
                    return;
                case "bullettime":
                    return;
                case "blizzard":
                    return;
                case "none":
                    return;
            }
        }

        void Update()
        {
            if (powerActive)
                UpdatePowerIndicator();
        }

        //Cast a ray from center of the screen to world, and then render a circle there
        void UpdatePowerIndicator()
        {
            TrackableHit hit;
            TrackableHitFlags raycastFilter = TrackableHitFlags.PlaneWithinPolygon |
                TrackableHitFlags.FeaturePointWithSurfaceNormal;

            //Cast a ray from the center of the screen
            if (Frame.Raycast(Screen.width / 2, Screen.height / 2, raycastFilter, out hit))
            {
                // Use hit pose and camera pose to check if hittest is from the
                // back of the plane, if it is, no need to create the anchor.
                if ((hit.Trackable is DetectedPlane) &&
                    Vector3.Dot(FirstPersonCamera.transform.position - hit.Pose.position,
                        hit.Pose.rotation * Vector3.up) < 0)
                {
                    Debug.Log("Hit at back of the current DetectedPlane");
                }
                else
                {
                    var circleRadius = new GameObject { name = "Circle" };
                    circleRadius.transform.parent = GameObject.Find("Anchor").transform;
                    circleRadius.DrawCircle(1f, 0.01f);
                }
            }
        }

        //Show PowerUI and hide draw button
        private void ShowPowerUI()
        {
            powerActive = true;
            DrawButton.SetActive(false);
            foreach (var obj in GameObject.FindGameObjectsWithTag("PowerUI"))
            {
                obj.SetActive(true);
            }
            
        }

        //Hide PowerUI and show draw button
        private void HidePowerUI()
        {
            powerActive = false;
            DrawButton.SetActive(true);
            foreach (var obj in GameObject.FindGameObjectsWithTag("PowerUI"))
            {
                obj.SetActive(false);
            }
        }

        //When player clicks the Accept button
        public void OnAcceptButtonClick()
        {
            Debug.Log("POWER ACTIVATE!");
            HidePowerUI();
        }

        //When player clicks the Cancel button
        public void OnCancelButtonClick()
        {
            Debug.Log("Cancelled");
            HidePowerUI();
        }
    }
}