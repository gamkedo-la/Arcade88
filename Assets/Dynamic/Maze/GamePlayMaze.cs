using UnityEngine;
using System.Collections;

public class GamePlayMaze : PixelScreenLib {
	public Texture2D mazeImg;
	Color32[] mazeBitmap;

	int dotX = 25;
	int dotY = 20;

	public override void PerPixelGameBootup() {
		disableAutoScreenClear = true; // maze
		mazeBitmap = mazeImg.GetPixels32();
	}

	int aiDX=0;
	int aiDY=1;
	public override void PerGameFakeAIInput() {
		int nextX = dotX+aiDX;
		int nextY = dotY+aiDY;

		if(AttemptMoveTo(nextX, nextY) == false || Random.Range(0,60)<2) {
			aiDX = aiDY = 0;
			switch(Random.Range(0, 4)) {
			case 0:
				aiDX = 1;
				break;
			case 1:
				aiDX = -1;
				break;
			case 2:
				aiDY = 1;
				break;
			case 3:
				aiDY = -1;
				break;
			}
		}
	}

	public override void PerGameInput() {
		int nextX = dotX;
		int nextY = dotY;

		if(score > 0) {
			return;
		}

		int moveSpeed = 2;

		if(Input.GetKey(KeyCode.LeftArrow)) {
			nextX-=moveSpeed;
		}
		if(Input.GetKey(KeyCode.RightArrow)) {
			nextX+=moveSpeed;
		}
		
		if(Input.GetKey(KeyCode.UpArrow)) {
			nextY-=moveSpeed;
		}
		if(Input.GetKey(KeyCode.DownArrow)) {
			nextY+=moveSpeed;
		}

		AttemptMoveTo(nextX, nextY);
	}

	private bool AttemptMoveTo(int nextX, int nextY) {
		Color32 destCol = getBitmapColor(nextX,nextY,mazeBitmap, mazeImg.width, mazeImg.height);

		if( destCol == blackCol ) {
			dotX = nextX;
			dotY = nextY;
			return true;
		} else if( destCol == greenCol ) {
			addToScore( (int) ((endOfPlayTime-Time.time) * 100) );
			if(timerLeft >= 10) {
				endOfPlayTime = Time.time + 9;
			}
			dotX = nextX;
			dotY = nextY;
		}
		return false;
	}

	private void drawMaze() {
		copyBitmapFromToColorArray(0,0,
		                           128, 128,
		                           0,0,
		                           mazeBitmap,mazeImg.width);

		if(score > 0) {
			drawBoxAt(0,screenHeight/2-10,screenWidth,25,bgCol);

			drawStringCentered(screenWidth/2,screenHeight/2-8,yellowCol,"Your Score: " + score);
			drawStringCentered(screenWidth/2,screenHeight/2+8,yellowCol,"High Score: " + highScore);
		}
	}

	void PlaceDotStart() {
		int mazeWid = mazeImg.width, mazeHei=mazeImg.height;
		score = 0;
		for(int x=0;x<mazeImg.width;x++) {
			for(int y=0;y<mazeImg.height;y++) {
				if( getBitmapColor(x,y,mazeBitmap, mazeWid, mazeHei) == whiteCol ) {
					dotX = x;
					dotY = y;
					return;
				}
			}
		}
	}

	public override void PerGameStart() {
		PlaceDotStart();
	}

	public override void PerGameExit() {
		PlaceDotStart();
	}

	public override void PerGameDemoMode() {
		drawMaze(); // for this game just let the ball bounce

		drawStringCentered(screenWidth/2,screenHeight/8,whiteCol,"Maze");
		if(highScore > 0) {
			drawStringCentered(screenWidth/2,screenHeight/8+8,yellowCol,"High Score: " + highScore);
		} else {
			drawStringCentered(screenWidth/2,screenHeight/8+8,yellowCol,"Walls Slow You Down");
		}
	}

	public override void PerGameDemoModeCoinRequestDisplay() {
		if( flashing ) {
			drawStringCentered(screenWidth/2,screenHeight/2,greenCol,"INSERT TOKEN");
		}
	}

	public override void PerGameTimerDisplay() {
		drawStringCentered(14,7,yellowCol,
		                   ""+ timerLeft);
	}

	public override void PerGameLogic() {
		drawMaze();
		drawBoxAt(dotX-1,dotY-1,3,3,whiteCol);
	}
	
}