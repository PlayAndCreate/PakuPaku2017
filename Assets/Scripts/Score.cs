using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//**********開始**********//
using UnityEngine.UI;
//**********終了**********//

public class Score : MonoBehaviour {
	
	//**********開始**********//
	public Text scoreText;
	public static int score = 0;
	//**********終了**********//
	
	
	
	// Use this for initialization
	void Start () {
		scoreText.text = "Score : 0";
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey(KeyCode.M))
		{
			//スコアに加算
			score += 10;
			
			//スコアを更新
			scoreText.text = "Score : " + score.ToString();
		}
	}
}
