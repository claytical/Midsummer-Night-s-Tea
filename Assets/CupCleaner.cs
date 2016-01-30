using UnityEngine;
using System.Collections;

public class CupCleaner : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnCollisionEnter2D(Collision2D coll) {
		//TODO: UNDER POUR
		Destroy(coll.transform.parent.gameObject);
	}
}
