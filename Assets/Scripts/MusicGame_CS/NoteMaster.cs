using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System.Runtime.InteropServices;

public class NoteMaster : MonoBehaviour {

	[SerializeField]
	private  Transform noteTarget ;

	[SerializeField]
	private float speed = 1;

	[SerializeField]
	private Transform camera;

	[SerializeField]
	private AudioSource bgm;

	[SerializeField]
	private int serialheight = 5;

	public int bpm = 155;

	public static NoteMaster self;

	private float bps;

	private GameObject notePrefab;
	private static SerialLib.UnitySerial serial;


	public float Bps{
		get{return bps;}
	}

	public GameObject Prefab {
		set{ notePrefab = value; }
	}

	void Awake(){
		self = this;
		try
		{
			Instantiate (Resources.Load (bgm.clip.name));
			serial = new SerialLib.UnitySerial("/dev/cu.usbmodem1411",9600,256);
			serial.ThreadStart();
		}
		catch
		{
			Debug.LogError("error" +
				"Resourcesに音楽の名前と同名のプレハブを作ってください(日本語NG)");
			UnityEditor.EditorApplication.isPlaying = false;
			return;
		}
	}

	IEnumerator Start(){
		Reset ();
		yield return new WaitForSeconds(3.0f); 
		bgm.Play ();
	}

	void Reset(){
		bps = 60f/bpm;
	}
		
	void Update(){

		//署名
		//加門康太　2017年10月15日 
		//--------------------------------書き加えた部分---------------------------------------------
		
//		宣言
//		Vector3 tmp = GameObject.Find("Sphere").transform.position;
//		Vector3 tmp2 = GameObject.Find("Sphere").transform.localScale;
//		
//		//移動
//		tmp.y -= 0.05f;
//		
//		//拡大
//		tmp2.x += 0.01f;
//		tmp2.y += 0.01f;
//		
//		//戻り値（？）
//		GameObject.Find("Sphere").transform.position = new Vector3(tmp.x,tmp.y,tmp.z);
//		GameObject.Find("Sphere").transform.localScale = new Vector3(tmp2.x, tmp2.y, tmp2.z);
		
		//--------------------------------ここまで---------------------------------------------------

		foreach (Note note in GameObject.FindObjectsOfType<Note> ()) {
			if (note.pos > 0) {
				float y = noteTarget.position.y + 1 * ((bps * note.time - bgm.time) * speed);
				note.transform.position = new Vector3 
					((-note.pos+note.pos*0.25f)*NoteMoveX(5.5f,noteTarget.position.y,y) + note.pos,y, note.transform.position.z);
				note.transform.localScale = new Vector3 (1.3f, 1.3f, 1) * NoteMoveX (noteTarget.position.y, 5.5f,y);
			} else {
				float y = noteTarget.position.y + 1 * ((bps * note.time - bgm.time) * speed);
				note.transform.position = new Vector3 
					((-note.pos+note.pos*0.25f)*NoteMoveX(5.5f,noteTarget.position.y,y) + note.pos,y, note.transform.position.z);
				note.transform.localScale = new Vector3 (1.3f, 1.3f, 1) * NoteMoveX (noteTarget.position.y, 5.5f,y);
			}
			if (note.transform.position.y <= -2) {
				Debug.Log ("Bad");
				note.gameObject.SetActive (false);
			}
		}
		string getData = serial.GetData ();
		int[] val;
		Debug.Log (getData);
//		Debug.Log (getData);
		if (getData != null) {
			string[] str = getData.Split(','); 
			val = new int[] {int.Parse (str [0]), int.Parse (str [1]), int.Parse (str [2]),
				int.Parse (str [3]), int.Parse (str [4]), int.Parse (str [5])};
		} else {
			val = new int[]{0,0,0,0,0,0};
		}

		if (Input.GetKeyDown(KeyCode.Q) || (1 <= val[0] && val[0] <= serialheight)){
			JudgeNote (new Vector3 (-5, 0, 0));
		}
		if (Input.GetKeyDown(KeyCode.W) || (1 <= val[1] && val[1] <= serialheight)){
			JudgeNote (new Vector3 (-3, 0, 0));
		}
		if (Input.GetKeyDown(KeyCode.E) || (1 <= val[2] && val[2] <= serialheight)){
			JudgeNote (new Vector3 (-1, 0, 0));
		}
		if (Input.GetKeyDown(KeyCode.R) || (1 <= val[3] && val[3] <= serialheight)){
			JudgeNote (new Vector3 (1, 0, 0));
		}
		if (Input.GetKeyDown(KeyCode.T) || (1 <= val[4] && val[4] <= serialheight)){
			JudgeNote (new Vector3 (3, 0, 0));
		}
		if (Input.GetKeyDown(KeyCode.Y) || (1 <= val[5] && val[5] <= serialheight)){
			JudgeNote (new Vector3 (5, 0, 0));
		}
	}

	public static float NoteMoveX(float max,float min,float ve){
		float range = max - min;
		float value = ve - min;

		if (ve >= 5.5) {
			return 0;
		} else {
			return value / range;
		}
	}

	void JudgeNote(Vector3 judgePos){
		Note nearNote = serchNearNote (judgePos);
		if (nearNote != null) {
			if (Mathf.Abs (nearNote.time * bps - bgm.time) <= bps / 4) {
 				Debug.Log ("Good!");
			} else if (Mathf.Abs (nearNote.time * bps - bgm.time) <= (bps / 4)*3) {
				Debug.Log ("OK");
			} else {
				Debug.Log ("Bad");
			}
			nearNote.gameObject.SetActive (false);
		}
	}

	Note serchNearNote(Vector3 target){
		float tmpTime = 0;           //距離用一時変数
		float nearTime = 0;          //最も近いオブジェクトの距離
		Note targetObj = null; 		 //現在の判定を取るノーツオブジェクト
		bool firstJudge = true;		 //初回判定通過用bool

		//タグ指定されたオブジェクトを配列で取得する
		foreach (Note note in GameObject.FindObjectsOfType<Note> ()){

//			Debug.Log (Mathf.Abs (note.time * bps - bgm.time) <= judgeTiming);
//			Debug.Log(target.x == note.transform.position.x);
			if (target.x == note.transform.position.x && Mathf.Abs(note.time * bps - bgm.time) <= bps/2) {
				tmpTime = Mathf.Abs(bgm.time*bps - note.time);
				//オブジェクトの距離が近いか、距離0であればオブジェクト名を取得
				//一時変数に距離を格納
				if (firstJudge || nearTime > tmpTime) {
					nearTime = tmpTime;
					//nearObjName = obs.name;
					targetObj = note;
					firstJudge = false;
				}
			}
		}
		return targetObj;
	}

	void OnDestroy()
	{
		serial.ThreadEnd();
	}
//	vsoid FixUpdate(){
//		Debug.Log ("Editer");
//	}
}