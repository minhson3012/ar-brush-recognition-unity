using UnityEngine;

namespace BrushGestures 
{
    public class FireScript: MonoBehaviour
    {
        public GameObject Fire;
        private GameObject power;
        private BrushPowers brushPowers;
        private GameObject dummy;
        private DummyScript dummyScript;
        

        public void SpawnFire()
        {
            brushPowers = GetComponent<BrushPowers>();
            power = Instantiate(Fire, GameObject.Find("Circle").transform.position, GameObject.Find("Circle").transform.rotation);
            power.transform.parent = GameObject.Find("Anchor").transform;
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