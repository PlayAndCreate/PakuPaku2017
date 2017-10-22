using UnityEngine;
using System.Collections;

public class LogoMoveManager : MonoBehaviour {

	Animation anim;
	FadeInManager FM;
	float time;
	public bool isEndAnime;

	void Start() {
		anim = GetComponent<Animation> ();
		FM = this.GetComponent<FadeInManager> ();
		isEndAnime = false;
	}

	void Update () {
		if (FM.isComplete) {
			Debug.Log ("!!!!"); 
			anim.Play();
			time += Time.deltaTime;
		}
		if (time > 1.0f) {
			isEndAnime = true;
		}
	}
}
