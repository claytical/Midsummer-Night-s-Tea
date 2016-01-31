using UnityEngine;
using System.Collections;

public class EndGame : MonoBehaviour {
	public PotControl pot;
	public GameObject holdUp;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void restart() {
		pot.refresh();
		holdUp.SetActive(true);
	}

	public void backtoMain() {
		pot.refresh();
	}


}
