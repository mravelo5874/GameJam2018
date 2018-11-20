using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public float moveSpeed;
	private float moveLimiter = 0.7f;
	private float horizontal;
	private float vertical;
	public Vector2 lastMove;

	private Animator anim;
	private bool playerMoving;
	private Rigidbody2D body;
	private static bool playerExists;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
		body = GetComponent<Rigidbody2D>();


		// when player is changing between scenes
		if (!playerExists) {
			playerExists = true;
			DontDestroyOnLoad(transform.gameObject);
		} else {
			Destroy(gameObject);
		}
	}

	void Update() {
		horizontal = Input.GetAxisRaw("Horizontal");
		vertical = Input.GetAxisRaw("Vertical");


	}
	

	void FixedUpdate() {

		playerMoving = false;


		if (horizontal != 0 && vertical != 0) {
			playerMoving = true;
			body.velocity = new Vector2((horizontal * moveSpeed) * moveLimiter * Time.deltaTime, (vertical * moveSpeed) * moveLimiter * Time.deltaTime);
		} else if (horizontal != 0) {
			playerMoving = true;
			body.velocity = new Vector2(horizontal * moveSpeed * Time.deltaTime, 0f);
		} else if (vertical != 0) {
			playerMoving = true;
			body.velocity = new Vector2(0f, vertical * moveSpeed * Time.deltaTime);
		} else {
			body.velocity = new Vector2(0f, 0f);
		}
		
		
		if (playerMoving) {
			lastMove = new Vector2(horizontal, vertical);
		}
		

		anim.SetFloat("MoveX", horizontal);
		anim.SetFloat("MoveY", vertical);
		anim.SetBool("isMoving", playerMoving);
		anim.SetFloat("LastMoveX", lastMove.x);
		anim.SetFloat("LastMoveY", lastMove.y);		
	}
}
