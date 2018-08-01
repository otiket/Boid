using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {

    private GameObject bird;
	// Use this for initialization
	void Start () {
        bird = GameObject.Find("Player");
	}
	
	// Update is called once per frame
	void Update () {
        //Vector3 center = GameManager.GetCenter();
        //transform.position = new Vector3(center.x, center.y, -10);
        transform.position = new Vector3(bird.transform.position.x, bird.transform.position.y, -10);
	}
}
