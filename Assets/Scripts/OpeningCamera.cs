using UnityEngine;
using System.Collections;

public class OpeningCamera : MonoBehaviour {
	Quaternion baseRot;
	Vector3 basePos;
	float totalRot = 90.0f;
	float shufflePeopleRot = 90.0f;
	// Use this for initialization
	void Start () {
		baseRot = transform.rotation;
		basePos = transform.position;
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
	}

	// Update is called once per frame
	void Update () {
		float turnAmt = Time.deltaTime * 6.0f;
		shufflePeopleRot += turnAmt;
		totalRot += turnAmt;
		if(shufflePeopleRot >= 360.0f) {
			shufflePeopleRot -= 360.0f;
			PlayerDistrib.instance.Shuffle();
		}
		transform.rotation = Quaternion.AngleAxis(totalRot, Vector3.up) * baseRot *
			Quaternion.AngleAxis( Mathf.Cos (Time.time*0.4f) * 0.7f, Vector3.up) *
			Quaternion.AngleAxis( Mathf.Cos (Time.time*0.15f) * 1.3f, Vector3.right);
		transform.position = basePos +
			Vector3.up * Mathf.Cos (Time.time*0.3f) * 0.15f +
			Vector3.right * Mathf.Cos (Time.time*0.11f) * 0.4f;
	}
}
