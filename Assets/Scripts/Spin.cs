using UnityEngine;
using System.Collections;

public class Spin : MonoBehaviour {
	public float spinAmt = 70.0f;

	// Update is called once per frame
	void LateUpdate () {
		transform.Rotate(0.0f, spinAmt * Time.deltaTime, 0.0f);
	}
}
