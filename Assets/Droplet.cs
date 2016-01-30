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
			Cup cup = coll.gameObject.GetComponentInParent<Cup>();
			//TODO: Check Tea Type
			if (cup.type == dropletType) {
				cup.drops++;
				if (cup.drops  >= 5 && cup.drops <= 8 && !cup.served) {
					if (!cup.served) {
						cup.GetComponentInParent<CupSpawner>().pot.GetComponent<PotControl>().cupsServed++;
						cup.GetComponent<Animator>().SetTrigger("perfect");
						cup.served = true;
						Debug.Log("Cup Served!");			
					}
				}
			}
			else {
				Debug.Log("Wrong tea!");
			}
			if (cup.drops > 8) {
				Debug.Log("Over Pour!");
			}
			//TODO: when cup collides, check drop amount for underpour
			Destroy(this.gameObject);

		}
		if (coll.gameObject.tag == "outside") {
			Destroy(this.gameObject);
		}
	}
}
