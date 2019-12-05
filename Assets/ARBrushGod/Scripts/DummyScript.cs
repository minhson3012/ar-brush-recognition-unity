using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyScript : MonoBehaviour
{
    private Animator animator;
    bool isShocked;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        isShocked = false;
        StartCoroutine(Reset(true, 0f));
    }

    public void setShocked()
    {
        isShocked = !isShocked;
        animator.SetBool("isShocked", isShocked);
        Debug.Log("Shock status: " + isShocked);
    }

    public void Die()
    {
        Debug.Log("DIE");
        animator.enabled = false;
        Rigidbody[] bodies = GetComponentsInChildren<Rigidbody>();
        Collider[] colliders = GetComponentsInChildren<Collider>();
        foreach (Rigidbody rb in bodies)
        {
            rb.isKinematic = false;
            rb.detectCollisions = true;
            // rb.useGravity = true;
        }
        foreach (Collider col in colliders)
        {
            col.enabled = true;
        }
        StartCoroutine(Reset(false, 1.5f));
    }

    IEnumerator Reset(bool enableAnimator, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        animator.enabled = enableAnimator;
        Rigidbody[] bodies = GetComponentsInChildren<Rigidbody>();
        Collider[] colliders = GetComponentsInChildren<Collider>();
        foreach (Rigidbody rb in bodies)
        {
            if (!rb.tag.Equals("Dummy"))
            {
                rb.detectCollisions = false;
            }
            rb.isKinematic = true;
            // rb.useGravity = true;
        }
        foreach (Collider col in colliders)
        {
            if (!col.tag.Equals("Dummy"))
                col.enabled = false;
        }
    }
}
