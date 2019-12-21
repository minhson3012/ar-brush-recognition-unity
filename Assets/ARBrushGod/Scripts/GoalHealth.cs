using UnityEngine.UI;
using UnityEngine;

public class GoalHealth : MonoBehaviour
{
    public float startingHealth = 150f;
    public float currentHealth;
    public Image healthBar;
    bool isDead;
    Canvas healthCanvas;

    void Awake()
    {
        currentHealth = startingHealth;
        healthCanvas = GetComponentInChildren<Canvas>();
        healthCanvas.worldCamera = Camera.main.GetComponent<Camera>();
    }

    void Update()
    {
        healthCanvas.transform.LookAt(Camera.main.transform);
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        healthBar.fillAmount = currentHealth / startingHealth;

        if(currentHealth <= 0 && !isDead)
        {
            Death();
        }
    }

    void Death ()
    {
        isDead = true;
    }
}
