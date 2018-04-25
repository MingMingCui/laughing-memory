using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRotate : MonoBehaviour {

    public bool RotateX = false;
    public bool RotateY = false;
    public bool RotateZ = false;
    public float Speed = 1.0f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        float delta = Speed * Time.deltaTime;
        this.transform.Rotate(RotateX ? delta : 0, RotateY ? delta : 0, RotateZ ? delta : 0);
	}
}
