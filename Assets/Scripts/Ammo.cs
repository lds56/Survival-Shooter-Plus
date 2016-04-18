using UnityEngine;
using System.Collections;

public class Ammo : MonoBehaviour {

	int type;

	const int typeNum = 7;

	// Use this for initialization
	void Start () {
		type = Random.Range(0, typeNum);
	}
	
	void OnCollisionEnter(Collision col) {
		var cdr = col.collider;
		if (cdr.CompareTag ("Player")) {
			cdr.GetComponentInChildren<PlayerShooting> ().addAmmo (type);
			notifyManager();
			Destroy(this.gameObject);
		} else {
			Physics.IgnoreCollision(cdr, this.GetComponent<Collider>());
		}
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(20 * Vector3.up * Time.deltaTime, Space.World);
	}

	void notifyManager() {
		GameObject.FindObjectOfType<AmmoManager> ().decAmmoAmount ();
	}
}
