using UnityEngine;
using System.Collections;

public class Person : MonoBehaviour {
	public SpriteRenderer shirt;
	public Color[] colors;

	// Use this for initialization
	void Start () {
		int t = Random.Range(0, colors.Length);
		shirt.color = colors[t];
		gameObject.GetComponentInParent<PersonPicker>().GetComponentInParent<Cup>().type = t;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
		
}
