using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour {

	public GameObject platformRes;

	public float speed = 7;

	// Gather screen properties
	Camera cam;
	float camHeight;
	float camWidth;

	// Calculate spawn and destroy boundaries
	float spawnBoundary;
	float destroyBoundary;

	// List of active platform containers
	List<GameObject> platforms = new List<GameObject> ();

	// Use this for initialization
	void Start () {
		// Gather screen properties
		cam = Camera.main;
		camHeight = 2f * cam.orthographicSize;
		camWidth = camHeight * cam.aspect;

		// Calculate spawn and destroy boundaries
		spawnBoundary = (camWidth / 2) + 2;
		destroyBoundary = -spawnBoundary;

		CreatePlatform ();
	}

	// Update is called once per frame
	void Update () {
		SpriteRenderer lpsr = LastPlatform ().GetComponent<SpriteRenderer> ();
		if (lpsr.bounds.max.x < spawnBoundary) {
			CreatePlatform ();
		}

		PrunePlatforms ();
	}

	void CreatePlatform () {
		GameObject container = Instantiate (platformRes, new Vector2 (), Quaternion.identity);
		PlatformController platform = container.GetComponent<PlatformController> ();
		// Create the first plaform
		if (platforms.Count == 0) {
			platform.Build (destroyBoundary, spawnBoundary, false);
		} else {
			SpriteRenderer lpsr = LastPlatform ().GetComponent<SpriteRenderer> ();
			float width = Random.Range (1, 64);
			platform.Build (lpsr.bounds.max.x + 5, lpsr.bounds.max.x + 5 + width, true);
		}
		platform.SetSpeed (speed);
		platforms.Add (container);
	}

	void PrunePlatforms () {
		while (FirstPlatform ().GetComponent<SpriteRenderer> ().bounds.max.x < destroyBoundary) {
			GameObject first = FirstPlatform ();
			platforms.Remove (first);
			Destroy (first);
		}
	}

	GameObject FirstPlatform () {
		return platforms [0];
	}
	GameObject LastPlatform () {
		return platforms [platforms.Count - 1];
	}
}
