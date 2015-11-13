using UnityEngine;
using System.Collections;

public class PlayableGame : MonoBehaviour {
	public string gameName;
	public string gameInstructions;
	public Transform standHere;
	public GameManager gameScreen;

	void Awake () {
		gameScreen.SetCab(this);
	}
}
