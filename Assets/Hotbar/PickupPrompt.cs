using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PickupPrompt : MonoBehaviour
{
    public SpriteRenderer InteractImage;
    private HotbarController inventoryController;
    private PauseMenu pauseMenu;
    private bool playerInItem = false;
    private AudioSource audioSource;

    void Start()
    {
        InteractImage.gameObject.SetActive(false);
        inventoryController = FindObjectOfType<HotbarController>();
        pauseMenu = FindObjectOfType<PauseMenu>();
        audioSource = GameObject.FindWithTag("WorldAudio").GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!pauseMenu.getOpen())
        {
            if (playerInItem && Input.GetKeyDown(KeyCode.E))
            {
                InteractImage.gameObject.SetActive(false);
                Item item = GetComponent<Item>();

                if (item != null)
                {
                    bool itemAdded = inventoryController.AddItem(gameObject);

                    if (itemAdded)
                    {
                        Destroy(gameObject);
                        
                        if(item.pickupSound != null)
                        {
                            audioSource.PlayOneShot(item.pickupSound);
                        }
                    }
                }
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerInItem = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            InteractImage.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            InteractImage.gameObject.SetActive(false);
            playerInItem = false;
        }
    }
}
