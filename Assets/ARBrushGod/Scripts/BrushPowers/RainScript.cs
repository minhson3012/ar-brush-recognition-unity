using UnityEngine;

namespace BrushGestures
{
    public class RainScript : MonoBehaviour
    {
        public GameObject Rain;
        public float radius = 0.5f;
        GameObject power;
        BrushPowers brushPowers;
        GameObject circle;
        public void SpawnRain()
        {
            brushPowers = GetComponent<BrushPowers>();
            circle = GameObject.Find("Circle");
            Vector3 position = new Vector3(circle.transform.position.x, circle.transform.position.y + 1f, circle.transform.position.z);
            power = Instantiate(Rain, position, Rain.transform.rotation);
            power.transform.parent = GameObject.Find("Anchor").transform;
            brushPowers.CleanupUI();
            Destroy(power, 5);
        }
    }
}