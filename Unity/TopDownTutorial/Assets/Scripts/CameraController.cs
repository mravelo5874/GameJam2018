using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	public GameObject followTarget;
	private Vector3 targetPos;
	public float moveSpeed;
	private static bool cameraExists;
	
	public BoxCollider2D boundsBox;
	private Vector3 minBounds;
	private Vector3 maxBounds;

	private Camera theCamera;
	private float halfHeight;
	private float halfWidth;

	// Use this for initialization
	void Start () {

		// when camera is changing between scenes
		if (!cameraExists) {
			cameraExists = true;
			DontDestroyOnLoad(transform.gameObject);
		} else {
			Destroy(gameObject);
		}

		// bounds of camera movement
		minBounds = boundsBox.bounds.min;
		maxBounds = boundsBox.bounds.max;

		// get the size of the camera
		theCamera = GetComponent<Camera>();
		halfHeight = theCamera.orthographicSize;
		halfWidth = halfHeight * Screen.width / Screen.height;
	}
	
	// Update is called once per frame
	void Update () {
		targetPos = new Vector3 (followTarget.transform.position.x, followTarget.transform.position.y, transform.position.z);
		transform.position = Vector3.Lerp (transform.position, targetPos, moveSpeed * Time.deltaTime);
		

		float clampedX = Mathf.Clamp(transform.position.x, minBounds.x + halfWidth, maxBounds.x - halfWidth);
		float clampedY = Mathf.Clamp(transform.position.y, minBounds.y + halfHeight, maxBounds.y - halfHeight);
		
		transform.position = new Vector3(clampedX, clampedY, transform.position.z);
	}
}
