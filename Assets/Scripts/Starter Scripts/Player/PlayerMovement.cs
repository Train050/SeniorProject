﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
	//This Script Should be placed on the parent object of your player IE the object named "Player"

	/*
    PUBLIC VARIABLES
    */

	[Header("Movement")]
	[Tooltip("If this is false, then this controller acts as a sidescroller. If it is true, it is a top down, make sure to make gravity 0 if this is true.")]
	public bool TopDownMovement = true;
	[Tooltip("This is whether or not the player can actually move")]
	public bool disabled = false;
	[Tooltip("The speed at which the player moves")]
	public float Speed = 10f;

	[Tooltip("(Only Matters in Side Scroller) Acceleration of the player. Smaller number = faster")]
	public float Acceleration = .05f;

	[Tooltip("(Only Matters in Top Down) Limits the speed when moving diagonally.")]
	public float DiagonalSpeedLimiter = 0.7f;

	[Tooltip("This means that you're 1D character is facing left by default (1D means you only face left or right)")]
	public bool SpriteFacingRight = false;

	//=========================================================================

	[Header("Jump -Only matters in Side Scroller-")]
	[Tooltip("Controls whether your player can jump or not.")]
	public bool canJump = false;
	[Tooltip("The force of your jump (Be sure to have your gravity set to 1 for side-scroller)")]
	public float JumpForce = 10f;
	[Tooltip("Number of jumps your player can do each time they touch the ground. (2 = Double jump)")]
	public int NumberOfJumps = 1;

	[Tooltip("The multiplier at which you fall down (used for smooth movement) and it can't be below 1")]
	public float FallMultiplier = 3f;

	//=========================================================================

	[Header("Raycast Jumping  -Only matters in Side Scroller-")]

	[Tooltip("Will show a red ray drawn from center of your sprite, it should extend from your box collider to touch the ground. If it doesn't reach the ground, change rayLength until it does. If you cannot see it, click the Gizmos button in the top right of the Game Window.")]
	public bool ShowDebugRaycast = false;
	[Tooltip("Select your ground layer so that the raycast can detect it")]
	public LayerMask groundLayer;
	[Tooltip("The length of the ray used to detect the ground.")]
	public float rayLength = 1;

	[Header("Audio")]
	public PlayerAudio playerAudio;

	/*
    PRIVATE VARIABLES
    */
	private Rigidbody2D rb;
	private Collider2D col;
	private SpriteRenderer rend;
	private Animator anim;
	private float HorizontalMovement;
	private float VerticalMovement;
	private Vector2 lastLookDirection;
	private bool isGrounded = false;
	private Vector3 currentVelocity = Vector3.zero;
	private int jumpsLeft; //how many jumps until the player can't jump anymore? reset when grounded.

	// Start is called before the first frame update
	void Start()
	{
		rb = GetComponent<Rigidbody2D>(); //Find the Rigidbody component on the gameobject this script is attached to.
		col = GetComponent<Collider2D>(); //Get Collider component
		rend = GetComponent<SpriteRenderer>(); //Get Sprite Renderer Component
		anim = GetComponent<Animator>(); //Get Animator Component
		playerAudio = GetComponent<PlayerAudio>();

		// Set IsNight based on scene name
		string currentScene = SceneManager.GetActiveScene().name;
		bool isNight = currentScene.Contains("Night");
		Debug.Log("isNight: " + isNight);
		anim.SetBool("isNight", isNight);
	}

	public void DisablePlayer(bool isDisabled)
	{
		disabled = isDisabled;
		if (disabled)
		{
			rb.linearVelocity = Vector2.zero;
			anim.SetBool("isMoving", false);
		}
	}

	// Update is called once per frame
	void Update()
	{
		//Debug.Log("Disabled: " + disabled); 
		//Debug.Log("(" + HorizontalMovement + ", " + VerticalMovement + ")");
		if (ShowDebugRaycast)
			Debug.DrawRay(col.bounds.center, Vector2.down * rayLength, Color.red); //draws a ray showing ray length

		if (!disabled) //If player movement is NOT disabled
		{
			//Get horizontal and vertical input. See Project Settings > Input Manager
			HorizontalMovement = Input.GetAxisRaw("Horizontal");
			VerticalMovement = Input.GetAxisRaw("Vertical");

			if (!TopDownMovement) //if the movement is not top down movement
			{
				VerticalMovement = Input.GetAxisRaw("Jump");
				anim.SetFloat("MoveVertical", VerticalMovement);

				//check if the player is grounded
				isGrounded = Physics2D.BoxCast(col.bounds.center, col.bounds.size, 0.0f, Vector2.down, rayLength, groundLayer);

				//if they are grounded, reset their jumps.
				if (isGrounded && VerticalMovement == 0) // Jane Apostol 10/14/23
				{
					jumpsLeft = NumberOfJumps;
					anim.SetBool("isJumping", false);

				}

				if (canJump && Input.GetButtonDown("Jump") && isGrounded && VerticalMovement != 0) //If the player jumps and is grounded
				{
					Jump();
				}
				else if (canJump && Input.GetButtonDown("Jump") && VerticalMovement != 0 && NumberOfJumps > 1 && jumpsLeft > 1) //Or if the player has jumps left
				{

					Jump();
					jumpsLeft--;
				}
			}
			//Animation
			// Lines 134-169 Jane Apostol 10/12/23
			if (!TopDownMovement) // when the movement is sidescroller
			{
				if (HorizontalMovement != 0)
				{
					anim.SetBool("isMoving", true);
					if (playerAudio && !playerAudio.WalkSource.isPlaying && playerAudio.WalkSource.clip != null)
					{
						playerAudio.WalkSource.Play();
					}
				}
				else
				{
					anim.SetBool("isMoving", false);
					if (playerAudio && playerAudio.WalkSource.isPlaying && playerAudio.WalkSource.clip != null)
					{
						playerAudio.WalkSource.Stop();
					}
				}
			}
			else // when the movement is topdown
			{
				anim.SetFloat("MoveHorizontal", HorizontalMovement);

				if (HorizontalMovement != 0 || VerticalMovement != 0)
				{
					anim.SetBool("isMoving", true);
					if (playerAudio && !playerAudio.WalkSource.isPlaying && playerAudio.WalkSource.clip != null)
					{
						playerAudio.WalkSource.Play();
					}
				}
				else
				{
					anim.SetBool("isMoving", false);
					if (playerAudio && playerAudio.WalkSource.isPlaying && playerAudio.WalkSource.clip != null)
					{
						playerAudio.WalkSource.Stop();
					}
				}
			}

		} else {
			anim.SetBool("isMoving", false);
			if (playerAudio && playerAudio.WalkSource.isPlaying && playerAudio.WalkSource.clip != null)
			{
				playerAudio.WalkSource.Stop();
			}
		}

		if (Input.GetKeyDown(KeyCode.D)) {
			FlipItems(true);
		} else if (Input.GetKeyDown(KeyCode.A)) {
			FlipItems(false);

		}

	}

	private void FixedUpdate()
	{
		//Debug.Log("Velocity: " + rb.velocity);
		if (!disabled)
		{
			if (!TopDownMovement) //If the game isn't topdown
				MoveSideScroller(HorizontalMovement); //move like a sidescroller
			else
				MoveTopDown(HorizontalMovement, VerticalMovement); //otherwise move like a topdown
		}
	}

	private void MoveTopDown(float Horizontal, float Vertical)
	{
		anim.SetFloat("MoveVertical", Vertical); //Tell the animator if the player is moving vertically.

		if (Horizontal != 0 && Vertical != 0) //If the player is moving diagonally
		{
			//Limit the horizontal and vertical speed
			//(If we don't do this moving diagonal is 2x faster than moving normally)
			Horizontal *= DiagonalSpeedLimiter;
			Vertical *= DiagonalSpeedLimiter;

		}
		else if (Horizontal != 0 || Vertical != 0)
		{
			lastLookDirection = new Vector2(Horizontal, Vertical);
		}

		rb.linearVelocity = new Vector3(Horizontal * Speed, Vertical * Speed); //Set velocity to move gameobject.


		FlipCheck(Horizontal);
	}

	private void MoveSideScroller(float move)
	{
		if (move != 0)
		{
			lastLookDirection = new Vector2(move, 0);
		}

		Vector3 targetVelocity = new Vector3(move * Speed, rb.linearVelocity.y); //Make target velocity how we want to move.

		rb.linearVelocity = Vector3.SmoothDamp(rb.linearVelocity, targetVelocity, ref currentVelocity, Acceleration); //Use smooth damp to simulate acceleration.

		//If your sprite has an idle where they are facing to the side, then you may need to uncomment this :)
		FlipCheck(move);

		if (!isGrounded) //if the player is in the air
		{

			if (rb.linearVelocity.y < 0) //if player is falling
			{
				//Make gravity harsher so they fall faster.
				rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (FallMultiplier - 1) * Time.deltaTime;
			}
			else if (rb.linearVelocity.y > 0 && !Input.GetButton("Jump")) //if player is jumping and holding jump button
			{
				//Make gravity less so they jump higher. Creates variable jump heights.
				rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (FallMultiplier - 1.5f) * Time.deltaTime;
			}

		}
	}

	private void Jump()
	{
		rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0); //Stop any previous vertical movement
		rb.AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse); //Add force upwards as an impulse.
		anim.SetBool("isJumping", true); // Jane Apostol 10/12/23

		if (playerAudio && playerAudio != null)
		{
			playerAudio.JumpSource.Play();
		}
	}

	private void FlipCheck(float move)
	{
		//Flip the sprite so that they are facing the correct way when moving
		if (move > 0 && !SpriteFacingRight) //if moving to the right and the sprite is not facing the right.
		{
			Flip();
		}
		else if (move < 0 && SpriteFacingRight) //if moving to the left and the sprite is facing right
		{
			Flip();
		}
		else if (move == 0 && !SpriteFacingRight) {
			Flip();
		}
	}

	private void Flip()
	{
		SpriteFacingRight = !SpriteFacingRight;
	}

	private void FlipItems(bool facingRight)
	{
		foreach (Transform child in transform) // 'transform' refers to the Player's transform
		{
			// Check if the child has a RectTransform (UI object)
			RectTransform itemRect = child.GetComponent<RectTransform>();
			Item item = child.GetComponent<Item>();

			if (itemRect != null)
			{
				Vector3 scale = itemRect.localScale;
				Vector3 pos = itemRect.localPosition;

				// Flip the item on the X-axis (left-right)
				if (facingRight) {
					scale.x = Math.Abs(scale.x);
					pos.x =  item.positionInHand.x;
				} else 
				{
					scale.x = -1 * Math.Abs(scale.x);
					pos.x = item.negPositionInHand.x;;
				}

				// Apply the flipped scale to each item
				itemRect.localScale = scale;
				itemRect.localPosition = pos;
			}
		}
	}

	public Vector2 GetLastLookDirection()
	{
		return lastLookDirection;
	}

	public void TriggerPlayerAttackAnimation()
	{
		anim.SetTrigger("isAttacking");
	}
	/*
	 * If animation breaks when persistent player is created, try this
	void OnEnable()
	{
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	void OnDisable()
	{
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}

	void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		bool isNight = scene.name.Contains("Night");
		anim.SetBool("isNight", isNight);
	} */

	//https://discussions.unity.com/t/how-to-use-scenemanager-onsceneloaded/624895/6
	void OnEnable()
	{
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	void OnDisable()
	{
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}

	void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		GameObject spawnPoint = GameObject.FindGameObjectWithTag("SpawnPoint");
		if(spawnPoint != null)
		{
			transform.position = spawnPoint.transform.position;	
		}
		else
		{
			Debug.Log("No spawn point found");
		}
		
	}
}
