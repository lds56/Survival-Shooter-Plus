using UnityEngine;
using System.Collections;

public class Heart : MonoBehaviour {

	public int health;

	// Use this for initialization
	void Start () {
	}
	
	void OnCollisionEnter(Collision col) {
		Debug.Log ("collision: HEART");
		var cdr = col.collider;
		if (cdr.CompareTag ("Player")) {
			cdr.GetComponentInChildren<PlayerHealth> ().addHealth (health);
			notifyManager();
			Destroy(this.gameObject);
		} else {
			Physics.IgnoreCollision(cdr, this.GetComponent<Collider>());
		}
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(20 * Vector3.up * Time.deltaTime);
	}
	
	void notifyManager() {
		GameObject.FindObjectOfType<AmmoManager> ().decHeartAmount ();
	}
}
