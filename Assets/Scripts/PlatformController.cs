using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour {

	#region Public Vars

	// Colliders
	public EdgeCollider2D groundCollider;
	public EdgeCollider2D wallCollider;

	// Resources
	public List<GameObject> obstacleResources;

	// Configurables
	public float obstacleMinGap = 8f;
	public float obstacleMaxGap = 16f;

	#endregion

	#region Private Vars

	// Properties
	float length;

	SpriteRenderer spriteRenderer;
	Rigidbody2D rb2d;

	List<GameObject> obstacles = new List<GameObject> ();

	#endregion

	// Use this for initialization
	void Start () {
	
	}

	// Update is called once per frame
	void Update () {
		
	}

	public void SetSpeed (float speed) {
		rb2d.velocity = Vector2.left * speed;
	}

	public void Build(float startX, float endX, bool containsObstacles) {
		rb2d = GetComponent<Rigidbody2D> ();
		spriteRenderer = GetComponent<SpriteRenderer> ();

		// Add ground tiles
		SetupGround (startX, endX);

		if (containsObstacles) {
			// Add obstacles and items
			AddObstaclesAndItems ();
		}
	}

	void SetupGround (float startX, float endX) {
		length = endX - startX;
		spriteRenderer.size = new Vector2 (length, 1f);
		float halfWidth = (spriteRenderer.size.x / 2f);
		float halfHeight = (spriteRenderer.size.y / 2f);
		float posX = startX + halfWidth;
		float posY = -2f - halfHeight;
		gameObject.transform.position = new Vector2 (posX, posY);
		Vector2 topLeft = new Vector2 (-halfWidth, halfHeight);
		Vector2 topRight = new Vector2 (halfWidth, halfHeight);
		groundCollider.points = new Vector2 [] {topLeft, topRight};
	}

	void AddObstaclesAndItems () {
		// Get range in which obstacles can be placed
		float obstMaxValidX = (spriteRenderer.size.x / 2) - obstacleMinGap;

		float platformMinX = -(spriteRenderer.size.x / 2);
		float obstMinX = platformMinX + obstacleMinGap;
		float obstMaxX = Mathf.Min(platformMinX + obstacleMaxGap, obstMaxValidX);

		// Randomly determine where in that range the next obstcle can be placed and calculate new range based off of that
		// While can add obstacles do add obstacles
		while (obstMinX < obstMaxValidX) {
			float obstacleX = Random.Range (obstMinX, obstMaxX);
			
			int obstacleIndex = Random.Range (0, obstacleResources.Count);
			Debug.LogWarning ("Obstacle Index: " + obstacleIndex + " - " + (obstacleResources.Count - 1));
			GameObject res = obstacleResources [obstacleIndex];
			PlacementProperties pp = res.GetComponent<PlacementProperties> ();

			float obstacleY = pp.verticalOffset + (spriteRenderer.size.y / 2);

			GameObject obstacle = Instantiate (res, transform);
			obstacle.transform.localPosition = new Vector2 (obstacleX, obstacleY);

			obstacles.Add (obstacle);

			obstMinX = obstacleX + obstacleMinGap;
			obstMaxX = Mathf.Min(obstMinX + obstacleMaxGap, obstMaxValidX);
		}
	}
}
