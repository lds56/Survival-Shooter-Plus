using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

	public Transform target;
	public float smoothing = 5f;
	public float step = 10f;

	Vector3 offset, newOffset;
	//Vector3 rightmost = new Vector3(21.3f, 15.0f, -23.1f);
	//Vector3 leftmost = new Vector3(-21.8f, 15.0f, -22.0f);

	bool willRotate = false;

	void Start() {
		offset = transform.position - target.position;
	}
	
	void Update() {
		if (Input.GetKeyDown (KeyCode.Q)) {
			offset = Quaternion.Euler (0, 90, 0) * offset;
			willRotate = true;
		} else if (Input.GetKeyDown (KeyCode.E)) {
			offset = Quaternion.Euler (0, -90, 0) * offset;
			willRotate = true;
		}
	}

	void LateUpdate() {

		Vector3 targetCamPos = target.position + offset;
		if (willRotate) {
			transform.position = Vector3.Slerp (transform.position, targetCamPos, smoothing * Time.deltaTime);
			transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(target.position - targetCamPos),
			                                     smoothing * Time.deltaTime);
		} else {
			transform.position = Vector3.Lerp (transform.position, targetCamPos, smoothing * Time.deltaTime);
		}

//		if (targetCamPos.x < leftmost.x ) {
//			targetCamPos.x = leftmost.x;
//		}
//		if (targetCamPos.x > rightmost.x) {
//			targetCamPos.x = rightmost.x;
//		}
//		transform.position = Vector3.Lerp (transform.position, targetCamPos, smoothing * Time.deltaTime);

	}

}
