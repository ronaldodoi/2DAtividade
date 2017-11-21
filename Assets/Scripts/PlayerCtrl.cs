﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour {

	public float horizontalSpeed = 10f;
	public float jumpSpeed = 600f;

	Animator anim;
	SpriteRenderer sr;
	Rigidbody2D rb;
	bool isJumping = false;


	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D>();
		sr = GetComponent<SpriteRenderer>();
		anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {

		float horizontalInput = Input.GetAxisRaw("Horizontal"); // -1: esquerda, 1: direita
		float horizontalPlayerSpeed = horizontalSpeed * horizontalInput;
		if (horizontalPlayerSpeed != 0){
			MoveHorizontal(horizontalPlayerSpeed);
		}
		else	{
		StopMovingHorizontal();
		}
	
		if(Input.GetButtonDown("Jump")){
			Jump();
		}
	
		ShowFalling();
	}

	void MoveHorizontal(float speed) {
		rb.velocity = new Vector2(speed,rb.velocity.y);
		if (speed < 0f) {
			sr.flipX = true;
		}		
		else if (speed > 0f){
			sr.flipX = false;
		} 
		if (!isJumping){
			anim.SetInteger("State", 2);
		}
		anim.SetInteger("State", 2);
	}

	void StopMovingHorizontal(){
		rb.velocity = new Vector2(0f, rb.velocity.y);
		if (!isJumping){	
			anim.SetInteger("State", 0);
		}
	}

	void ShowFalling() {
		if (rb.velocity.y < 0f){
			anim.SetInteger("State", 3);
		}	
	}

	void Jump(){
		isJumping = true;
		rb.AddForce(new Vector2(0f, jumpSpeed));
		anim.SetInteger("State", 1);

	}

	void OnCollisionEnter2D(Collision2D other){
		if (other.gameObject.layer == LayerMask.NameToLayer("Ground")){
			isJumping = false;
		}
	}

}

