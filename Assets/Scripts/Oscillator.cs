﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour {

    [SerializeField] Vector3 movementVector = new Vector3(10f, 10f, 10f);
    [SerializeField] float period = 2f;

    [Range(0,1)] [SerializeField]
    const float tau = Mathf.PI * 2;

    [SerializeField] Vector3 startingPos;

	// Use this for initialization
	void Start () {
        startingPos = transform.position;
    }
	
	// Update is called once per frame
	void Update ()
    {
        var cycle = Time.time / period;
        
        float rawSinWave = Mathf.Sin(cycle * tau);
        var movementFactor = rawSinWave / 2f + 0.05f;
        Vector3 offset = movementVector * movementFactor;
        transform.position = startingPos + offset;
	}
}
