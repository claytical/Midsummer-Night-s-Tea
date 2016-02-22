﻿using UnityEngine;
using System.Collections;

public class Cup : MonoBehaviour {
	public int drops;
	public int badDrops;
	public bool served = false;
	public float waitingTime;
	public bool waiting;
	public SpriteRenderer fill;
	public int type;

	private Vector3 waitingPosition;
	private Vector3 originalPosition;
	private int delayCheck;
	private PotControl pot;
	// Use this for initialization

	void Start () {
		originalPosition = transform.position;
		delayCheck = 0;
		pot = GetComponentInParent<CupSpawner>().pot.GetComponent<PotControl>();

	}

	public void move(int direction) {
		waitingPosition = new Vector3(Random.Range(direction * 5, direction),transform.position.y,transform.position.z);
	
	}
	// Update is called once per frame
	void Update () {
		GetComponent<Animator>().SetInteger("drops", drops);
		if (served) {
			//Move back to original spot
			transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y),originalPosition, 8 * Time.deltaTime);
			if (transform.position == originalPosition) {
				Destroy(gameObject);
			}

			if (delayCheck == 20) {
				delayedCheck();
			}
			delayCheck++;
		}
		else {
			//Move to play area
			if (transform.position == waitingPosition && !waiting) {
				waiting = true;
			}
			else {
				transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y),waitingPosition, 10 * Time.deltaTime);
			}
				
			//Count down wait time
			if (waiting) {
				waitingTime -= Time.deltaTime;
			}
		
			if (drops >= 6 || badDrops > 0 || waitingTime <= 0) {
				served = true;
				GetComponentInChildren<PersonPicker>().GetComponentInChildren<Person>().gameObject.GetComponent<Animator>().SetTrigger("smile");

			}

		
		}


	}


	private void delayedCheck() {
		if (drops == 6 && badDrops == 0) {
			pot.streak++;
			//PERFECT POUR
			if (pot.streak > 1) {
				pot.feedbackMessage.text = "PERFECT POUR x" + pot.streak + "!";
			}
			else {
				pot.feedbackMessage.text = "PERFECT POUR!";
			}
			pot.serve();
			pot.perfectPour();
			pot.feedback.SetTrigger("show");
			Debug.Log("PERFECT POUR");
			if(pot.streak%3==0) {
				pot.puckMode(true);
			}
		}

		else if (badDrops > 1) {
			//WRONG TEA

			pot.wrongTea();
			pot.feedbackMessage.text = "WRONG TEA!";
			pot.feedback.SetTrigger("show");
			pot.puckMode(false);
			Debug.Log("WRONG TEA");
		}
		else if (drops > 6 && drops <= 7) {
			pot.serve();
			pot.streak = 0;
		}
		else if(drops >= 8) {
			//OVER POUR
			pot.serve();
			pot.overPour();
			pot.feedbackMessage.text = "OVER POUR!";
			pot.feedback.SetTrigger("show");
			pot.streak = 0;
			Debug.Log("OVER POURED");
		}
		else {
			//Too late
//			pot.streak = 0;
			pot.missed();
			Debug.Log("WAITED TOO LONG");
		}

	}
		
}