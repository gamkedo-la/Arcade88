using UnityEngine;
using System;
using System.Collections;

[Serializable]
public class QuestionOption
{
	public string theyAsk;
	public bool yesIsRight;
}

public class AdultText : MonoBehaviour {
	public QuestionOption[] questionList;

	private int quesIdx = 0; // cycle for now, later random select via permutation

	public QuestionOption GetNextQues() {
		QuestionOption toRet = questionList[quesIdx];
		quesIdx++;
		if(quesIdx >= questionList.Length) {
			quesIdx = 0; // wrap for now
			Debug.Log("Out of quiz questions, TODO: Not Not Not randomization");
		}
		return toRet;
	}
}