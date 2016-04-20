using UnityEngine;
using System.Collections;

public class SniperRifle : IGun {
	
	public int damagePerShot = 80;
	public float range = 100f;
	public float timeBetweenBullets = 0.3f;
	public int ammoPerClip;
	
	public override float TimeBetweenBullets { get {return timeBetweenBullets;}}
	public override int AmmoPerClip {get {return ammoPerClip;}}
	public override string GunName {get {return "Sniper Rifle";}}
	
	public HitParticlesPool hpPool;
	
	Ray shootRay;
	RaycastHit shootHit;
	int shootableMask;
	
	ParticleSystem gunParticles;
	AudioSource gunAudio;
	Light gunLight;

	float forceStrength = 4f;
	
	void Start () {
		shootableMask = LayerMask.GetMask ("Shootable");
		gunParticles = GetComponent<ParticleSystem> ();
		gunAudio = GetComponent<AudioSource> ();
		gunLight = GetComponent<Light> ();
	}
	
	public override void disabled() {
		gunLight.enabled = false;
	}
	
	public override void shoot() {
		
		this.ammo--;
		
		gunAudio.Play ();
		
		gunLight.enabled = true;
		
		gunParticles.Stop ();
		gunParticles.Play ();

		Debug.Log ("Camera: " + Camera.main.name);

		if (Camera.main.name == "PlayerCamera") {
			shootRay = Camera.main.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));

		} else {
			shootRay.origin = transform.position;
			shootRay.direction = transform.forward;
		}
		
		if (Physics.Raycast (shootRay, out shootHit, range, shootableMask)) {
			EnemyHealth enemyHealth = shootHit.collider.GetComponent <EnemyHealth> ();
			if (enemyHealth != null) {
				enemyHealth.TakeDamage (damagePerShot, shootHit.point);
				shootHit.collider.GetComponent<Rigidbody> ().AddForce (forceStrength * shootRay.direction, ForceMode.Impulse);
			}
			if (!hpPool.playFlare (shootHit.point)) {
				Debug.LogError ("No Available Particles");
			}
		} else {
		}
	}
}
