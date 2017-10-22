using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteSystem : MonoBehaviour {

	[SerializeField]
	bool isEditer;

	[SerializeField]
	GameObject Editer;

	[SerializeField]
	GameObject Master;

//	[SerializeField]
//	private List<GameObject> notePrefab;
#if UNITY_EDITOR
	void Awake () {  
		Editer.SetActive (isEditer);
		Master.SetActive (!isEditer);
	}
#else
	void Awake (){
		Editer.SetActive (false);
		Master.SetActive (true);
	}
#endif
}  

//		if (isEditer) {
//			NoteEditer.self.Prefab = notePrefab [0];
//		} else {
//			NoteMaster.self.Prefab = notePrefab [0];
//		}  
