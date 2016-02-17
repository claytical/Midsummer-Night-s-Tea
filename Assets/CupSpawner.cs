using UnityEngine;
using System.Collections;

public class CupSpawner : MonoBehaviour {
	public GameObject[] cups;
	public GameObject[] couplesCups;
	public GameObject pot;
	public GameObject endPoint;
	public bool couples = false;
	public int direction;
	public float waitingTime;
	private float originalWaitingTime;
	private int cupCounter;

	// Use this for initialization
	void Start () {
		cupCounter = 0;
		originalWaitingTime = waitingTime;
	}
	
	// Update is called once per frame
	public void setWaitingTime(float wait) {
		waitingTime = wait;
		originalWaitingTime = wait;
	}

	public void resetWaitingTime() {
		cupCounter = 0;
		waitingTime = 8f;
	}

	void Update () {

		if (transform.childCount == 0) {
			NewCup();
		}

	}
		
	public void flip() {
		Vector3 newPos = new Vector3(-transform.position.x, transform.position.y);
		transform.position = newPos;
		Vector3 newPos2 = new Vector3(-endPoint.transform.position.x, endPoint.transform.position.y);
		endPoint.transform.position = newPos2;

	}

	void NewCup() {
		cupCounter++;
		Vector3 pos = transform.position;

		GameObject c;
		if (couples) {
			int selectedCup = (int) Random.Range(0, couplesCups.Length);
			c = (GameObject)Instantiate(couplesCups[selectedCup], pos, transform.rotation);
			c.GetComponent<CoupleCup>().waitingTime = waitingTime;
			c.GetComponent<CoupleCup>().move(direction);
		}
		else {
			int selectedCup = (int) Random.Range(0, cups.Length);
			c = (GameObject)Instantiate(cups[selectedCup], pos, transform.rotation);
			c.GetComponent<Cup>().waitingTime = waitingTime;
			c.GetComponent<Cup>().move(direction);
		}

		if (cupCounter%5 == 0) {
			waitingTime *= .85f;
		}
		c.transform.SetParent(gameObject.transform);

//		Invoke("NewCup", Random.Range(2.5f, 4f));
//		Invoke("NewCup", Random.Range(8f, 11f));
	}
}
