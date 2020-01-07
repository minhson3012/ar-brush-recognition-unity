using GoogleARCore;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace BrushGestures
{
    public class TreeScript : MonoBehaviour
    {
        public GameObject Tree;
        public float radius = 0.3f;
        public float speed = 0.5f;
        public float duration = 1.5f;
        public float inkCost = 3f;
        GameObject power;
        BrushPowers brushPowers;
        GameObject circle;
        Vector3 minScale = new Vector3(0.001f, 0.001f, 0.001f);
        Vector3 maxScale = new Vector3(0.006f, 0.006f, 0.006f);
        float lerpTime = 0f;
        GameObject[] enemies;

        public void SpawnTree()
        {
            brushPowers = GetComponent<BrushPowers>();
            circle = GameObject.FindGameObjectWithTag("Circle");
            Vector3 position = new Vector3(circle.transform.position.x, circle.transform.position.y, circle.transform.position.z);

            //Create power
            power = Instantiate(Tree, position, Tree.transform.rotation);
            power.transform.parent = GameObject.FindGameObjectWithTag("Anchor").transform;

            //Reduce Ink
            transform.GetComponent<InkScript>().ReduceInk(inkCost);

            //Cleanup
            brushPowers.CleanupUI();
            Destroy(power, 5);
        }

        void Update()
        {
            if (power)
            {
                if (lerpTime < 1.5f)
                {
                    power.transform.localScale = Vector3.Lerp(minScale, maxScale, lerpTime);
                    lerpTime += Time.deltaTime * speed;
                }
                CheckForHit();
            }
            else if (!power) lerpTime = 0f;


        }

        private void CheckForHit()
        {
            if ((enemies = GameObject.FindGameObjectsWithTag("Enemy")) != null)
            {
                foreach (GameObject e in enemies)
                {
                    float distance = Vector3.Distance(power.transform.position, e.transform.position);
                    if (distance <= radius)
                    {
                        //Stop enemies in tree range
                        EnemyMovement em = e.GetComponent<EnemyMovement>();
                        em.SetMoveTime(duration);
                    }
                }
            }
        }
    }
}