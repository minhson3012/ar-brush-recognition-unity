using UnityEngine;

namespace BrushGestures
{
    public class ThunderScript : MonoBehaviour
    {
        public GameObject Thunder;
        public float radius = 0.1f;
        private GameObject power;
        private BrushPowers brushPowers;
        // private GameObject dummy;
        // private DummyScript dummyScript;


        public void SpawnThunder()
        {
            brushPowers = GetComponent<BrushPowers>();
            power = Instantiate(Thunder, GameObject.Find("Circle").transform.position, GameObject.Find("Circle").transform.rotation);
            power.transform.parent = GameObject.Find("Anchor").transform;
            brushPowers.CleanupUI();
            CheckForHit();
            Destroy(power, 2);
        }

        private void CheckForHit()
        {
            // dummy = GameObject.FindGameObjectWithTag("Dummy");
            // dummyScript = dummy.GetComponent<DummyScript>();
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject e in enemies)
            {
                float distance = Vector3.Distance(power.transform.position, e.transform.position);
                if(distance <= radius)
                {
                    e.GetComponent<EnemyHealth>().TakeDamage(100f);
                }
            }
        }
    }
}