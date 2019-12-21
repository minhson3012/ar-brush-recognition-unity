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

            Collider[] colliders = Physics.OverlapSphere(power.transform.position, radius);

            foreach (Collider nearbyObject in colliders)
            {
                Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
                if (nearbyObject.tag.Equals("Enemy"))
                {
                    rb.AddExplosionForce(force, power.transform.position, radius);
                    // dummyScript.Die();
                    nearbyObject.GetComponent<EnemyHealth>().TakeDamage(200);
                }
            }
            Destroy(explosion, 2);
            Destroy(power);
            isSpawned = false;
        }
    }
}