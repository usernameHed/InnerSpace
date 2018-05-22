using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtTarget : MonoBehaviour {

    [SerializeField]
    private Transform target;
	
	// Update is called once per frame
	void FixedUpdate () {
        transform.LookAt(target);
	}
}
