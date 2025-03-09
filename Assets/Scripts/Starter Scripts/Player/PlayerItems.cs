using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItem : MonoBehaviour
{
	[Header("References")]

	[Tooltip("There are separate animators for the weapon. If you have a player attack animation from a sprite sheet, make this true. If your attack animation is the weapon itself moving (separate from the player) this can be false.")]
	public bool UsePlayerAttackAnimations = false;
	public Animator anim;
	public Rigidbody2D rb;
	public PlayerMovement playerMoveScript;

	[Header("Player Weapons")]
	[Tooltip("This is the list of all the weapons that your player uses")]
	public List<Item> itemList;
	[Tooltip("This is the current weapon that the player is using")]
	public Item item;
	[Tooltip("The coolDown before you can attack again")]
	public float coolDown = 0.4f;

	[Header("Audio")]
	public PlayerAudio playerAudio;

	private bool canAttack = true;


	private void Start()
	{
		anim = GetComponent<Animator>();
		rb = GetComponent<Rigidbody2D>();
		playerMoveScript = GetComponent<PlayerMovement>();
		playerAudio = GetComponent<PlayerAudio>();
		if (item == null && itemList.Count > 0)
		{
			item = itemList[0];
		}
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Alpha1))//Here is where you can hit the "1" key on your keyboard to activate this weapon
		{
			if (itemList.Count > 0)
			{
				switchItemAtIndex(0);
			}
		}
		else if (Input.GetKeyDown(KeyCode.Alpha2))//Remove this if you don't have multiple weapons
		{
			if (itemList.Count > 1)
			{
				switchItemAtIndex(1);
			}
		}

		if (Input.GetKey(KeyCode.Mouse0))
		{
			if (ItemType.Weapon.Equals(item.itemType)) {
				Attack();
				if (playerAudio && !playerAudio.AttackSource.isPlaying && playerAudio.AttackSource.clip != null)
				{
					playerAudio.AttackSource.Play();
				}
			}
		}
		else
		{
			StopAttack();
		}
	}

	public void Attack()
	{
		//This is where the weapon is rotated in the right direction that you are facing
		if (item && canAttack)
		{
			Debug.Log("Kablammo!");
			if (UsePlayerAttackAnimations)
            {
				Debug.Log("using player attack animation");
				playerMoveScript.TriggerPlayerAttackAnimation();

			}

			if (item is ProjectileWeapon)
				item.WeaponStart(this.transform, playerMoveScript.GetLastLookDirection(), rb.linearVelocity);
			else
				item.WeaponStart(this.transform, playerMoveScript.GetLastLookDirection());

			StartCoroutine(CoolDown());
		}
	}

	public void StopAttack()
	{
		if (item)
		{
			item.WeaponFinished();
		}
	}

	public void switchItemAtIndex(int index)
	{
		//Makes sure that if the index exists, then a switch will occur
		if (index < itemList.Count && itemList[index])
		{
			//Deactivate current weapon
			item.gameObject.SetActive(false);

			//Switch weapon to index then activate
			item = itemList[index];
			item.gameObject.SetActive(true);
		}

	}

	private IEnumerator CoolDown()
	{
		canAttack = false;
		yield return new WaitForSeconds(coolDown);
		canAttack = true;
	}
}
