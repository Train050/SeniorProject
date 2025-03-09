using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileItem : Item
{
	float duration;
	Item.Alignment alignment;

	private void Start()
	{
		itemType = ItemType.Weapon;
		StartCoroutine(Duration());
	}

	public ProjectileItem(float duration, Item.Alignment alignment, int healthValue)
	{
		itemType = ItemType.Weapon;
		this.duration = duration;
		this.alignment = alignment;
		this.healthValue = healthValue;
	}

	public void SetValues(float duration, Item.Alignment alignment, int healthValue)
	{
		this.duration = duration;
		this.alignment = alignment;
		this.healthValue = healthValue;

		GetComponent<Rigidbody2D>().gravityScale = 0;
	}

	IEnumerator Duration()
	{
		yield return new WaitForSeconds(duration);
		Destroy(gameObject);
	}

}
