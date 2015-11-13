using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StrobeCanvasScale : MonoBehaviour {
	CanvasScaler csScript;

	// Use this for initialization
	void Start () {
		csScript = GetComponent<CanvasScaler>();
	}
	
	// Update is called once per frame
	void Update () {
		csScript.scaleFactor = Mathf.Cos (Time.time) * 0.2f + 1.0f;
	}
}
