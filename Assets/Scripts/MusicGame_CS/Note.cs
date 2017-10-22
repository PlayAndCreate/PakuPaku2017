using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour {


	//ノーツのx軸
	[SerializeField]
	public float pos = 0;

	//ノーツの落ちてくるタイミング(整数値 4部音符,小数値 16部音符)(音楽のbpsに依存、音のbpmをしっかり設定しておくこと)
	[SerializeField]
	public float time = 0;
}
