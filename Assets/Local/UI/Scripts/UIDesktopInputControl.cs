using UnityEngine;
using System.Collections.Generic;

namespace WAR.UI {
	public class UIDesktopInputControl : MonoBehaviour
	{
		
		public GameObject setupPlane;
		
		private Vector2 mouseClickStart = new Vector2 ();
		
		
	    // Use this for initialization
		void Start ()
		{
	        #if UNITY_EDITOR
			GameObject.Instantiate (setupPlane);
	        #endif
		}
		
		void TriggerInitializeBoard (Vector2 mouseClickEnd)
		{
			Ray startRay = Camera.main.ScreenPointToRay (mouseClickStart);
			Plane groundPlane = new Plane (Vector3.up, Vector3.zero);
			float toPlane;
			if (groundPlane.Raycast (startRay, out toPlane)) {
				Ray stopRay = Camera.main.ScreenPointToRay (mouseClickEnd);
				float toPlaneEnd;
				if (groundPlane.Raycast (stopRay, out toPlaneEnd)) {
	                // find the bounds of the plane
					Vector3 planeStart = startRay.direction * toPlane + startRay.origin;
					Vector3 planeEnd = stopRay.direction * toPlaneEnd + stopRay.origin;
	                // find center point between planeStart and planeEnd
					Vector3 center = (planeStart + planeEnd) / 2;
	                // extent of our plane is distance from center point to planeStart/End
					Vector3 extent = 2 * (planeEnd - center);
	                // if we didn't drag a plane
					if (extent.magnitude < 0.01f) {
	                    // don't do anything
						return;
					}
					Vector3 extentAbs = new Vector3 (Mathf.Abs (extent.z), Mathf.Abs (extent.y), Mathf.Abs (extent.x));
	                // get the rotation of our plane
					Quaternion rotation = new Quaternion (Vector3.up.x, Vector3.up.y, Vector3.up.z, -1.0f);
	                // trigger an initializeBoard event with our center and extent
	                // pack the values pertaining to a plane into a container to publish with our InitializeBoard msg					
					UIInput.InitBoard(new UIPlane {center=center,extent=extentAbs,rotation=rotation});
				}
			}
		}
		
		
		
	    // Update is called once per frame
		void Update ()
		{
	        #if UNITY_EDITOR
			if (Input.GetMouseButtonDown (0)) {
				mouseClickStart = Input.mousePosition;
			}
			if (Input.GetMouseButtonUp (0)) {
				Vector2 mouseClickEnd = Input.mousePosition;
				TriggerInitializeBoard (mouseClickEnd);
			}
	        #endif
			
		}
	}		
}
