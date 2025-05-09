using UnityEngine;

public class HotbarController : MonoBehaviour
{
    //2 video tutorials that helped create the inventory system:
    // https://www.youtube.com/watch?v=CcfYUYgaBTw
    // https://www.youtube.com/watch?v=wlBJ0yZOYfM

    public GameObject player;
    public GameObject inventoryPanel;
    public GameObject slotPrefab;
    public int slotCount;
    public GameObject[] itemPrefabs;
    public Color defaultBackgroundColor;
    public Color selectedBackgroundColor;
    private HealthBar healthBar;
    private AudioSource audioSource;
    private PlayerAttack playerAttack;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerAttack = player.GetComponent<PlayerAttack>();
        for (int i = 0; i < slotCount; i++)
        {
            Slot slot = Instantiate(slotPrefab, inventoryPanel.transform).GetComponent<Slot>();
            slot.inventoryIndex = i;

            if (i < itemPrefabs.Length)
            {
                GameObject item = Instantiate(itemPrefabs[i], slot.transform);
                item.gameObject.SetActive(true);
                item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                item.transform.localScale = new Vector3(1, 1, 1);
                item.gameObject.SetActive(true);
                slot.currentItem = item;
                playerAttack.AddItem(itemPrefabs[i]);
            }
        }

        SelectSlot(0);
        healthBar = GameObject.Find("PlayerHealthBar").GetComponent<HealthBar>();
        audioSource = GameObject.FindWithTag("WorldAudio").GetComponent<AudioSource>();
    }

    public bool AddItem(GameObject itemPrefab)
    {
        Slot itemSlot = null;
        bool foundEmptySlot = false;

        foreach(Transform slotTransform in inventoryPanel.transform)
        {
            Debug.Log("Checking slot");
            Slot slot = slotTransform.GetComponent<Slot>();
            
            if(slot != null && slot.currentItem != null && itemPrefab.GetComponent<Item>().itemName == slot.currentItem.GetComponent<Item>().itemName 
            && slot.currentItem.GetComponent<Item>().isStackable)
            {
                slot.currentItem.GetComponent<Item>().itemQuantity++;
                Debug.Log("New Quantity for " + slot.currentItem.GetComponent<Item>().itemName + " : " + slot.currentItem.GetComponent<Item>().itemQuantity);
                return true;
            }

            if(slot != null && slot.currentItem == null)
            {
                if(foundEmptySlot == false)
                {
                    itemSlot = slot;
                    foundEmptySlot = true;
                }
            }
        }

        if(foundEmptySlot == true && itemSlot != null)
        {
            Debug.Log("Slot is empty");
            playerAttack.AddItem(itemPrefab);
            GameObject newItem = Instantiate(itemPrefab, itemSlot.transform);
            newItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            newItem.transform.localScale = new Vector3(1, 1, 1);
            itemSlot.currentItem = newItem;
            return true;
        }
        
        Debug.Log("Inventory is full");

        return false;
    }

    public void SelectSlot(int index)
    {
        //Hides currently selected item border and changes it to the new one
        foreach(Transform slotTransform in inventoryPanel.transform)
        {
            Slot slot = slotTransform.GetComponent<Slot>();
            if(slot != null && slot.backgroundImage != null)
            {
                slot.backgroundImage.color = defaultBackgroundColor;
            }
            if(slot != null && slot.borderFrame != null)
            {
                slot.borderFrame.SetActive(false);
            }
        }

        //Enables the border of the selected slot
        Slot selectedSlot = inventoryPanel.transform.GetChild(index).GetComponent<Slot>();

        if (selectedSlot != null && selectedSlot.backgroundImage != null)
        {
            selectedSlot.backgroundImage.color = selectedBackgroundColor;
        }
        
        if(selectedSlot != null && selectedSlot.borderFrame != null)
        {
            selectedSlot.borderFrame.SetActive(true);
        }

        Debug.Log("Selecting slot " + index);
        playerAttack.SwitchWeaponAtIndex(index);
    }

    public void UseItem(int currentIndex)
    {
        Slot selectedSlot = inventoryPanel.transform.GetChild(currentIndex).GetComponent<Slot>();
        
        if(selectedSlot != null && selectedSlot.currentItem != null && healthBar.currentHealth > 0)
        {
            Item item = selectedSlot.currentItem.GetComponent<Item>();

            if(item.itemType == ItemType.Consumable)
            {
                Debug.Log("Using consumable in slot " + currentIndex);

                if(healthBar != null)
                {
                    healthBar.Heal(item.healthValue);
                    Debug.Log("Healing player for " + item.healthValue + " through an item");
                }

                //Will decrement the amount of the item used if there are multiple
                //Otherwise, it destroys the item in the hotbar
                if(item.isStackable == true && item.itemQuantity > 1)
                {
                    item.itemQuantity--;
                }
                else
                {
                    Destroy(selectedSlot.currentItem);
                    selectedSlot.currentItem = null;
                }

                if(item.useSound != null && audioSource != null)
                {
                    audioSource.PlayOneShot(item.useSound);
                }
            }

            if(item.itemType == ItemType.Weapon)
            {
                Debug.Log("This is where we'd swing/fire the weapon");

                if(item.useSound != null && audioSource != null)
                {
                    audioSource.PlayOneShot(item.useSound);
                }
            }

            if(item.itemType == ItemType.Material)
            {
                Debug.Log("Material clicked. Need to do something with it");
                
                if(item.useSound != null && audioSource != null)
                {
                    audioSource.PlayOneShot(item.useSound);
                }
            }

            if(item.itemType == ItemType.Trap)
            {
                Debug.Log("Trap clicked. Place trap maybe?");
                
                if(item.useSound != null && audioSource != null)
                {
                    audioSource.PlayOneShot(item.useSound);
                }
            }
        }

        if (selectedSlot.currentItem == null) {
            playerAttack.RemoveItem(currentIndex);
        }
    }

    public void ClearInventory() {

       foreach(Transform slotTransform in inventoryPanel.transform)
        {
            Slot slot = slotTransform.GetComponent<Slot>();
            Destroy(slot.currentItem);
            slot.currentItem = null;
        }
    }

    public void UseArrow() {
        foreach(Transform slotTransform in inventoryPanel.transform)
        {
            Slot slot = slotTransform.GetComponent<Slot>();
            
            if(slot != null && slot.currentItem != null && slot.currentItem.GetComponent<Item>().itemName == "Arrow")
            {
                Item item = slot.currentItem.GetComponent<Item>();
                if(item.isStackable == true && item.itemQuantity > 1)
                {
                    item.itemQuantity--;
                }
                else
                {
                    Destroy(slot.currentItem);
                    slot.currentItem = null;
                }
                break;
            }
        }
    }
}
