using UnityEngine.UI;
using UnityEngine;

public class GoalHealth : MonoBehaviour
{
    public float startingHealth = 150f;
    public float currentHealth;
    public Image healthBar;
    bool isDead;

    void Awake()
    {
        currentHealth = startingHealth;
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
