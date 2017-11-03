using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//unityのエディタでのみ動く
#if UNITY_EDITOR
public class NoteEditer : MonoBehaviour {

	[SerializeField]
	private GameObject instantiateNote;

	[SerializeField]
	private  Transform noteTarget ;

	[SerializeField]
	private int bpm = 155;

	[SerializeField]
	private float speed = 1;

	[SerializeField]
	private Transform camera;

	public static NoteEditer self;

	public AudioSource bgm;

	private float bps;

	private bool canSave = false;

	private GameObject notePrefab;

	//	void Awake(){
	//		self = this;
	//		bps = 60f/bpm;
	//		try
	//		{
	//			Instantiate (Resources.Load (bgm.clip.name));
	//		}
	//		catch
	//		{
	//			Debug.LogError("error");
	//			UnityEditor.EditorApplication.isPlaying = false;
	//			GameObject notePrefabA = new GameObject(bgm.clip.name);
	//			notePrefabA.tag = "NoteFile";
	//			return;
	//		}
	//	}

	void Start(){
		self = this;
		bps = 60f/bpm;
		try
		{
			GameObject a = (GameObject)Instantiate (Resources.Load (bgm.clip.name)); 
			a.tag = "NoteFile";
		}
		catch
		{
			Debug.LogError("error:" +
				"Resourcesに音楽の名前と同名のプレハブを作ってください(日本語NG)");
			UnityEditor.EditorApplication.isPlaying = false;
			//			GameObject notePrefabA = new GameObject(bgm.clip.name);
			//			notePrefabA.tag = "NoteFile";
			return;
		}

		notePrefab = GameObject.FindGameObjectWithTag ("NoteFile");
		notePrefab.tag = "NoteFile";
	}

	public float Bps{
		get{return bps;}
	}

	public GameObject Prefab {
		set{ notePrefab = value; }
	}

	void Update(){
		//ノーツの移動
		foreach (Note note in GameObject.FindObjectsOfType<Note> ()) {
			note.transform.position = new Vector3 
				(note.pos, noteTarget.position.y + 1 * ((bps * note.time - bgm.time) * 5 * speed), note.transform.position.z);
		}

		//ノーツの出現
		if (Input.GetKeyDown(KeyCode.Q)){
			InstantiateObj (-5);
		}
		if (Input.GetKeyDown(KeyCode.W)){
			InstantiateObj (-3);
		}
		if (Input.GetKeyDown(KeyCode.E)){
			InstantiateObj (-1);
		}
		if (Input.GetKeyDown(KeyCode.R)){
			InstantiateObj (1);
		}
		if (Input.GetKeyDown(KeyCode.T)){
			InstantiateObj (3);
		}
		if (Input.GetKeyDown(KeyCode.Y)){
			InstantiateObj (5);
		}

		//画面を左クリックした時の先にあるオブジェクトの破棄
		if(Input.GetMouseButtonDown(0)){
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit = new RaycastHit();

			if (Physics.Raycast(ray, out hit)) {
				Destroy (hit.transform.gameObject);
			}
		}

		//音楽の頭出し
		if (Input.GetKeyDown(KeyCode.Return)){
			if (bgm.time > 0) {
				bgm.Stop ();
				bgm.time = 0;
			} else {
				bgm.Play ();
			}
		}

		//音楽の停止、再開
		if (Input.GetKeyDown(KeyCode.Space)){
			if (bgm.isPlaying) {
				bgm.Pause ();
			} else {
				bgm.Play ();
			}
		}

		//音楽の再生位置をbps分上下
		if (Input.GetKeyDown(KeyCode.UpArrow)){
			SoundTimeSlider(bps / 4);
		}
		if (Input.GetKeyDown(KeyCode.DownArrow)){
			SoundTimeSlider(-bps / 4);
		}
		if (Input.GetKeyDown(KeyCode.LeftArrow)){
			SoundTimeSlider(bps);
		}
		if (Input.GetKeyDown(KeyCode.RightArrow)){
			SoundTimeSlider(-bps);
		}
	}

	//BGMの時間をbpsに合うように調整、方向キーに対応する値を加算するメソット
	void SoundTimeSlider(float abc){
		float time = bgm.time / bps;
		int a = (int)time;
		int b = (int)(time * 100 % 100);
		if (b < 12.5f) {
			b = 0;
		} else if (b < 37.5f) {
			b = 25;
		} else if (b < 62.5f) {
			b = 50;
		}else if (b < 87.5f) {
			b = 75;
		}else{
			a += 1;
			b = 0;
		}
		bgm.time = (a + (float)b / 100) * bps + abc;
	}

	//ノーツをリソースから呼びだし、シーンへの配置とプレハブヘの組み込み、Noteスクリプトの変数変更
	void InstantiateObj(int posx){
		GameObject copy = instantiateNote;
		Note note = copy.GetComponent <Note>();

		note.pos = posx;

		float time = bgm.time / bps;
		int a = (int)time;
		int b = (int)(time * 100 % 100);
		if (b < 12.5f) {
			b = 0;
		} else if (b < 37.5f) {
			b = 25;
		} else if (b < 62.5f) {
			b = 50;
		}else if (b < 87.5f) {
			b = 75;
		}else{
			a += 1;
			b = 0;
		}

		note.time = a + (float)b / 100;
		GameObject NoteObj = Instantiate(copy);
		NoteObj.transform.parent = notePrefab.transform;
	}

	//Updateあとで行われるUpdate関数
	void LateUpdate(){
		if (canSave) {
			canSave = false;

			//プレハブ保存時に容量を削減するためにノーツを一点に集中させる。
			foreach (Note note in GameObject.FindObjectsOfType<Note> ()) {
				note.transform.position = new Vector3 (0, 0, 0);
				Debug.Log (note.time);
			}

			//プレハブをリソーシズに保存
			UnityEditor.PrefabUtility.CreatePrefab ("Assets/Resources/" + bgm.clip.name + ".prefab", notePrefab);
			UnityEditor.AssetDatabase.SaveAssets ();
		}

		//LeftShiftとSが押されている時
		if (Input.GetKeyDown (KeyCode.S) && Input.GetKey (KeyCode.LeftShift)) {

			//同一pos,timeのオブジェクトを破棄する
			for (int i = 0; i < GameObject.FindObjectsOfType<Note> ().Length; i++) {
				Note noteA = GameObject.FindObjectsOfType<Note> () [i];
				for (int j = i + 1; j < GameObject.FindObjectsOfType<Note> ().Length; j++) {
					Note noteB = GameObject.FindObjectsOfType<Note> () [j];
					if (noteA.time == noteB.time && noteA.pos == noteB.pos) {
						Destroy (noteB.transform.gameObject);
					}
				}
			}
			canSave = true;
		}
	}

	bool ResourcesCheacker(string name){
		foreach (GameObject go in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[]) {
			if (go.name == name) {
				return true;
			}
		}
		return false;
	}
}
#endif