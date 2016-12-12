using UnityEngine;
using System.Collections;

public class Person : MonoBehaviour {
	public SpriteRenderer shirt;
	public Color[] colors;
	public Color color;
	public bool spawning = false;
	public bool timesUp = false;
	private bool buggedOut = false;
	public float pauseBeforeSpawning = 3f;
	private Vector3 origin;

	// Use this for initialization
	void Start () {
		color.a = 255;
		shirt.color = color;
		origin = transform.position;
	}

	public void StartMoving() {
		pauseBeforeSpawning += Time.time;
		spawning = true;
	}

	public void Bugout() {
		if(!buggedOut) {
			Glass glass = GetComponentInChildren<Glass>();
			if(glass.drops < 8) {
				gameObject.GetComponent<Animator>().SetTrigger("bugging");
			}
			buggedOut = true;
		}
	}

	public void Drink() {
		gameObject.GetComponent<Animator>().SetTrigger("perfect");
		gameObject.GetComponentInChildren<ParticleSystem>().Play();
	}

	public void Clear() {
		Destroy(gameObject);
	}

	// Update is called once per frame
	void Update () {
		if(spawning) {
			if(pauseBeforeSpawning < Time.time) {

				transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y),transform.parent.transform.position, 10 * Time.deltaTime);
				GameObject glass = GetComponentInChildren<Glass>().gameObject;

				if(glass.transform.localScale.x >= 2.5f) {
					glass.transform.localScale *= .99f;
				}
				if(transform.position.y >= 2) {
					spawning = false;
				}
			}
		}

		if(timesUp) {
			transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y), origin, 10 * Time.deltaTime);
			if(transform.position.y <= -5) {
				Destroy(gameObject);
			}

		}
	}
		
}
