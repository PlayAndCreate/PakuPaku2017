using UnityEngine;
using System.Collections;

public class KeySetting : MonoBehaviour {

	void Start () {

	}

	void Update () {
		if ((Input.GetKeyDown(KeyCode.Space)))
			SceneLoad ();
	}
	public void SceneLoad () {
		Application.LoadLevel("Result");
	}
}
