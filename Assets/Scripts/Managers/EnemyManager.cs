using UnityEngine;
using UnityEngine.UI;

public class EnemyManager : MonoBehaviour
{
    public PlayerHealth playerHealth;
	public GameObject[] enemies;
    public float spawnTime = 3f;
    public Transform[] spawnPoints;

	public Text levelText;

	int maxAmount;
	int amount;
	int decAmount;
	int level;

	int[] enemyAmounts = new int[10] {10, 20, 40, 60, 100, 140, 180, 220, 260, 300};

	float timer;
    void Start ()
    {
		level = 0;
		initAmount ();

        //InvokeRepeating ("Spawn", spawnTime, spawnTime);
    }

	void Update() {
		timer += Time.deltaTime;
		if (timer > spawnTime) {
			timer = 0f;
			if (amount < maxAmount) {
				Debug.Log ("Enemy++");
				Spawn ();
			}
		}
	}


    void Spawn ()
    {
        if(playerHealth.currentHealth <= 0f)
        {
            return;
        }

        int spawnPointIndex = Random.Range (0, spawnPoints.Length);

		GameObject enemyInstance = (GameObject)Instantiate (enemies[spawnPointIndex], 
		                                                    spawnPoints[spawnPointIndex].position, 
		                                                    spawnPoints[spawnPointIndex].rotation);
		enemyInstance.transform.parent = this.transform;

		amount++;
    }

	void initAmount() {
		timer = 0f;
		amount = 0;
		maxAmount = enemyAmounts[level];
		decAmount = 0;
		levelText.text = "Level: " + level.ToString ();
	}

	public void decEnemyAmount() {
		Debug.Log ("Enemy--");
		decAmount ++;
		if (decAmount == maxAmount) {
			level++;
			initAmount();
		}
	}
}
