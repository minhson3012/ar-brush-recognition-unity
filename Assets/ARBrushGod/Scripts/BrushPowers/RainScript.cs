using UnityEngine;
using System.Collections;

namespace BrushGestures
{
    public class RainScript : MonoBehaviour
    {
        public GameObject Rain;
        public float radius = 0.5f;
        public float inkCost = 5f;
        GameObject power;
        BrushPowers brushPowers;
        GameObject circle;
        float timer;
        bool isActivated;

        void Start()
        {
            timer = 0f;
            isActivated = false;
        }
        public void SpawnRain()
        {
            //Instantiate power
            brushPowers = GetComponent<BrushPowers>();
            circle = GameObject.FindGameObjectWithTag("Circle");
            Vector3 position = new Vector3(circle.transform.position.x, circle.transform.position.y + 1f, circle.transform.position.z);
            power = Instantiate(Rain, position, Rain.transform.rotation);
            power.transform.parent = GameObject.FindGameObjectWithTag("Anchor").transform;

            //Reduce current ink
            transform.GetComponent<InkScript>().ReduceInk(inkCost);
            isActivated = true;

            //Cleanup UI
            brushPowers.CleanupUI();
            Destroy(power, 3);
        }

        void Update()
        {
            if (isActivated)
            {
                timer += Time.deltaTime;
                if (timer < 3f)
                {
                    CheckForHit();
                }
                else isActivated = false;
            }
        }

        private void CheckForHit()
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject e in enemies)
            {
                float distance = Vector3.Distance(power.transform.position, e.transform.position);
                if (distance <= radius)
                {
                    //Heavily slow every enemy for 5 seconds
                    EnemyMovement em = e.GetComponent<EnemyMovement>();
                    em.SetMoveTime(2.5f, 3f);
                }
            }
        }
    }
}