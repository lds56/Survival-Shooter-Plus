using UnityEngine;
using System.Collections;

public class RPGLauncher : IGun {
	
	public int damagePerShot = 100;
	public float range = 100f;
	public float timeBetweenBullets = 0.1f;
	public int ammoPerClip;
	
	public override float TimeBetweenBullets { get {return timeBetweenBullets;}}
	public override int AmmoPerClip {get {return ammoPerClip;}}
	public override string GunName {get {return "RPG Launcher";}}

	public HitParticlesPool hpPool;
	public GameObject gunBall;
	
	Ray shootRay;
	RaycastHit shootHit;
	int shootableMask;
	
	ParticleSystem gunParticles, newParticles;
	AudioSource gunAudio;
	Light gunLight;
	
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
		this.ammo --;

		gunAudio.Play ();
		
		gunLight.enabled = true;
		
		gunParticles.Stop ();
		gunParticles.Play ();
		
		shootRay.origin = transform.position;
		shootRay.direction = transform.forward;

		RocketBullet rocketBullet = Instantiate (gunBall).GetComponent<RocketBullet>();
		rocketBullet.shootAt(shootRay.origin, shootRay.direction, damagePerShot);

	}
}
