using UnityEngine;
using System.Collections;

public class CustomerSpawner : MonoBehaviour {
	public GameObject[] columns;
	public int numberToSpawn;
	public float timeBetweenSpawns;
	public bool shouldSpawn = false;
	public bool streak;
	public int overpoured = 0;
	public int streakLength;
	public int longestStreak;
	public int spawned;
	public int served;
	public PotControl pot;
	public GameObject[] teaSelections;
	public GameObject streakParticleSystem;
	public Teachievement achievements;
	private float timer;
	private int numberOfTeas;
	private bool bugging;

	public GameObject[] customers;
	// Use this for initialization
	void Start () {
		numberOfTeas = PlayerPrefs.GetInt("number of teas", 1);
		pot.numberOfTeas = numberOfTeas;
		for(int i = 0; i < numberOfTeas; i++) {
			teaSelections[i].SetActive(true);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(shouldSpawn) {
			timer+= Time.deltaTime;
			if(served == spawned) {
				checkPours();
			}
			else if((timeBetweenSpawns - 5) < timer) {
				//BUG OUT
				if (!bugging) {
					if(streak) {
						pot.poweredUpMusicSnapshotAlarm.TransitionTo(.01f);
						Debug.Log("Turning On Power Alarm");
					}
					else {
						pot.backgroundMusicSnapshotAlarm.TransitionTo(.01f);
						Debug.Log("Turning On Normal Alarm");
					}
						bugging = true;
				}
				checkTime();
				if(timeBetweenSpawns < timer) {
					checkPours();
				}
			}
		}
	}

	void checkTime() {
		for (int i = 0; i < columns.Length; i++) {
			Person[] people = columns[i].GetComponentsInChildren<Person>();
			for(int j = 0; j < people.Length; j++) {
				people[j].Bugout();
			}
		}
	}

	void checkPours() {
		int customersServed = 0;
		int customersToServe = 0;
		bool wrongtea = false;
		bool fuckedUp = false;
		for (int i = 0; i < columns.Length; i++) {
			Person[] people = columns[i].GetComponentsInChildren<Person>();
			customersToServe += people.Length;
		}

		for (int i = 0; i < columns.Length; i++) {
			Person[] people = columns[i].GetComponentsInChildren<Person>();
			for(int j = 0; j < people.Length; j++) {
				Glass glass = people[j].GetComponentInChildren<Glass>();
				if(glass.overpour) {
					//OVERPOURED, STILL COUNTS
					customersServed++;
					overpoured++;
					pot.overPour();
					fuckedUp = true;

				}
				else if(glass.wrongtea) {
					//WRONG TEA, DOES NOT COUNT
					fuckedUp = true;
					wrongtea = true;
				}
				else if (glass.drops < 8) {
					//UNDERPOURED OR NOT SERVED
					fuckedUp = true;
					if (glass.drops == 0) {
						//NOT SERVED, DOES NOT COUNT
					}
					else {
						//UNDERPOURED, STILL COUNTS
						customersServed++;
					}
				}
				else {
					customersServed++;
					people[j].Drink();
				}

			}
		}

		if (!fuckedUp && customersServed == customersToServe) {
			//PERFECT
			streakLength += customersServed;
			if(streakLength > longestStreak) {
				longestStreak = streakLength;
			}

			if(streakLength > 1) {
				streak = true;
				pot.perfectPour(streakLength);
				pot.GetComponent<Animator>().SetTrigger("perfect");
				pot.poweredUpMusicSnapshot.TransitionTo(.01f);
			}
		}
		else if(customersServed == customersToServe && fuckedUp) {
			//OVER POURED OR UNDERPOURED
			endStreak();
			pot.badPourFx();
			pot.feedbackMessage.text = "BAD POUR!";
			pot.feedback.SetTrigger("show");

		}
		else if (customersServed != customersToServe) {
			//WRONG TEA OR NOT SERVED
			if(streak) {
				//END STREAK
				endStreak();
				pot.badPourFx();
				pot.feedbackMessage.text = "WRONG TEA!";
				pot.feedback.SetTrigger("show");

			}
			else {
				if (wrongtea) {
					pot.wrongTea();
				}
				else {
					pot.missed();
				}
			}
		}
			
		if (streakParticleSystem.activeSelf != streak) {
			streakParticleSystem.SetActive(streak);
		}

		pot.cupsServed += customersServed;
		LevelUp();
		GenerateRow();
	}

	void endStreak() {
		streak = false;
		streakLength = 0;
		pot.backgroundMusicSnapshot.TransitionTo(.01f);
		pot.GetComponent<Animator>().SetTrigger("normal");
	}

	void LevelUp() {
		if(pot.cupsServed > 4 && pot.cupsServed < 15 && numberOfTeas < 2) {
			//UNLOCK FIRST NEW TEA
			numberOfTeas = 2;
		}

		else if(pot.cupsServed > 15 && pot.cupsServed < 30 && numberOfTeas < 3) {
			//UNLOCK FIRST NEW TEA
			numberOfTeas = 3;
		}

		else if(pot.cupsServed > 30 && pot.cupsServed < 40 && numberOfTeas < 4) {
			//UNLOCK FIRST NEW TEA
			numberOfTeas = 4;
		}

		else if(pot.cupsServed > 40 && pot.cupsServed < 50 && numberOfTeas < 5) {
			//UNLOCK FIRST NEW TEA
			numberOfTeas = 5;
		}

		else if(pot.cupsServed > 50 && pot.cupsServed < 60 && numberOfTeas < 6) {
			//UNLOCK FIRST NEW TEA
			numberOfTeas = 6;
		}

		else if(pot.cupsServed > 60 && pot.cupsServed < 70 && numberOfTeas < 7) {
			//UNLOCK FIRST NEW TEA
			numberOfTeas = 7;
		}

		if(pot.cupsServed >= 3 && pot.cupsServed < 10) {
			numberToSpawn = 2;
		}
		else if(pot.cupsServed >= 10 && pot.cupsServed < 15) {
			timeBetweenSpawns = 24;
		}
		else if(pot.cupsServed >= 15 && pot.cupsServed < 25) {
			numberToSpawn = 3;
		}
		else if(pot.cupsServed >= 25 && pot.cupsServed < 35) {
			timeBetweenSpawns = 23;
		}
		else if(pot.cupsServed >= 35 && pot.cupsServed < 45) {
			numberToSpawn = 4;
		}
		else if(pot.cupsServed >= 45 && pot.cupsServed < 55) {
			timeBetweenSpawns = 22;
		}
		else if(pot.cupsServed >= 55 && pot.cupsServed < 65) {
			timeBetweenSpawns = 21;
		}
		else if(pot.cupsServed >= 65 && pot.cupsServed < 75) {
			timeBetweenSpawns = 20;
		}
		else if(pot.cupsServed >= 75 && pot.cupsServed < 85) {
			timeBetweenSpawns = 19.5f;
		}
		else if(pot.cupsServed >= 85 && pot.cupsServed < 95) {
			timeBetweenSpawns = 19;
		}
		else if(pot.cupsServed >= 95 && pot.cupsServed < 105) {
			timeBetweenSpawns = 18.5f;
		}
		else if(pot.cupsServed >= 105 && pot.cupsServed < 115) {
			timeBetweenSpawns = 18;
		}
		else if(pot.cupsServed >= 115 && pot.cupsServed < 125) {
			timeBetweenSpawns = 17.5f;
		}
		else if(pot.cupsServed >= 125 && pot.cupsServed < 135) {
			timeBetweenSpawns = 17;
		}
		else if(pot.cupsServed >= 135 && pot.cupsServed < 145) {
			timeBetweenSpawns = 16.5f;
		}
		else if(pot.cupsServed >= 145 && pot.cupsServed < 155) {
			timeBetweenSpawns = 16;
		}
		else if(pot.cupsServed >= 155 && pot.cupsServed < 165) {
			timeBetweenSpawns = 15.5f;
		}
		else if(pot.cupsServed >= 165 && pot.cupsServed < 175) {
			timeBetweenSpawns = 15;
		}
		else if(pot.cupsServed >= 175 && pot.cupsServed < 185) {
			timeBetweenSpawns = 14.5f;
		}

		if(pot.numberOfTeas != numberOfTeas) {
			//leveled up!
			pot.numberOfTeas = numberOfTeas;
			for(int i = 0; i < numberOfTeas; i++) {
				teaSelections[i].SetActive(true);
			}
			achievements.Unlock(numberOfTeas);
		}
	}

	public void Resume() {
//		Time.timeScale = 1f;
	}
	public void ClearCustomers() {
		for(int i = 0; i < columns.Length; i++) {
			Person[] people = columns[i].GetComponentsInChildren<Person>();
			for(int j = 0; j < people.Length; j++) {
				Destroy(people[j].gameObject);
			}
		}

	}

	void GenerateRow() {
		Debug.Log("TIMER: " + timer);
		for(int i = 0; i < columns.Length; i++) {
			Person[] people = columns[i].GetComponentsInChildren<Person>();
			for(int j = 0; j < people.Length; j++) {
				people[j].timesUp = true;
			}
		}
		timer = 0f;
		served = 0;
		spawned = 0;
		bugging = false;

		for (int i = 0; i < columns.Length; i++) {
			if (spawned < numberToSpawn) {
				int shouldSpawnTest = Random.Range(0,100);
				if(shouldSpawnTest > 50) {
					spawnCustomer(customers[Random.Range(0,numberOfTeas)], columns[i].transform.position, columns[i]);
					spawned++;
				}
			}
		}
	}

	public void StartSpawning() {
		GenerateRow();
		shouldSpawn = true;
	}
	private void spawnCustomer(GameObject go, Vector3 place, GameObject parent) {
		place.y -= (Camera.main.orthographicSize * 2f);
		GameObject o = (GameObject) Instantiate(go, place, transform.rotation);
		o.transform.parent = parent.transform;
		o.GetComponent<Person>().StartMoving();

	}

}
