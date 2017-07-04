using System;
using System.Linq;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using WAR.UI;

namespace WAR.Engine {
	public class WARBoardControl : MonoBehaviour {
		
		// public static reference to self so create a singleton
		public static WARBoardControl instance;
		public static WARBoardControl control {
			get {
				if (instance) { return instance; }
				instance = FindObjectOfType<WARBoardControl> ();
				if (instance) { /*TODO add init sequence if necessary*/return instance; }
				Debug.LogError("No WARBoardControl instance was found!");
				return null;
			}
		}
		
		// set of prefabs to initialize the board with
		public GameObject tablePrefab;
		public List<GameObject> planetPrefabs;
		
		public void Start () {
			var uiInput = FindObjectOfType<UIInput> (); 
			uiInput.onBoardInit += initBoard;
		}
		
		public void initBoard (UIPlane plane) {
			print("something");
		}
	}
}

//     // when the game first starts
//     member this.Awake() =
//         let argDict = new Dictionary<string,obj>()
//		 let event = {
//		             action = "InitializeBoard"
//		             args = argDict
//		 }
//         WAREventControl.AddListener event (UnityAction<WAREvent>(this.initializeBoard))
     
//     // initialize the game board given an event center and extent
//     member this.initializeBoard event = 
//         // ensure our event is a proper WAREvent
//         match event with
//         | { WAREvent.action = action; WAREvent.args = args; } ->
//             // get the center data
//             let mutable test_center = obj()
//             let center =
//                 // look for our center data
//                 if args.TryGetValue("center", &test_center) then
//                 // use some reflection to convert the generic type to Vector3
//                     let ty = test_center.GetType()
//                     let vl = ty.GetProperty "Value"
//                     let point = vl.GetValue(test_center, null) :?> Vector3
//                     Some point
//                 else
//                     None
//             // get the extent data
//             let mutable test_extent = obj()
//             let extent =
//                 // look for our extent data
//                 if args.TryGetValue("extent", &test_extent) then
//                     // use some reflection to conver the generic type to Vector3
//                     let ty = test_extent.GetType()
//                     let vl = ty.GetProperty "Value"
//                     let point = vl.GetValue (test_extent, null) :?> Vector3
//                     Some point
//                 else
//                     None
//             // get the rotation from the data
//             let mutable test_rotation = obj()
//             let rotation =
//                 // look for our extent data
//                 if args.TryGetValue("rotation", &test_rotation) then
//                     // use some reflection to conver the generic type to Vector3
//                     let ty = test_rotation.GetType()
//                     let vl = ty.GetProperty "Value"
//                     let point = vl.GetValue (test_rotation, null) :?> Quaternion
//                     Some point
//                 else
//                     None
             
//             // log the activity
//             Debug.Log <| "callBack: initializeBoard @ center: " + center.ToString() + ", " + 
//                                                      "extent: " + extent.ToString() + ", " +
//                                                      "rotation: " + rotation.ToString()

//             // initialize the table actor at the appropriate place
//             WARActorTable.Initialize {
//                tablePrefab = this.tablePrefab 
//                center = center 
//                gridUnit = 10 
//                rotation = rotation 
//                planetPrefabList = this.planetPrefabList 
//                extent = extent 
//             }
