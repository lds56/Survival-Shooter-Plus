using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
	public float explosionForce = 4;

	public void bang(Vector3 point) {
		point.y = 10f;
		transform.position = point;
		play ();
		//addForce ();
	}

	void play() {
		var systems = GetComponentsInChildren<ParticleSystem>();
		foreach (ParticleSystem system in systems)
		{
			system.Clear();
			system.Play();
		}
	}
	
	void addForce()
	{
		// wait one frame because some explosions instantiate debris which should then
		// be pushed by physics force
		//yield return null;
		
		float r = 10;
		var cols = Physics.OverlapSphere(transform.position, r);
		var rigidbodies = new List<Rigidbody>();
		foreach (var col in cols)
		{
			if (col.attachedRigidbody != null && !rigidbodies.Contains(col.attachedRigidbody))
			{
				rigidbodies.Add(col.attachedRigidbody);
			}
		}
		foreach (var rb in rigidbodies)
		{
			rb.AddExplosionForce(explosionForce, transform.position, r, 1, ForceMode.Impulse);
		}
	}
}
