using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HitParticlesPool: MonoBehaviour {

	public ParticleSystem explosion;
	public ParticleSystem hitParticle;
	public ParticleSystem flare;
	
	List<ParticleSystem> explosionSet = new List<ParticleSystem>();
	List<ParticleSystem> hitParticlesSet = new List<ParticleSystem>();
	List<ParticleSystem> flareSet = new List<ParticleSystem>();

	void Start() {

	}

	public bool playExplosion(Vector3 p) {
		playParticles (p, explosion, explosionSet);
		return true;
	}

	public bool playHitParticles(Vector3 p) {
		playParticles (p, hitParticle, hitParticlesSet);
		return true;
	}
	
	public bool playFlare(Vector3 p) {
		playParticles (p, flare, flareSet);
		return true;
	}

	public void playParticles(Vector3 p, ParticleSystem psOld, List<ParticleSystem> psSet) {
		foreach (ParticleSystem ps in psSet) {
			if (!ps.IsAlive()) {
				ps.transform.position = p;
				ps.Play ();
				return;
			}
		}
		
		ParticleSystem newps = (ParticleSystem) Instantiate(psOld);
		newps.transform.position = p;
		newps.Play ();
		psSet.Add(newps);

		if (psSet.Count > 100) {
			Debug.LogError ("PsSet is too large!");
		}
	}

}
