using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace WAR.UI {
	public class UIInput : Manager<UIInput> {
		// the two possible cameras to use
		[FoldoutGroup("Prefabs")] [SerializeField] GameObject mobileCamera;
		[FoldoutGroup("Prefabs")] [SerializeField] GameObject desktopCamera;
		// and the input control prefabs
		[FoldoutGroup("Prefabs")] [SerializeField] GameObject desktopInputControl;
		[FoldoutGroup("Prefabs")] [SerializeField] GameObject mobileInputControl;
		
		// delegate function to handle our board init events
		public delegate void InitBoardEventFn(UIPlane plane);
		public event InitBoardEventFn onBoardInit;
		
		public static void InitBoard (UIPlane plane) {
			UIInput.Instance.onBoardInit(plane);
		}
		
		void Awake() {
			#if UNITY_EDITOR || UNITY_STANDALONE_WIN
			GameObject.Instantiate(desktopCamera);
			GameObject.Instantiate(desktopInputControl);
			#else
			Camera cam = GameObject.Instantiate(mobileCamera)
								   .GetComponent<Camera>();
			GameObject.Instantiate(mobileInputControl);
			GetComponent<UnityARCameraManager>().SetCamera(cam);
			#endif
		}
	}
	
	public enum Layers {
		TableTile = 8,
	}	
}
