using System;
using System.Linq;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using WAR.UI;


namespace WAR.Engine{};

namespace WAR.Engine.Board {
	public class WARControlBoard : MonoBehaviour {
		// the instance of the table actor managing the board
		private GameObject table;
		
		// public static reference to self so create a singleton
		public static WARControlBoard instance;
		public static WARControlBoard control {
			get {
				if (instance) { return instance; }
				instance = FindObjectOfType<WARControlBoard>();
				if (instance) { /*TODO add init sequence if necessary*/return instance; }
				Debug.LogError("No WARControlBoard instance was found!");
				return null;
			}
		}
		
		public void Start () {
			// initialize the board at the appropriate time
			FindObjectOfType<UIInput>().onBoardInit += initBoard;
		}
		
		public void initBoard (UIPlane plane) {
			// remove an existing table if there is one
			if (table != null) {
				GameObject.Destroy(table);
			}
			
			// create a new object
			table = createTable(plane);
		}
		
		// return the a new table with cool stuffs
		private GameObject createTable(UIPlane plane) {
            // create an empty game object to attach things to
            GameObject go = new GameObject("table");
			
			// make an instance of the singleton table actor
            WARActorTable table = go.AddComponent<WARActorTable>() as WARActorTable;
            table.initialize(plane, GRID_TYPE.hex);

			// return the table tree
			return go;
		}
	}
}            
