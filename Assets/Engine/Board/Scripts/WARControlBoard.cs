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
using WAR.Tools;
using WAR.Units;
using WAR.Pathfinder;

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
		[EnumToggleButtons]
		public GRID_TYPE gridType = GRID_TYPE.hex;
		
		private IWARPathfinder pathfinder;
		[EnumToggleButtons]
		public PATHFINDER_TYPE pathfinderType = PATHFINDER_TYPE.astar;
		
		public void Start () {
			// initialize the board at the appropriate time
			StartCoroutine(WaitForUIInput());
			// a move order is issued when there is a clicked with a non-zero selection
			UIInput.TouchObservable.Where(_ => WARControlSelection.Selection.Count > 0).Subscribe(moveObject);
		}
		
		IEnumerator WaitForUIInput() {
			while (UIInput.Instance == null) yield return null;
			UIInput.Instance.onBoardInit += initBoard;
		}
		
		
		public void initBoard (UIPlane plane) {
			// remove an existing table if there is one
			if (table != null) {
				GameObject.Destroy(table);
				// clear the selection
				WARControlSelection.ClearSelection();
			}
			
			// create a new object
			table = createTable(plane);
		}
		
		// return the a new table with cool stuffs
		private GameObject createTable(UIPlane plane) {
			// instantiate the appropriate pathfinder
			switch (pathfinderType) {
			// if we are making an astar
			case PATHFINDER_TYPE.astar:
				pathfinder = new WARPathAStar();	
				break;
			}
			
			// spawn the appropriate grid for the 
			switch(gridType) {
			// if we are building a hex grid
			case GRID_TYPE.hex:
				// fill our plane extent with hex slots
				WARHexGrid hexGrid = gameObject.AddComponent<WARHexGrid>() as WARHexGrid;
				hexGrid.initialize(plane, hexSlot, pathfinder);
				grid = hexGrid;
				break;
			default:
				print("could not instantiate cell with type " + gridType);
				return null;
			}
				
			// draw the grid
			grid.createGrid();
			
			// add a ship to play with
			var ship = GameObject.Instantiate(
				WARToolUnitFinder.GetByArmyUnitName("Shmoogaloo","ShmooTroop"), gameObject.transform
			).GetComponent<WARUnit>() as WARGridObject;
			grid.addObjectsToCell(0,new List<WARGridObject>{ship});
			
			// we're done here
			return gameObject;
		}
		
		// when a move order is issued
		public void moveObject(Vector3 pos) {
			// find the cell underneath the point we clicked
			RaycastHit hit;
			int layerMask = 1 << (int)Layers.TableTile;
			
			// if there is an object under the vector
			if (Physics.Raycast(ray: Camera.main.ScreenPointToRay(pos), hitInfo: out hit, maxDistance: 5, layerMask: layerMask)) {
				// the id of the cell we clicked on 
				var id = hit.collider.GetComponent<WARActorCell>().id;
				// we clicked on cell so move the current select to the cell
				var list = new List<WARGridObject>();
				foreach (var selected in WARControlSelection.Selection ) {
					list.Add(selected);
				}
				
				// move the selection to the right cell
				grid.moveObjectsToCell(id, list);	 			
			}
		}
	}
}            
