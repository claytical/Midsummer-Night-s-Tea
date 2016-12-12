using UnityEngine;
using System.Collections;

public class Droplet : MonoBehaviour {
	public int dropletType;
	private float spawnTime;
	// Use this for initialization
	void Start () {
		spawnTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		if(Time.time > spawnTime + 1f) {
			Destroy(this.gameObject);
		}
	}

	void OnCollisionEnter2D(Collision2D coll) {
		if(coll.gameObject.tag == "glass") {
			Glass glass = coll.gameObject.GetComponentInParent<Glass>();
			glass.Fill(GetComponent<SpriteRenderer>().color);
			Destroy(this.gameObject);
		}

		if (coll.gameObject.tag == "cup") {
			Cup cup = coll.gameObject.GetComponentInParent<Cup>();
//			PotControl pot = coll.gameObject.GetComponentInParent<CupSpawner>().pot.GetComponent<PotControl>();
//			pot.droplet[dropletType].GetComponent<SpriteRenderer>().color
			cup.fill.color = GetComponent<SpriteRenderer>().color;
			if (cup.type == dropletType) {
				cup.drops++;
			}
			else {
				cup.badDrops++;
			}
			Destroy(this.gameObject);

		}
		if (coll.gameObject.tag == "outside") {
			Destroy(this.gameObject);

		}

		if (coll.gameObject.tag == "cupA" || coll.gameObject.tag == "cupB") {
			
			CoupleCup cup = coll.gameObject.transform.parent.parent.GetComponent<CoupleCup>();
		
			if (cup.type == dropletType) {
				if (coll.gameObject.tag == "cupA") {
					cup.fills[0].color = GetComponent<SpriteRenderer>().color;
					cup.dropsCupA++;
				}
				if (coll.gameObject.tag == "cupB") {
					cup.fills[1].color = GetComponent<SpriteRenderer>().color;
					cup.dropsCupB++;
				}
			}
			else {
				cup.badDrops++;
			}
			Destroy(this.gameObject);

		}
	}
}
