﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WAR.UI;
using WAR.Ships;
using WAR.Utils;

namespace WAR.Board {

    public enum GRID_TYPE {
        hex,
        square,
        grav
    };
	
	public class WARActorTable : MonoBehaviour {
		
		public float globalGridScale = 0.03f;
		
		private IWARGrid grid;
		
		public GameObject initialize(UIPlane plane, GameObject hexSlot, GRID_TYPE cellType){
			switch(cellType) {
			// if we are building a hex grid
			case GRID_TYPE.hex:
				// fill our plane extent with hex slots
				WARHexGrid hexGrid = gameObject.AddComponent<WARHexGrid>() as WARHexGrid;
				hexGrid.initialize(plane,hexSlot);
				grid = hexGrid as IWARGrid;
				break;
			default:
				print("could not instantiate cell with type " + cellType);
				return null;
			}
			
			// draw the grid
			grid.CreateGrid();
			
			// add a ship to play with
			var ship = GameObject.Instantiate(WARLibraryShip.ships[0], gameObject.transform).GetComponent<WARShip>() as WARGridObject;
			grid.AddObjectsToCell(0, new List<WARGridObject> {ship});
			
			// we're done here
			return gameObject;
        }
	}
}
