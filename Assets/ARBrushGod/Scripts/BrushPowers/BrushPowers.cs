using GoogleARCore;
using UnityEngine;
using UnityEngine.UI;

namespace BrushGestures
{
    public class BrushPowers : MonoBehaviour
    {
        //ARCore Camera
        public Camera FirstPersonCamera;
        //Screen Reticle
        public GameObject Reticle;
        //Draw Button
        public Button DrawButton;
        //Accept Button
        public Button AcceptButton;
        //Cancel Button
        public Button CancelButton;
        public GameObject PowerText;
        //Is power active?
        bool powerActive = false;
        //Radius circle
        GameObject circleRadius;
        //Radius circle line color
        Color lineColor;
        //Gesture recognizer
        GestureScript gesture;
        //Thunder script
        ThunderScript thunderScript;
        BombScript bombScript;
        RainScript rainScript;
        TreeScript treeScript;
        FireScript fireScript;
        WindScript windScript;
        InkScript inkScript;
        float currentRadius;

        void Start()
        {
            gesture = GetComponent<GestureScript>();
            thunderScript = GetComponent<ThunderScript>();
            bombScript = GetComponent<BombScript>();
            rainScript = GetComponent<RainScript>();
            treeScript = GetComponent<TreeScript>();
            fireScript = GetComponent<FireScript>();
            windScript = GetComponent<WindScript>();
            inkScript = GetComponent<InkScript>();
        }
        public void InvokePower(string power)
        {
            if (power.Equals("none"))
            {
                return;
            }
            else
            {
                SpawnCircle();
                ShowPowerUI();
                switch (power)
                {
                    case "thunder":
                        if (CheckForRemainingInk(thunderScript.inkCost))
                        {
                            SetCircle("THUNDER", thunderScript.radius, Color.yellow);
                            AcceptButton.onClick.AddListener(thunderScript.SpawnThunder);
                        }
                        else CleanupUI();
                        return;
                    case "wind":
                        if (CheckForRemainingInk(windScript.inkCost))
                        {
                            SetCircle("WIND", windScript.radius, Color.cyan);
                            AcceptButton.onClick.AddListener(windScript.SpawnWind);
                        }
                        else CleanupUI();
                        return;
                    case "null":
                        if (CheckForRemainingInk(bombScript.inkCost))
                        {
                            SetCircle("BOMB", bombScript.radius, Color.white);
                            AcceptButton.onClick.AddListener(bombScript.SpawnBomb);
                        }
                        else CleanupUI();
                        return;
                    case "rain":
                        if (CheckForRemainingInk(rainScript.inkCost))
                        {
                            SetCircle("RAIN", rainScript.radius, Color.blue);
                            AcceptButton.onClick.AddListener(rainScript.SpawnRain);
                        }
                        else CleanupUI();
                        return;
                    case "tree":
                        if (CheckForRemainingInk(treeScript.inkCost))
                        {
                            SetCircle("TREE", treeScript.radius, Color.green);
                            AcceptButton.onClick.AddListener(treeScript.SpawnTree);
                        }
                        else CleanupUI();
                        return;
                    case "fire":
                        if (CheckForRemainingInk(fireScript.inkCost))
                        {
                            SetCircle("FIRE", fireScript.radius, Color.red);
                            AcceptButton.onClick.AddListener(fireScript.SpawnFire);
                        }
                        else CleanupUI();
                        return;
                }
            }
        }

        void Update()
        {
            if (powerActive)
                UpdatePowerIndicator();
        }

        //Set up circle
        void SetCircle(string powerText, float radius, Color powerColor)
        {
            lineColor = powerColor;
            circleRadius.GetComponentInChildren<TextMesh>().text = powerText;
            circleRadius.GetComponentInChildren<TextMesh>().color = powerColor;
            currentRadius = radius;
        }

        // Spawn a circle radius following the reticle
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
                    circleRadius.transform.parent = GameObject.FindGameObjectWithTag("Anchor").transform;
                    circleRadius.transform.position = new Vector3(hit.Pose.position.x, hit.Pose.position.y, hit.Pose.position.z);
                    circleRadius.transform.rotation = hit.Pose.rotation;
                    circleRadius.DrawCircle(currentRadius, 0.01f, lineColor);
                }
            }
        }

        //Show PowerUI and hide draw button
        public void ShowPowerUI()
        {
            powerActive = true;
            DrawButton.gameObject.SetActive(false);
            Reticle.SetActive(true);
            AcceptButton.gameObject.SetActive(true);
            CancelButton.gameObject.SetActive(true);
        }

        //Hide PowerUI and show draw button
        public void HidePowerUI()
        {
            powerActive = false;
            DrawButton.gameObject.SetActive(true);
            Reticle.SetActive(false);
            AcceptButton.gameObject.SetActive(false);
            CancelButton.gameObject.SetActive(false);
        }

        //Destroy circle radius
        void DestroyCircle()
        {
            Destroy(circleRadius);
        }

        void SpawnCircle()
        {
            circleRadius = new GameObject { name = "Circle", tag = "Circle" };
            var textObject = Instantiate(PowerText, new Vector3(circleRadius.transform.position.x, circleRadius.transform.position.y, circleRadius.transform.position.x - 0.15f),
                                         PowerText.transform.rotation);
            textObject.transform.parent = circleRadius.transform;
        }

        //When player clicks the Accept button
        public void CleanupUI()
        {
            Debug.Log("Cleaning up!");
            AcceptButton.onClick.RemoveAllListeners();
            HidePowerUI();
            DestroyCircle();
            gesture.DestroyLines();
        }

        bool CheckForRemainingInk(float inkAmount)
        {
            if (inkScript.currentInk < inkAmount) return false;
            return true;
        }
    }
}