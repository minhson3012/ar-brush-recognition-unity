using UnityEngine;

namespace BrushGestures
{
    public class RainScript : MonoBehaviour
    {
        public GameObject Rain;
        GameObject power;
        BrushPowers brushPowers;
        GameObject dummy;
        DummyScript dummyScript;
        GameObject circle;
        // bool isSpawned = false;
        public void SpawnRain()
        {
            // countdown = delay;
            dummy = GameObject.FindGameObjectWithTag("Dummy");
            dummyScript = dummy.GetComponent<DummyScript>();
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