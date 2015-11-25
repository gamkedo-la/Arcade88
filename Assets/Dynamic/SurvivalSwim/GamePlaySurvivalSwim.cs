using UnityEngine;
using System.Collections;

public class GamePlaySurvivalSwim : PixelScreenLib {
	public Texture2D playerImg;
	PixelSprite playerSprite;

	public Texture2D birdImg;
	PixelSprite birdSprite;

	public Texture2D sharkImg;
	PixelSprite sharkSprite;

	Color oceanCol = new Color(0.5f,0.5f,1.0f);

	bool isBird = false;
	float enemyX = 0;

	int playerDodge = 0;

	public override void PerPixelGameBootup() {
		disableAutoScreenClear = true; // sky and water

		playerSprite = new PixelSprite(playerImg);
		birdSprite = new PixelSprite(birdImg, 16,16); // each animation is 16 pixels wide
		sharkSprite = new PixelSprite(sharkImg);
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

	private void charactersDraw() {
		drawBoxAt(0,0,screenWidth,screenHeight>>1,bgCol); // sky
		drawBoxAt(0,screenHeight>>1,screenWidth,screenHeight>>1,oceanCol); // ocean

		if(isBird) {
			enemyX -= 3.0f;
			birdSprite.drawImage(this,(int)enemyX-8,45);
		} else {
			enemyX -= 2.0f;
			sharkSprite.drawImage(this,(int)enemyX-8, 70);
		}

		if(enemyX < 0.0f) {
			enemyX = screenWidth;
			isBird = Random.Range(0,100) < 50.0f;
		}

		playerSprite.drawImage(this, 15,60 + playerDodge*15);
	}


	public override void PerGameStart() {

	}

	public override void PerGameExit() {

	}

	public override void PerGameDemoMode() {
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
		drawStringCentered(screenWidth/2,screenHeight/2+10,yellowCol,
		                   ""+ timerLeft);
	}

	public override void PerGameLogic() {
		charactersDraw();
	}
	
}