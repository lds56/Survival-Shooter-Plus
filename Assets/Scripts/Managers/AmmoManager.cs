using UnityEngine;
using System.Collections;

public class AmmoManager : MonoBehaviour {

	public int interval;
	public Ammo ammo;
	public Heart heart;
	public int maxAmount;

	const float la = 34f, lb = 34f;
	const float ammoY = 0.66f;
	int counter;
	int ammoAmount = 0, heartAmount = 0;

	// Use this for initialization
	void Start () {
		counter = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (ammoAmount + heartAmount < maxAmount) {
			counter++;
			Debug.Log ("Amount: " + (ammoAmount + heartAmount));
			if (counter > interval) {
				counter = 0;
				bool isAmmo = (Random.value < 0.7f);

				Vector3 p;
				do {
					float ammoX = Random.Range (-la, la);
					float ammoZ = Random.Range (-(lb - Mathf.Abs(ammoX)), lb - Mathf.Abs(ammoX));
					p = new Vector3(ammoX, ammoY, ammoZ);
				} while(Physics.OverlapSphere(p, 5f).Length == 0);

				if (isAmmo) {
					ammoAmount ++;
					Instantiate(ammo, p, ammo.transform.rotation);
				} else {
					heartAmount ++;
					Instantiate(heart, p, Quaternion.identity);
				}
			}
		}
	}

	public void decAmmoAmount() {
		ammoAmount --;
		if (ammoAmount < 0)
			Debug.LogError ("Invalid amount");
	}

	
	public void decHeartAmount() {
		heartAmount --;
		if (heartAmount < 0)
			Debug.LogError ("Invalid amount");
	}
}
