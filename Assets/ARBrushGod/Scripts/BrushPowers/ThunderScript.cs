using GoogleARCore;
using UnityEngine;
using UnityEngine.UI;

namespace BrushGestures 
{
    public class ThunderScript: MonoBehaviour
    {
        public GameObject Thunder;
        private GameObject power;
        private BrushPowers brushPowers;
        private GameObject dummy;
        private DummyScript dummyScript;
        

        public void SpawnThunder()
        {
            brushPowers = GetComponent<BrushPowers>();
            power = Instantiate(Thunder, GameObject.Find("Circle").transform.position, GameObject.Find("Circle").transform.rotation);
            brushPowers.CleanupUI();
            CheckForHit();
            Destroy(power, 2);
        }

        private void CheckForHit()
        {
            dummy = GameObject.FindGameObjectWithTag("Dummy");
            dummyScript = dummy.GetComponent<DummyScript>();
            float distance = Vector3.Distance(power.transform.position, dummy.transform.position);
            Debug.Log("Distance: " + distance);
            if(distance <= 0.1f)
            {
                dummyScript.setShocked();
            }
        }
    }
}