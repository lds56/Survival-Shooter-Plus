using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WeaponManager : MonoBehaviour {

	public Sprite[] gunSprites;

	Text grenadeText, ammoText;
	Image gunImage, scopeImage;

	int grenadeAmount, ammoAmount;

	int screenWidth, screenHeight;

	// Use this for initialization
	void Awake () {
		Text[] texts = GetComponentsInChildren<Text>();
		grenadeText = texts [0];
		ammoText = texts [1];

		Image[] images = GetComponentsInChildren<Image> ();
		scopeImage = images [0];
		gunImage = images [2];

		screenWidth = Screen.width;
		screenHeight = Screen.height;

		//Debug.Log ("Screen: " + screenWidth + "x" + screenHeight);
		//RectTransform rt = scopeImage.gameObject.GetComponent<RectTransform>();
		//Debug.Log ("screen width: " + screenWidth + ", width: " + rt.rect.height);
		//rt.sizeDelta = new Vector2(rt.rect.width * screenHeight / rt.rect.height, 0);
		//if (rt.rect.height < screenHeight) {
		//	rt.localScale();
			//rt.sizeDelta = new Vector2(rt.rect.width * screenHeight / rt.rect.height, screenHeight);
		//}
	}

	public void setGrenadeAmount(int amount) {
		grenadeText.text = amount.ToString();
	}

	public void setAmmoAmount(int amount) {
		ammoText.text = amount.ToString();
	}

	public void setGunImage(int idx) {
		gunImage.sprite = gunSprites [idx];
		//gunImage.
	}

	public void setScope(bool scopeFlag) {
		scopeImage.enabled = scopeFlag;
	}
	
}
