using UnityEngine;
using System.Collections;

public class Hold : MonoBehaviour {
	private float timer;
	public PotControl pot;
	// Use this for initialization
	void Start () {
		timer = 300f;
	}
	
	// Update is called once per frame
	void Update () {
		timer--;

		if (Input.GetKey(KeyCode.UpArrow )){
			begin();
		}
		if (timer < 0) {
			begin();
		}
	}

	void begin() {
		pot.pour();
		timer = 300;
		gameObject.SetActive(false);

	}
}
