using UnityEngine;
using System.Collections;

public class MenuManager : MonoBehaviour {

	public GameObject[] titleScreen;
	public GameObject[] inGame;
	public GameObject[] nukeAtStart;
	bool onTitle = true;

	void UpdateLive() {
		for(int i = 0; i < titleScreen.Length; i++) {
			titleScreen[i].SetActive(onTitle);
		}
		for(int i = 0; i < inGame.Length; i++) {
			inGame[i].SetActive(onTitle==false);
		}
		if(onTitle) {
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		} else {
			PlayerDistrib.instance.ForgetIfTried();
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}
	}

	// Use this for initialization
	void Start () {
		for(int i = 0; i < nukeAtStart.Length; i++) {
			nukeAtStart[i].SetActive(false);
		}

		UpdateLive();
	}
	
	public void ChangeMenuStateTo(bool thisState) {
		onTitle = thisState;
		UpdateLive();
	}
}
