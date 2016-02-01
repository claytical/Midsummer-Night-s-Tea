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
	public GameObject completePanel;
	public GameObject startPanel;
	public GameObject cupSpawner;
	public AudioSource backgroundMusic;
	public AudioClip perfectPourFx;
	public AudioClip refillFx;
	public AudioClip wrongTeaFx;

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

	// Use this for initialization
	void Start () {
		Color c = droplet[selectedDroplet].GetComponent<SpriteRenderer>().color;
		c.a = 100f;
		GetComponent<SpriteRenderer>().color = c;

	}
	public void perfectPour() {
		GetComponent<AudioSource>().PlayOneShot(perfectPourFx);
	}
	public void wrongTea() {
		GetComponent<AudioSource>().PlayOneShot(wrongTeaFx);
	}

	public void refill() {
		GetComponent<AudioSource>().PlayOneShot(refillFx);
		hud.SetTrigger("normal");
	}

	public void refresh() {
		pouring = false;
		hud.SetTrigger("normal");
		dropsLeft = 100;
		cupsServed = 0;
		served.text = cupsServed.ToString();
		teaLeft.text = dropsLeft.ToString();
		streak = 0;
		selectedDroplet = 0;
		foreach (Transform childTransform in cupSpawner.transform) {
			Destroy(childTransform.gameObject);
		}
	}
	// Update is called once per frame
	void Update () {
		if (pouring) {

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

//				GetComponent<SpriteRenderer>().color = droplet[selectedDroplet].GetComponent<SpriteRenderer>().color;
				//GetComponent<Animator>().SetInteger("type",selectedDroplet);

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
					Vector3 pos = spigot.transform.position;
					Vector3 upright = new Vector3(0,0,0);
					Quaternion qua = new Quaternion(0,0,0,0);
					GameObject tea = (GameObject)Instantiate(droplet[selectedDroplet], pos, qua);	
					dropsLeft--;
					if (dropsLeft <= 0) {
						backgroundMusic.Stop();
						endServed.text = served.text + " Customers Served!";
						completePanel.SetActive(true);
						pouring = false;
					}

				}
			}
			served.text = cupsServed.ToString();
			teaLeft.text = dropsLeft.ToString();
		}
	}

	public void pour() {
		pouring = true;
		cupSpawner.SetActive(true);

	}

	float map(float s, float a1, float a2, float b1, float b2)
	{
		return b1 + (s-a1)*(b2-b1)/(a2-a1);
	}
}
