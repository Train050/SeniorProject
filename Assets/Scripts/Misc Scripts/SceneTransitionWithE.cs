using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneTransitionWithE : MonoBehaviour
{
    public string sceneName;
    public GameObject ePrompt;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ePrompt.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collision detected");
        if (collision.gameObject.tag == "Player")
        {
            //Will change this to space bar later, when we have a sprite for it
            //This will probably be a canvas overlay or just a floating text saying press space to return
            ePrompt.gameObject.SetActive(true);
            Debug.Log("Player in area to return to parking lot");
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //Will change this to space bar later, when we have a sprite for it
            if (Input.GetKeyDown(KeyCode.E))
            {
                SceneManager.LoadScene(sceneName);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            ePrompt.gameObject.SetActive(false);
            Debug.Log("Player left the return to parking lot area");
        }
    }
}
