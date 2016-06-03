using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CToggleCredits : MonoBehaviour {
	Text creditsText;
	string fullCredits = "";
	string placeholderInstruction = "H to Toggle Help";
	bool showingCredits = false;

	// Use this for initialization
	void Start () {
		creditsText = GetComponent<Text>();
		fullCredits = creditsText.text;
		creditsText.text = placeholderInstruction;
	}
	
	// Update is called once per frame
	void Update () {
		if( MenuManager.tournyMode ) {
			creditsText.text = PlayerDistrib.instance.ScoresReport();
		} else if(Input.GetKeyDown(KeyCode.H)) {
			showingCredits = !showingCredits;
			if(showingCredits) {
				creditsText.text = fullCredits + "\n"+placeholderInstruction;
			} else {
				creditsText.text = placeholderInstruction;
			}
		}
	}
}
