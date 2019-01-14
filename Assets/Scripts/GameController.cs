using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

	#region Public Vars

	public LevelController levelController;

	public Text scoreText;

	#endregion

	#region Private Vars

	bool gameActive = false;

	float score;

	#endregion

	// Use this for initialization
	void Start () {
		gameActive = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (gameActive) {
			score += Time.deltaTime;
			scoreText.text = "" + score;
		}
	}

	public void HandlePlayerDeath() {
		levelController.HandlePlayerDeath ();
		gameActive = false;
	}

	public void Restart() {
		SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
	}
}
