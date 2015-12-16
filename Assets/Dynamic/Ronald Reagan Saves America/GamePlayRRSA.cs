using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RRPlatform {
	public float x,y, width;

	public RRPlatform() {
	}
	public RRPlatform(float startX, float startY, float myWidth) {
		x = startX;
		y = startY;
		width = myWidth;
	}
	
	public void ShiftPlatform(float moveX) {
		x += moveX;
	}
}

public class GamePlayRRSA : PixelScreenLibBigText {
	public Texture2D truefaceImg;
	PixelSprite truefaceSprite;
	public Texture2D rrImg;
	PixelSprite rrSprite;

	float rrX = 25;
	float rrY = 20;
	float rrXV = 3.4f;
	float rrYV = 1.4f;

	float fakeGravity = 3.1f;

	bool rrIsInAir = false;

	List<RRPlatform> platformList = new List<RRPlatform>();
	List<RRPlatform> platformSpares = new List<RRPlatform>();

	// universal shared platform values
	float platSpeedX = 1;
	float platformH = 14.0f*5.0f;

	public override void PerPixelGameBootup() {
		rrSprite = new PixelSprite(rrImg);
		truefaceSprite = new PixelSprite(truefaceImg);
	}

	public override void PerGameFakeAIInput() {
		if(Random.Range(0, 100) < 10) {
			if(rrIsInAir == false) {
				rrYV = -40.0f;
				rrIsInAir = true;
			} else if(rrYV<-4.5f) {
				rrYV = -4.5f;
			}
		}
	}

	public override void PerGameInput() {
		if(rrIsInAir == false) {
			if(Input.GetKeyDown(KeyCode.Space)) {
				rrYV = -40.0f;
				rrIsInAir = true;
			}
		} else if(rrYV<-4.5f) {
			if(Input.GetKeyUp(KeyCode.Space)) {
				rrYV = -4.5f;
			}
		}
	}

	// move and draw function
	private void rrMoveAndDraw() {
		if(rrIsInAir) {
			rrYV += fakeGravity;
		}

		rrX += rrXV;
		rrY += rrYV;

		rrIsInAir = true;
		foreach( RRPlatform onePlat in platformList ) {
			if( (rrX-10 > onePlat.x && rrX-10 < onePlat.x + onePlat.width &&
				rrY >= onePlat.y && rrY < onePlat.y + platformH) ||
				(rrX+10 > onePlat.x && rrX+10 < onePlat.x + onePlat.width &&
					rrY >= onePlat.y && rrY < onePlat.y + platformH)) {
				rrIsInAir = false;
				rrY = onePlat.y;
				rrYV = 0.0f;
			}
		}

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
			InstantLoseFromTimeDrain();
			// Debug.Log ("Died");
		}

		rrSprite.drawImage(this, (int)rrX-32,(int)rrY-64);
	}

	void Centerrr() {
		rrX = screenWidth/4;
		rrY = screenHeight/2;
		rrXV = 0.0f;
		rrYV = 0.0f;

		platSpeedX = -5;
	}

	int platLong() {
		return Random.Range(21, 35) * 4;
	}
	int platGap() {
		return Random.Range(6, 9) * 4-(int)(platSpeedX*3);
	}
	int platHeight() {
		return (int)(Random.Range(0.4f, 0.9f) * screenHeight);
	}

	public override void PerGameStart() {
		platformList.Clear();
		platformSpares.Clear();

		float nextPlatformX = 0;
		float howLongIsNextPlatform;
		float platformGap;

		howLongIsNextPlatform = 0.8f * screenWidth;
		platformGap = 0.1f * screenWidth;
		platformList.Add ( new RRPlatform(0, (int)(0.75f*screenHeight), howLongIsNextPlatform) );
		nextPlatformX += howLongIsNextPlatform + platformGap;

		for(int i=0;i<8;i++) {
			howLongIsNextPlatform = platLong();
			platformGap = platGap();

			platformList.Add ( new RRPlatform(nextPlatformX,
				platHeight(), (int)howLongIsNextPlatform) );
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
		truefaceSprite.drawImage(this, 0,0);
		MP_drawStringCentered(screenWidth/4-20,screenHeight/4-20,yellowCol,"BEST");
		MP_drawStringCentered(screenWidth/4-20,screenHeight/4-10,yellowCol,""+highScore);
	}	

	public override void PerGameDemoModeCoinRequestDisplay() {
		if( flashing ) {
			MP_drawStringCentered(screenWidth/4-20,15,redCol,"INSERT");
			MP_drawStringCentered(screenWidth/4-20,25,redCol,"TOKEN");
		}
	}

	public override void PerGameTimerDisplay() {
		MP_drawStringCentered(screenWidth/8,10,yellowCol,
		                   ""+ timerLeft);
	}

	void moveAndDrawPlatforms() {

		/*platformX += platSpeedX;
		platformY += platSpeedY;

		drawBoxAt((int)platformX, (int)platformY,(int)platformW,(int)platformH,yellowCol);*/

		float rightMostPlat = -1;
		for(int i=platformList.Count-1; i>=0;i--) {
			RRPlatform onePlat = platformList[i];

			onePlat.x += platSpeedX;
			if(onePlat.x + onePlat.width > rightMostPlat) {
				rightMostPlat = onePlat.x + onePlat.width;
			}
			// Debug.Log ( "X:"+(int)onePlat.x + " Y:" + (int)onePlat.y + " W:" + (int)onePlat.width + " H:" + (int)platformH );
			drawBoxAt((int)onePlat.x, (int)onePlat.y,(int)onePlat.width,(int)platformH,yellowCol);
			if(onePlat.x+onePlat.width<0) {
				platformSpares.Add(onePlat);
				platformList.RemoveAt(i);
				addToScore(1);
			} 
		}

		platSpeedX = -7.5f - (score / 3.25f);
			
		if(rightMostPlat < screenWidth) {
			RRPlatform extra;
			if(platformSpares.Count > 0) {
				extra = platformSpares[0];
				platformSpares.RemoveAt(0);
			} else {
				extra = new RRPlatform();
			}
			int platformGap = platLong();
			int howLongIsNextPlatform = platGap();
			extra.x = rightMostPlat + platformGap;
			extra.y = platHeight();
			extra.width = (int)howLongIsNextPlatform;
			platformList.Add(extra);
		}
	}

	// seems like this is our main loop	
	public override void PerGameLogic() {


		rrMoveAndDraw();

		moveAndDrawPlatforms();

		MP_drawStringCentered(20,15,blackCol,"SCORE");
		MP_drawStringCentered(20,25,blackCol,""+score);

		MP_drawStringCentered(screenWidth/4-20,15,blackCol,"BEST");
		MP_drawStringCentered(screenWidth/4-20,25,blackCol,""+highScore);

		// Debugging screen
		/*
		Debug.Log (screenHeight + "HEIGHT");
		Debug.Log (screenWidth + "WIDTH");
		*/
	}
	
}