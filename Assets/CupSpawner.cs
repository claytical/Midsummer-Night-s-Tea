using UnityEngine;
using System.Collections;

public class CupSpawner : MonoBehaviour {
	public GameObject[] cups;
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
		int selectedCup = (int) Random.Range(0, cups.Length);
		GameObject c = (GameObject)Instantiate(cups[selectedCup], pos, transform.rotation);	
		c.transform.SetParent(gameObject.transform);

		Invoke("NewCup", Random.Range(1.5f, 4f));

	}
}
