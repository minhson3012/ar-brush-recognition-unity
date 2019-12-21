using UnityEngine;

namespace BrushGestures
{
    public class WindScript : MonoBehaviour
    {
        public GameObject Wind;
        public float radius = 0.5f;
        GameObject power;
        BrushPowers brushPowers;
        GameObject circle;
        public void SpawnWind()
        {
            brushPowers = GetComponent<BrushPowers>();
            circle = GameObject.Find("Circle");
            Vector3 position = new Vector3(circle.transform.position.x - 0.2f, circle.transform.position.y + 0.1f, circle.transform.position.z);
            power = Instantiate(Wind, position, Quaternion.LookRotation(Camera.main.transform.forward, Camera.main.transform.up));
            power.transform.parent = GameObject.Find("Anchor").transform;
            // power.transform.rotation = Quaternion.LookRotation(-Camera.main.transform.forward, Camera.main.transform.up);
            brushPowers.CleanupUI();
            // Destroy(power, 15);
        }
    }
}