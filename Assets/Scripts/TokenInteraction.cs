﻿using UnityEngine;
using System.Collections;

public class TokenInteraction : MonoBehaviour {
	public int tokenDelta;
	public int billDelta;

	public PlayableGame pgScript;

	void Start() {
		pgScript = gameObject.GetComponent<PlayableGame>();
	}

	public void tokenExchange (ArcadePlayer interactor) {
		if( interactor.tokenBillsChange(billDelta, tokenDelta) 
		   && pgScript) {
			pgScript.gameScreen.isPlaying = true;
		}
	}
}
