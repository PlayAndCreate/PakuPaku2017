using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyMath: MonoBehaviour {
	public static float ZeroOnef(float max,float min,float ve){
		float range = max - min;
		float value = ve - min;

		if (value / range <= 0f) {
			return 0;
		} else if (value / range >= 1f) {
			return 1;
		} else {
			return value / range;
		}
	}
}
