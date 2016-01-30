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
	public GameObject completePanel;
	private int counter = 0;
	public int startingPourAngle = 255;
	public int endingPourAngle = 325;
	public int cupsServed = 0;
	public int dropsLeft = 100;
	public float tiltSpeed = 1.5f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown("space")) {
			selectedDroplet++;
			if (selectedDroplet >= droplet.Length) {
				selectedDroplet = 0;
			}
			GetComponent<Animator>().SetInteger("type",selectedDroplet);

		}
		float h = CrossPlatformInputManager.GetAxis("Horizontal") * tiltSpeed;
		Vector3 rot = new Vector3(0,0,h);
		transform.Rotate(rot);
		if (transform.rotation.eulerAngles.z <= endingPourAngle && transform.rotation.eulerAngles.z >= startingPourAngle && dropsLeft > 0) {
			counter++;
			int stepper = (int) map(transform.rotation.eulerAngles.z, startingPourAngle, endingPourAngle, 1, 20);
			if (counter%stepper == 0) {
				Vector3 pos = spigot.transform.position;
				Vector3 upright = new Vector3(0,0,0);
				Quaternion qua = new Quaternion(0,0,0,0);
				GameObject tea = (GameObject)Instantiate(droplet[selectedDroplet], pos, qua);	
				dropsLeft--;
				if (dropsLeft <= 0) {
					Debug.Log("It's over");
					completePanel.SetActive(true);
				}

			}
		}
		served.text = cupsServed.ToString();
		teaLeft.text = dropsLeft.ToString();
	}

	float map(float s, float a1, float a2, float b1, float b2)
	{
		return b1 + (s-a1)*(b2-b1)/(a2-a1);
	}
}
