using UnityEngine;
using System.Collections;

public class IGun: MonoBehaviour {
	
	public virtual int AmmoPerClip { get; protected set;}
	public virtual float TimeBetweenBullets { get; protected set;}
	public virtual string GunName { get; protected set;}

	public int ammo { get; set;}

	float effectsDisplayTime = 0.2f;
	float timer = 0f;

	public virtual void fire() {
		timer += Time.deltaTime;
		
		if(ammo > 0 && Input.GetButton ("Fire1") && timer >= TimeBetweenBullets && Time.timeScale != 0)
		{
			timer = 0f;
			shoot ();
		}
		
		if(timer >= TimeBetweenBullets * effectsDisplayTime)
		{
			disabled();
		}
	}

	public virtual void disabled () {
	
	}

	public virtual void shoot () {
	
	}
	
}
