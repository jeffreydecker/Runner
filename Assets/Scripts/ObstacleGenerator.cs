using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleGenerator : MonoBehaviour {

	// --- Public --- //

	// How often an obstacle spawns
	//
	// To increase difficulty this can be dropped to 1.5 over time
	// Maybe we have a min and max where min and max are 2 to start
	// and over time min drops to 1.5.
	// Fluctuation is good though.
	public float cadence = 2.0f;

	// How fast do obstacles move in units per sec
	public float speed = 7.0f;

	// Obstacle resources
	public List<GameObject> obstacles;
	// How often they should spawn (float) relative to
	// the total of all frequencies.
	public List<float> spawnFrequencies;

	// Ground resources
	public List<GameObject> grounds;

	// --- Private --- //
	
	// Screen Properties
	Camera cam;
	float height;
	float width;

	// Trigger point to spawn more grounds
	float spawnBoundary;

	// Trigger point to destroy objects
	float destroyBoundary;

	// How long until the next object spawn
	float countDown = 0;

	// The total of all spawn frequencies for obstacles
	float frequencyTotal = 0;

	// All of the visible level objects
	List<GameObject> objects = new List<GameObject>();

	// Last ground block produced
	GameObject lastGround;

	// Use this for initialization
	void Start () {
		// Gather screen properties
		cam = Camera.main;
		height = 2f * cam.orthographicSize;
		width = height * cam.aspect;

		// Calculate spawn and destroy boundaries
		spawnBoundary = (width / 2) + 2;
		destroyBoundary = -spawnBoundary;
		
		// Get total spawn frequency for all obstacles
		foreach (float f in spawnFrequencies) {
			frequencyTotal += f;
		}

		// Lay initial ground
		GameObject colRes = Resources.Load<GameObject> ("GroundCollider");
		GameObject collider = Instantiate (colRes, new Vector3 (), Quaternion.identity) as GameObject;
		AddGround (destroyBoundary, spawnBoundary, collider);
	}
	
	// Update is called once per frame
	void Update () {
		AddGroundAfterLast();
		PruneObjects();
	}

	void PruneObjects() {
		while (objects.Count > 0 && objects[0].GetComponent<SpriteRenderer>().bounds.max.x < destroyBoundary) {
			Destroy(objects[0]);
			objects.RemoveAt(0);
		}
	}

	void InitGround () {

	}

	void CreateGround() {
		GameObject colRes = Resources.Load<GameObject> ("GroundCollider");
		GameObject collider = Instantiate (colRes, new Vector3 (), Quaternion.identity) as GameObject; 

		GameObject firstGround = null;

		while (lastGround == null || lastGround.GetComponent<SpriteRenderer>().bounds.min.x < spawnBoundary) {
			// Get a ground res
			int index = Random.Range(0, grounds.Count);
			GameObject groundRes = grounds[index] as GameObject;

			// Determine the position of the next ground tile
			float y = -0.5f;
			float x;

			if (lastGround == null) {
				x = -(width / 2);
			} else {
				x = lastGround.GetComponent<SpriteRenderer>().bounds.max.x + (groundRes.GetComponent<SpriteRenderer>().size.x / 2);
			}

			// Instantiate the next ground tile
			lastGround = Instantiate(groundRes, new Vector3(x, y), Quaternion.identity) as GameObject;
			lastGround.transform.parent = collider.transform;
			//lastGround.GetComponent<Rigidbody2D>().velocity = new Vector2(-speed, 0);
			objects.Add(lastGround);

			if (firstGround == null) {
				firstGround = lastGround;
			}
		}

		SpriteRenderer srFirst = firstGround.GetComponent<SpriteRenderer> ();
		float xFirst = srFirst.bounds.min.x;
		float yFirst = srFirst.bounds.max.y;
		Vector2 colliderStart = new Vector2 (xFirst, yFirst);
		SpriteRenderer srLast = lastGround.GetComponent<SpriteRenderer> ();
		float xLast = srLast.bounds.max.x;
		float yLast = srFirst.bounds.max.y;
		Vector2 colliderEnd = new Vector2 (xLast, yLast);
		collider.GetComponent<EdgeCollider2D> ().points = new Vector2[] {colliderStart, colliderEnd};

		collider.GetComponent<Rigidbody2D> ().velocity = Vector2.left * speed;
	}


	void AddGroundAfterLast() {
		// Get last ground
		// Set tile count for new ground
		// Create new ground from start point through tile count

		float tileCount = Random.Range (8, 64);
		bool initial = true;

		if (lastGround.transform.position.x > spawnBoundary) {
			return;
		}

		GameObject colRes = Resources.Load<GameObject> ("GroundCollider");
		GameObject collider = Instantiate (colRes, new Vector3 (), Quaternion.identity) as GameObject;

		GameObject firstGround = null;

		for (int i = 0; i < tileCount; i++) {
			// Get a ground res
			int index = Random.Range (0, grounds.Count);
			GameObject groundRes = grounds [index] as GameObject;

			// Determine the position of the next ground tile
			float y = -0.5f;
			float x;

			if (initial) {
				x = lastGround.gameObject.transform.position.x + 3;
				initial = false;
			} else {
				x = lastGround.GetComponent<SpriteRenderer> ().bounds.max.x + (groundRes.GetComponent<SpriteRenderer> ().size.x / 2);
			}

			// Instantiate the next ground tile
			lastGround = Instantiate (groundRes, new Vector3 (x, y), Quaternion.identity) as GameObject;
			lastGround.transform.parent = collider.transform;
			objects.Add (lastGround);

			if (firstGround == null) {
				firstGround = lastGround;
			}
		}

		SpriteRenderer srFirst = firstGround.GetComponent<SpriteRenderer> ();
		float xFirst = srFirst.bounds.min.x;
		float yFirst = srFirst.bounds.max.y;
		Vector2 colliderStart = new Vector2 (xFirst, yFirst);
		SpriteRenderer srLast = lastGround.GetComponent<SpriteRenderer> ();
		float xLast = srLast.bounds.max.x;
		float yLast = srFirst.bounds.max.y;
		Vector2 colliderEnd = new Vector2 (xLast, yLast);
		collider.GetComponent<EdgeCollider2D> ().points = new Vector2 [] { colliderStart, colliderEnd };

		collider.GetComponent<Rigidbody2D> ().velocity = Vector2.left * speed;
	}

	void AddGround (float startX, float endX, GameObject parent) {
		GameObject firstGround = null;

		while(lastGround == null || lastGround.transform.position.x < endX) {
			// Get a ground res
			int index = Random.Range (0, grounds.Count);
			GameObject groundRes = grounds [index] as GameObject;

			// Determine the position of the next ground tile
			float y = -0.5f;
			float x;

			if (firstGround == null) {
				x = startX;
			} else {
				x = lastGround.GetComponent<SpriteRenderer> ().bounds.max.x + (groundRes.GetComponent<SpriteRenderer> ().size.x / 2);
			}

			// Instantiate the next ground tile
			lastGround = Instantiate (groundRes, new Vector3 (x, y), Quaternion.identity) as GameObject;
			lastGround.transform.parent = parent.transform;
			objects.Add (lastGround);

			if (firstGround == null) {
				firstGround = lastGround;
			}
		}

		SpriteRenderer srFirst = firstGround.GetComponent<SpriteRenderer> ();
		float xFirst = srFirst.bounds.min.x;
		float yFirst = srFirst.bounds.max.y;
		Vector2 colliderStart = new Vector2 (xFirst, yFirst);
		SpriteRenderer srLast = lastGround.GetComponent<SpriteRenderer> ();
		float xLast = srLast.bounds.max.x;
		float yLast = srFirst.bounds.max.y;
		Vector2 colliderEnd = new Vector2 (xLast, yLast);

		parent.GetComponent<EdgeCollider2D> ().points = new Vector2 [] { colliderStart, colliderEnd };
		parent.GetComponent<Rigidbody2D> ().velocity = Vector2.left * speed;
	}


	void SpawnObstacle() { 
		// TODO - Add gap as an option
		//GameObject temp = lastGround;
		//AddGroundAfterLast();
		//temp.SetActive(false);
		return;
		float value = Random.Range (0.0f, frequencyTotal);
		float total = 0;
		int index = 0;
		
		for (int i = 0; i < spawnFrequencies.Count; i++) {
			total += spawnFrequencies[i];
			if (total > value) {
				index = i;
				break;
			}
		}

		GameObject res = obstacles[index] as GameObject;
		float xOffset = res.GetComponent<PlacementProperties>().verticalOffset;
		GameObject go = Instantiate(res, new Vector3(10, xOffset), Quaternion.identity) as GameObject;
		go.GetComponent<Rigidbody2D>().velocity = new Vector2(-speed, 0);

		countDown = cadence;
	}
}
