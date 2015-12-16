﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ArcadePlayer : MonoBehaviour {
	public Text tokenBillText;
	public GameObject parentDialog;
	public TokenInteraction lastAdultSpokenTo;
	public Text adultSays;
	private bool expectingResponse;

	public GameObject gameOverMessage;

	public static PlayableGame playingNow = null;

	Coroutine prevMsgReset = null;

	public Transform dialogLookFocus;

	Rigidbody rb;
	public int ticketNum = 0;
	public int bills = 0;
	public int tokens = 0;

	public float zoomFOV = 45.0f;
	private float baseFOV;

	IEnumerator resetMessage() {
		yield return new WaitForSeconds(1.25f);
		tokenBillsChange(0,0);
	}

	string pluralString(int count, string rootWord) {
		if(count == 1) {
			return count+" "+rootWord;
		} else {
			return count+" "+rootWord+"s";
		}
	}

	public bool tokenBillsChange(int billDelta, int tokenDelta) {
		bool hadEnoughTokens = false;

		if(playingNow != null) {
			return hadEnoughTokens;
		}

		string disclaimer = "";

		if(prevMsgReset != null) {
			StopCoroutine(prevMsgReset);
			prevMsgReset = null;
		}

		if(bills + billDelta < 0) {
			disclaimer = "\nNeed bills!";
			prevMsgReset = StartCoroutine( resetMessage() );
		} else if(tokens + tokenDelta < 0) {
			disclaimer = "\nNeed tokens!";
			prevMsgReset = StartCoroutine( resetMessage() );
		} else {
			bills += billDelta;
			tokens += tokenDelta;

			if(billDelta < 0) {
				disclaimer = "\nSpent "+pluralString(-billDelta,"Bill");
			}
			else if(billDelta > 0) {
				disclaimer = "\nGot "+pluralString(billDelta,"Bill");
			}

			if(tokenDelta < 0) {
				SoundCenter.instance.PlayClipOn( SoundCenter.instance.coinSpend,
				                                transform.position, 1.0f);
				disclaimer += "\nSpent "+pluralString(-tokenDelta,"Token");
				hadEnoughTokens = true;
			}
			else if(tokenDelta > 0) {
				SoundCenter.instance.PlayClipOn( SoundCenter.instance.coinGet,
				                                transform.position, 1.0f);
				disclaimer += "\nGot "+pluralString(tokenDelta,"Token");
			}

			if( disclaimer.Length > 0 ) {
				prevMsgReset = StartCoroutine( resetMessage() );
			} else {
				disclaimer = "\nSpacebar to Use";
			}
		}

		tokenBillText.text = "Tickets: " +ticketNum+"\nBills: $" + bills +
			"\nTokens: " + tokens + disclaimer;

		return hadEnoughTokens;
	}

	// for adult dialog button
	public void TokenInteractionWith(bool wasYesAnswered) {
		parentDialog.SetActive(false);
		hideMouse();
		// if(lastAdultSpokenTo.tag == "Parent") {
			if(expectingResponse == wasYesAnswered) {
				if(lastAdultSpokenTo) {
					lastAdultSpokenTo.tokenExchange(this);
				}
				SoundCenter.instance.PlayClipOn( SoundCenter.instance.billGet,
				                                transform.position, 1.0f);
			} else {
				if(gameOverMessage) {
					gameOverMessage.SetActive(true);
				}
				// Application.LoadLevel( Application.loadedLevel );
				SoundCenter.instance.PlayClipOn( SoundCenter.instance.adultTalk,
				                                transform.position, 1.0f);
			}
		// }
	}

	void hideMouse() {
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	void showMouse() {
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
	}

	// Use this for initialization
	void Start() {
		baseFOV = Camera.main.fieldOfView;
		hideMouse();
		rb = GetComponent<Rigidbody>();
		tokenBillsChange(1,0);
		if(gameOverMessage) {
			gameOverMessage.SetActive(false);
		}
	}

	void LookToward(Transform thatObject) {
		Vector3 rotDiff = thatObject.position-transform.position;
		rotDiff.y = 0.0f;
		transform.rotation = Quaternion.Slerp(transform.rotation,
		                                      Quaternion.LookRotation(rotDiff),
		                                      Time.deltaTime);
	}

	void TakePositionFor(PlayableGame whichGame) {
		Vector3 sameHeightFix = whichGame.standHere.position;
		sameHeightFix.y = transform.position.y;
		transform.position = Vector3.Slerp(transform.position,
		                                   sameHeightFix,
		                                   Time.deltaTime);
	}

	IEnumerator StartGameInAMoment(PlayableGame playScript) {
		yield return new WaitForSeconds(0.05f); // keeps spacebar from counting as input on this game
		playScript.playerHere = gameObject;
		if(prevMsgReset != null) {
			StopCoroutine(prevMsgReset);
		}
		playingNow = playScript;
		playingNow.gameScreen.GameStart();
		tokenBillText.text = "" + playingNow.gameName.Replace("\\n", "\n") + "\nBACKSPACE: QUIT\n" +
			playingNow.gameInstructions.Replace("\\n", "\n");
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKey( KeyCode.Escape )) {
			showMouse();
			Application.Quit();
		}

		if(gameOverMessage && gameOverMessage.activeSelf) {
			if(Input.GetKeyDown(KeyCode.Space)) {
				Application.LoadLevel(Application.loadedLevel);
			}
			return; // game frozen, player lost
		}

		if(playingNow != null) {
			LookToward(playingNow.transform);
			TakePositionFor(playingNow);

			if(Input.GetKeyDown(KeyCode.Backspace) || playingNow.gameScreen.isPlaying == false) {
				// playingNow.gameScreen.isPlaying = false; // nah, leave it running but cease input!

				PlayerDistrib.instance.Shuffle(); // BEFORE releasing player assignment, or it may appear here
				playingNow.playerHere = null; // freeing up player assignment AFTER the shuffle

				playingNow = null;
				tokenBillsChange(0,0);
			}
			return;
		}

		if(parentDialog.activeSelf) {
			LookToward(dialogLookFocus.transform);
			return; // freeze while in dialog
		}

		if(Input.GetKeyDown(KeyCode.Space)) {
			RaycastHit rhInfo;
			if( Physics.Raycast(transform.position, transform.forward, out rhInfo, 3.0f)) {
				AdultText atScript = rhInfo.collider.GetComponent<AdultText>();
				if(atScript) {
					parentDialog.SetActive(true);
					showMouse();
					lastAdultSpokenTo = rhInfo.collider.GetComponent<TokenInteraction>();
					dialogLookFocus = rhInfo.collider.transform;

					if(atScript) {
						QuestionOption grabQues = atScript.GetNextQues();
						int randNots = Random.Range(0,5);
						expectingResponse = 
							(randNots % 2 ==1 ? grabQues.yesIsRight == false :
							 					grabQues.yesIsRight);

						string notReplace = "";
						for(int i=0;i<randNots;i++) {
							notReplace += " not";
						}
						
						adultSays.text = grabQues.theyAsk.Replace(" %NOTS%",notReplace)/* +
							"\n(Expecting: " + (expectingResponse ? "Yes" : "No")+")"*/;
					} else {
						adultSays.text = "(Needs dialog data!)";
					}
					SoundCenter.instance.PlayClipOn( SoundCenter.instance.adultTalk,
					                                transform.position, 1.0f);
				} else {
					int hadTokens = tokens;

					PlayableGame playScript = rhInfo.collider.GetComponent<PlayableGame>();

					TokenInteraction tiScript = rhInfo.collider.GetComponent<TokenInteraction>();

					if(tiScript == null) {
						return;
					}

					float distFromFront;

					if(playScript) {
						if(playScript.playerHere != null) {
							distFromFront = 500.0f;
						} else {
							distFromFront = Vector3.Distance(transform.position,
								playScript.gameScreen.myCab.standHere.position);
						}
					} else {
						distFromFront = 0.0f;
					}
					
					if(distFromFront < 1.5f) {
						bool hadTheMoney = false;
						if(playScript && playScript.gameScreen.isPlaying) {
							hadTheMoney = true;
							Debug.Log("no tokens needed, already in play");
						} else if(tiScript) {
							hadTheMoney = tiScript.tokenExchange(this);
						} else {
							Debug.Log("Touched: " + rhInfo.collider.name);
						}

						if(playScript && hadTheMoney) {
							StartCoroutine(StartGameInAMoment(playScript));
						}
					}
				}
			} else {
				Debug.Log ("Not close enough");
			}
		}

		float mouseOrHorizKeys =
			Input.GetAxis("Horizontal") + Input.GetAxis("Mouse X");
		mouseOrHorizKeys = Mathf.Clamp(mouseOrHorizKeys, -1.0f, 1.0f);

		transform.Rotate(Vector3.up, mouseOrHorizKeys * Time.deltaTime * 85.0f);
	}

	void FixedUpdate() {
		if(gameOverMessage && gameOverMessage.activeSelf) {
			return; // game frozen, player lost
		}

		if(parentDialog.activeSelf==false && playingNow == null) {
			rb.velocity = transform.forward * Input.GetAxis("Vertical") * 5.0f;
		}

		float cameraK = 0.89f;
		if(playingNow != null) {
			Camera.main.fieldOfView *= cameraK;
			Camera.main.fieldOfView += zoomFOV*(1.0f-cameraK);
		} else {
			Camera.main.fieldOfView *= cameraK;
			Camera.main.fieldOfView += baseFOV*(1.0f-cameraK);
		}
	}
}
