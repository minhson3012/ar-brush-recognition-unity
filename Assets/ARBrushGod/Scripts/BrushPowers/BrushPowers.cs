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
        //Is power active?
        private bool powerActive = false;
        //Radius circle
        private GameObject circleRadius;
        //Radius circle line color
        private Color lineColor;
        //Gesture recognizer
        private GestureScript gesture;
        //Thunder script
        private ThunderScript thunderScript;
        private BombScript bombScript;
        private RainScript rainScript;
        private TreeScript treeScript;
        
        void Start()
        {
            gesture = GetComponent<GestureScript>();
            thunderScript = GetComponent<ThunderScript>();
            bombScript = GetComponent<BombScript>();
            rainScript = GetComponent<RainScript>();
            treeScript = GetComponent<TreeScript>();
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
                        lineColor = Color.yellow;
                        AcceptButton.onClick.AddListener(thunderScript.SpawnThunder);
                        return;
                    case "wind":
                        lineColor = Color.cyan;
                        return;
                    case "null":
                        lineColor = Color.white;
                        AcceptButton.onClick.AddListener(bombScript.SpawnBomb);
                        return;
                    case "rain":
                        lineColor = Color.blue;
                        AcceptButton.onClick.AddListener(rainScript.SpawnRain);
                        return;
                    case "tree":
                        lineColor = Color.green;
                        AcceptButton.onClick.AddListener(treeScript.SpawnTree);
                        return;
                    case "blizzard":
                        return;
                }
            }
        }

        void Update()
        {
            if (powerActive)
                UpdatePowerIndicator();
        }

        /// <summary>
        /// Spawn a circle radius following the reticle
        /// </summary>
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
                    circleRadius.transform.parent = GameObject.Find("Anchor").transform;
                    circleRadius.transform.position = new Vector3(hit.Pose.position.x, hit.Pose.position.y, hit.Pose.position.z);
                    circleRadius.transform.rotation = hit.Pose.rotation;
                    circleRadius.DrawCircle(0.1f, 0.01f, lineColor);
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
        private void DestroyCircle()
        {
            // if (circleRadius.GetComponent<Renderer>())
            // {
            //     Destroy(circleRadius.GetComponent<Renderer>().material);
            // }
            Destroy(circleRadius);
        }

        private void SpawnCircle()
        {
            circleRadius = new GameObject { name = "Circle" };
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

        //When player clicks the Cancel button
        // public void OnCancelButtonClick()
        // {
        //     Debug.Log("Cancelled");
        //     HidePowerUI();
        //     DestroyCircle();
        //     gesture.DestroyLines();
        // }
    }
}