using UnityEngine;
using System.Collections;

public class CupCleaner : MonoBehaviour {
	public PotControl pot;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnCollisionEnter2D(Collision2D coll) {
		//TODO: UNDER POUR
		if (!coll.gameObject.GetComponentInParent<Cup>().served) {
			pot.feedbackMessage.text = "NOT ENOUGH!";
			pot.feedback.SetTrigger("show");
			pot.streak = 0;
			pot.strikes++;
		}
		Destroy(coll.transform.parent.gameObject);
	}
}
