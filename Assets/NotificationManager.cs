using UnityEngine;
using UnityEngine.UI;

public class NotificationManager : MonoBehaviour
{
	public float restartDelay = 5f;
	
	Animator anim;
	float restartTimer;

	bool isOver = false;

	Text notiText;
	
	void Awake()
	{
		anim = GetComponent<Animator>();
		notiText = GetComponentInChildren<Text> ();
	}
	
	
	void Update()
	{
		if (isOver)
		{
			if(Input.GetKeyDown("return")){
				Application.LoadLevel(Application.loadedLevel);
				isOver = false;
			}
		}
	}

	public void notifyDead() {
		anim.SetTrigger("GameOver");
		isOver = true;
	}

	public void notifyNotification(string notification) {
		notiText.text = notification;
		anim.SetTrigger ("Notified");
	}
}
