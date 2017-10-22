using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LogoManager : MonoBehaviour {

	public GameObject PACLogo;
	public GameObject JointMediaLogo;
	public GameObject MultipliedBy;

	public float time;

	void Start () {
		PACLogo			.GetComponent<FadeInManager>()		.isStart	= false;
		PACLogo			.GetComponent<FadeInManager>()		.isComplete	= false;
		JointMediaLogo	.GetComponent<FadeInManager>()		.isStart	= false;
		JointMediaLogo	.GetComponent<FadeInManager>()		.isComplete	= false;
		MultipliedBy	.GetComponent<FadeInManager>()		.isStart	= false;
		MultipliedBy	.GetComponent<FadeInManager>()		.isComplete	= false;

		PACLogo			.GetComponent<LogoMoveManager> ()	.isEndAnime = false;
	}
	
	void Update () {
		PACLogo.GetComponent<FadeInManager>().isStart = true;

		if (PACLogo.GetComponent<LogoMoveManager> ().isEndAnime) {
			MultipliedBy.GetComponent<FadeInManager> ().isStart = true;
		}

		if (MultipliedBy.GetComponent<FadeInManager> ().isComplete) {
			JointMediaLogo.GetComponent<FadeInManager>().isStart = true;
		}

		if (JointMediaLogo.GetComponent<FadeInManager> ().isComplete) {
			time += Time.deltaTime;
		}
		if (time > 1.5f) {
			Application.LoadLevel("Title");
		}
	}
}
