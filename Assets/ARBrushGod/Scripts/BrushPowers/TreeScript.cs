using GoogleARCore;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace BrushGestures
{
    public class TreeScript : MonoBehaviour
    {
        public GameObject Tree;
        public float speed = 0.5f;
        public float duration = 1.5f;
        GameObject power;
        BrushPowers brushPowers;
        GameObject circle;
        Vector3 minScale = new Vector3(0.001f, 0.001f, 0.001f);
        Vector3 maxScale = new Vector3(0.006f, 0.006f, 0.006f);
        float lerpTime = 0f;

        public void SpawnTree()
        {
            brushPowers = GetComponent<BrushPowers>();
            circle = GameObject.Find("Circle");
            Vector3 position = new Vector3(circle.transform.position.x, circle.transform.position.y, circle.transform.position.z);
            power = Instantiate(Tree, position, Tree.transform.rotation);
            power.transform.parent = GameObject.Find("Anchor").transform;
            brushPowers.CleanupUI();
            Destroy(power, 5);
        }

        void Update()
        {
            if (power && lerpTime < 1.5f)
            {
                power.transform.localScale = Vector3.Lerp(minScale, maxScale, lerpTime);
                lerpTime += Time.deltaTime * speed;
            }
            else if(!power) lerpTime = 0f;
        }
    }
}