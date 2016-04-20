using UnityEngine;
using System.Collections;

public class Railgun : IGun {
	
	public int damagePerShot;
	public float range = 100f;
	public float timeBetweenBullets = 0.15f;
	public int ammoPerClip;
	
	public override float TimeBetweenBullets { get {return timeBetweenBullets;}}
	public override int AmmoPerClip {get {return ammoPerClip;}}
	public override string GunName {get {return "Railgun";}}

	Ray shootRay;
	RaycastHit shootHit;
	int shootableMask;
	int counter, maxCounter = 3;

	bool startFire;

	ParticleSystem gunParticles;
	LineRenderer gunLine;
	AudioSource gunAudio;
	Light gunLight;
	
	void Start () {
		shootableMask = LayerMask.GetMask ("Shootable");
		gunParticles = GetComponent<ParticleSystem> ();
		gunAudio = GetComponent<AudioSource> ();
		gunLight = GetComponent<Light> ();
		gunLine = GetComponent<LineRenderer> ();
	}

	public override void fire() {
		if (ammo > 0) {
			if (Input.GetButtonDown ("Fire1")) {
				counter = 0;
				gunAudio.Play ();
				gunLight.enabled = true;
				gunParticles.Play ();
			} else if (Input.GetButton ("Fire1")) {
				counter ++;
				if (counter > maxCounter) {
					counter = 0;
					this.ammo --;
				}
				shoot ();

			} else if (Input.GetButtonUp ("Fire1")) {
				counter = 0;
			} else {
				disabled();
			}
		} else {
			disabled();
		}
	}
	
	public override void disabled() {
		gunAudio.Stop ();
		gunParticles.Stop ();
		gunLine.enabled = false;
		gunLight.enabled = false;
	}
	
	public override void shoot() {	
	
		gunLine.enabled = true;

		gunLine.SetPosition (0, transform.position);
		
		shootRay.origin = transform.position;
		shootRay.direction = transform.forward;

		RaycastHit[] hits;
		hits = Physics.RaycastAll (shootRay, range, shootableMask);

		foreach (RaycastHit shootHit in hits) {
			EnemyHealth enemyHealth = shootHit.collider.GetComponent <EnemyHealth> ();
			if (enemyHealth != null) {
				enemyHealth.TakeDamage (damagePerShot, shootHit.point);
			}
		}

		gunLine.SetPosition (1, shootRay.origin + shootRay.direction * range);

	}
}
