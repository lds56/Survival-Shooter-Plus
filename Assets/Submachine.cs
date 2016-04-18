using UnityEngine;
using System.Collections;

public class Submachine : IGun {
	
	public int damagePerShot = 10;
	public float range = 100f;
	public float scaleLimit = 0.5f;  
	public float timeBetweenBullets = 0.05f;
	public int ammoPerClip;
	
	public override float TimeBetweenBullets { get {return timeBetweenBullets;}}
	public override int AmmoPerClip {get {return ammoPerClip;}}
	public override string GunName {get {return "Submachine";}}
	
	public HitParticlesPool hpPool;
	
	Ray shootRay;
	RaycastHit shootHit;
	int shootableMask;
	float z = 10;
	
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

		float randomRadius = Random.Range( 0, scaleLimit );        
		float randomAngle = Random.Range ( 0, 2 * Mathf.PI );
		
		//Calculating the raycast direction
		Vector3 direction = new Vector3(
			randomRadius * Mathf.Cos( randomAngle ),
			randomRadius * Mathf.Sin( randomAngle ),
			z
			);
		
		//Make the direction match the transform
		//It is like converting the Vector3.forward to transform.forward
		
		shootRay.origin = transform.position;
		shootRay.direction = transform.TransformDirection( direction.normalized );
		
		if(Physics.Raycast (shootRay, out shootHit, range, shootableMask))
		{
			EnemyHealth enemyHealth = shootHit.collider.GetComponent <EnemyHealth> ();
			if(enemyHealth != null)
			{
				enemyHealth.TakeDamage (damagePerShot, shootHit.point);
			}
			gunLine.SetPosition (1, shootHit.point);
			if (!hpPool.playHitParticles(shootHit.point)) {
				Debug.LogError("No Available Hit Particles");
			}
		}
		else
		{
			gunLine.SetPosition (1, shootRay.origin + shootRay.direction * range);
		}
	}
}
