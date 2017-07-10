using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
						
						// increment the id counter
						id++;
					}
				}
			}
			
			// align the table to match the design			ated plane
			transform.rotation = plane.rotation;	
		}
		
		public override void addObjectsToCell(int cellId, List<WARGridObject> objects) {
			// find the designated cell
			//var cell = cells.Where(c => c.id == cellId) as WARActorCell;
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
		
	}
}
