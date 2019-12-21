using UnityEngine;

namespace BrushGestures
{
    public class FireScript : MonoBehaviour
    {
        public GameObject Fire;
        public float radius = 0.1f;
        public float inkCost = 2f;
        private GameObject power;
        private BrushPowers brushPowers;
        // private GameObject dummy;
        // private DummyScript dummyScript;


        public void SpawnFire()
        {
            brushPowers = GetComponent<BrushPowers>();
            power = Instantiate(Fire, GameObject.FindGameObjectWithTag("Circle").transform.position, GameObject.FindGameObjectWithTag("Circle").transform.rotation);
            power.transform.parent = GameObject.FindGameObjectWithTag("Anchor").transform;
            brushPowers.CleanupUI();
            transform.GetComponent<InkScript>().ReduceInk(inkCost);
            CheckForHit();
            Destroy(power, 2);
        }

        private void CheckForHit()
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject e in enemies)
            {
                float distance = Vector3.Distance(power.transform.position, e.transform.position);
                if (distance <= radius)
                {
                    e.GetComponent<EnemyHealth>().TakeDamage(100f);
                }
            }
        }
    }
}