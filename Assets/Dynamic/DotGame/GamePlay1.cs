using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OtherDot {
	public enum DotTeam {eatIt,eatsYou};
	
	public float x,y;
	public float speedMult;
	public float goalOffX, goalOffY;
	public Vector2 vel = Vector2.zero;
	public DotTeam myTeam = DotTeam.eatIt;

	public OtherDot(int screenWid, int screenHei) {
		x = Random.Range(0,screenWid);
		y = Random.Range(0,screenHei);
		speedMult = Random.Range(0.7f,1.1f);
		goalOffX = Random.Range(-8.0f,8.0f);
		goalOffY = Random.Range(-8.0f,8.0f);
		vel = Random.insideUnitCircle.normalized * 2.25f;
	}
}

public class GamePlay1 : PixelScreenLib {
	List<OtherDot> allDots = new List<OtherDot>();

	float chaserSpeed = 1.15f;

	float ballX = 25;
	float ballY = 20;
	float ballXV = 3.4f;
	float ballYV = 1.4f;

	float ballTouchRadBlue = 5.5f;
	float ballTouchRadRed = 1.5f;

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
		
		drawBoxAt((int)ballX-1,(int)ballY-1,3,3,greenCol);
	}

	void CenterBall() {
		ballX = screenWidth/2;
		ballY = screenHeight/2;
	}

	public override void PerGameStart() {
		allDots.Clear();
		for(int i = 0; i<25; i++) {
			OtherDot nextDot = new OtherDot(screenWidth,screenHeight);
			allDots.Add( nextDot );
		}

		CenterBall();
	}

	public override void PerGameExit() {
		CenterBall();
	}

	public override void PerGameDemoMode() {
		ballBounceAndDraw(); // for this game just let the ball bounce

		drawStringCentered(screenWidth/2,screenHeight/8,whiteCol,"DOT GAME");
		drawStringCentered(screenWidth/2,screenHeight/8+8,redCol,"HIGH SCORE: " + highScore);
	}

	public override void PerGameDemoModeCoinRequestDisplay() {
		if( flashing ) {
			drawStringCentered(screenWidth/2,screenHeight/2,greenCol,"INSERT TOKEN");
		}
	}

	public override void PerGameTimerDisplay() {
		drawStringCentered(screenWidth/2,5,yellowCol,
		                   ""+ timerLeft);
	}

	public override void PerGameLogic() {
		drawStringCentered(screenWidth/2,15,yellowCol,
		                   "Score: "+ score);
		drawStringCentered(screenWidth/2,screenHeight-20,yellowCol,
		                   "High Score: "+ highScore);

		Vector2 delta;
		foreach(OtherDot eachDot in allDots) {
			delta.x = ballX-eachDot.x;
			delta.y = ballY-eachDot.y;

			if(eachDot.myTeam == OtherDot.DotTeam.eatsYou) {
				if(delta.magnitude < ballTouchRadRed) {
					InstantLoseFromTimeDrain();
				}
				delta.x += eachDot.goalOffX;
				delta.y += eachDot.goalOffY;
				eachDot.vel.x = delta.x;
				eachDot.vel.y = delta.y;
				eachDot.vel = eachDot.vel.normalized * chaserSpeed * eachDot.speedMult;
			} else if(delta.magnitude < ballTouchRadBlue) {
				eachDot.myTeam = OtherDot.DotTeam.eatsYou;
				addToScore(1);
				eachDot.vel.x = delta.x+ballXV; // based on future position...
				eachDot.vel.y = delta.y+ballYV;
				eachDot.vel = -eachDot.vel.normalized * chaserSpeed * 3.0f; // jump away first
			}
			eachDot.x += eachDot.vel.x;
			eachDot.y += eachDot.vel.y;

			if(eachDot.x > screenWidth && eachDot.vel.x > 0) {
				eachDot.vel.x *= -1.0f;
			}
			if(eachDot.x < 0 && eachDot.vel.x < 0) {
				eachDot.vel.x *= -1.0f;
			}
			if(eachDot.y > screenHeight && eachDot.vel.y > 0) {
				eachDot.vel.y *= -1.0f;
			}
			if(eachDot.y < 0 && eachDot.vel.y < 0) {
				eachDot.vel.y *= -1.0f;
			}

			drawBoxAt((int)eachDot.x-1,(int)eachDot.y-1,2,2,(eachDot.myTeam==OtherDot.DotTeam.eatsYou ? redCol : cyanCol));
		}
		ballBounceAndDraw();
	}
	
}