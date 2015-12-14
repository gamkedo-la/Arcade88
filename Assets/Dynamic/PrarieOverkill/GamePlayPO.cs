using UnityEngine;
using System.Collections;

public class GamePlayPO : PixelScreenLib {

	enum direction {W, NW, N, NE, E, SE, S, SW};

	public Texture2D playerImg;
		private PixelSprite playerPOSprite;
		private float pX;
		private float pY;
		private direction playerFacing;
	
	public Texture2D buffaloImg;
		private PixelSprite buffaloSprite;
		private float bX;
		private float bY;

	public Texture2D laserImg;
		private PixelSprite laserSprite1;
		private PixelSprite laserSprite2;
		private float l1X;
		private float l1Y;
		private float l2X;
		private float l2Y;
		private direction laser1Facing; 
		private direction laser2Facing;
		private bool laser1Ready;
		private bool laser2Ready;

	public Texture2D treeImg;
		private PixelSprite treeSprite;
		private float tX;
		private float tY;

	
	
	

	float ballX = 25;
	float ballY = 20;
	float ballXV = 3.4f;
	float ballYV = 1.4f;

	public override void PerPixelGameBootup() { // happens once per game universe
		playerPOSprite = new PixelSprite(playerImg, 16);
			playerPOSprite.isAnimating = false;

		buffaloSprite = new PixelSprite(buffaloImg);
			buffaloSprite.isAnimating = false;

		treeSprite = new PixelSprite(treeImg);
			treeSprite.isAnimating = false;

		laserSprite1 = new PixelSprite(laserImg, 16);
		laserSprite2 = new PixelSprite(laserImg, 16);
			laserSprite1.isAnimating = false;
			laserSprite2.isAnimating = false;
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

		if (anyDirHeld){ // Player Movement
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
		if(!laser1Ready){
			switch (laser1Facing){
				case direction.W:
					l1X --;
					break;
				case direction.NW:
					l1X --;
					l1Y --;
					break;
				case direction.N:
					l1Y --;
					break;	
				case direction.NE:
					l1X ++;
					l1Y --;
					break;	
				case direction.E:
					l1X ++;
					break;
				case direction.SE:
					l1X ++;
					l1Y ++;
					break;	
				case direction.S:
					l1Y ++;
					break;
				case direction.SW:
					l1X --;
					l1Y ++;
					break;				
			} // Laser 1 Movement
		}
		if(!laser2Ready){
			switch (laser2Facing){
				case direction.W:
					l2X --;
					break;
				case direction.NW:
					l2X --;
					l2Y --;
					break;
				case direction.N:
					l2Y --;
					break;	
				case direction.NE:
					l2X ++;
					l2Y --;
					break;	
				case direction.E:
					l2X ++;
					break;
				case direction.SE:
					l2X ++;
					l2Y ++;
					break;	
				case direction.S:
					l2Y ++;
					break;
				case direction.SW:
					l2X --;
					l2Y ++;
					break;				
			} // Laser 2 Movement
		}
		if(Input.GetKeyDown(KeyCode.Space)) {
			FireLaser();
		}
		if(l1X < 0 || l1X > screenWidth || l1Y < 0 || l1Y > screenHeight)
		{
			laser1Ready = true;
		}
		if(l2X < 0 || l2X > screenWidth || l2Y < 0 || l2Y > screenHeight)
		{
			laser2Ready = true;
		}

		playerPOSprite.drawFrame = (int)playerFacing;
		laserSprite1.drawFrame = (int)laser1Facing;
		laserSprite2.drawFrame = (int)laser2Facing;
		
	}

	private void Draw() {
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
		if(!laser1Ready){
			laserSprite1.drawImage(this, (int)l1X, (int)l1Y);
		}
		if(!laser2Ready){
			laserSprite2.drawImage(this, (int)l2X, (int)l2Y);
		}
		
	}

	private void FireLaser(){
		if (laser1Ready){
			l1X = pX;
			l1Y = pY;
			laser1Facing = playerFacing;
			laser1Ready = false;
		}
		else if (laser2Ready){
			l2X = pX;
			l2Y = pY;
			laser2Facing = playerFacing;
			laser2Ready = false;
		}

	}



	void CenterPlayer() {
		pX = screenWidth/2;
		pY = screenHeight/2;
	}

	public override void PerGameStart() { // happens every time cabinet is started
		CenterPlayer();
		laser1Ready = true;
		laser2Ready = true;
	}

	public override void PerGameExit() { // ever time game over
		CenterPlayer();
	}

	public override void PerGameDemoMode() {
		Draw(); // for this game just let the ball bounce

		drawStringCentered(screenWidth/2,screenHeight/8,whiteCol,"PRAIRIE OVERKILL");
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
		Draw();
	}
	
}