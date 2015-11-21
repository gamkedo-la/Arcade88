using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RRPlatform {
	public float x,y, width;

	public RRPlatform(float startX, float startY, float myWidth) {
		x = startX;
		y = startY;
		width = myWidth;
	}
	
	public void ShiftPlatform(float moveX) {
		x += moveX;
	}
}

public class GamePlayRRSA : PixelScreenLib {
	public Texture2D rrImg;
	Color32[] rrBitmap;

	float rrX = 25;
	float rrY = 20;
	float rrXV = 3.4f;
	float rrYV = 1.4f;

	float fakeGravity = 0.9f;

	bool rrIsInAir = false;

	List<RRPlatform> platformList = new List<RRPlatform>();

	// universal shared platform values
	float platSpeedX = 1;
	float platformH = 14.0f;

	public override void PerPixelGameBootup() {
		rrBitmap = rrImg.GetPixels32();

	}

	public override void PerGameInput() {
		if(Input.GetKeyDown(KeyCode.Space) && rrIsInAir == false) {
			rrYV = -10.0f;
			rrIsInAir = true;
		}
	}

	// move and draw function
	private void rrMoveAndDraw() {
		if(rrIsInAir) {
			rrYV += fakeGravity;
		}

		foreach( RRPlatform onePlat in platformList ) {
			if( rrX > onePlat.x && rrX < onePlat.x + onePlat.width &&
			   rrY > onePlat.y && rrY < onePlat.y + platformH) {
				rrIsInAir = false;
				rrY = onePlat.y-1.0f;
				rrYV = 0.0f;
			}
		}

		rrX += rrXV;
		rrY += rrYV;
		
		if(rrX < 0 && rrXV < 0.0f) {
			rrXV *= -1.0f;
		}
		if(rrX > screenWidth && rrXV > 0.0f) {
			rrXV *= -1.0f;
		}
		if(rrY < 0 && rrYV < 0.0f) {
			rrYV *= -1.0f;
		}
		if(rrY > screenHeight && rrYV > 0.0f) {
			rrYV = 0.0f;
			rrIsInAir = false;
			Debug.Log ("Died");
		}

		drawStickManAt((int)rrX,(int)rrY);
	}

	void drawStickManAt(int atX, int atY) {
		copyBitmapFromToColorArray(0,0,
		                           16, 16,
		                           (int)rrX-8,(int)rrY-16,
		                           rrBitmap,16);
	}
	
	void Centerrr() {
		rrX = screenWidth/4;
		rrY = screenHeight/2;
		rrXV = 0.0f;
		rrYV = 0.0f;

		platSpeedX = -1;
	}

	public override void PerGameStart() {
		platformList.Clear();

		float nextPlatformX = 0;
		float howLongIsNextPlatform;
		float platformGap;
		for(int i=0;i<10;i++) {
			howLongIsNextPlatform = Random.Range(5,20);
			platformGap = Random.Range(8,12);

			platformList.Add ( new RRPlatform(nextPlatformX,
			                                  (int)(Random.Range(0.4f,0.8f)*screenHeight),
			                                  (int)howLongIsNextPlatform) );
			nextPlatformX += howLongIsNextPlatform + platformGap;
		}

		Centerrr();
		// disableAutoScreenClear = true;
	}

	public override void PerGameExit() {
		Centerrr();
	}

	// Demo Screen player sees in game before putting in tokens
	public override void PerGameDemoMode() {
		rrMoveAndDraw(); // for this game just let the rr bounce

		drawStringCentered(screenWidth/2,screenHeight/8,whiteCol,"Ronald Reagan");
		drawStringCentered(screenWidth/2,screenHeight/8+8,redCol,"Saves America");
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

	void moveAndDrawPlatforms() {

		/*platformX += platSpeedX;
		platformY += platSpeedY;

		drawBoxAt((int)platformX, (int)platformY,(int)platformW,(int)platformH,yellowCol);*/

		foreach( RRPlatform onePlat in platformList ) {
			onePlat.x += platSpeedX;
			// Debug.Log ( "X:"+(int)onePlat.x + " Y:" + (int)onePlat.y + " W:" + (int)onePlat.width + " H:" + (int)platformH );
			drawBoxAt((int)onePlat.x, (int)onePlat.y,(int)onePlat.width,(int)platformH,yellowCol);
		}
	}

	// seems like this is our main loop	
	public override void PerGameLogic() {


		rrMoveAndDraw();

		moveAndDrawPlatforms();


		// Debugging screen
		/*
		Debug.Log (screenHeight + "HEIGHT");
		Debug.Log (screenWidth + "WIDTH");
		*/
	}
	
}