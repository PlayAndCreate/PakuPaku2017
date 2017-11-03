using UnityEngine;
using System.Collections;

public class Reflection : MonoBehaviour {
	float time;
	float scale;
	RectTransform rectTrans;
	// Use this for initialization
	void Start () {
		time = 0.0f;
		rectTrans = GetComponent<RectTransform> ();
		scale = rectTrans.localScale.x;
	}
	
	// Update is called once per frame
	void Update () {
		time += Time.deltaTime;
		if ((int)time % 2 == 0) {
			rectTrans.localScale = new Vector3 (1*scale, 1, 1);
		} else {
			rectTrans.localScale = new Vector3 (-1*scale, 1, 1);
		}
	}
}
