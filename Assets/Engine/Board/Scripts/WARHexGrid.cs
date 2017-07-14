using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityTest;
using System.Linq;
using WAR.UI;

namespace WAR.Board {
	public class WARHexGrid : WARGrid {
		
		private UIPlane plane;
		private GameObject hexPrefab;
		
		private float globalGridScale = 0.01f;
		
		public void initialize(UIPlane plane, GameObject hexPrefab) {
			this.plane = plane;
			this.hexPrefab = hexPrefab;
		}
		
		public override void createGrid () {
			// position and rotate our table actor container to match plane.center and rotation				
			transform.position = plane.center;
			
			// create a grid of hexagons given our plane.extent (x,z) for (width,height)
			// TODO, figure out which direction we dragged so we can always start the grid
			// at bottom left of the created plane
			Vector3 origin = transform.position - plane.extent * 0.5f;
			
			// outter radius is from center to vertex
			float outterRadius = transform.localScale.z * globalGridScale;
			// inner radius is from center to edge
			float innerRadius = outterRadius * Mathf.Sqrt(3) / 2f;//magic hexagon math
			
			// how many columns and rows of hexagon tiles will fit in our desired plane
			int numberOfColumns = Mathf.CeilToInt(plane.extent.x/(3f * outterRadius));
			int numberOfRows = Mathf.CeilToInt(plane.extent.z/innerRadius);
			// grid cannot exist without atleast two rows
			numberOfRows = numberOfRows > 1 ? numberOfRows : 2;
			
			// start the id counter
			var id = 0;
			// loop through our number of columns and rows
			for (int x = 0; x < numberOfColumns; x++){
				for (int y = 0; y < numberOfRows; y++){
					// each column is 1.5 outter radii away from eachother and each row is 1 inner radius away
					Vector3 offset = new Vector3(
						x * 3f * outterRadius,
						0f,
						y * innerRadius
					);
					
					// we create double the number of cells in a column and offset half to fill the gaps
					if(y%2==0){
						offset += new Vector3(1.5f * outterRadius, 0f, 0f);
					}
					// TODO we probably want to use a WARActorGrid class to hold the cells themselves
					// and give us some control functions (distance, grid coords to cell obj, etc.)
					// we create our hex cell as a child of our game object container, go
					GameObject hex = GameObject.Instantiate(hexPrefab, origin+offset, hexPrefab.transform.rotation, transform);
					
					// and scale the grid based on our global scale factor
					hex.transform.localScale = globalGridScale * hex.transform.localScale;
					WARActorCell cell = hex.GetComponent<WARActorCell>().Init();
					if (cell){
						// make sure we get a unique id next time
						cell.id = id;
						// and add it to the list
						cells.Add(cell);
						cell.GetComponentInChildren<TextMesh>().text = id.ToString();
						cell.neighbors = FindCellNeighborIDs(cell.id,numberOfColumns,numberOfRows);
						
						// increment the id counter
						id++;
					}
				}
			}
			
			// align the table to match the designated plane
			transform.rotation = plane.rotation;	
		}
		
		public static List<int> FindCellNeighborIDs(int cellId, int numberOfColumns, int numberOfRows) {
			int top = cellId + 2;
			int bottom = cellId - 2;
			int topLeft;
			int topRight;
			int bottomLeft;
			int bottomRight;
			int cellMod = cellId % numberOfRows;
			
			
			int bottomRightCornerID = numberOfRows * numberOfColumns - 1 - (numberOfRows - 1);
			
			// if we are an even cell
			if (cellMod % 2 == 0){
				// calculate even dependent neighbors
				topLeft = cellId + 1;
				bottomLeft = cellId - 1;
			} else {
				// calculate odd dependent neighbors
				topLeft = cellId - (numberOfRows - 1);
				bottomLeft = cellId - (numberOfRows + 1);
			}
			topRight = topLeft + numberOfRows;
			bottomRight = bottomLeft + numberOfRows;
			
			// top left corner
			if (cellId == numberOfRows - 2) {
				// ask for southeast corner
				return new List<int>{bottom,bottomRight,topRight};
			}
			// top right corner
			if (cellId == numberOfRows * numberOfColumns - 1) {
				// ask for south-southwest corner
				return new List<int>{bottom,bottomLeft};
			}
			// bottom right corner
			if (cellId == bottomRightCornerID) {
				// ask for north-northwest corner
				return new List<int>{top,topLeft};
			}
			// bottom left corner
			if (cellId == 1) {
				// ask for northeast corner
				return new List<int>{top,topRight,bottomRight};
			}
			// left most edge
			if (cellId < numberOfRows && cellId % 2 == 1) {
				// all but the eastern cells
				return new List<int>{top,topRight,bottomRight,bottom};
			}
			// right most edge
			if (cellId > bottomRightCornerID && cellId % 2 == 0) {
				// all but the western cells
				return new List<int>{top,topLeft,bottomLeft,bottom};
			}
			
			// are we on the bottom 
			if (cellMod == 0) {
				// ask for northern three neighbors
				return new List<int>{top,topLeft,topRight};
			}
			// else one from bottom
			if (cellMod == 1) {
				// ask for all but bottom neighbor
				return new List<int>{top,topLeft,topRight,bottomLeft,bottomRight};
			}
			// are we on the top
			if (cellMod == numberOfRows - 1) {
				// ask for southern three neighbors
				return new List<int>{bottom,bottomLeft,bottomRight};
			}
			// else one from top
			if (cellMod == numberOfRows - 2) {
				// ask for all but the top neighbor
				return new List<int>{bottom,bottomLeft,bottomRight,topLeft,topRight};
			}
		
			// else we are not a special case and we can return all 6 neighbors
			return new List<int>{top,topLeft,topRight,bottom,bottomLeft,bottomRight};
		}
	
		// TODO, cleanup these functions, they are verbose..
		
		public override void addObjectsToCell(int cellId, List<WARGridObject> objects) {
			// find the designated cell
			WARActorCell cell = null;
			foreach (var c in cells) {
				if (c.id == cellId) {
					cell = c;
					break;
				}
			}
			
			// if we found the cell
			if (cell != null) {
				// add the given objects 
				foreach (var obj in objects){
					cell.objects.Add(obj);
				}
			}
			// we were told to add objects to a cell we couldn't find
			else {
				Debug.LogError("Could not add objects to cell with id " + cellId);
			}
		}
		public override void removeObjectsFromCell(int cellId, List<WARGridObject> objects) {
			// find the designated cell
			WARActorCell cell = null;
			foreach (var c in cells) {
				if (c.id == cellId) {
					cell = c;
					break;
				}
			}
			
			// if we found the cell
			if (cell != null) {
				// remove the given objects 
				foreach (var obj in objects){
					cell.objects.Remove(obj);
				}
			}
			// we were told to add objects to a cell we couldn't find
			else {
				Debug.LogError("Could not remove objects from cell with id " + cellId);
			}
		}
		
	}
}
