using UnityEngine;
using System.Collections;

public class Couple : MonoBehaviour {
	public SpriteRenderer[] shirts;
	public Color[] colors;

	// Use this for initialization
	void Start () {
		int t = Random.Range(0, colors.Length);
		for (int i = 0; i < shirts.Length; i++) {
			shirts[i].color = colors[t];
		}
		gameObject.GetComponentInParent<CoupleCup>().type = t;

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
