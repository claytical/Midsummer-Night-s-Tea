using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Hold : MonoBehaviour {
	private float timer;
	public AudioSource[] loops;
	public PotControl pot;
	public Text countdown;
	// Use this for initialization
	void Start () {
		timer = 300f;
	}
	
	// Update is called once per frame
	void Update () {
		/*
		timer--;

		if (Input.anyKey){
			begin();
		}

		if (timer < 0) {
			begin();
		}
		*/
	}

	public void updateCountdown(string time) {
		countdown.text = time;
	}

	void begin() {
		for(int i = 0; i < loops.Length; i++) {
			loops[i].Play();
		}
		pot.Begin();
		pot.backgroundMusicSnapshot.TransitionTo(.01f);
		pot.spawner.StartSpawning();
		timer = 300;
		gameObject.SetActive(false);

	}
}
