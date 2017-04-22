using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public GameObject displayPanel;
	public Sprite startScreen;
	public Sprite winScreen;
	public Sprite loseScreen;
	public int grannyCount = 3;

	enum State {Start, Running, Paused, End};
	private State currentState;
	private float timer;

	private List<GameObject> grannies;

	// Use this for initialization
	void Start () {
		currentState = State.Start;
		timer = 0f;
		grannies = new List<GameObject>();
	}
	
	// Update is called once per frame
	void Update () {
		HandleInput ();
		if (currentState == State.Running) {
			SpawnGrannies ();
			CheckVictory ();
		}
	}

	private void HandleInput(){
		if (Input.GetKeyDown (KeyCode.P)) {
			if (currentState == State.Running){
				Pause (true);
			} else if (currentState == State.Paused){
				Pause (false);
			}
		} else if (Input.GetKeyDown (KeyCode.R)) {
			if (currentState == State.Start) {
				currentState = State.Running;
				// Hide main menu
				displayPanel.SetActive (false);
			} else {
				SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
			}
		} else if (Input.GetKeyDown (KeyCode.Q)) {
			Application.Quit ();
		}
	}

	private void SpawnGrannies(){
		timer += Time.deltaTime;
		if (timer >= 2f) {
			timer = 0f;
			if (grannyCount > 0) {
				grannyCount--;

				GameObject granny = Instantiate(Resources.Load ("Prefabs/Sporty_Granny/Granny")) as GameObject;
				granny.transform.position = new Vector3 (Random.Range(-3f,3f), 10, Random.Range(-3f,3f));
				grannies.Add (granny);
			}
		}
	}

	private void CheckVictory(){
		bool won = (grannyCount <= 0);
		bool lost = false;
		foreach (GameObject granny in grannies){
			// Check if any granny has collided with the player
			if (granny.GetComponent<GrannyController> ().touchedElvis == true) {
				lost = true;
			}

			// Check if all the grannies are off the edge.
			if (granny.transform.position.y >= 8) {
				won = false;
			}
		}

		if (won) {
			currentState = State.End;
			displayPanel.GetComponent<Image> ().sprite = winScreen;
			displayPanel.SetActive (true);
		} else if (lost) {
			currentState = State.End;
			displayPanel.GetComponent<Image> ().sprite = loseScreen;
			displayPanel.SetActive (true);
		}
	}
		
	// Pause or unpause the game.
	private void Pause(bool freeze){

		if (freeze) {
			Time.timeScale = 0f;
			displayPanel.GetComponent<Image> ().sprite = startScreen;
			currentState = State.Paused;
		} else {
			Time.timeScale = 1f;
			currentState = State.Running;
		}
		// If paused, display the main menu
		displayPanel.SetActive (freeze);

		// Tell the grannies the game is paused.
		foreach (GameObject granny in grannies) {
			granny.GetComponent<GrannyController> ().paused = freeze;
		}
	}
}
