using UnityEngine;
using System.Collections;

public class MenuManager : MonoBehaviour {

	public GameObject[] titleScreen;
	public GameObject[] inGame;
	public GameObject[] nukeAtStart;
	bool onTitle = true;
	public static bool tournyMode;

	private AudioSource musicPlayer;
	public static MenuManager instance;

	void UpdateLive() {
		for(int i = 0; i < titleScreen.Length; i++) {
			titleScreen[i].SetActive(onTitle);
		}
		for(int i = 0; i < inGame.Length; i++) {
			inGame[i].SetActive(onTitle==false);
		}
		if(onTitle) {
			musicPlayer.enabled = true;

			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		} else {
			musicPlayer.enabled = (tournyMode==false);

			PlayerDistrib.instance.ForgetIfTried();
			PlayerDistrib.instance.Shuffle();
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}
	}

	// Use this for initialization
	void Start () {
		instance = this;

		musicPlayer = GetComponent<AudioSource>();

		for(int i = 0; i < nukeAtStart.Length; i++) {
			nukeAtStart[i].SetActive(false);
		}

		UpdateLive();
	}

	public void StartGame(bool doTournyMode) {
		tournyMode = doTournyMode;
		if(tournyMode) {
			PlayerDistrib.instance.WipeHighScores();
		}

		ChangeMenuStateTo(false);
	}
	
	public void ChangeMenuStateTo(bool thisState) {
		onTitle = thisState;
		UpdateLive();
	}
}
