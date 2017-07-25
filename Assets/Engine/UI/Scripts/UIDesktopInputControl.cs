using UnityEngine;
using System.Collections.Generic;
using WAR.Game;

namespace WAR.UI {
	public class UIDesktopInputControl : MonoBehaviour
	{		
		// save the position of the mouse when we first click to draw our debug plane
		private Vector2 mouseClickStart = new Vector2 ();
		private Vector3[] corners = new Vector3[4];
		
		void TriggerInitializeBoard(UIPlane plane)
		{
			// don't do anything on a single click
			if(plane.extent.magnitude < 0.1f) {return;}
            // get the rotation of our plane
			Quaternion rotation = new Quaternion(0f,Camera.main.transform.rotation.y,0f,1f);
            // trigger an initializeBoard event with our center and extent
            // pack the values pertaining to a plane into a container to publish with our InitializeBoard msg
			UpdatePlane(plane.start,plane.end,300f);
			UIInput.InitBoard(new UIPlane(start: plane.start,end: plane.end,rotation: rotation));
		}
		
		UIPlane FindPlaneExtent() {
			Ray startRay = Camera.main.ScreenPointToRay (mouseClickStart);
			Plane groundPlane = new Plane (Vector3.up, Vector3.zero);
			Vector3 start;
			Vector3 end;
			float toPlane;
			if (groundPlane.Raycast (startRay, out toPlane)) {
				Ray stopRay = Camera.main.ScreenPointToRay (Input.mousePosition);
				float toPlaneEnd;
				if (groundPlane.Raycast (stopRay, out toPlaneEnd)) {
	                // find the bounds of the plane
					start = startRay.direction * toPlane + startRay.origin;
					end = stopRay.direction * toPlaneEnd + stopRay.origin;
					UpdatePlane(start,end);
				}
			}
			return new UIPlane(start: start,end: end);
		}

	    // Update is called once per frame
		void Update()
		{
			if (WARControlGame.Mode.Value.current == GAME_MODE.setup) {
				// save our click start position
				if (Input.GetMouseButtonDown(0)) {
					mouseClickStart = Input.mousePosition;
				}
				if (Input.GetMouseButtonUp(0)) {
					// TODO use this layer mask to figure out if we are hitting a tile or not when we click
					Vector2 mouseClickEnd = Input.mousePosition;
					TriggerInitializeBoard(FindPlaneExtent());
				}
				// while the mouse is down, draw a plane to represent the drag
				if (Input.GetMouseButton(0)) {
					UIPlane plane = FindPlaneExtent();
					UpdatePlane(plane.start,plane.end);
				}			
				DrawPlane();
			}
		}
		
		void UpdatePlane(Vector3 planeStart, Vector3 planeEnd, float delay = 0f) {
			Vector3 extent = (planeEnd - planeStart);
			corners[0] = planeStart;
			corners[1] = planeStart + Vector3.Scale(extent,Vector3.right);
			corners[2] = planeStart + Vector3.Scale(extent,Vector3.forward);
			corners[3] = planeEnd;
		}
			
		void DrawPlane() {
			Debug.DrawLine(corners[0],corners[1],Color.red);
			Debug.DrawLine(corners[0],corners[2],Color.red);
			Debug.DrawLine(corners[3],corners[1],Color.red);
			Debug.DrawLine(corners[3],corners[2],Color.red);
		}
	}		
}
