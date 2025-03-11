using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{

	[Header("Enemy Weapon")]
	[Tooltip("This is the current weapon that the enemy is using")]
	public Item weapon;

	[Header("Parameters")]

	public bool showAttackRadius = false;
	public float attackRadius = 5f;
	[Tooltip("The coolDown before you can attack again")]
	public float coolDown = 0.5f;
	private bool canAttack = true;

	private Animator anim;

	private GameObject healthBar;

    private void Start()
    {
		healthBar = GameObject.Find("PlayerHealthBar");
    }

    private void Update()
	{
		anim = GetComponent<Animator>();
		Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, attackRadius);
		foreach (Collider2D other in colliders)
		{
			Debug.Log(other);
			if (anim.GetBool("isDead")) // Jane Apostol Fall '23
            {
				canAttack = false;
            }

			if (canAttack && other.CompareTag("Player"))
			{
				Debug.Log("Attack Player");
				Attack(other.transform.position - this.transform.position);
			}
		}
	}

	public void Attack(Vector2 attackDir)
	{
		anim.SetBool("isAttacking", true);
		//This is where the weapon is rotated in the right direction that you are facing
		if (weapon && canAttack)
		{
			Debug.Log("Attacking Player!");

			if(healthBar != null)
			{
				healthBar.GetComponent<HealthBar>().TakeDamage(weapon.healthValue);
			}
			// anim.SetBool("isAttacking", false);
			StartCoroutine(CoolDown());
		}
	}

	public void StopAttack()
	{
		if (weapon)
		{
			weapon.WeaponFinished();
		}
	}

	private IEnumerator CoolDown()
	{
		canAttack = false;
		// anim.SetBool("isChasing", false);
		yield return new WaitForSeconds(coolDown);
		anim.SetBool("isAttacking", false);
		canAttack = true;
	}

	private void OnDrawGizmos()
	{
		if (showAttackRadius)
			Gizmos.DrawWireSphere(transform.position, attackRadius);
	}
}
