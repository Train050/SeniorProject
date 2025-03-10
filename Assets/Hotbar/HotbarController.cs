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
    private GameObject healthBar;

    private PlayerAttack playerAttack;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log(itemPrefabs);
        playerAttack = player.GetComponent<PlayerAttack>();
        for (int i = 0; i < slotCount; i++)
        {
            Slot slot = Instantiate(slotPrefab, inventoryPanel.transform).GetComponent<Slot>();

            if (i < itemPrefabs.Length)
            {
                GameObject item = Instantiate(itemPrefabs[i], slot.transform);
                item.gameObject.SetActive(true);
                item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                item.transform.localScale = new Vector3(1, 1, 1);
                item.gameObject.SetActive(true);
                slot.currentItem = item;
                playerAttack.appendItem(itemPrefabs[i]);
            }
        }

        // SelectSlot(0);
        healthBar = GameObject.Find("PlayerHealthBar");
    }

    public bool AddItem(GameObject itemPrefab)
    {
        foreach(Transform slotTransform in inventoryPanel.transform)
        {
            Debug.Log("Checking slot");
            Slot slot = slotTransform.GetComponent<Slot>();
            if(slot != null && slot.currentItem == null)
            {
                Debug.Log("Slot is empty");
                GameObject newItem = Instantiate(itemPrefab, slot.transform);
                newItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                newItem.transform.localScale = new Vector3(1, 1, 1);
                slot.currentItem = newItem;
                return true;
            }
        }
        Debug.Log("Inventory is full");

        playerAttack.appendItem(itemPrefab);
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
        playerAttack.switchWeaponAtIndex(index);
    }

    public void UseItem(int currentIndex)
    {
        Slot selectedSlot = inventoryPanel.transform.GetChild(currentIndex).GetComponent<Slot>();
        
        if(selectedSlot != null && selectedSlot.currentItem != null)
        {
            Item item = selectedSlot.currentItem.GetComponent<Item>();

            if(item.itemType == ItemType.Consumable)
            {
                Debug.Log("Using consumable in slot " + currentIndex);

                if(healthBar != null)
                {
                    healthBar.GetComponent<HealthBar>().Heal(item.healthValue);
                    Debug.Log("Healing player for " + item.healthValue + " through an item");
                }

                Destroy(selectedSlot.currentItem);
                selectedSlot.currentItem = null;
            }

            if(item.itemType == ItemType.Weapon)
            {
                Debug.Log("This is where we'd swing/fire the weapon");
            }

            if(item.itemType == ItemType.Material)
            {
                Debug.Log("Material clicked. Need to do something with it");
            }

            if(item.itemType == ItemType.Trap)
            {
                Debug.Log("Trap clicked. Place trap maybe?");
            }
        }

        if (selectedSlot.currentItem == null) {
            playerAttack.removeItem(currentIndex);
        }
    }
}
