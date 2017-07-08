using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using WAR.UI;
using UniRx;

namespace WAR.Board {
	public class WARControlBoard : Manager<WARControlBoard> {
		// the instance of the table actor managing the board
		private GameObject table;
		public GameObject hexSlot;
		
		public void Start () {
			// initialize the board at the appropriate time
			StartCoroutine(WaitForUIInput());
		}
		IEnumerator WaitForUIInput() {
			while (UIInput.Instance == null) yield return null;
			print("adding init board");
			UIInput.Instance.onBoardInit += initBoard;
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
			// initialize and return our table container
			return table.initialize(plane, hexSlot, GRID_TYPE.hex);
		}
	}
}            
