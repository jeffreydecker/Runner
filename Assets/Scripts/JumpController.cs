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

	#region Public Vars

	public Text text;
	public Text timeText;

	/* TODO
	 * - Reset can double jump when is grounded
	 */

	public float jumpForce = 350;

	public bool isGrounded = false;
	
	public bool canDoubleJump = false;

	#endregion

	#region Private Vars

	// Jump Height Tracking
	float groundedHeight = float.MaxValue;
	float maxHeight = float.MinValue;

	// Air Time Tracking
	float jumpTime;

	// Quick Refs
	Rigidbody2D rb2d;
	SpriteRenderer spriteRen;

	#endregion

	// Use this for initialization
	void Start () {
		rb2d = gameObject.GetComponentInParent<Rigidbody2D>();
		spriteRen = gameObject.GetComponentInParent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Space)) {
			if (isGrounded || canDoubleJump) {
				canDoubleJump = isGrounded;
				rb2d.velocity = new Vector2();
				rb2d.AddRelativeForce (Vector2.up * jumpForce);
				if (isGrounded) {
					jumpTime = Time.time;
				}
			}
		}

		if (isGrounded) {
			spriteRen.color = Color.green;
		} else if (canDoubleJump) {
			spriteRen.color = Color.yellow;
		} else {
			spriteRen.color = Color.red;
		}
	}

	private void FixedUpdate () {
		maxHeight = Mathf.Max (maxHeight, transform.position.y);
	}

	void OnTriggerEnter2D(Collider2D collision) {

		isGrounded = true;
		canDoubleJump = true;
 		ShowAndClearJumpHeight ();
		ShowAndClearJumpTime ();
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

	void ShowAndClearJumpTime() {
		float airTime = Time.time - jumpTime;
		timeText.text = "" + airTime;
	}
}
