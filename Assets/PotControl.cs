using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.UI;
using UnityEngine.Audio;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class PotControl : MonoBehaviour {
	public GameObject[] droplet;
	public int selectedDroplet = 0;
	public int numberOfTeas;
	public GameObject spigot;
	public Text served;
	public Text endServed;
	public Text failureMessage;
	public GameObject completePanel;
	public GameObject gameControl;
	public CustomerSpawner spawner;
	public AudioClip perfectPourFx;
	public AudioClip refillFx;
	public AudioClip wrongTeaFx;
	public AudioClip comboFx;
	public AudioClip overPourFx;
	public AudioClip outOfTeaFx;
	public AudioClip missedFx;
	public Wallpaper wallpaper;

	public Animator feedback;
	public Animator hud;
	public Text feedbackMessage;
	public int streak = 0;
	public int startingPourAngle = 255;
	public int endingPourAngle = 325;
	public int cupsServed = 0;
	public int strikes = 0;
	public int dropsLeft = 100;
	public float tiltSpeed = 1.5f;
	public bool gravity = false;
	public bool pouring = false;
	private bool gameOver = false;

	private float gravitySpeed = .9f;
	private float failTimer;
	private int counter = 0;
	private float debounce = 0.0f;
	public float repeat = 1f;
	private bool pressingPourButton;

	public AudioMixerSnapshot backgroundMusicSnapshot;
	public AudioMixerSnapshot poweredUpMusicSnapshot;
	public AudioMixerSnapshot musicOffSnapshot;
	public AudioMixerSnapshot backgroundMusicSnapshotAlarm;
	public AudioMixerSnapshot poweredUpMusicSnapshotAlarm;

	// Use this for initialization
	void Start () {
		PlayerPrefs.DeleteAll();
		refresh();
		musicOffSnapshot.TransitionTo(.01f);
		Debug.Log("MUSIC OFF SNAPSHOT");

	}

	public void perfectPour(int count) {
		streak = count;
		if (streak > 1) {
			GetComponent<AudioSource>().PlayOneShot(comboFx,2f);
			feedbackMessage.text = "PERFECT POUR x" + count;
		}
		else {
			GetComponent<AudioSource>().PlayOneShot(perfectPourFx,2f);
			feedbackMessage.text = "PERFECT POUR!";
		}
		feedback.SetTrigger("show");

	}
	public void overPour() {
		GetComponent<AudioSource>().PlayOneShot(overPourFx,2f);
	}
		
	public void serve() {
		cupsServed++;
		if (cupsServed%8 == 0) {
			feedbackMessage.text = "REFILL!";
			refill();
			dropsLeft += 95;
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
		gameOver = false;
		gameControl.SetActive(true);
//		backgroundMusicSnapshot.TransitionTo(.01f);
//		Debug.Log("BG SNAPSHOT");

		hud.SetTrigger("normal");
		dropsLeft = 105;
		cupsServed = 0;
		served.text = cupsServed.ToString();
		streak = 0;
		spawner.streak = false;
		spawner.streakLength = 0;
		spawner.longestStreak = 0;
		spawner.streakParticleSystem.SetActive(false);
		selectedDroplet = 0;
		Color c = droplet[selectedDroplet].GetComponent<SpriteRenderer>().color;
		c.a = 100f;
		GetComponent<SpriteRenderer>().color = c;


	}

	public void badPourFx() {
		GetComponent<AudioSource>().PlayOneShot(missedFx,2f);
	}

	public void wrongTea() {
		if (!gameOver) {
			GetComponent<AudioSource>().PlayOneShot(wrongTeaFx,2f);
			musicOffSnapshot.TransitionTo(.01f);
			Debug.Log("MUSIC OFF SNAPSHOT");
		}
		gameOver = true;
		spawner.shouldSpawn = false;
		failureMessage.text = "You must be exhausted, serving customers the wrong tea and all. Rest up. Tomorrow is a new day.";
		endServed.text = served.text + " Customers Served";
		completePanel.SetActive(true);
		gameControl.SetActive(false);
		pouring = false;
	}
		
	public void missed() {
		if(!gameOver) {
			GetComponent<AudioSource>().PlayOneShot(missedFx,2f);
			musicOffSnapshot.TransitionTo(.01f);
			Debug.Log("MUSIC OFF SNAPSHOT");

		}
		gameOver = true;
		spawner.shouldSpawn = false;
		failureMessage.text = "Better close up your shop, looks like you've got too many customers to handle. Rest up. Tomorrow is a new day.";
		endServed.text = served.text + " Customers Served";
		completePanel.SetActive(true);
		gameControl.SetActive(false);
		pouring = false;

	}

	public void lowTea() {
		feedbackMessage.text = "RUNNING OUT OF TEA!";
		feedback.SetTrigger("show");

	}
	// Update is called once per frame

	public void chooseTea() {
		selectedDroplet++;
		if (selectedDroplet >= numberOfTeas) {
			selectedDroplet = 0;
		}
		Color c = droplet[selectedDroplet].GetComponent<SpriteRenderer>().color;
		c.a = 100f;
		GetComponent<SpriteRenderer>().color = c;

	}

	public void selectTea(int tea) {
		selectedDroplet = tea;
		Color c = droplet[selectedDroplet].GetComponent<SpriteRenderer>().color;
		c.a = 100f;
		GetComponent<SpriteRenderer>().color = c;
	}

	public void move() {
		Vector3 pos = new Vector3(transform.position.x,transform.position.y,transform.position.z);

		if (transform.position.x > -8 || transform.position.x < 8) {
			if(Input.acceleration.y != 0) {
				pos.x += (Input.acceleration.y * .5f);
			}
			else {
				pos.x += (Input.GetAxis("Horizontal") * .5f);
			}
		}
		transform.position = pos;

	}
	public void moveLeft() {
		Vector3 pos = new Vector3(transform.position.x,transform.position.y,transform.position.z);

		if (transform.position.x > -8) {
			pos.x -= .1f;
		}
		transform.position = pos;

	}

	public void moveRight() {
		Vector3 pos = new Vector3(transform.position.x,transform.position.y,transform.position.z);
		if (transform.position.x < 8) {
			pos.x +=.1f;
		}
		transform.position = pos;

	}

	void Update () {
		if (pouring) {

			if(Input.GetButtonDown("Fire2")) {
				chooseTea();
			}
			move();

			if(!IsInvoking("CheckPour")) {
				InvokeRepeating("CheckPour",0,repeat);
			}

			if(Input.GetButton("Fire1") || pressingPourButton) {
				transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, -90), Time.deltaTime * 10f);
			}

			else {
				transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, 0), Time.deltaTime * 10f);
			}
				
			served.text = cupsServed.ToString();
		}
		else {
			CancelInvoke("CheckPour");
		}
	}

	public void pressingPour() {
		pressingPourButton = true;
	
	}

	public void stopPouring() {
		pressingPourButton = false;
	}

	void CheckPour() {
//		if(Input.GetButton("Fire1")) {
			if (transform.rotation.eulerAngles.z <= 290 && transform.rotation.eulerAngles.z  >= 200) {
				Pour();
//			}
		}

	}
	public void Pour() {
		if(Time.realtimeSinceStartup - debounce > repeat) { 
			Vector3 pos = spigot.transform.position;
			Quaternion qua = new Quaternion(0,0,0,0);
			GameObject tea = (GameObject)Instantiate(droplet[selectedDroplet], pos, qua);	
		}
	}

	public void Begin() {
		gameOver = false;
		pouring = true;
//		backgroundMusicSnapshot.TransitionTo(.01f);


	}

	float map(float s, float a1, float a2, float b1, float b2)
	{
		return b1 + (s-a1)*(b2-b1)/(a2-a1);
	}
}
