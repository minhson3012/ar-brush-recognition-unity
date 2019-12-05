using UnityEngine;
using System.Collections.Generic;
using PDollarGestureRecognizer;
using QDollarGestureRecognizer;
using UnityEngine.UI;

namespace BrushGestures
{
    public class GestureScript : MonoBehaviour
    {
        public Transform gestureOnScreenPrefab;
        public GameObject GestureCanvas;
        public GameObject DrawButton;
        private List<Gesture> trainingSet = new List<Gesture>();
        private List<Point> points = new List<Point>();
        private int strokeId = -1;
        private Vector3 virtualKeyPosition = Vector2.zero;
        private int vertexCount = -1;
        private List<LineRenderer> gestureLinesRenderer = new List<LineRenderer>();
        private LineRenderer currentGestureLineRenderer;
        private bool recognized;
        private bool isAllowedToDraw = false;
        private BrushPowers brushPowers;

        // Start is called before the first frame update
        void Start()
        {
            //Load pre-made gestures
            TextAsset[] gesturesXml = Resources.LoadAll<TextAsset>("GestureSet/");
            foreach (TextAsset gestureXml in gesturesXml)
                trainingSet.Add(GestureIO.ReadGestureFromXML(gestureXml.text));
            brushPowers = GetComponent<BrushPowers>();
        }

        // Update is called once per frame
        void Update()
        {
            if (isAllowedToDraw)
            {
                //Debug.Log("Drawing: " + isAllowedToDraw);
                RenderLines();
            }
        }

        public void OnDrawButtonClick()
        {
            if (!isAllowedToDraw)
            {
                DrawButton.GetComponentInChildren<Text>().text = "Done";
                isAllowedToDraw = true;
                Debug.Log("Drawing");
            }
            else
            {
                OnRecognizeButtonClick();
                isAllowedToDraw = false;
                DrawButton.GetComponentInChildren<Text>().text = "Draw";
                Debug.Log("Done");
            }
        }

        public void OnRecognizeButtonClick()
        {
            string gestureResult;
            try
            {
                recognized = true;
                // foreach (var point in points.ToArray())
                // {
                //     // Debug.Log("X:" + point.X + " " + "Y:" + point.Y + " " + "strokeID: " + point.StrokeID);
                // }

                //Recognize drawing
                Gesture candidate = new Gesture(points.ToArray());
                gestureResult = QPointCloudRecognizer.Classify(candidate, trainingSet.ToArray());
                Debug.Log(gestureResult);
            }
            catch
            {
                gestureResult = "none";
            }
            //Activate power visualizer
            DestroyLines();
            brushPowers.InvokePower(gestureResult);
            Debug.Log(gestureResult);
        }

        public void DestroyLines()
        {
            if (recognized)
            {

                recognized = false;
                strokeId = -1;

                points.Clear();

                foreach (LineRenderer lineRenderer in gestureLinesRenderer)
                {

                    lineRenderer.positionCount = 0;
                    Destroy(lineRenderer.gameObject);
                }

                gestureLinesRenderer.Clear();
            }
        }

        private void RenderLines()
        {
            if (Input.touchCount > 0 || Input.GetMouseButton(0))
            {
                virtualKeyPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y);
            }

            if (Input.GetMouseButtonDown(0))
            {
                ++strokeId;

                Transform tmpGesture = Instantiate(gestureOnScreenPrefab, transform.position, transform.rotation) as Transform;
                tmpGesture.transform.SetParent(GameObject.Find("First Person Camera").transform);
                currentGestureLineRenderer = tmpGesture.GetComponent<LineRenderer>();
                currentGestureLineRenderer.startWidth = 0.003f;
                currentGestureLineRenderer.endWidth = 0.003f;

                gestureLinesRenderer.Add(currentGestureLineRenderer);

                vertexCount = 0;
            }

            if (Input.GetMouseButton(0))
            {
                points.Add(new Point(virtualKeyPosition.x, -virtualKeyPosition.y, strokeId));

                currentGestureLineRenderer.positionCount = ++vertexCount;
                currentGestureLineRenderer.SetPosition(vertexCount - 1, Camera.main.ScreenToWorldPoint(new Vector3(virtualKeyPosition.x, virtualKeyPosition.y, 0.15f)));
            }
        }
    }
}