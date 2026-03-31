using UnityEngine;
using ClearSky;

public class PlayerHealth : MonoBehaviour
{
    public int health = 5;
    private bool isDead = false;

    private SimplePlayerController controller;

    void Start()
    {
        controller = GetComponent<SimplePlayerController>();
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        health -= damage;
        Debug.Log("Player hit! Health: " + health);

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;

        Debug.Log("Player Dead");

        // Disable movement
        if (controller != null)
        {
            controller.Die();
        }
    }
}