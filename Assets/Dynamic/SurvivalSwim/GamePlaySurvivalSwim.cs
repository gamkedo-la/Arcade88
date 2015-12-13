using UnityEngine;
using System.Collections;

public class GamePlaySurvivalSwim : PixelScreenLib {
	public Texture2D playerImg;
	PixelSprite playerSprite;

	public Texture2D birdImg;
	PixelSprite birdSprite;

	public Texture2D sharkImg;
	PixelSprite sharkSprite;

	int collDist = 12;
	int playerX = 20;

	float gameSpeedMult;

	Color oceanCol = new Color(0.5f,0.5f,1.0f);

	bool isBird = false;
	float enemyX = 0;

	int playerDodge = 0;

	public override void PerPixelGameBootup() {
		disableAutoScreenClear = true; // sky and water

		playerSprite = new PixelSprite(playerImg);
		birdSprite = new PixelSprite(birdImg, 16); // each animation is 16 pixels wide
		sharkSprite = new PixelSprite(sharkImg);

		gameSpeedMult = 1.0f;
	}

	public override void PerGameInput() {
		if(Input.GetKey(KeyCode.UpArrow)) {
			playerDodge = -1;
		} else if(Input.GetKey(KeyCode.DownArrow)) {
			playerDodge = 1;
		} else {
			playerDodge = 0;
		}
	}

	private void respawnEnemy() {
		enemyX = screenWidth;
		isBird = Random.Range(0,100) < 50.0f;
	}

	private void charactersDraw() {
		drawBoxAt(0,0,screenWidth,screenHeight>>1,bgCol); // sky
		drawBoxAt(0,screenHeight>>1,screenWidth,screenHeight>>1,oceanCol); // ocean

		if(isBird) {
			enemyX -= 4.0f * gameSpeedMult;
			birdSprite.drawImage(this,(int)enemyX-8,45);
		} else {
			enemyX -= 3.0f * gameSpeedMult;
			sharkSprite.drawImage(this,(int)enemyX-8, 70);
		}

		if(enemyX < 0.0f) {
			respawnEnemy();
			if(isPlaying) {
				addToScore(1);
			}
		}

		playerSprite.drawImage(this, playerX-4,60 + playerDodge*15);

		drawStringCentered(screenWidth-15,screenHeight-15,yellowCol, "high");
		drawStringCentered(screenWidth-15,screenHeight-8,yellowCol,
		                   ""+ highScore);
	}


	public override void PerGameStart() {
		gameSpeedMult = 1.0f;
		respawnEnemy();
	}

	public override void PerGameExit() {
		gameSpeedMult = 1.0f;
	}

	public override void PerGameDemoMode() {
		playerDodge = 0;
		if(Mathf.Abs(playerX-enemyX)<collDist) {
			if(isBird) {
				playerDodge = 1;
			} else {
				playerDodge = -1;
			}
		}

		charactersDraw(); // for this game just let the ball bounce

		drawStringCentered(screenWidth/2,screenHeight/8,whiteCol,"Survival Swim");
		drawStringCentered(screenWidth/2,screenHeight/8+8,redCol,"Keep on Swimmin");
	}

	public override void PerGameDemoModeCoinRequestDisplay() {
		if( flashing ) {
			drawStringCentered(screenWidth/2,screenHeight/2,greenCol,"INSERT TOKEN");
		}
	}

	public override void PerGameTimerDisplay() {
		drawStringCentered(screenWidth/2,screenHeight-15,yellowCol, "time");
		drawStringCentered(screenWidth/2,screenHeight-8,yellowCol,
		                   ""+ timerLeft);
	}

	public override void PerGameLogic() {
		float timePerc = timerLeft/playTime;
		timePerc *= timePerc;
		timePerc *= timePerc;
		gameSpeedMult = 1.0f +
			(1.0f-timePerc)*3.0f;
		charactersDraw();
		if(Mathf.Abs(playerX-enemyX)<collDist) {
			if(isBird) {
				if(playerDodge != 1) {
					InstantLoseFromTimeDrain();
				}
			} else {
				if(playerDodge != -1) {
					InstantLoseFromTimeDrain();
				}
			}
		}

		drawStringCentered(15,screenHeight-15,yellowCol, "score");
		drawStringCentered(15,screenHeight-8,yellowCol,
		                   ""+ score);
	}
	
}