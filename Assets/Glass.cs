using UnityEngine;
using System.Collections;

public class Glass : MonoBehaviour {
	public GameObject liquid;
	public BoxCollider2D waterLevel;
	public int drops;
	public bool overpour;
	public bool wrongtea;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void Fill(Color c) {
		//TODO: Check correct color liquid
		Color cc = GetComponentInParent<Person>().color;
		if(cc.r == c.r && cc.g == c.g && cc.b == c.b) {
		}
		else {
			wrongtea = true;

		}
		liquid.GetComponent<SpriteRenderer>().color = new Color(c.r, c.g, c.b);
		drops++;

		if(liquid.transform.localScale.y < 10) {
			liquid.transform.localScale += new Vector3(0, .8f, 0);
			waterLevel.offset += new Vector2(0, .2f);
		}

		if(drops > 12) {
			overpour = true;

			//TURN OFF STREAK
			transform.parent.GetComponentInParent<CustomerSpawner>().streakParticleSystem.SetActive(false);		
			transform.parent.GetComponentInParent<CustomerSpawner>().streak = false;
			transform.parent.GetComponentInParent<CustomerSpawner>().pot.backgroundMusicSnapshot.TransitionTo(.01f);


		}
		if (drops == 8) {
			transform.parent.GetComponentInParent<CustomerSpawner>().served++;
			if(gameObject.GetComponentInParent<AudioSource>()) {
				gameObject.GetComponentInParent<Animator>().SetTrigger("smile");
				gameObject.GetComponentInParent<AudioSource>().Play();
			}
		}
	}
}
