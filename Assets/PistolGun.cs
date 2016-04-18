using UnityEngine;
using System.Collections;

public class PistolGun : IGun {
	
	public int damagePerShot = 20;
	public float range = 100f;
	public float timeBetweenBullets = 0.15f;
	public int ammoPerClip;

	public override float TimeBetweenBullets { get {return timeBetweenBullets;}}
	public override int AmmoPerClip {get {return ammoPerClip;}}
	public override string GunName {get {return "Pistol";}}

	public HitParticlesPool hpPool;

	Ray shootRay;
	RaycastHit shootHit;
	int shootableMask;

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

	public override void disabled() {
        gunLine.enabled = false;
        gunLight.enabled = false;
	}

	public override void shoot() {

		this.ammo--;
		
		gunAudio.Play ();
		
		gunLight.enabled = true;
		
		gunParticles.Stop ();
		gunParticles.Play ();
		
		gunLine.enabled = true;
		gunLine.SetPosition (0, transform.position);

		shootRay.origin = transform.position;
		shootRay.direction = transform.forward;
		
		if(Physics.Raycast (shootRay, out shootHit, range, shootableMask))
		{
			EnemyHealth enemyHealth = shootHit.collider.GetComponent <EnemyHealth> ();
			if(enemyHealth != null)
			{
				enemyHealth.TakeDamage (damagePerShot, shootHit.point);
			}
			gunLine.SetPosition (1, shootHit.point);
			if (!hpPool.playHitParticles(shootHit.point)) {
				Debug.LogError("No Available Particles");
			}
		}
		else
		{
			gunLine.SetPosition (1, shootRay.origin + shootRay.direction * range);
		}
	}
}
