using GoogleARCore;
using UnityEngine;
using UnityEngine.UI;

namespace BrushGestures
{
    public class BombScript : MonoBehaviour
    {
        public GameObject Bomb;
        public float delay = 3f;
        public GameObject explosionEffect;
        public float radius = 0.1f;
        public float force = 500f;
        public float inkCost = 4f;
        GameObject power;
        BrushPowers brushPowers;
        GameObject dummy;
        DummyScript dummyScript;
        bool isSpawned = false;
        float countdown;
        public void SpawnBomb()
        {
            countdown = delay;
            brushPowers = GetComponent<BrushPowers>();
            power = Instantiate(Bomb, GameObject.FindGameObjectWithTag("Circle").transform.position, GameObject.FindGameObjectWithTag("Circle").transform.rotation);
            power.transform.parent = GameObject.FindGameObjectWithTag("Anchor").transform;
            transform.GetComponent<InkScript>().ReduceInk(inkCost);
            brushPowers.CleanupUI();
            isSpawned = true;
        }
        void Update()
        {
            if (isSpawned)
            {
                countdown -= Time.deltaTime;
                if (countdown <= 0f)
                {
                    Explode();
                }
            }
        }

        private void Explode()
        {
            var explosion = Instantiate(explosionEffect, power.transform.position, power.transform.rotation);

            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject e in enemies)
            {
                float distance = Vector3.Distance(power.transform.position, e.transform.position);
                if (distance <= radius)
                {
                    e.GetComponent<EnemyHealth>().TakeDamage(200f);
                }
            }
            Destroy(explosion, 2);
            Destroy(power);
            isSpawned = false;
        }
    }
}