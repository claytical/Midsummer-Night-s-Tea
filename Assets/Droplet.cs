using UnityEngine;
using System.Collections;

public class Droplet : MonoBehaviour {
	public int dropletType;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.tag == "cup") {
			Debug.Log("Cup Collision");
//			coll.gameObject.GetComponentInParent<CupSpawner>().spills = 0;

			Cup cup = coll.gameObject.GetComponentInParent<Cup>();
			PotControl pot = coll.gameObject.GetComponentInParent<CupSpawner>().pot.GetComponent<PotControl>();
			//TODO: Check Tea Type
			if (cup.type == dropletType) {
				cup.drops++;
				if (cup.drops  >= 4 && cup.drops <= 6 && !cup.served) {
					if (!cup.served) {
						pot.GetComponent<PotControl>().cupsServed++;
						if (cup.badDrops == 0) {
							pot.streak++;
							pot.perfectPour();
//							cup.GetComponent<Animator>().SetTrigger("perfect");
							cup.GetComponentInChildren<PersonPicker>().GetComponentInChildren<Person>().gameObject.GetComponent<Animator>().SetTrigger("smile");
							if (pot.streak > 1) {
								pot.feedbackMessage.text = "PERFECT POUR x" + pot.streak + "!";
								//TODO: PERFECT POUR STREAK > 5 PUCK MODE
								pot.cupSpawner.GetComponent<CupSpawner>().puckMode();
							}
							else {
								pot.feedbackMessage.text = "PERFECT POUR!";
							}
							pot.feedback.SetTrigger("show");
						}
							cup.served = true;
						if (pot.cupsServed%7 == 0) {
							pot.feedbackMessage.text = "REFILL!";
							pot.refill();
							pot.dropsLeft += 100;
							pot.feedback.SetTrigger("show");
						}
						Debug.Log("Cup Served!");			
					}
				}
			}
			else {
				Debug.Log("Wrong tea!");
				pot.wrongTea();
				pot.feedbackMessage.text = "WRONG TEA!";
				pot.feedback.SetTrigger("show");

				cup.badDrops++;
				pot.streak = 0;
			}
			//TODO: delay check on perfect pour to allow for over pours. requires architecture update on cup or pot.
			/*
			if (cup.drops > 8) {
				pot.feedbackMessage.text = "OVER POUR!";
				pot.feedback.SetTrigger("show");

				Debug.Log("Over Pour!");
				pot.streak = 0;
			}
			*/
			//TODO: when cup collides, check drop amount for underpour
			Destroy(this.gameObject);

		}
		if (coll.gameObject.tag == "outside") {
/*			Counter counter =coll.gameObject.GetComponent<Counter>();

			counter.spills++;
			counter.totalSpills++;
			if (counter.spills > 5) {
				Debug.Log("SPILLAGE!");
			}
*/
			Destroy(this.gameObject);



		}
	}
}
