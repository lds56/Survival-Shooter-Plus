using UnityEngine;
using UnityEngine.UI;

public class PlayerShooting : MonoBehaviour
{
	public IGun[] guns;
	public Grenade grenade;
	public float grenadeRate;
	public float maxStrength;
	public float deltaStrength;

	public int maxGrenadeAmount = 20;
	
	public WeaponManager weaponManager;
	public NotificationManager notiManager;

	float nextGrenade;
	float throwStrength;
	float nextStrength = 0f;
	float strengthRate = 0.08f;
	float time;

	float minRatio = 0.2f;

	int curIdx;
	IGun curGun;

	int grenadeAmount;

	bool weaponUITrigger = false;

    void Awake ()
    {
		
		Random.seed = (int)System.DateTime.Now.Ticks;
		//strengthRate = 500 * Time.deltaTime;
		foreach (var gun in guns) {
			gun.transform.SetParent(this.transform);
			gun.transform.localPosition = Vector3.zero;
		}

		curGun = guns [4];

		grenadeAmount = maxGrenadeAmount;

		updateCanvas ();

    }

    void Update ()
    {
		switchGun ();
		fire ();
		throwGrenade ();
    }

	void LateUpdate() {
		updateCanvas ();
	}

	void fire() {
		//if (curGun.ammo > 0) {
			curGun.fire ();
		//}
	}

	void switchGun() {
		float scroll = Input.GetAxis("Mouse ScrollWheel");
		if (scroll > 0f) {
			curIdx = (curIdx + 1) % guns.Length;
		} else if (scroll < 0f) {
			curIdx = (curIdx - 1 + guns.Length) % guns.Length;
		}
		curGun = guns[curIdx];
		//Debug.Log ("scroll: " + scroll);
		//Debug.Log ("Idx: " + curIdx);
	}

	void throwGrenade() {
		if (Input.GetButton("Fire2") && throwStrength<maxStrength) {
			throwStrength += deltaStrength * strengthRate;
			Debug.Log(throwStrength);
		}
		
		if (Input.GetButtonUp("Fire2") && grenadeAmount > 0) {
			
			if (Time.time > nextGrenade) {
				nextGrenade = Time.time + grenadeRate;
				
				Grenade g = (Grenade) Instantiate(grenade, this.transform.position, this.transform.rotation);
				g.throwForward(throwStrength);

				grenadeAmount--;
			}
			
			throwStrength = 0f;
		}
	}
//
//	void zoom() {
//		if (Input.GetKeyDown () == "Z") {
//			.fieldOfView = Mathf.Lerp(camera.fieldOfView,zoom,Time.deltaTime*smooth);
//		}
//	}

	void updateCanvas() {
		weaponManager.setGrenadeAmount (grenadeAmount);
		weaponManager.setAmmoAmount (curGun.ammo);
		weaponManager.setGunImage (curIdx);
	}

	public bool isholdingSniper() {
		return curIdx == 5;
	}

    public void DisableEffects ()
    {
		curGun.disabled ();
    }

	public void addAmmo(int type) {

		if (type == guns.Length) {
			int lastAmount = grenadeAmount;

			grenadeAmount += (int)(maxGrenadeAmount * (minRatio + Random.value * (1 - minRatio)));
			if (grenadeAmount > maxGrenadeAmount)
				grenadeAmount = maxGrenadeAmount;

			notiManager.notifyNotification("Grenade +" + (grenadeAmount - lastAmount));

		} else {
			int lastAmount = guns[type].ammo;
			IGun theGun = guns[type];

			int amount = (int)(theGun.AmmoPerClip * (minRatio + Random.value * (1 - minRatio)));

			if (theGun.ammo + amount > theGun.AmmoPerClip)
				theGun.ammo = theGun.AmmoPerClip;
			else {
				theGun.ammo = theGun.ammo + amount;
			}

			notiManager.notifyNotification(guns[type].name + " ammo +" + (theGun.ammo - lastAmount));
		}
	}

	public void setScope(bool scopeFlag) {
		weaponManager.setScope (scopeFlag);
	}

	public void setVolume(float v) {
		foreach (var gun in guns) {
			gun.GetComponent<AudioSource>().volume = v;
		} 
	}

}
