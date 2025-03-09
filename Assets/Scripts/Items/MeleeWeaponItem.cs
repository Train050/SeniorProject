using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeaponItem : Item
{
	[Header("Melee Properties")]
	public Animator MeleeWeaponAnimator;
	private Collider2D col; //Collider that deals the damage
	
	
	private void Start()
	{
		itemType = ItemType.Weapon;
		col = GetComponent<Collider2D>();
		MeleeWeaponAnimator = GetComponent<Animator>();
	}

	public override void WeaponStart(Transform wielderPosition, Vector2 lastLookDirection)
	{
		base.WeaponStart(wielderPosition, lastLookDirection);

		if (MeleeWeaponAnimator)
		{
			MeleeWeaponAnimator.SetFloat("Horizontal", lastLookDirection.x);
			MeleeWeaponAnimator.SetFloat("Vertical", lastLookDirection.y);


			MeleeWeaponAnimator.SetTrigger("Attack_TopDown");

		}

		col.enabled = true;
	}

	public override void WeaponFinished()
	{
		col.enabled = false;
	}
}
