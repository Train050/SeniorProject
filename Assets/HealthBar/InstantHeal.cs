using UnityEngine;
using System.Collections.Generic;

public class InstantHeal : MonoBehaviour
{
    public int HealingToPlayer = 30;
    public HealthBar playerHealth;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Debug.Log("Instant Heal");
            playerHealth.Heal(HealingToPlayer);
            Destroy(gameObject);
            Debug.Log(playerHealth.currentHealth);
        }
    }
}
