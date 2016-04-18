using UnityEngine;
using System.Collections;

public class Shotgun : IGun {
	
	public int damagePerShot = 30;
	public float range = 100f;
	public float scaleLimit = 1f;    
	public float timeBetweenBullets = 0.15f;
	public int ammoPerClip;
	public HitParticlesPool hpPool;
	
	public override float TimeBetweenBullets { get {return timeBetweenBullets;}}
	public override int AmmoPerClip {get {return ammoPerClip;}}
	public override string GunName {get {return "Shotgun";}}
	
	Ray shootRay;
	RaycastHit shootHit;
	int shootableMask;
	float z = 10;
	
	ParticleSystem gunParticles;
	LineRenderer[] gunLines;
	AudioSource gunAudio;
	Light gunLight;
	
	void Start () {
		shootableMask = LayerMask.GetMask ("Shootable");
		gunParticles = GetComponent<ParticleSystem> ();
		gunAudio = GetComponent<AudioSource> ();
		gunLight = GetComponent<Light> ();
		gunLines = GetComponentsInChildren<LineRenderer>(true);

		foreach (LineRenderer gunLine in gunLines) {
			gunLine.enabled = false;
		}
	}
	
	public override void disabled() {
		foreach (LineRenderer gunLine in gunLines) {
			gunLine.enabled = false;
		}
		gunLight.enabled = false;
	}
	
	public override void shoot() {

		this.ammo -= gunLines.Length;
		if (this.ammo < 0)
			this.ammo = 0;
		
		gunAudio.Play ();
		
		gunLight.enabled = true;
		
		gunParticles.Stop ();
		gunParticles.Play ();
		
		foreach (LineRenderer gunLine in gunLines) {
			gunLine.enabled = true;
			gunLine.SetPosition (0, transform.position);


			//float randomRadius = scaleLimit;             
			//  The Ray-hits will be in a circular area
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
		
			if (Physics.Raycast (shootRay, out shootHit, range, shootableMask)) {
				EnemyHealth enemyHealth = shootHit.collider.GetComponent <EnemyHealth> ();
				if (enemyHealth != null) {
					enemyHealth.TakeDamage (damagePerShot, shootHit.point);
				}
				gunLine.SetPosition (1, shootHit.point);
				if (!hpPool.playHitParticles(shootHit.point)) {
					Debug.LogError("No Available Particles");
				}
			} else {
				gunLine.SetPosition (1, shootRay.origin + shootRay.direction * range);
			}
		}
	}
}
