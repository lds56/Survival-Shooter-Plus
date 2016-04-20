using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grenade : MonoBehaviour {

	public int initalSpeed;
	public float waitingTime;
	public int damage;
	public float explosionForce;

	Rigidbody rb;
	MeshRenderer mr;
	HitParticlesPool hpPool;

	float timer;

	float torque = 10f;

	// Use this for initialization
	void Awake () {
		timer = 0f;
		hpPool = GameObject.FindGameObjectWithTag ("Pool").GetComponent<HitParticlesPool>();
		rb = GetComponent<Rigidbody> ();
	}

	public void throwForward(float strength) {
		Vector3 speed = (initalSpeed + strength) * this.transform.forward;
		speed.y = 7f;
		rb.velocity = speed;

		rb.AddTorque(transform.forward * torque);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		timer += Time.deltaTime;
		if (timer >= waitingTime) {
			hpPool.playExplosion(this.transform.position);
			addExpolosiveForce();
			timer = 0f;
			Destroy(this.gameObject);
		}
	}
	
	void addExpolosiveForce()
	{
		Vector3 p = this.transform.position;
		float r = 5;
		var cols = Physics.OverlapSphere(p, r);
		var rigidbodies = new List<Rigidbody>();
		foreach (var col in cols)
		{
			if (col.attachedRigidbody != null && !rigidbodies.Contains(col.attachedRigidbody))
			{
				EnemyHealth enemyHealth = col.GetComponent <EnemyHealth> ();
				if (enemyHealth != null) {
					enemyHealth.TakeDamage (damage, p);
				}
				
				PlayerHealth playerHealth = col.GetComponent <PlayerHealth> ();
				if (playerHealth != null) {
					playerHealth.TakeDamage (damage);
				}

				rigidbodies.Add(col.attachedRigidbody);
			}
		}
		foreach (var rbd in rigidbodies)
		{
			rbd.AddExplosionForce(explosionForce, p, r, 1, ForceMode.Impulse);
		}
	}
}
