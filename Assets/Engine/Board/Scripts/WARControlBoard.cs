using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UniRx;
using Sirenix.OdinInspector;
using WAR.UI;
using WAR.Game;
using WAR.Pathfinder;
using WAR.Tools;

namespace WAR.Board {
	public enum GRID_TYPE {
		hex,
		grav,
		square,
	}
	
	public class WARControlBoard : Manager<WARControlBoard> {
		// the instance of the table actor managing the board
		private GameObject table;
		public GameObject hexSlot;
		
		private WARGrid grid;
		public static WARGrid Grid {
			get {
				return WARControlBoard.Instance.grid;
			}
		}
		
		[EnumToggleButtons]
		public GRID_TYPE gridType = GRID_TYPE.hex;
		
		private IWARPathfinder pathfinder;
		[EnumToggleButtons]
		public PATHFINDER_TYPE pathfinderType = PATHFINDER_TYPE.astar;
		
		
		
		// return the a new table with cool stuffs
		public static void CreateTable(UIPlane plane) {
			// remove an existing table if there is one
			if (Instance.table != null) {
				GameObject.Destroy(Instance.table);
				// clear the selection
				WARControlSelection.ClearSelection();
			}
			var tableObject = new GameObject();
			
			// instantiate the appropriate pathfinder
			switch (Instance.pathfinderType) {
			// if we are making an astar
			case PATHFINDER_TYPE.astar:
				Instance.pathfinder = new WARPathAStar();	
				break;
			}
			
			// spawn the appropriate grid for the 
			switch(Instance.gridType) {
			// if we are building a hex grid
			case GRID_TYPE.hex:
				// fill our plane extent with hex slots
				WARHexGrid hexGrid = tableObject.AddComponent<WARHexGrid>() as WARHexGrid;
				hexGrid.initialize(plane, Instance.hexSlot, Instance.pathfinder);
				Instance.grid = hexGrid;
				break;
			default:
				print("could not instantiate cell with type " + Instance.gridType);
				return;
			}
				
			// draw the grid
			Instance.grid.createGrid();
			
			// we're done here
			Instance.table = tableObject;
		}
		
		// move objects to a cell on our grid
		public static void MoveObjectsToCell(int cellId, List<WARGridObject> objects) {
			WARControlBoard.Grid.moveObjectsToCell(cellId, objects);	 			
		}
		
		// a static wrapper adding objects to the grid
		public static void AddObjectsToCell(int cellId, List<WARGridObject> objects) {
			// make sure the objects we added are parents to the root game object
			foreach (var obj in objects) {
				obj.transform.SetParent(WARControlBoard.Instance.table.transform);
			}
			
			// add the objects to the right cell
			WARControlBoard.Grid.addObjectsToCell(cellId, objects);
		}
	}
}            
