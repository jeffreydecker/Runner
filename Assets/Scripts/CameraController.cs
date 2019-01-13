using UnityEngine;

public class CameraController : MonoBehaviour {

	public GameObject followTarget;
	public float cameraDeltaX;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		float followTargetX = followTarget.transform.position.x;

		Vector3 cameraPosition = transform.position;
		cameraPosition.x = cameraDeltaX + followTargetX;
		transform.position = cameraPosition;

	}
}
