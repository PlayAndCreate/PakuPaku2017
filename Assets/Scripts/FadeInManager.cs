using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FadeInManager: MonoBehaviour {
	
	[SerializeField]
	private float alfa;
	private float r, g, b;
	[SerializeField]
	private float time;
	[SerializeField]
	float speed;
	public bool isStart, isComplete;

	void Start () {
		speed = 0.75f;
		r = GetComponent<Image> ().color.r;
		g = GetComponent<Image> ().color.g;
		b = GetComponent<Image> ().color.b;
		alfa = 0;
		isStart = false;
		isComplete = false;
	}
	
	void Update () {
		GetComponent<Image> ().color = new Color (r, g, b, alfa);
		alfa = Mathf.Log(time + 1.0f);
		if (isStart) {
			time += speed * Time.deltaTime;
		}
		if (alfa > 1) {
			isComplete = true;
		}
	}
}
