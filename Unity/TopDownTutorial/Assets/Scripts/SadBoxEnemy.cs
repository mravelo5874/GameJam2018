using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SadBoxEnemy : MonoBehaviour {

	public float moveSpeed;
	private bool isMoving;
	public float timeBetweenMove;
	private float timeBetweenMoveCounter;
	public float timeToMove;
	private float timeToMoveCounter;
	private Vector3 moveDirection;

	private Rigidbody2D body;

	// Use this for initialization
	void Start () {
		body = GetComponent<Rigidbody2D>();

		timeBetweenMoveCounter = timeBetweenMove;
		timeToMoveCounter = timeToMove;
	}
	
	// Update is called once per frame
	void Update () {

		if(isMoving) {

			timeToMoveCounter -= Time.deltaTime;
			body.velocity = moveDirection;

			if(timeToMoveCounter < 0f) {
				timeBetweenMoveCounter = timeBetweenMove;
			}
		} else {

			timeBetweenMoveCounter -= Time.deltaTime;
			body.velocity = new Vector3(0f, 0f, 0f);

			if(timeBetweenMoveCounter < 0f) {
				isMoving = true;
				timeToMoveCounter = timeToMove;

				moveDirection = new Vector3(Random.Range(-1f, 1f) * moveSpeed, Random.Range(-1f, 1f) * moveSpeed, 0f);
			}
		}
	}
}
