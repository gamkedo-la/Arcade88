using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GamePlay2 : GameManager {
	public CanvasScaler demoLayer;
	public Text CoinText;
	public Text TimeText;
	public Text HighScoreText;
	public GameObject Ant3D;
	public GameObject Footprint3D;
	Vector2 antPos = new Vector2(0.0f,0.0f);
	Vector2 antVel = new Vector2(0.5f,0.3f);
	Vector3 antVec;
	Vector3 antRot;
	int stomping = 0;
	Vector3 footCorrected;
	string savedBottomMessage;

	/* Reminder:
	 * override void PerPixelGameBootup() {
	 * is only for 2D pixel games. Just use Start() here for 3D games.
	  */

	void Start() {
		antVec = Ant3D.transform.localPosition;
		antRot = Ant3D.transform.localRotation.eulerAngles;
		footCorrected = Footprint3D.transform.position;
	}

	public override void PerGameFakeAIInput() {
		footCorrected = Footprint3D.transform.localPosition;
		if(stomping == 0) {
			if(Random.Range(0, 100) < 95) {
				if(footCorrected.x > antPos.x) {
					footLeft();
				} else {
					footRight();
				}
				if(footCorrected.y > antPos.y) {
					footUp();
				} else {
					footDown();
				}
				if(Random.Range(0, 100) < 4) {
					stomping = 1;
				}
			}
		}
		StompUpdate();
	}

	new void Update() {
		base.Update();
		GameLogic(); // 3D game so it's running in Unity's components, no coroutine driving it
	}

	void footLeft() {
		footCorrected += Vector3.left * Time.deltaTime * 0.7f;
		if(footCorrected.x < -0.32f) {
			footCorrected.x = -0.32f;
		}
	}
	void footRight() {
		footCorrected += Vector3.right * Time.deltaTime * 0.7f;
		if(footCorrected.x > 0.32f) {
			footCorrected.x = 0.32f;
		}
	}
	void footUp() {
		footCorrected += Vector3.up * Time.deltaTime * 0.5f;
		if(footCorrected.y > 0.213f) {
			footCorrected.y = 0.213f;
		}
	}
	void footDown() {
		footCorrected += Vector3.down * Time.deltaTime * 0.5f;
		if(footCorrected.y < -0.213f) {
			footCorrected.y = -0.213f;
		}
	}

	public override void PerGameInput() {
		footCorrected = Footprint3D.transform.localPosition;

		if(stomping == 0) {
			if(Input.GetKey(KeyCode.LeftArrow)) {
				footLeft();
			}
			if(Input.GetKey(KeyCode.RightArrow)) {
				footRight();
			}
			if(Input.GetKey(KeyCode.UpArrow)) {
				footUp();
			}
			if(Input.GetKey(KeyCode.DownArrow)) {
				footDown();
			}

			if(Input.GetKeyDown(KeyCode.Space)) {
				stomping = 1;
			}
		}

		StompUpdate();
	}

	void StompUpdate() {
		footCorrected.z += Time.deltaTime*stomping*5.0f;

		if(stomping > 0) {
			if(footCorrected.z >= 0.5f) {
				stomping = -1;
			}
		}
		if(stomping < 0) {
			if(footCorrected.z < -0.5f) {
				stomping = 0;
			}
		}

		Footprint3D.transform.localPosition = footCorrected;
	}

	public override void PerGameStart() {
		savedBottomMessage = TimeText.text;
		demoLayer.gameObject.SetActive(false);
		CoinText.text = (flashing ? "STOMP ANTS!" : "");;
	}

	public override void PerGameExit() {
		TimeText.text = savedBottomMessage;
		demoLayer.gameObject.SetActive(true);
	}

	public override void PerGameDemoMode() {
		PerGameLogic();
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
		antPos.x += antVel.x * Time.deltaTime;
		antPos.y += antVel.y * Time.deltaTime;

		antVel.x += Random.Range(-5.0f,5.0f) * Time.deltaTime;
		antVel.y += Random.Range(-5.0f,5.0f) * Time.deltaTime;

		if(antVel.magnitude > 2.0f) {
			antVel = antVel.normalized * 2.0f;
		}

		if(antVel.x > 0.0f) {
			if(antPos.x > 0.5f) {
				antVel.x *= -1.0f;
				antVel.Normalize();				
			}
		} else {
			if(antPos.x < -0.5f) {
				antVel.x *= -1.0f;
				antVel.Normalize();				
			}
		}
		if(antVel.y > 0.0f) {
			if(antPos.y > 0.5f) {
				antVel.y *= -1.0f;
				antVel.Normalize();				
			}
		} else {
			if(antPos.y < -0.5f) {
				antVel.y *= -1.0f;
				antVel.Normalize();				
			}
		}

		antVec.x = antPos.x;
		antVec.y = antPos.y;
		Ant3D.transform.localPosition = antVec;
		antRot.z = Mathf.Atan2(-antVel.y,antVel.x) * Mathf.Rad2Deg+0.0f;
		Ant3D.transform.localRotation = Quaternion.Euler(antRot);
	}

	public void GotStomp() {
		if(stomping > 0) { 
			stomping = -1;
			addToScore(1);
			if(footCorrected.x > 0.0f) {
				antPos.x = -0.5f;
			} else  {
				antPos.x = 0.5f;
			}
			if(footCorrected.y > 0.0f) {
				antPos.y = -0.5f;
			} else  {
				antPos.y = 0.5f;
			}
			//antPos.x = 0.5f * (Random.Range(0,100)<50 ? 1.0f : -1.0f);
			//antPos.y = 0.5f * (Random.Range(0,100)<50 ? 1.0f : -1.0f);
			CoinText.text = "Ants: "+score;
			HighScoreText.text = "High Score: "+highScore;
		}
	}
	
}