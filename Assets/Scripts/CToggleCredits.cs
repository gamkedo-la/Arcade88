using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CToggleCredits : MonoBehaviour {
	Text creditsText;
	string fullCredits = "";
	string placeholderInstruction = "C to Toggle Credits";
	bool showingCredits = false;

	// Use this for initialization
	void Start () {
		creditsText = GetComponent<Text>();
		fullCredits = creditsText.text;
		creditsText.text = placeholderInstruction;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.C)) {
			showingCredits = !showingCredits;
			if(showingCredits) {
				creditsText.text = fullCredits + "\n"+placeholderInstruction;
			} else {
				creditsText.text = placeholderInstruction;
			}
		}
	}
}
