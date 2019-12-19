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
        healthCanvas.worldCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
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
