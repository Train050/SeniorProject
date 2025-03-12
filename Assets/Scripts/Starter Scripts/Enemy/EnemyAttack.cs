using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{

	[Header("Enemy Weapon")]
	[Tooltip("This is the current weapon that the enemy is using")]
	public Item weapon;

	[Header("Parameters")]

	public bool showChaseRadius = false;
	public bool showAttackRadius = false;
	public float chaseRadius = 3f;
	public float attackRadius = 2f;
	[Tooltip("The coolDown before you can attack again")]
	public float coolDown = 0.5f;
	private bool canAttack = true;
	private bool isAttacking = false;

	private Animator anim;

	private GameObject healthBar;

    private void Start()
    {
		healthBar = GameObject.Find("PlayerHealthBar");
    }

    private void Update()
	{
		anim = GetComponent<Animator>();

		if (anim.GetBool("isDead")) // Jane Apostol Fall '23
		{
			canAttack = false;
		}

		bool shouldAttack = false;
		Collider2D[] attackColliders = Physics2D.OverlapCircleAll(transform.position, attackRadius);
		foreach (Collider2D other in attackColliders)
		{
			shouldAttack = shouldAttack || canAttack && other.CompareTag("Player");

			if (shouldAttack)
				Attack(other.transform.position - this.transform.position);
		}


		bool shouldChase = false;
		Collider2D[] chaseColliders = Physics2D.OverlapCircleAll(transform.position, chaseRadius);
		foreach (Collider2D other in chaseColliders)
		{
			shouldChase = shouldChase || other.CompareTag("Player");
		}

		anim.SetBool("isChasing", shouldChase && !shouldAttack);

	}

	public void Attack(Vector2 attackDir)
	{
		isAttacking = true;
		anim.SetBool("isAttacking", isAttacking);
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
		isAttacking = false;
		anim.SetBool("isAttacking", false);
		canAttack = true;
	}

	private void OnDrawGizmos()
	{
		if (showAttackRadius)
			Gizmos.DrawWireSphere(transform.position, attackRadius);
		if (showChaseRadius)
			Gizmos.DrawWireSphere(transform.position, chaseRadius);
	}
}
