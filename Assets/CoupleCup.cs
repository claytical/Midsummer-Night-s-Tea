using UnityEngine;
using System.Collections;

public class CoupleCup : MonoBehaviour {

	public float waitingTime;
	public int dropsCupA;
	public int dropsCupB;
	public SpriteRenderer[] fills;
	public int badDrops;
	public int type;

	private Vector3 waitingPosition;
	private Vector3 originalPosition;
	private PotControl pot;
	private int delayCheck;
	private bool served;
	private bool waiting;

	// Use this for initialization
	void Start () {
		originalPosition = transform.position; //15 and -15
		delayCheck = 0;
		pot = GetComponentInParent<CupSpawner>().pot.GetComponent<PotControl>();

	}
	
	// Update is called once per frame
	void Update () {
		Animator[] cups = GetComponentsInChildren<Animator>();
		cups[0].SetInteger("drops",dropsCupA);
		cups[1].SetInteger("drops",dropsCupB);

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


			if ((dropsCupA >= 6 && dropsCupB >= 6) || badDrops > 0 || waitingTime <= 0) {
				served = true;
			}

		}

	}

	public void move(int direction) {
		if (direction > 0) {
			waitingPosition = new Vector3(2f,transform.position.y,transform.position.z);
		}
		else {
			waitingPosition = new Vector3(-6f,transform.position.y,transform.position.z);
		}
		Debug.Log("WAITING POSITION: " + waitingPosition.x);
	}

	private void delayedCheck() {
		if (dropsCupA == 6 && dropsCupB == 6 && badDrops == 0) {
			pot.streak+=2;
			//PERFECT POUR
			if (pot.streak > 1) {
				pot.feedbackMessage.text = "PERFECT POUR x" + pot.streak + "!";
			}
			else {
				pot.feedbackMessage.text = "PERFECT POUR!";
			}
			pot.serve();
			pot.serve();
//			GetComponentInChildren<PersonPicker>().GetComponentInChildren<Person>().gameObject.GetComponent<Animator>().SetTrigger("smile");
			pot.perfectPour();
			pot.feedback.SetTrigger("show");
			Debug.Log("PERFECT POUR");
		}

		else if (badDrops > 1) {
			//WRONG TEA

			pot.wrongTea();
			pot.feedbackMessage.text = "WRONG TEA!";
			pot.feedback.SetTrigger("show");
			Debug.Log("WRONG TEA");
		}

		else if (dropsCupA > 6 && dropsCupA <= 8 && dropsCupB > 6 && dropsCupB <= 8) {
			pot.serve();
			pot.streak = 0;
		}

		else if(dropsCupA > 6 || dropsCupB > 6) {
			//OVER POUR
			pot.serve();
			pot.serve();
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
