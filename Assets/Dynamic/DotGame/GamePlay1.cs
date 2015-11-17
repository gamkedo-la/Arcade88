using UnityEngine;
using System.Collections;

public class GamePlay1 : PixelScreenLib {
	float ballX = 25;
	float ballY = 20;
	float ballXV = 3.4f;
	float ballYV = 1.4f;

	public override void PerPixelGameBootup() {
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
		
		drawBoxAt((int)ballX-1,(int)ballY-1,3,greenCol);
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

		drawStringCentered(screenWidth/2,screenHeight/8,whiteCol,"BOUNCY DOT");
		drawStringCentered(screenWidth/2,screenHeight/8+8,redCol,"NO ONE CAN HANDLE IT");
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