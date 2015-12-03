using UnityEngine;
using System.Collections;

public class HatGame : PixelScreenLib {
	enum HatGamePhase {Demo, Conceal,Shuffle,Choose,Reveal,EndScreen};

	HatGamePhase gamePhase;
	int blinderSize;
	int blinderMax;
	public int shuffleTimes = 5;
	int shuffleCount;

	public Texture2D hatManCowbImg;
	private PixelSprite hatManCowbSprite;

	public Texture2D hatManGentImg;
	private PixelSprite hatManGentSprite;

	public Texture2D hatManPiraImg;
	private PixelSprite hatManPiraSprite;

	public Texture2D arrowImg;
	private PixelSprite arrowSprite;

	string[] names = {"Cowboy", "Gentleman", "Pirate"};
	int goalSlot;
	int[] dudeCoordX = {0,0,0};
	int[] dudeSlotGoal = {0,1,2};
	float[] dudeDrawNow = {0.0f,0.0f,0.0f};
	int selected = 1;

	public override void PerPixelGameBootup() {
		gamePhase = HatGamePhase.Demo;
		blinderMax = screenHeight-(int)(screenHeight*0.5)-9;
		blinderSize = 0;

		hatManCowbSprite = new PixelSprite(hatManCowbImg);
		hatManGentSprite = new PixelSprite(hatManGentImg);
		hatManPiraSprite = new PixelSprite(hatManPiraImg);

		arrowSprite = new PixelSprite(arrowImg);
		dudeCoordX[0] = (int)(screenWidth*0.25);
		dudeCoordX[1] = (int)(screenWidth*0.5);
		dudeCoordX[2] = (int)(screenWidth*0.75);
		for(int i=0;i<3;i++) {
			dudeDrawNow[i] = dudeCoordX[ dudeSlotGoal[i] ];
		}
	}

	public override void PerGameInput() {
		if(gamePhase == HatGamePhase.EndScreen) {
			if(Input.GetKeyDown(KeyCode.Space)) {
				endOfPlayTime = Time.time;
			}
		}

		if(gamePhase != HatGamePhase.Choose) {
			return;
		}

		if(Input.GetKeyDown(KeyCode.LeftArrow)) {
			selected--;
			if(selected < 0) {
				selected += 3;
			}
		}
		if(Input.GetKeyDown(KeyCode.RightArrow)) {
			selected++;
			if(selected >= 3) {
				selected -= 3;
			}
		}

		if(Input.GetKeyDown(KeyCode.Space)) {
			gamePhase = HatGamePhase.Reveal;
		}
	}

	private void dudesDraw() {
		int yPos = (int)(screenHeight*0.5);

		float slideK = 0.75f;
		int happy=0;
		for(int i=0;i<3;i++) {
			dudeDrawNow[i] = slideK * dudeDrawNow[i] +
				(1.0f- slideK) * dudeCoordX[ dudeSlotGoal[i] ];
			if(Mathf.Abs( dudeDrawNow[i] - dudeCoordX[ dudeSlotGoal[i] ] ) < 1.0f) {
				happy++;
			}
		}
		if(happy == 3 && gamePhase == HatGamePhase.Shuffle) {
			if(shuffleCount<=0) {
				goalSlot = Random.Range(0,3);
				gamePhase = HatGamePhase.Choose;
			} else  for(int i=0;i<3;i++) {
				int swapFrom = dudeSlotGoal[i];
				int swapWith = Random.Range(0,2);
				if(swapWith == i) {
					swapWith++;
					if(swapWith>=2) {
						swapWith = 0;
					}
				}
				dudeSlotGoal[i] = dudeSlotGoal[swapWith];
				dudeSlotGoal[swapWith] = swapFrom;
			}
			shuffleCount--;
		} else if(gamePhase == HatGamePhase.Demo) {
			for(int i=0;i<3;i++) {
				dudeSlotGoal[i] = i;
			}
		}

		hatManCowbSprite.drawImage(this,(int)dudeDrawNow[0]-16,yPos);
		hatManGentSprite.drawImage(this,(int)dudeDrawNow[1]-16,yPos);		
		hatManPiraSprite.drawImage(this,(int)dudeDrawNow[2]-16,yPos);
	}

	public override void PerGameStart() {
		gamePhase = HatGamePhase.Conceal;
	}

	public override void PerGameExit() {
		gamePhase = HatGamePhase.Demo;
		blinderSize = 0;
	}

	public override void PerGameDemoMode() {
		dudesDraw();

		drawStringCentered(screenWidth/2,screenHeight*3/4,blackCol,"Cowboy, Gentleman, Pirate");

		drawStringCentered(screenWidth/2,screenHeight/8,whiteCol,"HAT GUESSING GAME");
		drawStringCentered(screenWidth/2,screenHeight/8+8,greenCol,"WHOSE HAT IS IT, ANYWAY?");
	}

	public override void PerGameDemoModeCoinRequestDisplay() {
		if( flashing ) {
			drawStringCentered(screenWidth/2,screenHeight/2,greenCol,"INSERT TOKEN");
		}
	}

	public override void PerGameTimerDisplay() {
		drawStringCentered(8,5,yellowCol,
		                   ""+ timerLeft);
	}

	public override void PerGameLogic() {
		dudesDraw();

		int yPos = (int)(screenHeight*0.5);

		if(gamePhase == HatGamePhase.EndScreen) {
			drawStringCentered(screenWidth/2,screenHeight/8,(flashing ? yellowCol : greenCol),
			                   (goalSlot == dudeSlotGoal[selected] ? "YOU'RE A WINNER!" : "YOU GOT IT WRONG!"));
		} else if(gamePhase != HatGamePhase.Reveal) {
			if(flashing) {
				drawStringCentered(screenWidth/2,screenHeight/8,yellowCol,"WATCH THOSE HATS!");
			}
		} else {
			if(flashing) {
				drawStringCentered(screenWidth/2,screenHeight/8,whiteCol,"HERE'S THE ANSWER!");
			}
		}

		if(gamePhase == HatGamePhase.Conceal) {
			blinderSize++;
			if(blinderSize >= blinderMax) {
				blinderSize = blinderMax;
				gamePhase = HatGamePhase.Shuffle;
				shuffleCount = shuffleTimes;
			}
		} else if(gamePhase == HatGamePhase.Reveal) {
			blinderSize--;
			if(blinderSize <= 0) {
				blinderSize = 0;
				gamePhase = HatGamePhase.EndScreen;
			}
		}
		drawBoxAt(0,screenHeight-blinderSize,screenWidth,blinderSize,whiteCol);

		if(gamePhase == HatGamePhase.Choose) {
			arrowSprite.drawImage(this,dudeCoordX[selected]-8,yPos-16);
			drawStringCentered(screenWidth/2,screenHeight*3/4,blackCol,"Which is the: "+names[goalSlot]);
		}
		if(gamePhase == HatGamePhase.EndScreen) {
			drawStringCentered(screenWidth/2+1,screenHeight*3/4,blackCol,"Which hat is the "+names[goalSlot]+"'S?");
			drawStringCentered(screenWidth/2+1,screenHeight*3/4+6,whiteCol,"You picked: "+names[dudeSlotGoal[selected]]);
		}
	}
	
}