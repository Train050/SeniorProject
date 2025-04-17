﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseBehaviour : StateMachineBehaviour
{
    private Transform playerPos;
    public float speed;
    private Animator anim;
    [Tooltip("To use Blend Tree it needs the following parameters: float \"distance\", float \"Horizontal\", float \"Vertical\", bool \"SpriteFacingRight\" ")]
    public bool useBlendTree = false;

    private bool turnedAround = false;

    private float speedDiff;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state

    void OnEnable()
    {
         speedDiff = Random.Range(0, 0.5f);
    }
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").transform; // load in the player's position
        anim = animator;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Transform enemyPos = animator.transform;
        float step = (speed + speedDiff) * Time.deltaTime;
        
        if(useBlendTree)
        {
            Vector3 randomizer = new Vector3(Random.Range(-5, 5), Random.Range(-5, 5));
            Vector3 blendTreePos = (playerPos.position + randomizer - enemyPos.transform.position);
            anim.SetFloat("Horizontal", blendTreePos.x);
            anim.SetFloat("Vertical", blendTreePos.y);

            FlipCheck(blendTreePos.x);
        }

        if (playerPos.position.x < enemyPos.transform.position.x && !turnedAround) {
            Vector3 currentScale = anim.transform.localScale;
            currentScale.x *= -1;
            anim.transform.localScale = currentScale;
            turnedAround = true;
        } 
        else if (playerPos.position.x > enemyPos.transform.position.x && turnedAround)
        {
            Vector3 currentScale = anim.transform.localScale;
            currentScale.x *= -1;
            anim.transform.localScale = currentScale;
            turnedAround = false;
        }
        



        enemyPos.position = Vector2.MoveTowards(enemyPos.transform.position, playerPos.position, step); // move towards the player
    }

    private void OnTriggerStay2D(Collider2D other) 
    {
        Vector2 lineToObject = (anim.transform.position - other.transform.position).normalized;
        anim.transform.position = Vector2.MoveTowards(anim.transform.position, -1 * lineToObject, speed * Time.deltaTime); // don't get too stuck on objects (barely noticeable)
    }

    // Flip Check & Flip code from Player Movement Script
    private void FlipCheck(float move)
	{
		//Flip the sprite so that they are facing the correct way when moving
		if (move > 0 && !anim.GetBool("SpriteFacingRight")) //if moving to the right and the sprite is not facing the right.
		{
			Flip();
		}
		else if (move < 0 && anim.GetBool("SpriteFacingRight")) //if moving to the left and the sprite is facing right
		{
			Flip();
		}
	}

	private void Flip()
	{
		anim.SetBool("SpriteFacingRight", !anim.GetBool("SpriteFacingRight")); //flip whether the sprite is facing right
		Vector3 currentScale = anim.transform.localScale;
		currentScale.x *= -1;
		anim.transform.localScale = currentScale;
	}
}
