using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
0 - idle
1 - run
2 - jump
3 - fall
4 - land
5 - attack
6 - dead
 */


public class PlayerCtrl : MonoBehaviour {

	public float horizontalSpeed = 10f;
	public float jumpSpeed = 600f;

	Rigidbody2D rb;

	SpriteRenderer sr;

	Animator anao;

	bool isJumping = false;

	public Transform Feet;
	public float feetW = 0.5f;
	public float feetH = 0.1f;

	public bool isGrounded;
	public LayerMask whatIsGround;

	bool canDoubleJump = false;
	public float delayForDJ = 0.2f;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D>();
		sr = GetComponent<SpriteRenderer>();
		anao = GetComponent<Animator>();
	}
	
	void OnDrawGizmos(){
			Gizmos.DrawWireCube(Feet.position, new Vector3(feetW, feetH, 0f));
	}

	// Update is called once per frame
	void Update () {
		
		if(transform.position.y < GM.instance.yMinLive){
			GM.instance.killPlayer();
		}

		isGrounded = Physics2D.OverlapBox(new Vector2(Feet.position.x, Feet.position.y), new Vector2(feetW, feetH), 360.0f, whatIsGround);

		float horizontalInput = Input.GetAxisRaw("Horizontal");//-1 = Left // 1 = Right
		float horizontalPlayerSpeed = horizontalSpeed * horizontalInput;

		if(horizontalPlayerSpeed != 0){
			MoveHorizontal(horizontalPlayerSpeed);
		}
		else{
			StopMovingHorizontal();
		}

		if(Input.GetButtonDown("Jump")){
			Jump();
		}
		
		ShowFall();
	}

	void MoveHorizontal(float speed){
		rb.velocity = new Vector2(speed, rb.velocity.y);

		if(speed < 0f){
			sr.flipX = true;
		}
		else if(speed > 0f){
			sr.flipX = false;
		}
		if(!isJumping){
			anao.SetInteger("State", 1);
		}
	}

	void StopMovingHorizontal(){
		rb.velocity = new Vector2(0f,rb.velocity.y);
		if(!isJumping){
			anao.SetInteger("State", 0);
		}
	}

	void ShowFall(){
		if(rb.velocity.y<0f){
			anao.SetInteger("State", 3);
		}
	}

	void Jump(){
		if(isGrounded){
			isJumping = true;
			rb.AddForce(new Vector2(0f, jumpSpeed));
			AudioManager.instance.PlayJumpSound(gameObject);
			anao.SetInteger("State", 2);

			Invoke("EnableDJ", delayForDJ);
		}

		if(canDoubleJump && !isGrounded){
			rb.velocity = Vector2.zero;
			rb.AddForce(new Vector2(0f, jumpSpeed));
			AudioManager.instance.PlayJumpSound(gameObject);
			anao.SetInteger("State", 2);
			canDoubleJump = false;	
		}
	}

	void EnableDJ(){
		canDoubleJump = true;
	}

	void OnCollisionEnter2D(Collision2D other){
		if(other.gameObject.layer == LayerMask.NameToLayer("Ground")){
			isJumping = false;
		}
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Coin"))
        {
			AudioManager.instance.PlayCoinPickupSound(other.gameObject);
            SFXManager.instance.ShowCoinParticles(other.gameObject);
			GM.instance.IncrementCoinCount();
            Destroy(other.gameObject);
        }
    }

}