using UnityEngine;
using System.Collections;

public class StompTester : MonoBehaviour {
	public GamePlay2 reportStompTo;

	void OnCollisionEnter(Collision col) {
		reportStompTo.GotStomp();
	}
}
