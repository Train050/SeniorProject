using UnityEngine;

public class BearTrapHit : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    Animator animator;
    GameObject healthBar;
    public GameObject parentTrap;
    public HealthBar playerHealth;
    Item item;
    
    void Start()
    {
        item = parentTrap.GetComponent<Item>();
        animator = parentTrap.GetComponent<Animator>();
    }

    // Update is called once per frame
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && animator.GetBool("isClosed") == false)
        {
            Debug.Log("Player hit the trap!");
            animator.SetBool("isClosed", true);

            if(parentTrap != null)
            {
                Debug.Log("Parent trap does " + item.damageAmoumt + " damage");
                playerHealth.GetComponent<HealthBar>().TakeDamage(item.damageAmoumt);
            }
        }
        else if (collision.gameObject.tag == "Enemy" && animator.GetBool("isClosed") == false)
        {
            animator.SetBool("isClosed", true);

            //This is where you would damage the enemy (once that system is added)
        }
    }
    void Update()
    {

    }
}
