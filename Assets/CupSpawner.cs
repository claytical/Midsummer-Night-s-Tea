using UnityEngine;
using System.Collections;

public class CupSpawner : MonoBehaviour {
	public GameObject[] cups;
	public GameObject[] couplesCups;
	public GameObject pot;
	public GameObject endPoint;
	public Wallpaper wallpaper;
	public bool endless = true;
	public bool puck = false;
	public int puckTimer;

	// Use this for initialization
	void Start () {
		if (endless) {
			Invoke("NewCup", 3.0f);
			}
		}
	
	// Update is called once per frame
	void Update () {
		if (puck) {
			puckTimer--;
			if (puckTimer <= 0) {
				puck = false;

			}
		}
	}

	public void puckMode() {
		puck = true;
		puckTimer = 200;
	}

	public void flip() {
		Vector3 newPos = new Vector3(-transform.position.x, transform.position.y);
		transform.position = newPos;
		Vector3 newPos2 = new Vector3(-endPoint.transform.position.x, endPoint.transform.position.y);
		endPoint.transform.position = newPos2;

	}

	void NewCup() {
		Vector3 pos = transform.position;
		if (puck) {
			int selectedCup = (int) Random.Range(0, couplesCups.Length);
			GameObject c = (GameObject)Instantiate(couplesCups[selectedCup], pos, transform.rotation);
			c.transform.SetParent(gameObject.transform);
		}
		else {
			int selectedCup = (int) Random.Range(0, cups.Length);
			GameObject c = (GameObject)Instantiate(cups[selectedCup], pos, transform.rotation);
			c.transform.SetParent(gameObject.transform);
		}

		Invoke("NewCup", Random.Range(2.5f, 4f));

	}
}
