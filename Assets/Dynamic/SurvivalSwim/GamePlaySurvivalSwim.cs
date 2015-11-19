using UnityEngine;
using System.Collections;

public class GamePlaySurvivalSwim : PixelScreenLib {
	public Texture2D playerImg;
	Color32[] playerBitmap;

	public Texture2D birdImg;
	Color32[] birdBitmap;

	public Texture2D sharkImg;
	Color32[] sharkBitmap;

	Color oceanCol = new Color(0.5f,0.5f,1.0f);

	bool isBird = false;
	float enemyX = 0;

	int playerDodge = 0;

	int birdFlapFrame = 0;

	float ballX = 25;
	float ballY = 20;
	float ballXV = 3.4f;
	float ballYV = 1.4f;

	IEnumerator cycleAnims() {
		while(true) {
			birdFlapFrame++;
			if(birdFlapFrame >= 4) {
				birdFlapFrame = 0;
			}
			yield return new WaitForSeconds(0.125f);
		}
	}

	public override void PerPixelGameBootup() {
		playerBitmap = playerImg.GetPixels32();
		birdBitmap = birdImg.GetPixels32();
		sharkBitmap = sharkImg.GetPixels32();

		StartCoroutine(cycleAnims());
		// Debug.Log (birdBitmap);
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

	private void ballBounceAndDraw() {
		ballX += ballXV;
		ballY += ballYV;
		
		if(ballX < 0 && ballXV < 0.0f) {
			ballXV *= -1.0f;
		}
		if(ballX > screenWidth && ballXV > 0.0f) {
			ballXV *= -1.0f;
		}
		if(ballY < 0 && ballYV < 0.0f) {
			ballYV *= -1.0f;
		}
		if(ballY > screenHeight && ballYV > 0.0f) {
			ballYV *= -1.0f;
		}
		
		// drawBoxAt((int)ballX-1,(int)ballY-1,3,greenCol);
		drawBoxAt(0,screenHeight>>1,screenWidth,oceanCol,screenHeight>>1);

		if(isBird) {
			enemyX -= 3.0f;
			copyBitmapFromToColorArray(0,0,
			                           16, 16,
			                           (int)enemyX-8,45,
			                           birdBitmap,birdImg.width,
			                           birdFlapFrame); // anim as last argument

		} else {
			enemyX -= 2.0f;
			copyBitmapFromToColorArray(0,0, // start x,y from source
		                           16, 8, // width and height from source
		                           (int)enemyX-8, 70, // destination x,y
		                           sharkBitmap,sharkImg.width); // bitmap and its width
									// no anim frame for shark
		}

		if(enemyX < 0.0f) {
			enemyX = screenWidth;
			isBird = Random.Range(0,100) < 50.0f;
		}

		copyBitmapFromToColorArray(0,0, 8, 8,
		                           15,60 + playerDodge*15,
		                           playerBitmap,playerImg.width); // anim as last argument

		// paintThis.Apply();
	}

	void CenterBall() {
		ballX = screenWidth/2;
		ballY = screenHeight/2;
	}

	public override void PerGameStart() {
		CenterBall();
	}

	public override void PerGameExit() {
		CenterBall();
	}

	public override void PerGameDemoMode() {
		ballBounceAndDraw(); // for this game just let the ball bounce

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
		ballBounceAndDraw();
	}
	
}