using UnityEngine;
using System.Collections;

public class CupSpawner : MonoBehaviour {
	public GameObject[] cups;
	public GameObject[] couplesCups;
	public GameObject pot;
	public GameObject endPoint;
	public Wallpaper wallpaper;
	public bool endless = true;
	public int direction;
	public GameObject puckCock;
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

		if(!puckCock.GetComponent<AudioSource>().isPlaying && puckCock.activeSelf) {
			wallpaper.setDay();
			pot.GetComponent<PotControl>().backgroundMusic.Play();
			puckCock.SetActive(false);		
		}

		if (transform.childCount == 0) {
			NewCup();
		}

	}

	public void puckMode() {
		wallpaper.setNight();
		puckCock.SetActive(true);


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
		int selectedCup = (int) Random.Range(0, cups.Length);
		GameObject c = (GameObject)Instantiate(cups[selectedCup], pos, transform.rotation);
		if (cupCounter%5 == 0) {
			waitingTime *= .85f;
		}
		c.GetComponent<Cup>().waitingTime = waitingTime;
		c.GetComponent<Cup>().move(direction);
		c.transform.SetParent(gameObject.transform);

//		Invoke("NewCup", Random.Range(2.5f, 4f));
//		Invoke("NewCup", Random.Range(8f, 11f));
	}
}
