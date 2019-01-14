using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Note: This script can be used aas a "FlightController" for a flappy bird style game
// if the check for isGrounded and canDoubleJump is removed. Tweeks may need to be made
// to gravity on object and force of "jump" or in that case "flap". I should make that
// game also, with a slight variation that there may be multiple openings and when there
// are, one will have a collectable item in it.

public class JumpController : MonoBehaviour {

	public Text text;

	/* TODO
	 * - Reset can double jump when is grounded
	 */

	public float jumpForce = 300;

	public bool isGrounded = false;
	
	public bool canDoubleJump = false;

	float groundedHeight = float.MaxValue;
	float maxHeight = float.MinValue;

	// Refs
	Rigidbody2D rb2d;
	
	// Use this for initialization
	void Start () {
		rb2d = gameObject.GetComponentInParent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Space)) {
			if (isGrounded || canDoubleJump) {
				canDoubleJump = isGrounded;
				rb2d.velocity = new Vector2();
				rb2d.AddRelativeForce (Vector2.up * jumpForce);
			}
		}
	}

	private void FixedUpdate () {
		maxHeight = Mathf.Max (maxHeight, transform.position.y);
	}

	void OnTriggerEnter2D(Collider2D collision) {
		isGrounded = true;
		canDoubleJump = false;
 		ShowAndClearJumpHeight ();
	}

	void OnTriggerExit2D (Collider2D collision) {
		isGrounded = false;
	}

	void ShowAndClearJumpHeight() {
		float jumpHeight = Mathf.Abs (maxHeight - groundedHeight);
		text.text = "" + jumpHeight;
		maxHeight = float.MinValue;
		groundedHeight = transform.position.y;
	}
}
