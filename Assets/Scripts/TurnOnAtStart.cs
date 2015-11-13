using UnityEngine;
using System.Collections;

public class TurnOnAtStart : MonoBehaviour {
	public GameObject toEnable;

	void Awake() {
		toEnable.SetActive(true);
	}
}
