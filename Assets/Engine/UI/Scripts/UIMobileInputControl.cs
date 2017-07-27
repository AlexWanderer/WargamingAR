using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.XR.iOS;
using UnityEngine;
using WAR.Game;

namespace WAR.UI {
	public class UIMobileInputControl : MonoBehaviour
	{
		public GameObject planePrefab;
		private UnityARAnchorManager unityARAnchorManager;
		
	    // Use this for initialization
		void Start ()
		{
	        #if !UNITY_EDITOR
	        unityARAnchorManager = new UnityARAnchorManager ();
	        UnityARUtility.InitializePlanePrefab (planePrefab);
	        #endif
		}
		
		void OnDestroy ()
		{
			unityARAnchorManager.Destroy ();
		}
		
		
		public void Update ()
		{
	        // if mouse button 0 was clicked
			if (Input.touchCount > 0 && WARGame.Mode.Value.current == GAME_MODE.setup) {
	            // figure out where we touched
				Touch touch = Input.GetTouch (0);
				if (touch.phase == TouchPhase.Ended) {
					Vector2 screenPoint = Camera.main.ScreenToViewportPoint (touch.position);
					ARPoint point = new ARPoint (){ x = screenPoint.x, y = screenPoint.y };
					
	                // notify the WAREngine that a touch start event occured
					TriggerPointEvent ("TouchStart", touch.position);
					
	                // find all hitResults of type ExistingPlane
					List<ARHitTestResult> hitResults = UnityARSessionNativeInterface.GetARSessionNativeInterface ().HitTest (
						point, ARHitTestResultType.ARHitTestResultTypeExistingPlaneUsingExtent
					);
	                // if we hit something
					if (hitResults.Count > 0) {
						Ray ray = Camera.main.ScreenPointToRay (touch.position);
						RaycastHit hit;
						if (Physics.Raycast (ray, out hit, 10.0f)) {
	                        // find the object we hit
							GameObject go = hit.collider.gameObject;
							
	                        // pack the values pertaining to a plane into a container and trigger InitBoard							
							UIInput.InitBoard(new UIPlane {center=go.transform.position,
														   extent=go.transform.localScale * 10,
														   rotation=go.transform.rotation});
							
	                        // we are done looking for anchors so we will not attach a collideable game obect to our anchors
							UnityARUtility.InitializePlanePrefab (new GameObject ());
	                        // clear the current set of planes and their anchor data
							unityARAnchorManager.Destroy ();
						}
					}
				}
			}
		}
		
		static void TriggerPointEvent (string eventName, Vector3 point)
		{
			print ("Trigger point event");
			//Dictionary<string, object> args = new Dictionary<string, object> ();
			//args.Add ("point", FSharpOption<Vector3>.Some (point));
			//WAREventControl.Trigger (new WAREvent (eventName, args));
		}
	}
}
