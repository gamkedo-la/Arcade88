using UnityEngine;
using System.Collections;

public class GamePlayPO : PixelScreenLib {

	enum direction {W, NW, N, NE, E, SE, S, SW};

	public Texture2D playerImg;
	public Texture2D buffaloImg;
	public Texture2D treeImg;
	private PixelSprite playerPOSprite;
	private float pX;
	private float pY;
	//private PixelSprite buffaloSprite;
	//private PixelSprite treeSprite;

	private direction playerFacing; // 1-8, starting with left and moving clockwise

	float ballX = 25;
	float ballY = 20;
	float ballXV = 3.4f;
	float ballYV = 1.4f;

	public override void PerPixelGameBootup() { // happens once per game universe
		playerPOSprite = new PixelSprite(playerImg, 16);
		playerPOSprite.isAnimating = false;
		//buffaloSprite = new PixelSprite(buffaloImg);
		//treeSprite = new PixelSprite(treeImg);
	}

	public override void PerGameInput() {

		// arrow keys set position frame 1 and then move after

	
		bool anyDirHeld = false;

		if(Input.GetKey(KeyCode.LeftArrow)) { 
			playerFacing = direction.W;
			anyDirHeld = true;
		}
		if(Input.GetKey(KeyCode.UpArrow)) { 
			playerFacing = direction.N;
			anyDirHeld = true;
		}
		if(Input.GetKey(KeyCode.RightArrow)) { 
			playerFacing = direction.E;
			anyDirHeld = true;
		}
		if(Input.GetKey(KeyCode.DownArrow)) { 
			playerFacing = direction.S;
			anyDirHeld = true;
		}
		if(Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.UpArrow)) { 
			playerFacing = direction.NW;
			anyDirHeld = true;
		}
		if(Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.DownArrow)) { 
			playerFacing = direction.SW;
			anyDirHeld = true;
		}
		if(Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.DownArrow)) { 
			playerFacing = direction.SE;
			anyDirHeld = true;
		}
		if(Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.UpArrow)) { 
			playerFacing = direction.NE;
			anyDirHeld = true;
		}

		if (anyDirHeld){
			switch (playerFacing){
				case direction.W:
					pX --;
					break;
				case direction.NW:
					pX --;
					pY --;
					break;
				case direction.N:
					pY --;
					break;	
				case direction.NE:
					pX ++;
					pY --;
					break;	
				case direction.E:
					pX ++;
					break;
				case direction.SE:
					pX ++;
					pY ++;
					break;	
				case direction.S:
					pY ++;
					break;
				case direction.SW:
					pX --;
					pY ++;
					break;				
			}
		}

		playerPOSprite.drawFrame = (int)playerFacing;

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

		playerPOSprite.drawImage(this, (int)pX, (int)pY);
	}



	void CenterPlayer() {
		pX = screenWidth/2;
		pY = screenHeight/2;
	}

	public override void PerGameStart() { // happens every time cabinet is started
		CenterPlayer();
	}

	public override void PerGameExit() { // ever time game over
		CenterPlayer();
	}

	public override void PerGameDemoMode() {
		ballBounceAndDraw(); // for this game just let the ball bounce

		drawStringCentered(screenWidth/2,screenHeight/8,whiteCol,"PRARIE OVERKILL");
		drawStringCentered(screenWidth/2,screenHeight/8+8,redCol,"9000 LBS OF BUFFALO");
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