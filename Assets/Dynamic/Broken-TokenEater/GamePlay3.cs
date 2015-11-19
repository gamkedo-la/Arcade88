using UnityEngine;
using System.Collections;

public class GamePlay3 : PixelScreenLib {

	public override void PerPixelGameBootup() {
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
		for(int i=0;i<1400;i++) {
			float randX = Random.Range(0,screenWidth);
			float randY = Random.Range(0,screenHeight);
			drawBoxAt((int)randX,(int)randY,2,2,(Random.Range(0,10)<5 ? whiteCol : blackCol));
		}
		
		drawString(15,15,redCol,"1234567890");
		drawString(15,35,yellowCol,"ABCDEFGHIJKLMNOPQRSTUVWXYZ");		
	}

	public override void PerGameDemoModeCoinRequestDisplay() {
		if( flashing ) {
			drawStringCentered(screenWidth/2,screenHeight/2,greenCol,"OUT OF ORDER");
		}
	}

}