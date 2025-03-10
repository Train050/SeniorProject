using UnityEngine;

public class FireEPrompt : MonoBehaviour
{
    public GameObject InteractImage;
    public GameObject InventoryPanel;
    private GameObject gameManager;
    private GameObject fire;
    private PauseMenu pauseMenu;
    bool isFirewood = false;
    int firewoodCount = 0;
    public AudioClip woodOnFire;
    private AudioSource audioSource;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = GameObject.Find("GameManagers");
        fire = GameObject.Find("Fire");
        pauseMenu = FindObjectOfType<PauseMenu>();
        audioSource = GameObject.FindWithTag("WorldAudio").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
         if (Input.GetKeyDown(KeyCode.E) && InteractImage.active && !pauseMenu.getOpen())
            {
                if(gameManager != null)
                {
                    //If there is fire wood on the player, then it will find it and put it in the fire (destroying it)
                    for(int i = 0; i < InventoryPanel.transform.childCount; i++)
                    {
                        Slot slot = InventoryPanel.transform.GetChild(i).GetComponent<Slot>();
                        
                        if(slot.currentItem == null)
                        {
                            continue;
                        }
                        
                        Item item = slot.currentItem.GetComponent<Item>();

                        if(slot.currentItem != null && item.itemType == ItemType.Firewood)
                        {
                            Debug.Log("Firewood added to fire");
                            
                            Destroy(slot.currentItem);
                            slot.currentItem = null;
                            firewoodCount--;
                            audioSource.PlayOneShot(woodOnFire);
                            
                            if(firewoodCount == 0)
                            {
                                isFirewood = false;
                            }

                            //Increase the brightness of the fire
                            if(fire != null)
                            {
                                Debug.Log("Increasing light radius of fire");
                                LightRadiusController lightRadiusController = fire.GetComponent<LightRadiusController>();
                                lightRadiusController.IncreaseRadius();
                            }
                            break;
                        }
                    }
                }
            }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //This could be optimized, might cause FPS issues later on
        //Ensures the player still have firewood in their inventory when they're at the campfire
        if (collision.gameObject.tag == "Player")
        {
            firewoodCount = 0;
            isFirewood = false;
            
            for(int i = 0; i < InventoryPanel.transform.childCount; i++)
            {
                Slot slot = InventoryPanel.transform.GetChild(i).GetComponent<Slot>();
                
                if(slot.currentItem == null)
                {
                    continue;
                }
                
                Item item = slot.currentItem.GetComponent<Item>();

                if(slot.currentItem != null && item.itemType == ItemType.Firewood)
                {
                    isFirewood = true;
                    firewoodCount++;
                }
            }

            if(!isFirewood)
            {
                InteractImage.gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            firewoodCount = 0;
            isFirewood = false;
            
            for(int i = 0; i < InventoryPanel.transform.childCount; i++)
            {
                Slot slot = InventoryPanel.transform.GetChild(i).GetComponent<Slot>();
                
                if(slot.currentItem == null)
                {
                    continue;
                }
                
                Item item = slot.currentItem.GetComponent<Item>();

                if(slot.currentItem != null && item.itemType == ItemType.Firewood)
                {
                    isFirewood = true;
                    firewoodCount++;
                }
            }

            if(isFirewood && firewoodCount > 0)
            {
                InteractImage.gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            InteractImage.gameObject.SetActive(false);
        }
    }
}
