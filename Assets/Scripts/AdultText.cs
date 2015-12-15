using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class QuestionOption
{
	public string theyAsk;
	public bool yesIsRight;
}

public class AdultText : MonoBehaviour {
	public QuestionOption[] questionList;
	List<QuestionOption> randOrder;

	public void RandOrder<T>(IList<T> list)  
	{  
		int n = list.Count;  
		while (n > 1) {  
			n--;  
			int k = UnityEngine.Random.Range(0, n);
			T value = list[k];  
			list[k] = list[n];  
			list[n] = value;  
		}  
	}

	private int quesIdx = 0; // cycle for now, later random select via permutation

	void Shuffle() {
		randOrder.Clear();
		for(int i = 0; i < questionList.Length; i++) {
			randOrder.Add(questionList[i]);
		}
		RandOrder(randOrder);
	}

	void Start() {
		randOrder = new List<QuestionOption>();
		Shuffle();
	}

	public QuestionOption GetNextQues() {
		/*QuestionOption toRet = questionList[quesIdx];
		quesIdx++;
		if(quesIdx >= questionList.Length) {
			quesIdx = 0; // wrap for now
		}*/
		QuestionOption toRet;
		toRet = randOrder[0];
		randOrder.RemoveAt(0);
		if(randOrder.Count <= 0) {
			Shuffle();
		}
		return toRet;
	}
}