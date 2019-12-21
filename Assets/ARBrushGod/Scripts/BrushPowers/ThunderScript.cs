using UnityEngine;

namespace BrushGestures
{
    public class ThunderScript : MonoBehaviour
    {
        public GameObject Thunder;
        public float radius = 0.1f;
        public float inkCost = 2f;
        private GameObject power;
        private BrushPowers brushPowers;
        // private GameObject dummy;
        // private DummyScript dummyScript;


        public void SpawnThunder()
        {
            //Instantiate power
            brushPowers = GetComponent<BrushPowers>();
            power = Instantiate(Thunder, GameObject.FindGameObjectWithTag("Circle").transform.position, GameObject.FindGameObjectWithTag("Circle").transform.rotation);
            power.transform.parent = GameObject.FindGameObjectWithTag("Anchor").transform;

            //Reduce ink amount
            GetComponent<InkScript>().ReduceInk(inkCost);

            //Cleanup UI
            brushPowers.CleanupUI();

            //Check for hits
            CheckForHit();

            //Destroy prefab
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