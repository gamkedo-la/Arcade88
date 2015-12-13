using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FallingCatOrDog {
	public float x,y;
	public float xv,yv;
	public bool isFlipped;
	public bool isDog;

	public FallingCatOrDog() {
		x = y = 10;
		xv = yv = 0;
		isFlipped = (Random.Range(0,10)<5);
		isDog = (Random.Range(0,10)<5);
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
	private PixelSprite dodgerSprite;

	public Texture2D catImg;
	private PixelSprite catSprite;

	public Texture2D dogImg;
	private PixelSprite dogSprite;

	float playerX;
	int playerY;

	List<FallingCatOrDog> catsAndDogs = new List<FallingCatOrDog>();

	public override void PerPixelGameBootup() {
		dodgerSprite = new PixelSprite(dodgerImg);
		catSprite = new PixelSprite(catImg);
		dogSprite = new PixelSprite(dogImg);

		ResetAllEnemies();
	}

	public override void PerGameInput() {
		float playerSpeed = 23.0f;
		float playerEdgeMargin = 5.0f;

		if(Input.GetKey(KeyCode.LeftArrow) && playerX > playerEdgeMargin) {
			playerX -= playerSpeed * Time.deltaTime;
			dodgerSprite.isFacingLeft = true;
		} else if(Input.GetKey(KeyCode.RightArrow) && playerX < screenWidth-1-playerEdgeMargin) {
			playerX += playerSpeed * Time.deltaTime;
			dodgerSprite.isFacingLeft = false;
		}
	}

	void ResetEnemy(FallingCatOrDog enemy) {
		float spawnMargin = 6;
		enemy.SetPos( Random.Range( spawnMargin, screenWidth-1-spawnMargin),
		             spawnMargin-Random.Range(0.0f,screenHeight));
		enemy.xv = Random.Range(-0.5f,0.5f);
		enemy.yv = Random.Range (2.0f,3.4f);
	}

	void addPet() {
		FallingCatOrDog nextChar = new FallingCatOrDog();
		ResetEnemy(nextChar);
		catsAndDogs.Add( nextChar );
	}

	void ResetAllEnemies() {
		catsAndDogs.Clear();
		float spawnMargin = 6;
		for(int i = 0; i<10; i++) {
			FallingCatOrDog nextChar = new FallingCatOrDog();
			ResetEnemy(nextChar);
			catsAndDogs.Add( nextChar );
		}
	}

	void MoveAndDrawEnemies() {
		foreach(FallingCatOrDog catOrDog in catsAndDogs) {
			catOrDog.Move();
			if(catOrDog.y >= screenHeight) {
				ResetEnemy(catOrDog);
			} else {
				if(catOrDog.y > playerY) {
					if(Mathf.Abs(catOrDog.x - playerX) < 6) {
						InstantLoseFromTimeDrain();
					}
				}

				if(catOrDog.isDog) {
					dogSprite.isFacingLeft = catOrDog.isFlipped;
					dogSprite.drawImage(this,(int)catOrDog.x-8,(int)catOrDog.y-8);
				} else {
					catSprite.isFacingLeft = catOrDog.isFlipped;
					catSprite.drawImage(this,(int)catOrDog.x-8,(int)catOrDog.y-8);
				}
			}
		}
		drawStringCentered(screenWidth-15,5,yellowCol, "high");

		drawStringCentered(screenWidth-15,15,yellowCol, ""+ highScore);
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
		drawStringCentered(15,5,whiteCol, "time");
		drawStringCentered(15,15,whiteCol,
		                   ""+ timerLeft);
	}

	public override void PerGameLogic() {
		if(catsAndDogs.Count < (playTime-timerLeft)) {
			addPet();
		}
		if(score < ((endOfPlayTime-Time.time) * 100) ) {
			addToScore(1);
		}
		MoveAndDrawEnemies();
		dodgerSprite.drawImage(this,(int)playerX-8,playerY-11);
		drawStringCentered(screenWidth+15,5,whiteCol, "score");
		drawStringCentered(screenWidth+15,15,whiteCol, ""+ score);
	}
	
}