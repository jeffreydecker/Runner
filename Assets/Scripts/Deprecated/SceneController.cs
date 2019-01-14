using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour {

	// public GameObject ground;

	// Use this for initialization
	void Start () {
		var ratio = 1f;
		if (Screen.width > Screen.height) {
			ratio = (float) Screen.width / Screen.height;
		} else if (Screen.height > Screen.width) {
			ratio = (float) Screen.height / Screen.width;
		}

		Debug.Log ("w: " + Screen.width + " h: " + Screen.height);
		Debug.Log ("r: " + ratio);

		Camera cam = Camera.main;
		float height = 2f * cam.orthographicSize;
		float width = height * cam.aspect;

		Debug.Log ("o: " + Screen.orientation);
		Debug.Log ("orth: " + Camera.main.orthographicSize);

		//var width = Camera.main.orthographicSize * 2f * ratio;// Screen.height / Screen.width;
		Debug.Log ("width: " + width);

		// ground.GetComponent<SpriteRenderer> ().size = new Vector2 (width, 1f);
		// ground.GetComponent<BoxCollider2D> ().size = new Vector2 (width, 1f);
	}
	
	// Update is called once per frame
	void Update () {
	}
}
