using UnityEngine;
using System.Collections;

public class EnemyAttack : MonoBehaviour
{
    public float timeBetweenAttacks = 1f;
    public int attackDamage = 10;


    Animator anim;
    GameObject player;
    PlayerHealth playerHealth;
    EnemyHealth enemyHealth;
    bool playerInRange;
    float timer;
	
	Ray shootRay;
	RaycastHit shootHit;
	int shootableMask;
	int range = 5;

    void Awake ()
    {
        player = GameObject.FindGameObjectWithTag ("Player");
        playerHealth = player.GetComponent <PlayerHealth> ();
        enemyHealth = GetComponent<EnemyHealth>();
        anim = GetComponent <Animator> ();
    }


    void OnTriggerEnter (Collider other)
    {
        if(other.gameObject == player)
        {
            playerInRange = true;
        }
    }


    void OnTriggerExit (Collider other)
    {
        if(other.gameObject == player)
        {
            playerInRange = false;
        }
    }


    void Update ()
    {
        timer += Time.deltaTime;

        if(timer >= timeBetweenAttacks && enemyHealth.currentHealth > 0)
        {
//			if (this.gameObject.name == "Hellephant(Clone)") {
//				if (PlayerInRay())
//					HellephantAttack();
//			}
//			else {
				if (playerInRange)
         			Attack ();
			//}
        }

        if(playerHealth.currentHealth <= 0)
        {
            anim.SetTrigger ("PlayerDead");
        }
    }


    void Attack ()
    {
        timer = 0f;

        if(playerHealth.currentHealth > 0)
        {
            playerHealth.TakeDamage (attackDamage);
        }
    }

//	bool PlayerInRay() {
//		shootRay.origin = transform.position;
//		shootRay.direction = transform.forward;
//		
//		if(Physics.Raycast (shootRay, out shootHit, range, shootableMask))
//		{
//			PlayerHealth playerHealth = shootHit.collider.GetComponent <PlayerHealth> ();
//			if(playerHealth != null)
//			{
//				return true;
//			} else {
//				return false;
//			}
//		}
//		else
//		{
//			return false;
//		}
//	}
//
//	void HellephantAttack() {
//
//	}
}
