namespace GoogleARCore.Examples.HelloAR
{
    using System.Collections.Generic;
    using GoogleARCore;
    using GoogleARCore.Examples.Common;
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.EventSystems;

#if UNITY_EDITOR
    // Set up touch input propagation while using Instant Preview in the editor.
    using Input = InstantPreviewInput;
#endif

    /// <summary>
    /// Controls the HelloAR example.
    /// </summary>
    public class SceneController : MonoBehaviour
    {
        /// <summary>
        /// The first-person camera being used to render the passthrough camera image (i.e. AR
        /// background).
        /// </summary>
        public Camera FirstPersonCamera;

        /// <summary>
        /// A prefab to place when a raycast from a user touch hits a feature point.
        /// </summary>
        public GameObject GameObjectPrefab;
        public GameObject PlacementIndicator;
        public GameObject SceneLoader;
        public Button DrawButton;
        public Button ResetButton;
        public GameObject InkUI;
        /// <summary>
        /// True if the app is in the process of quitting due to an ARCore connection error,
        /// otherwise false.
        /// </summary>
        private bool m_IsQuitting = false;
        private bool isInstantiated = false;
        public Vector3 startPosition;
        public Quaternion startRotation;
        Transform indicatorTransform;

        /// <summary>
        /// The Unity Awake() method.
        /// </summary>
        public void Awake()
        {
            // Enable ARCore to target 60fps camera capture frame rate on supported devices.
            // Note, Application.targetFrameRate is ignored when QualitySettings.vSyncCount != 0.
            Application.targetFrameRate = 60;
        }

        /// <summary>
        /// The Unity Update() method.
        /// </summary>
        public void Update()
        {
            _UpdateApplicationLifecycle();
            TrackableHit hit;
            TrackableHitFlags raycastFilter = TrackableHitFlags.PlaneWithinPolygon |
                TrackableHitFlags.FeaturePointWithSurfaceNormal;
            var screenCenter = Camera.main.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
            //Raycast from center of screen
            if (Frame.Raycast(screenCenter.x, screenCenter.y, raycastFilter, out hit))
            {
                if (hit.Trackable is DetectedPlane)
                {
                    DetectedPlane detectedPlane = hit.Trackable as DetectedPlane;
                    if (detectedPlane.PlaneType == DetectedPlaneType.HorizontalUpwardFacing)
                    {
                        //If there's no indicator
                        if (indicatorTransform == null)
                        {
                            SpawnIndicator(hit.Pose.position, hit.Pose.rotation);
                        }
                        else
                        {
                            Touch touch;
                            if (!isInstantiated)
                            {
                                //If there's no touch input, update indicator's transform
                                if (Input.touchCount < 1 || (touch = Input.GetTouch(0)).phase != TouchPhase.Began)
                                {
                                    UpdateIndicator(hit.Pose.position, hit.Pose.rotation);
                                    return;
                                }

                                //Otherwise spawn stuff
                                if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
                                {
                                    // var gameObject = SpawnDummy(hit.Pose.position, hit.Pose.rotation);
                                    // startPosition = hit.Pose.position;
                                    // startRotation = hit.Pose.rotation;

                                    // Create an anchor to allow ARCore to track the hitpoint as understanding of
                                    // the physical world evolves.
                                    var anchor = hit.Trackable.CreateAnchor(hit.Pose);
                                    anchor.gameObject.tag = "Anchor";

                                    // Make game object a child of the anchor.
                                    // gameObject.transform.parent = anchor.transform;

                                    //Activate UI
                                    DrawButton.gameObject.SetActive(true);
                                    InkUI.gameObject.SetActive(true);
                                    // ResetButton.gameObject.SetActive(true);

                                    //Deactivate indicator
                                    indicatorTransform.gameObject.SetActive(false);
                                    isInstantiated = true;

                                    //Load scene
                                    var sceneLoader = SceneLoader.GetComponent<SceneLoader>();
                                    sceneLoader.LoadScene("Game");

                                    //Disable Point Cloud and Plane Generator
                                    GameObject.FindGameObjectWithTag("PointCloud").SetActive(false);
                                    GameObject.FindGameObjectWithTag("PlaneGenerator").SetActive(false);
                                }
                            }
                        }
                    }
                }
            }
        }
        public void UpdateIndicator(Vector3 position, Quaternion rotation)
        {
            if (Session.Status == SessionStatus.Tracking)
            {
                indicatorTransform.gameObject.SetActive(true);
                indicatorTransform.position = position;
                Vector3 oneCentimeterUp = Vector3.up * 0.01f;
                indicatorTransform.Translate(oneCentimeterUp, Space.Self);
                indicatorTransform.rotation = rotation;
            }
            else
            {
                indicatorTransform.gameObject.SetActive(false);
            }
        }
        public void SpawnIndicator(Vector3 position, Quaternion rotation)
        {
            indicatorTransform = Instantiate(PlacementIndicator, position, rotation).transform;
        }
        public GameObject SpawnDummy(Vector3 position, Quaternion rotation)
        {
            return Instantiate(GameObjectPrefab, position, rotation);
        }

        /// <summary>
        /// Check and update the application lifecycle.
        /// </summary>
        private void _UpdateApplicationLifecycle()
        {
            // Exit the app when the 'back' button is pressed.
            if (Input.GetKey(KeyCode.Escape))
            {
                Application.Quit();
            }

            // Only allow the screen to sleep when not tracking.
            if (Session.Status != SessionStatus.Tracking)
            {
                Screen.sleepTimeout = SleepTimeout.SystemSetting;
            }
            else
            {
                Screen.sleepTimeout = SleepTimeout.NeverSleep;
            }

            if (m_IsQuitting)
            {
                return;
            }

            // Quit if ARCore was unable to connect and give Unity some time for the toast to
            // appear.
            if (Session.Status == SessionStatus.ErrorPermissionNotGranted)
            {
                _ShowAndroidToastMessage("Camera permission is needed to run this application.");
                m_IsQuitting = true;
                Invoke("_DoQuit", 0.5f);
            }
            else if (Session.Status.IsError())
            {
                _ShowAndroidToastMessage(
                    "ARCore encountered a problem connecting.  Please start the app again.");
                m_IsQuitting = true;
                Invoke("_DoQuit", 0.5f);
            }
        }

        /// <summary>
        /// Actually quit the application.
        /// </summary>
        private void _DoQuit()
        {
            Application.Quit();
        }

        /// <summary>
        /// Show an Android toast message.
        /// </summary>
        /// <param name="message">Message string to show in the toast.</param>
        private void _ShowAndroidToastMessage(string message)
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject unityActivity =
                unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

            if (unityActivity != null)
            {
                AndroidJavaClass toastClass = new AndroidJavaClass("android.widget.Toast");
                unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
                {
                    AndroidJavaObject toastObject =
                        toastClass.CallStatic<AndroidJavaObject>(
                            "makeText", unityActivity, message, 0);
                    toastObject.Call("show");
                }));
            }
        }
    }
}
