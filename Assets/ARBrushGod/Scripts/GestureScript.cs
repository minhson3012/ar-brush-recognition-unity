using UnityEngine;
using System.Collections.Generic;
using PDollarGestureRecognizer;
using QDollarGestureRecognizer;

public class GestureScript : MonoBehaviour
{
    public Transform gestureOnScreenPrefab;
    private List<Gesture> trainingSet = new List<Gesture>();
    private List<Point> points = new List<Point>();
    private int strokeId = -1;
    private Vector3 virtualKeyPosition = Vector2.zero;
    private int vertexCount = -1;
    private List<LineRenderer> gestureLinesRenderer = new List<LineRenderer>();
    private LineRenderer currentGestureLineRenderer;
    private bool recognized;

    // Start is called before the first frame update
    void Start()
    {
        //Load pre-made gestures
        TextAsset[] gesturesXml = Resources.LoadAll<TextAsset>("GestureSet/");
        foreach (TextAsset gestureXml in gesturesXml)
            trainingSet.Add(GestureIO.ReadGestureFromXML(gestureXml.text));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0 || Input.GetMouseButton(0))
        {
            virtualKeyPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y);
        }

        if (Input.GetMouseButtonDown(0))
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

            ++strokeId;

            Transform tmpGesture = Instantiate(gestureOnScreenPrefab, transform.position, transform.rotation) as Transform;
            currentGestureLineRenderer = tmpGesture.GetComponent<LineRenderer>();

            gestureLinesRenderer.Add(currentGestureLineRenderer);

            vertexCount = 0;
            Debug.Log(strokeId);
        }

        if (Input.GetMouseButton(0))
        {
            points.Add(new Point(virtualKeyPosition.x, -virtualKeyPosition.y, strokeId));

            currentGestureLineRenderer.positionCount = ++vertexCount;
            currentGestureLineRenderer.SetPosition(vertexCount - 1, Camera.main.ScreenToWorldPoint(new Vector3(virtualKeyPosition.x, virtualKeyPosition.y, 10)));
        }
    }

    public void OnRecognizeButtonClick()
    {
        recognized = true;
        foreach (var point in points.ToArray()){
            Debug.Log("X:" + point.X + " " + "Y:" + point.Y + " " + "strokeID: " + point.StrokeID);
        }
        
        Gesture candidate = new Gesture(points.ToArray());
        string gestureResult = QPointCloudRecognizer.Classify(candidate, trainingSet.ToArray());
        Debug.Log(gestureResult);
    }
}
