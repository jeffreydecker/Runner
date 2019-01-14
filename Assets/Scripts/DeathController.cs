using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathController : MonoBehaviour {

	public PlayerController playerController;

	// Use this for initialization
	void Start () {
		playerController = gameObject.GetComponentInParent<PlayerController> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnTriggerEnter2D (Collider2D collision) {
		if (collision.gameObject.tag == "Obstacle" 
			|| collision.gameObject.tag == "Ground") {
			playerController.ObstacleTriggerEnter ();
		}
	}
}
