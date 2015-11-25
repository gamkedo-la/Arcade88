using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	public bool isPlaying;
	public float playTime = 4.0f;
	protected float endOfPlayTime;
	protected int timerLeft = 0;
	protected PlayableGame myCab;
	protected bool flashing;

	public static int animFrameStep;
	
	protected int score = 0;
	protected int highScore = 0;

	protected void clearScore() {
		score = 0;
	}

	IEnumerator stepAllAnims() {
		while(true) {
			animFrameStep++;
			yield return new WaitForSeconds(0.2f);
		}
	}
	
	protected void addToScore(int scoreDelta) {
		score += scoreDelta;
		if(score > highScore) {
			highScore = score;
		}
	}

	public void SetCab(PlayableGame cabinet) {
		myCab = cabinet;
	}

	void Start () {		
		isPlaying = false;
		StartCoroutine( stepAllAnims() );
	}

	public void Update() {
		if(ArcadePlayer.playingNow && ArcadePlayer.playingNow.gameScreen == this) {
			PerGameInput();
		}
	}

	public void GameStart() {
		endOfPlayTime = Time.time + playTime;
		clearScore();
		PerGameStart();
	}

	public void GameLogic() {
		flashing = ((int)(Time.time * 2.0f)%2==1);
		if(isPlaying) {
			timerLeft = (int)(endOfPlayTime-Time.time);
			if(timerLeft<=0) {
				timerLeft = 0; // guarding against some rounding error going negative
				PerGameExit();
				isPlaying = false; // will return to PerGameDemoMode, need a gameOver timer though
			} else {
				PerGameLogic();
				PerGameTimerDisplay();
			}
		} else {
			PerGameDemoMode();
			PerGameDemoModeCoinRequestDisplay();
		}
	}

	public virtual void PerGameStart() {
		Debug.Log (myCab.gameName +
		           ": Each game should override PerGameStart");
	}

	public virtual void PerGameExit() {
		Debug.Log (myCab.gameName +
		           ": Each game should override PerGameExit");
	}

	public virtual void PerGameLogic() {
		Debug.Log (myCab.gameName +
		           ": Each game should override PerGameLogic");
	}
	
	public virtual void PerGameTimerDisplay() {
		Debug.Log (myCab.gameName +
		           ": Each game should override PerGameLogic");
	}

	public virtual void PerGameDemoMode() {
		Debug.Log (myCab.gameName +
		           ": Each game should override PerGameDemoMode");
	}

	public virtual void PerGameDemoModeCoinRequestDisplay() {
		Debug.Log (myCab.gameName +
		           ": Each game should override PerGameDemoModeCoinRequestDisplay");
	}

	public virtual void PerGameInput() {
		Debug.Log (myCab.gameName +
		           ": Each game should override PerGameInput");
	}

}