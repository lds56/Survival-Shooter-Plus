using UnityEngine;
using System.Collections.Generic;

public class PlayerMovement : MonoBehaviour
{
	public float speed = 5f;
	public Camera worldCamera;
	public Camera playerCamera;

	public float zoom = 100f;
	public float zoomSmooth = 10f;

	public PortalManager portalManager;

	float horizontalSpeed = 10f, verticalSpeed = 10f;

	Vector3 movement;
	Animator anim;

	Rigidbody playerRigidBody;
	int floorMask;

	float camRayLength = 100f;
	float h, v;
	float originalFOV;

	float gravitySpeed = 1f;
	float gravityASpeed = 1f;

	Ray camRay;

	PlayerShooting playerShooting;

	bool viewFlag = true;
	bool zoomFlag = false;
	Dictionary<string, float> dict = new Dictionary<string, float>() {{"Horizontal", 0}, {"Vertical", 0}};

	struct MoveState {
		public string hMask;
		public string vMask;
		public int hParam;
		public int vParam;
		
		public MoveState(string hMask, string vMask, int hParam, int vParam) {
			this.hMask = hMask;
			this.vMask = vMask;
			this.hParam = hParam;
			this.vParam = vParam;
		}
	}

	int stateNo = 0;
	MoveState[] moveStates;

	void Awake() {
		moveStates = new MoveState[4] {
			new MoveState ("Horizontal", "Vertical", 1, 1),
			new MoveState ("Vertical", "Horizontal", 1, -1),
			new MoveState ("Horizontal", "Vertical", -1, -1),
			new MoveState ("Vertical", "Horizontal", -1, 1)
		};
		floorMask = LayerMask.GetMask ("Floor");
		anim = GetComponent<Animator> ();
		playerRigidBody = GetComponent<Rigidbody> ();

		playerShooting = GetComponentInChildren<PlayerShooting> ();

		originalFOV = playerCamera.fieldOfView;
	}

	void Update() {
		h = Input.GetAxisRaw ("Horizontal");
		v = Input.GetAxisRaw ("Vertical");
		
		dict ["Horizontal"] = h;
		dict ["Vertical"] = v;

		if (Input.GetKeyDown (KeyCode.Q)) {
			stateNo = (stateNo + 1) % 4;
		} else if (Input.GetKeyDown (KeyCode.E)) {
			stateNo = (stateNo - 1 + 4) % 4;
		}

		if (Input.GetKeyDown (KeyCode.V)) {
			worldCamera.enabled = !worldCamera.enabled;
			playerCamera.enabled = !playerCamera.enabled;
			viewFlag = !viewFlag;
		}

		if (Input.GetKeyDown (KeyCode.Z)) {
			zoomFlag = true;
		}
		if (Input.GetKeyUp (KeyCode.Z)) {
			zoomFlag = false;
		}
	}

	void FixedUpdate() {
		Zoom ();
		Move (h, v);
		Turning ();
		Animating (h, v);
	}

	void Move(float h, float v) {
		if (viewFlag) // Third Person View
			ThirdPersonMove (h, v);
		else {
			FirstPersonMove(h, v);
		}
	}

	void Turning() {
		if (viewFlag) // Third Person View
			ThirdPersonTurning ();
		else {
			FirstPersonTurning();
		}
	}

	void Zoom() {
		if (!viewFlag && playerShooting.isholdingSniper ()) {
			if (zoomFlag) {
				playerCamera.fieldOfView = Mathf.Lerp (playerCamera.fieldOfView, zoom, Time.deltaTime * zoomSmooth);
				playerShooting.setScope (true);
			} else {
				playerCamera.fieldOfView = Mathf.Lerp (playerCamera.fieldOfView, originalFOV, Time.deltaTime * zoomSmooth);
				playerShooting.setScope (false);
			}
		} else {
			if (zoomFlag) {
				zoomFlag = false;
				playerCamera.fieldOfView = Mathf.Lerp (playerCamera.fieldOfView, originalFOV, Time.deltaTime * zoomSmooth);
				playerShooting.setScope (false);
			}
		}
	}

	void ThirdPersonMove(float h, float v) {

		h = dict[moveStates[stateNo].hMask] * moveStates[stateNo].hParam;
		v = dict[moveStates[stateNo].vMask] * moveStates[stateNo].vParam;

		movement.Set (h, -gravitySpeed, v);
		movement = movement.normalized * speed * Time.deltaTime;

		//transform.Translate (movement);
		playerRigidBody.MovePosition (transform.position + movement);
	}

	void FirstPersonMove(float h, float v) {

		if (portalManager.nearEntry (this.transform.position)) {
			playerRigidBody.MovePosition (portalManager.pickExit ());
		} else {
			//gravitySpeed += gravityASpeed;
			playerRigidBody.MovePosition(transform.position + 
			                             0.015f * transform.forward * v * speed + 
			                             0.015f * transform.right   * h * speed +
			                             (-0.1f * gravitySpeed) * transform.up);
		}
	}

	void ThirdPersonTurning(){
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;

		Ray camRay = worldCamera.ScreenPointToRay (Input.mousePosition);

		RaycastHit floorHit;

		if (Physics.Raycast (camRay, out floorHit, camRayLength, floorMask)) {
			Vector3 playerToMouse = floorHit.point-transform.position;
			playerToMouse.y = 0f;

			Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
			playerRigidBody.MoveRotation(newRotation);
		}
	}

	void FirstPersonTurning() {		

		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		
//		camRay = worldCamera.ScreenPointToRay (Input.mousePosition);
//
//		RaycastHit skyboxHit;
//		
//		if (Physics.Raycast (camRay, out skyboxHit, 10.0f, floorMask)) {
//			Vector3 playerToMouse = floorHit.point-transform.position;
//			playerToMouse.y = 0f;
//			
//			Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
//			playerRigidBody.MoveRotation(newRotation);
//		}

		float fph = horizontalSpeed * Input.GetAxis ("Mouse X");
		transform.Rotate (0, fph, 0);

		float fpv = verticalSpeed * Input.GetAxis ("Mouse Y");
		playerCamera.transform.Rotate (-fpv, 0, 0);

	}

	void Animating(float h, float v) {
		if (viewFlag) {
			bool walking = h != 0f || v != 0f;
			anim.SetBool ("IsWalking", walking);
		} else {
			anim.SetBool ("IsWalking", false);
		}
	}

}
