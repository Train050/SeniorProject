using UnityEngine;

public class HotbarItemSelection : MonoBehaviour
{
    private HotbarController hotbarController;
    private PauseMenu pauseMenu; 
    public GameObject hotbar;
    public int currentIndex = 0;

    void Start()
    {
        hotbarController = FindObjectOfType<HotbarController>();
        pauseMenu = FindObjectOfType<PauseMenu>();
    }

    void Update()
    {
        if (!pauseMenu.getOpen())
        {
            if(Input.GetKeyDown(KeyCode.Alpha0))
            {
                currentIndex = 9;
                hotbarController.SelectSlot(currentIndex);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                currentIndex = 0;
                hotbarController.SelectSlot(currentIndex);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                currentIndex = 1;
                hotbarController.SelectSlot(currentIndex);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                currentIndex = 2;
                hotbarController.SelectSlot(currentIndex);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                currentIndex = 3;
                hotbarController.SelectSlot(currentIndex);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                currentIndex = 4;
                hotbarController.SelectSlot(currentIndex);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                currentIndex = 5;
                hotbarController.SelectSlot(currentIndex);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                currentIndex = 6;
                hotbarController.SelectSlot(currentIndex);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha8))
            {
                currentIndex = 7;
                hotbarController.SelectSlot(currentIndex);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha9))
            {
                currentIndex = 8;
                hotbarController.SelectSlot(currentIndex);
            }

            //Scroll wheel up
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                currentIndex++;

                if (currentIndex > 9)
                {
                    currentIndex = 0;
                }
                hotbarController.SelectSlot(currentIndex);
            }
            //Scroll wheel down
            else if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                currentIndex--;

                if (currentIndex < 0)
                {
                    currentIndex = 9;
                }
                hotbarController.SelectSlot(currentIndex);
            }

            //Left click outside of the hotbar will consume the item
            //TODO: Check to see if the item is consumable
            if (Input.GetMouseButtonDown(0) && !MouseOverHotbar(Input.mousePosition))
            {
                hotbarController.UseItem(currentIndex);
            }
        }
    }

    //Checks if the mouse is over the hotbar
    bool MouseOverHotbar(Vector2 mousePosition)
    {
        RectTransform hotbarRect = hotbar.GetComponent<RectTransform>();

        return RectTransformUtility.RectangleContainsScreenPoint(hotbarRect, mousePosition);
    }
}