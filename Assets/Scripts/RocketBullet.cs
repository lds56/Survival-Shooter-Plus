using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RocketBullet : MonoBehaviour {

	public float speed;
	public float aspeed;
	public float explosionForce = 4;

	bool shooting;
	bool willShoot;
	Vector3 target;
	int damage;

	Rigidbody rb;
	HitParticlesPool hpPool;

	void Awake() {
		rb = GetComponent<Rigidbody> ();
		hpPool = GameObject.FindObjectOfType<HitParticlesPool> ();

		willShoot = false;
	}

	void FixedUpdate() {
		if (this.shooting) {
			rb.AddForce(aspeed * this.transform.right, ForceMode.Acceleration);
		}
	}

	void OnCollisionEnter(Collision col) {
		if (!hpPool.playExplosion(col.transform.position)) {
			Debug.LogError("No Available Explosion");
		}
		addExpolosiveForce (col.transform.position, damage);
		rb.velocity = Vector3.zero;
		Destroy (this.gameObject);
	}

	public bool shootAt(Vector3 source, Vector3 direction, int damage) {
		Vector3 newPosition = source + direction;
		newPosition.y = 0.5f;
		this.transform.position = newPosition;
		this.transform.right = direction.normalized;
		this.shooting = true;
		this.damage = damage;
		rb.velocity = speed * this.transform.right;
		return true;
	}
	
	void addExpolosiveForce(Vector3 p, int damage)
	{
		float r = 3;
		var cols = Physics.OverlapSphere(p, r);
		var rigidbodies = new List<Rigidbody>();
		foreach (var col in cols)
		{
			if (col.attachedRigidbody != null && !rigidbodies.Contains(col.attachedRigidbody))
			{
				EnemyHealth enemyHealth = col.GetComponent <EnemyHealth> ();
				if (enemyHealth != null) {
					Debug.Log("health");
					enemyHealth.TakeDamage (damage, p);
				}
				rigidbodies.Add(col.attachedRigidbody);
			}
		}
		Debug.Log (rigidbodies.Count);
		foreach (var rbd in rigidbodies)
		{
			rbd.AddExplosionForce(explosionForce, p, r, 1, ForceMode.Impulse);
		}
	}

}
