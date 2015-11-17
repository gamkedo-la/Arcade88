using UnityEngine;
using System.Collections;

public class GamePlaySurvivalSwim : PixelScreenLib {
	public Texture2D birdImg;
	Color32[] birdBitmap;

	public Texture2D sharkImg;
	Color32[] sharkBitmap;

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
		birdBitmap = birdImg.GetPixels32();
		sharkBitmap = sharkImg.GetPixels32();

		StartCoroutine(cycleAnims());
		// Debug.Log (birdBitmap);
	}

	public override void PerGameInput() {
		if(Input.GetKey(KeyCode.LeftArrow) && ballXV > 0.0f) {
			ballXV *= -1.0f;
		}
		if(Input.GetKey(KeyCode.RightArrow) && ballXV < 0.0f) {
			ballXV *= -1.0f;
		}
		
		if(Input.GetKey(KeyCode.UpArrow) && ballYV > 0.0f) {
			ballYV *= -1.0f;
		}
		if(Input.GetKey(KeyCode.DownArrow) && ballYV < 0.0f) {
			ballYV *= -1.0f;
		}
		if(Input.GetKeyDown(KeyCode.Space)) {
			ballX = Random.Range(0.0f, screenWidth);
			ballY = Random.Range(0.0f, screenHeight);
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

		copyBitmapFromToColorArray(0,0, // start x,y from source
		                           16, 8, // width and height from source
		                           (int)ballX-1,(int)ballY-1, // destination x,y
		                           sharkBitmap,sharkImg.width); // bitmap and its width
									// no anim frame for shark

		copyBitmapFromToColorArray(0,0, 16, 16,
		                           32,76,
		                           birdBitmap,birdImg.width,
		                           birdFlapFrame); // anim as last argument
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