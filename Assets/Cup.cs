﻿using UnityEngine;
using System.Collections;

public class Cup : MonoBehaviour {
	public int drops;
	public float speed = .1f;
	public bool served = false;
	public int type;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		transform.position += -transform.right * speed;
	}


}
