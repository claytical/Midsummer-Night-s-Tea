using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.UI;

public class PotControl : MonoBehaviour {
	public GameObject[] droplet;
	public int selectedDroplet = 0;
	public GameObject spigot;
	public Text teaLeft;
	public Text served;
	public Text endServed;
	public Text failureMessage;
	public GameObject completePanel;
	public GameObject[] cupSpawner;
	public AudioSource backgroundMusic;
	public AudioClip perfectPourFx;
	public AudioClip refillFx;
	public AudioClip wrongTeaFx;
	public AudioClip comboFx;

	public Animator feedback;
	public Animator hud;
	public Text feedbackMessage;
	private int counter = 0;
	public int streak = 0;
	public int startingPourAngle = 255;
	public int endingPourAngle = 325;
	public int cupsServed = 0;
	public int strikes = 0;
	public int dropsLeft = 100;
	public float tiltSpeed = 1.5f;
	public bool gravity = false;
	public bool pouring = false;
	private float gravitySpeed = .9f;
	private float failTimer;


	// Use this for initialization
	void Start () {
		refresh();
	}
	public void perfectPour() {
		if (streak > 1) {
			GetComponent<AudioSource>().PlayOneShot(comboFx,2f);

		}
		else {
			GetComponent<AudioSource>().PlayOneShot(perfectPourFx,2f);
		}
	}
		
	public void serve() {
		cupsServed++;
		if (cupsServed%7 == 0) {
			feedbackMessage.text = "REFILL!";
			refill();
			dropsLeft += 75;
			feedback.SetTrigger("show");
		}

	}

	public void refill() {
		GetComponent<AudioSource>().PlayOneShot(refillFx);
		hud.SetTrigger("normal");
	}

	public void refresh() {
		pouring = false;
		failTimer = 9999f;

		hud.SetTrigger("normal");
		dropsLeft = 100;
		cupsServed = 0;
		served.text = cupsServed.ToString();
		teaLeft.text = dropsLeft.ToString();
		streak = 0;
		selectedDroplet = 0;
		clearCups();
		Color c = droplet[selectedDroplet].GetComponent<SpriteRenderer>().color;
		c.a = 100f;
		GetComponent<SpriteRenderer>().color = c;

	}

	private void clearCups() {
		for (int i = 0; i < cupSpawner.Length; i++) {
			foreach (Transform childTransform in cupSpawner[i].transform) {
				Destroy(childTransform.gameObject);
			}
			cupSpawner[i].GetComponent<CupSpawner>().resetWaitingTime();
			cupSpawner[i].SetActive(false);
		}

	}

	public void wrongTea() {
		clearCups();
		backgroundMusic.Stop();
		GetComponent<AudioSource>().PlayOneShot(wrongTeaFx,2f);
	
		failureMessage.text = "WRONG TEA!";
		endServed.text = served.text + " Customers Served";
		completePanel.SetActive(true);
		pouring = false;
	}

	public void outOfTea() {
		GetComponent<AudioSource>().PlayOneShot(wrongTeaFx,2f);
		clearCups();
		backgroundMusic.Stop();
		failureMessage.text = "OUT OF TEA!";
		endServed.text = served.text + " Customers Served";
		completePanel.SetActive(true);
		pouring = false;

	}

	public void missed() {
		GetComponent<AudioSource>().PlayOneShot(wrongTeaFx,2f);

		clearCups();
		failureMessage.text = "TOO SLOW";
		backgroundMusic.Stop();
		endServed.text = served.text + " Customers Served";
		completePanel.SetActive(true);
		pouring = false;

	}
	// Update is called once per frame

	void Update () {
		if (pouring) {
			if (dropsLeft <= 0) {
				failTimer -= Time.deltaTime;
			}
			if (failTimer <= 0) {
				//FAIL
				outOfTea();
			}
			if (dropsLeft == 15) {
				hud.SetTrigger("low");
			}
			if(Input.GetKeyDown("space")) {
				selectedDroplet++;
				if (selectedDroplet >= droplet.Length) {
					selectedDroplet = 0;


				}

				if (dropsLeft == 10) {
					feedbackMessage.text = "RUNNING OUT OF TEA!";
					feedback.SetTrigger("show");
				}
				Color c = droplet[selectedDroplet].GetComponent<SpriteRenderer>().color;
				c.a = 100f;
				GetComponent<SpriteRenderer>().color = c;


			}


			if (gravity) {
				if (Input.GetKey(KeyCode.UpArrow )){
					if (transform.rotation.z > 0) {
						gravitySpeed = 0f;
					}
					else {
						gravitySpeed = -1.9f;
					}
				}
				else {
					if (transform.rotation.z <= -.7) {
						gravitySpeed = 0f;
					}
					else {
						gravitySpeed = 1.9f;
					}
				}

				Vector3 pos = new Vector3(transform.position.x,transform.position.y,transform.position.z);

				if(Input.GetKey(KeyCode.LeftArrow)) {
					if (transform.position.x > -7) {
						pos.x -= .1f;
					}
				}

				if(Input.GetKey(KeyCode.RightArrow)) {
					if (transform.position.x < 7) {
						pos.x +=.1f;
					}
				}

				transform.position = pos;


				gravitySpeed *= 1.9f;
				Vector3 rot = new Vector3(0,0, -1 * gravitySpeed);
				transform.Rotate(rot);	

			
			
			}
			else {
				float h = CrossPlatformInputManager.GetAxis("Horizontal") * tiltSpeed;
				Vector3 rot = new Vector3(0,0,h);
				transform.Rotate(rot);
			}
				
			if (transform.rotation.eulerAngles.z <= endingPourAngle && transform.rotation.eulerAngles.z >= startingPourAngle && dropsLeft > 0) {
				counter++;
				int stepper = (int) map(transform.rotation.eulerAngles.z, startingPourAngle, endingPourAngle, 1, 15);
				if (counter%stepper == 0) {
					if (dropsLeft > 0) {
						dropsLeft--;
						if (dropsLeft == 0) {
								failTimer = 3f;
						}
						Vector3 pos = spigot.transform.position;
						Vector3 upright = new Vector3(0,0,0);
						Quaternion qua = new Quaternion(0,0,0,0);
						GameObject tea = (GameObject)Instantiate(droplet[selectedDroplet], pos, qua);	
					}
				}
			}
			served.text = cupsServed.ToString();
			teaLeft.text = dropsLeft.ToString();
		}
	}

	public void pour() {
		pouring = true;
		for (int i = 0; i < cupSpawner.Length; i++) {
			cupSpawner[i].SetActive(true);

		}
		backgroundMusic.Play();

	}

	float map(float s, float a1, float a2, float b1, float b2)
	{
		return b1 + (s-a1)*(b2-b1)/(a2-a1);
	}
}
