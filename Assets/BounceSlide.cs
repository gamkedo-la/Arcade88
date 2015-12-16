using UnityEngine;
using System.Collections;

public class BounceSlide : MonoBehaviour {
	public GameObject slideAlong;
	public GameObject goalLeft;
	public GameObject goalRight;

	// Use this for initialization
	void Start () {
	
	}

	public int ScoreTest() {
		if(transform.localPosition.x > goalLeft.transform.localPosition.x &&
		   transform.localPosition.x < goalRight.transform.localPosition.x) {
			float basis;
			if(transform.localPosition.x < 0.0f) {
				basis = transform.localPosition.x - goalLeft.transform.localPosition.x;
			} else {
				basis = goalRight.transform.localPosition.x-transform.localPosition.x;
			}
			return 100 + (int)(basis*100000.0f);
		} else {
			return 0;
		}
		
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 leftEdge = slideAlong.transform.position - slideAlong.transform.right*0.5f*slideAlong.transform.localScale.x;
		Vector3 rightEdge = slideAlong.transform.position + slideAlong.transform.right*0.5f*slideAlong.transform.localScale.x;
		float timePerc = 0.5f + 0.5f*Mathf.Cos(Time.time*0.5f*1.5f);
		transform.position = timePerc * leftEdge + (1.0f - timePerc) * rightEdge;
	}
}
