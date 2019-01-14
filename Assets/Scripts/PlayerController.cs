using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public GameController gameController;

	Rigidbody2D rb2d;

	// Use this for initialization
	void Start () {
		rb2d = GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ObstacleTriggerEnter() {
		Debug.Log ("PLAYER DIED! Trigger entered for obstacle");
		gameController.HandlePlayerDeath ();
		rb2d.bodyType = RigidbodyType2D.Static;
	}
}
