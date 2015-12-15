using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GamePlayVolcano : GameManager {
	public CanvasScaler demoLayer;
	public Text CoinText;
	public Text TimeText;
	public Text HighScoreText;
	string savedBottomMessage;
	
	/* Reminder:
	 * override void PerPixelGameBootup() {
	 * is only for 2D pixel games. Just use Start() here for 3D games.
	  */

	new void Update() {
		base.Update();
		GameLogic(); // 3D game so it's running in Unity's components, no coroutine driving it
	}

	public override void PerGameFakeAIInput() {
		if(Random.Range(0, 100) < 40) {
			addToScore(15);
		}
	}

	public override void PerGameInput() {
		if(Input.GetKeyDown(KeyCode.Space)) {
			addToScore(15);
		}
	}

	public override void PerGameStart() {
		savedBottomMessage = TimeText.text;
		demoLayer.gameObject.SetActive(false);
		CoinText.text = "Time for Max Boom!";
	}

	public override void PerGameExit() {
		TimeText.text = savedBottomMessage;
		demoLayer.gameObject.SetActive(true);
	}

	public override void PerGameDemoMode() {
		demoLayer.scaleFactor = Mathf.Cos (Time.time) * 0.2f + 1.0f;
		HighScoreText.text = "High Score: "+highScore;
	}

	public override void PerGameDemoModeCoinRequestDisplay() {
		CoinText.text = (flashing ? "insert token" : "");
	}

	public override void PerGameTimerDisplay() {
		TimeText.text = ""+timerLeft;
	}

	public override void PerGameLogic() {
		CoinText.text = "Damage: "+score;
		// no self driven code yet for this 3D demo, it's in the components instead, Unity-style
	}
	
}