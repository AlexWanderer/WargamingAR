using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WAR.UI {
	
	public class UIInput : MonoBehaviour {
		// delegate function to handle our board init events
		public delegate void InitBoardEventFn(UIPlane plane);
		public event InitBoardEventFn onBoardInit;
		
		public static void InitBoard (UIPlane plane){
			control.onBoardInit(plane);
		}
		// public static reference to self so create a singleton
		public static UIInput instance;
		public static UIInput control {
			get {
				if (instance) { return instance; }
				instance = FindObjectOfType<UIInput> ();
				if (instance) { /*TODO add init sequence if necessary*/return instance; }
				Debug.LogError("No WARBoardControl instance was found!");
				return null;
			}
		}
		
	}
}
