using UnityEngine;
using System.Collections;

public class TurnOnAtStart : MonoBehaviour {
	public GameObject toEnable;

	void Awake() {
		if(toEnable) {
			toEnable.SetActive(true);
		}
	}
}
