using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GamePlayVolcano : GameManager {
	public CanvasScaler demoLayer;
	public Text CoinText;
	public Text TimeText;
	public Text HighScoreText;
	string savedBottomMessage;

	public BounceSlide slider;
	public ParticleSystem smoke;
	public ParticleSystem rocks;
	public ParticleSystem fire;
	public ParticleSystem blast;

	/* Reminder:
	 * override void PerPixelGameBootup() {
	 * is only for 2D pixel games. Just use Start() here for 3D games.
	  */

	new void Update() {
		base.Update();
		GameLogic(); // 3D game so it's running in Unity's components, no coroutine driving it
	}

	public override void PerGameFakeAIInput() {
		if(Random.Range(0, 100) < 4) {
			AttemptButton();
		}
	}

	void AttemptButton() {
		if(score > 0) {
			return;
		}
		int newScore = slider.ScoreTest();
		if(newScore > 0) {
			slider.GetComponent<BounceSlide>().enabled = false;
			// smoke.enableEmission = true;
			rocks.enableEmission = true;
			rocks.Emit(100);
			fire.enableEmission = true;
			blast.enableEmission = true;
			addToScore(newScore);
			if(timerLeft >= 10) {
				endOfPlayTime = Time.time + 9;
			}
		}
	}

	public override void PerGameInput() {
		if(Input.GetKeyDown(KeyCode.Space)) {
			AttemptButton();
		}
	}

	public override void PerGameStart() {
		slider.GetComponent<BounceSlide>().enabled = true;
		// smoke.enableEmission = false;
		rocks.enableEmission = false;
		fire.enableEmission = false;
		blast.enableEmission = false;
		// smoke.Clear();
		rocks.Clear();
		fire.Clear();
		blast.Clear();

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
		CoinText.text = "Damage: "+slider.ScoreTest();//score;
		// no self driven code yet for this 3D demo, it's in the components instead, Unity-style
	}
	
}