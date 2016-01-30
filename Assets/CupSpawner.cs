using UnityEngine;
using System.Collections;

public class CupSpawner : MonoBehaviour {
	public GameObject cup;
	public GameObject pot;
	public bool endless = false;

	// Use this for initialization
	void Start () {
		if (endless) {
			Invoke("NewCup", 3.0f);
			}
		}
	
	// Update is called once per frame
	void Update () {
	
	}

	void NewCup() {
		Vector3 pos = transform.position;
		GameObject c = (GameObject)Instantiate(cup, pos, transform.rotation);	
		c.transform.SetParent(gameObject.transform);

		Invoke("NewCup", 3.0f);

	}
}
