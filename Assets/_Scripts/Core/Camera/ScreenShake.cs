﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour {

    [SerializeField] private  float screenBump;
    [SerializeField] private float shakeDuration;

    private float shakeTimer;
    private Vector3 camPosition;
    private float xPos;
    private float yPos;

	// Use this for initialization
	void Start ()
    {
        camPosition = transform.localPosition;
        shakeTimer = 0;
        xPos = 0;
        yPos = 0;
	}
   
    public void Shake()
    {
        Debug.Log("ici shake la camera");
        /*lock(this)
        {
            if (shakeTimer <= shakeDuration)
            {
                xPos = Random.Range(-1, 2) * screenBump;
                yPos = Random.Range(-1, 2) * screenBump;

                transform.localPosition = new Vector3(xPos, yPos, transform.localPosition.z);

                shakeTimer++;
                StartCoroutine(ShakeWaiting());
            }
            else
            {
                transform.localPosition = camPosition;
                shakeTimer = 0;
            }
        }*/
    }
   
    IEnumerator ShakeWaiting()
    {
        yield return new WaitForSeconds(0.05f);
        Shake();
        yield return null;
    }
}

