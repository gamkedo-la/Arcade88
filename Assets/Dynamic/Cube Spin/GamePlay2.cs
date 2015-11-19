using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GamePlay2 : GameManager {
	public CanvasScaler demoLayer;
	public Text CoinText;
	public Text TimeText;
	public Text HighScoreText;
	public Spin spinCube;
	string savedBottomMessage;

	Quaternion cubeOldRot;

	/* Reminder:
	 * override void PerPixelGameBootup() {
	 * is only for 2D pixel games. Just use Start() here for 3D games.
	  */

	void Update() {
		base.Update();
		GameLogic(); // 3D game so it's running in Unity's components, no coroutine driving it
	}

	public override void PerGameInput() {
		if(Input.GetKey(KeyCode.LeftArrow)) {
			spinCube.transform.Rotate(Vector3.up, Time.deltaTime*90.0f);
		}
		if(Input.GetKey(KeyCode.RightArrow)) {
			spinCube.transform.Rotate(Vector3.up, Time.deltaTime*-90.0f);
		}
		
		if(Input.GetKey(KeyCode.UpArrow)) {
			spinCube.transform.Rotate(Vector3.right, Time.deltaTime*90.0f);
		}
		if(Input.GetKey(KeyCode.DownArrow)) {
			spinCube.transform.Rotate(Vector3.right, Time.deltaTime*-90.0f);
		}
		if(Input.GetKeyDown(KeyCode.Space)) {
			spinCube.enabled = !spinCube.enabled;
		}
	}

	public override void PerGameStart() {
		spinCube.enabled = true;
		savedBottomMessage = TimeText.text;
		demoLayer.gameObject.SetActive(false);
		CoinText.text = "SPIN THAT CUBE!";

		cubeOldRot = spinCube.transform.rotation;
	}

	public override void PerGameExit() {
		spinCube.enabled = true;
		spinCube.transform.rotation = Quaternion.identity;
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
		addToScore( ((int)Quaternion.FromToRotation(spinCube.transform.rotation*Vector3.up,
		                                       cubeOldRot*Vector3.up).eulerAngles.magnitude > 4 ? 5 : 0));
		           addToScore( ((int)Quaternion.FromToRotation(spinCube.transform.rotation*Vector3.right,
		                                         cubeOldRot*Vector3.right).eulerAngles.magnitude > 4) ? 7 : 0);
		CoinText.text = "Spins: "+score;
		cubeOldRot = spinCube.transform.rotation;
		// no self driven code yet for this 3D demo, it's in the components instead, Unity-style
	}
	
}