using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectClicker : MonoBehaviour {

	void Update() {

		if (Input.GetMouseButtonDown(0)) {
			// Debug.Log("click...");
			CastRay();
		}
	}

	void CastRay () {
			
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit2D hit = Physics2D.Raycast (ray.origin, ray.direction, Mathf.Infinity);

			if (hit) {
				Debug.DrawLine(ray.origin, hit.point);
				Debug.Log("Hit object: " + hit.collider.gameObject.name);
			}
	} 
}
