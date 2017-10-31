using UnityEngine;
using System.Collections;
using System.IO.Ports;
using System.Runtime.InteropServices;

public class SerialTest : MonoBehaviour {
	public GameObject rocket;
	public static SerialLib.UnitySerial serial;
	public int Decision = 0;

	void Start ()
	{
		serial = new SerialLib.UnitySerial("/dev/cu.usbmodem14311",9600,256);
		serial.ThreadStart();
		
	}
	
	void Update () 
	{
		
		string str = serial.GetData();
		
//		if(str != null){
//			
//			float r = float.Parse (str);
//			
//			if(r >= 50.0f && r <= 100.0f){
//				 Decision = 1;
//				}
//			else if(r < 10.0f){
//				 Decision = -1;
//			}
//			else{
//				Decision = 0;
//			}
//		}
		Debug.Log(str);
		
	}
	
	
	void OnDestroy()
	{
		serial.ThreadEnd();
	}
	
	
}
