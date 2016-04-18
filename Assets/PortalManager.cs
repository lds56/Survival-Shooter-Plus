using UnityEngine;
using System.Collections;

public class PortalManager : MonoBehaviour {

	Vector3 entry;
	Vector3[] exits;

	int portalIndex;

	public float nearDistance = 1.5f;

	// Use this for initialization
	void Awake () {
		Transform[] portals = GetComponentsInChildren<Transform>();
//		Debug.Log ("Portals length" + portals.Length);
//		foreach (var t in portals) {
//			Debug.Log("portal : " + t.position);
//		}
		entry = portals [1].position;
		exits = new Vector3[3] {
			portals [2].position,
			portals [3].position,
			portals [4].position
		};
	}
	
	public bool nearEntry(Vector3 p) {
		return Vector3.Distance (p, entry) < nearDistance;
	}

	public Vector3 pickExit() {
		return exits [Random.Range(0, 3)];
	}

}
