using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileWeaponItem : Item
{
	[Header("Projectile Properties")]
	public GameObject Projectile;
	public Transform ProjectileSpawn;
	public bool WeaponOnLeft = false;
	public bool ShootTowardsMouse = false;
	public float Force = 100f;
	public float Duration = 10f;
	[Tooltip("The starting velocity that a projectile might have")]
	public Vector2 InitialProjectileVelocity;
	public bool Gravity = true;

	private Vector2 mousePosition;
	private float angle;

	private void FixedUpdate()
	{
		if (ShootTowardsMouse)
			mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
	}

	public override void WeaponStart(Transform wielderPosition, Vector2 lastLookDirection, Vector2 currentVelocity)
	{
		Debug.Log("Shoot!");
		bool shootingRight = mousePosition.x - ProjectileSpawn.position.x > 0;
		bool shootingUp = mousePosition.y - ProjectileSpawn.position.y > 0;

		base.WeaponStart(wielderPosition, lastLookDirection);
		Vector3 offset = new Vector3(0, 0);
		Quaternion flip = new Quaternion(0, 0, -1, 0);
		GameObject bullet = Instantiate(Projectile, ProjectileSpawn.position + offset, Quaternion.identity * flip);
		bullet.GetComponent<ProjectileItem>().SetValues(Duration, alignmnent, healthValue);
		Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>(); ;

		

		Vector2 lookDir = mousePosition - rb.position;
		float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
		rb.rotation = angle;

		if (!Gravity)
			rb.gravityScale = 0;


		if (ShootTowardsMouse)
			rb.AddForce((new Vector3(currentVelocity.x, currentVelocity.y, 0f)) + (Vector3.Normalize(new Vector3(mousePosition.x, mousePosition.y, 0f) - transform.position)) * Force, ForceMode2D.Impulse);
		else
			rb.AddForce((InitialProjectileVelocity * lastLookDirection) + currentVelocity + lastLookDirection * Force, ForceMode2D.Impulse);
		
	}
}
