using UnityEngine;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DamageOnExit : MonoBehaviour
{
    public int DamageToPlayer = 1;
    public float TimeToDamage = 1f;
    private HealthBar playerHealth;
    private bool playerInside = false;
    private Coroutine exitCoroutine;

    /*
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Damage over time");
            playerHealth.TakeDamageOverTime(DamageToPlayer, TimeToDamage);
            //Destroy(gameObject);
            Debug.Log(playerHealth.currentHealth);
        }
    }
    */

    private void Start()
    {
        playerHealth = GameObject.Find("PlayerHealthBar").GetComponent<HealthBar>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Damage on Exit: Player entered light radius");
            playerInside = true;

            // Stop the coroutine if the player re-enters light
            if (exitCoroutine != null)
            {
                StopCoroutine(exitCoroutine);
                exitCoroutine = null;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Damage on Exit: Player left light radius");
            playerInside = false;

            //checks if player is outside radius every second by starting coroutine
            if (exitCoroutine == null)
            {
                exitCoroutine = StartCoroutine(RepeatedExitTrigger());
            }
        }
    }

    private System.Collections.IEnumerator RepeatedExitTrigger()
    {
        while (!playerInside && playerHealth != null && playerHealth.currentHealth > 0)
        {
            Debug.Log("Damage on Exit: Player outside light radius for 1s; take dmg");

            // Wait until the damage over time coroutine finishes
            yield return StartCoroutine(playerHealth.TakeDamageOverTime(DamageToPlayer, TimeToDamage));

            Debug.Log(playerHealth.currentHealth);
        }

        //exitCoroutine = null; // reset when finished
    }
}