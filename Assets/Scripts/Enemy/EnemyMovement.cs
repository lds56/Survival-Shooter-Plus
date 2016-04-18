using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour
{
    Transform player;
    PlayerHealth playerHealth;
    EnemyHealth enemyHealth;
    NavMeshAgent nav;

	Vector3 wanderDest, newPosition;
	NavMeshHit hit;
	float maxWalkDistance = 5f;
	float disappearingHeight = 4.5f;

	bool autoDest = false;

    void Awake ()
    {
        player = GameObject.FindGameObjectWithTag ("Player").transform;
        playerHealth = player.GetComponent <PlayerHealth> ();
        enemyHealth = GetComponent <EnemyHealth> ();
        nav = GetComponent <NavMeshAgent> ();
    }


    void Update ()
    {
        if(enemyHealth.currentHealth > 0 && playerHealth.currentHealth > 0)
        {
			if (player.position.y < disappearingHeight) {
				nav.SetDestination (player.position);
			} else {
				newPosition = player.position;
				newPosition.y = 0;
				nav.SetDestination (newPosition);
			}
        }
        else
        {
            nav.enabled = false;
        }
    }

//	void wander() {
//		if (autoDest || transform.position == wanderDest) {
//			if (autoDest) 
//				autoDest = false;
//
//			Vector3 direction = Random.insideUnitSphere * maxWalkDistance;
//			direction += transform.position;
//
//			NavMesh.SamplePosition (direction, out hit, Random.Range (0f, maxWalkDistance), 1);
//			wanderDest = hit.position;
//
//			//Debug.DrawLine(transform.position, wanderDest);
//			Debug.Log("Pos: " + transform.position + ", wander: " + wanderDest );
//			nav.SetDestination (wanderDest);
//		}
//	}
}
