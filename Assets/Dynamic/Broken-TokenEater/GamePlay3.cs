using UnityEngine;
using System.Collections;

public class GamePlay3 : PixelScreenLib {

	Color[] grays = new Color[5];
	int offCycle=0;

	public override void PerPixelGameBootup() {
		disableAutoScreenClear = true;

		for(int i=0;i<grays.Length;i++) {
			float fadeAmt = ((float)(i))/((float)(grays.Length));
			grays[i] = new Color(fadeAmt,fadeAmt,fadeAmt);
		}
	}

	public override void PerGameStart() {
		Debug.Log ("No game init needed for broken game");
	}

	public override void PerGameExit() {
		Debug.Log ("No game exit needed for broken game");
	}
		
	public override void PerGameInput() {
		isPlaying = false;
	}

	public override void PerGameDemoMode() {
		PerGameLogic();
	}

	public override void PerGameLogic() {
		int colCount = grays.Length;
		for(int i=0;i<128;i++) {
			for(int ii=0;ii<128;ii++) {
				if(Random.Range(0,20)<8) {
					screenBuffer[ i + (ii * screenWidth) ] = grays[Random.Range(0,colCount)];
				}
			}
		}
		
		drawString(15,15,redCol,"123456789ABCDEF");
		drawString(15,35,yellowCol,"ABCDEFGHIJKLMNOPQRSTUVWXYZ");		
	}

	public override void PerGameDemoModeCoinRequestDisplay() {
		if( flashing ) {
			drawStringCentered(screenWidth/2,screenHeight/2+1,blackCol,"OUT OF ORDER");
			drawStringCentered(screenWidth/2,screenHeight/2-1,blackCol,"OUT OF ORDER");
			drawStringCentered(screenWidth/2+1,screenHeight/2,blackCol,"OUT OF ORDER");
			drawStringCentered(screenWidth/2-1,screenHeight/2,blackCol,"OUT OF ORDER");
			drawStringCentered(screenWidth/2,screenHeight/2,whiteCol,"OUT OF ORDER");
		}
	}

}