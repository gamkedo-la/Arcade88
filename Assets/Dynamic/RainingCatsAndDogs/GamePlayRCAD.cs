using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BasicCharacter {
	public float x,y;
	public float xv,yv;
	public Color32[] bmp;

	public BasicCharacter(Color32[] useBmp) {
		x = y = 10;
		xv = yv = 0;
		bmp = useBmp;
	}

	public void SetPos(float sX, float sY) {
		x = sX;
		y = sY;
	}

	public void Move() {
		x += xv;
		y += yv;
	}
}

public class GamePlayRCAD : PixelScreenLib {
	public Texture2D dodgerImg;
	Color32[] dodgerBitmap;

	public Texture2D catImg;
	Color32[] catBitmap;

	public Texture2D dogImg;
	Color32[] dogBitmap;

	float playerX;
	int playerY;
	bool runLeft = false;

	List<BasicCharacter> catsAndDogs = new List<BasicCharacter>();

	public override void PerPixelGameBootup() {
		dodgerBitmap = dodgerImg.GetPixels32();
		catBitmap = catImg.GetPixels32();
		dogBitmap = dogImg.GetPixels32();
		ResetAllEnemies();
	}

	public override void PerGameInput() {
		float playerSpeed = 23.0f;
		float playerEdgeMargin = 5.0f;

		if(Input.GetKey(KeyCode.LeftArrow) && playerX > playerEdgeMargin) {
			playerX -= playerSpeed * Time.deltaTime;
			runLeft = true;
		} else if(Input.GetKey(KeyCode.RightArrow) && playerX < screenWidth-1-playerEdgeMargin) {
			playerX += playerSpeed * Time.deltaTime;
			runLeft = false;
		}
	}

	void ResetEnemy(BasicCharacter enemy) {
		float spawnMargin = 6;
		enemy.SetPos( Random.Range( spawnMargin, screenWidth-1-spawnMargin),
		             spawnMargin-Random.Range(0.0f,screenHeight));
		enemy.xv = Random.Range(-0.5f,0.5f);
		enemy.yv = Random.Range (2.0f,3.4f);
	}

	void ResetAllEnemies() {
		catsAndDogs.Clear();
		float spawnMargin = 6;
		for(int i = 0; i<15; i++) {
			BasicCharacter nextChar = 
				new BasicCharacter(Random.Range(0,100)<50 ? dogBitmap : catBitmap);
			ResetEnemy(nextChar);
			catsAndDogs.Add( nextChar );
		}
	}

	void MoveAndDrawEnemies() {
		foreach(BasicCharacter catOrDog in catsAndDogs) {
			catOrDog.Move();
			if(catOrDog.y >= screenHeight) {
				ResetEnemy(catOrDog);
			} else {
				copyBitmapFromToColorArray(0,0,
				                           16, 16,
				                           (int)catOrDog.x-8,(int)catOrDog.y-8,
				                           catOrDog.bmp,16);
			}
		}
	}

	public override void PerGameStart() {
		playerX = screenWidth/2;
		playerY = screenHeight-5;

		ResetAllEnemies();
	}

	public override void PerGameExit() {
		// ResetAllEnemies();
	}

	public override void PerGameDemoMode() {
		MoveAndDrawEnemies();

		drawStringCentered(screenWidth/2,screenHeight/8,whiteCol,"Raining Cats");
		drawStringCentered(screenWidth/2,screenHeight/8+8,whiteCol,"and Dogs");
	}

	public override void PerGameDemoModeCoinRequestDisplay() {
		if( flashing ) {
			drawStringCentered(screenWidth/2,screenHeight/2,yellowCol,"INSERT TOKEN");
		}
	}

	public override void PerGameTimerDisplay() {
		drawStringCentered(screenWidth-15,15,whiteCol,
		                   ""+ timerLeft);
	}

	public override void PerGameLogic() {
		MoveAndDrawEnemies();
		copyBitmapFromToColorArray(0,0,
			                       16, 16,
		                           (int)playerX-8,playerY-11,
		                           dodgerBitmap,dodgerImg.width,0,runLeft);
	}
	
}