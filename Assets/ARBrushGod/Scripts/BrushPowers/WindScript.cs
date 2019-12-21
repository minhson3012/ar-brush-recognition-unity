using UnityEngine;
using System.Collections;

namespace BrushGestures
{
    public class WindScript : MonoBehaviour
    {
        public GameObject Wind;
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
        public void SpawnWind()
        {
            //Instantiate power
            brushPowers = GetComponent<BrushPowers>();
            circle = GameObject.FindGameObjectWithTag("Circle");
            Vector3 position = new Vector3(circle.transform.position.x - 0.2f, circle.transform.position.y + 0.1f, circle.transform.position.z);
            power = Instantiate(Wind, position, Quaternion.LookRotation(Camera.main.transform.forward, Camera.main.transform.up));
            power.transform.parent = GameObject.FindGameObjectWithTag("Anchor").transform;

            //Reduce current ink
            transform.GetComponent<InkScript>().ReduceInk(inkCost);
            isActivated = true;
            // power.transform.rotation = Quaternion.LookRotation(-Camera.main.transform.forward, Camera.main.transform.up);

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
                    em.SetMoveTime(5f, 3f);
                }
            }
        }
    }
}