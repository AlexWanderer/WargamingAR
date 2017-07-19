using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using WAR.UI;

namespace WAR.Board {

	public abstract class WARMovableObject : WARGridObject {
		// the amount of time between cells
		public float speed = 0.5f;
		// the current cell we are under
		private WARActorCell lastCell;
		
		private Coroutine pathRoutine;

		public void followPath(List<int> path, WARGrid grid)  {
			// make sure the source isn't highlighted
			grid.GetCell(path[0]).highlighted.Value = false;
			
			// start walking the path
			pathRoutine = StartCoroutine(walkPath(path, grid));
		}

		private IEnumerator walkPath(List<int> path, WARGrid grid) {
			// if we are in the middle of walking a path
			if (pathRoutine != null) {
				// stop the previous routine
				StopCoroutine(pathRoutine);
			}
			
			// the last cell we saw	
			var lastCell = path[0];
			
			foreach(var cell in path.Skip(1)) {
				// the source and target cells
				var source = grid.GetCell(lastCell);
				var target = grid.GetCell(cell);
				
				gameObject.transform.LookAt(target.transform.position);
				
				// move between this cell and the next
				yield return moveBetweenCells(source, target, grid);
				
				// make sure we move between this cell next time
				lastCell = cell;
			}
			
		}

		// coroutine to move object between cells
		private IEnumerator moveBetweenCells(WARActorCell source, WARActorCell target, WARGrid grid) {
			// the moment in time along the path between the two cells 
			var i = 0.0f;
			// the amount to move per tick
			var rate = 1.0f/speed;

			// if the cell and the target are the same then we're done
			if (source == target) {
				yield break;
			}
			// make sure the current cell highlighted
			lastCell = grid.GetCell(grid.findCellsUnderObject(this)[0]);
			lastCell.highlighted.Value = true;

			// until we reach the end
			while (i < 1.0) {
				// increment the distance counter to the next tick
				i += Time.deltaTime * rate;
				
				// raycast to find the cell under us
				var currentCell = grid.GetCell(grid.findCellsUnderObject(this)[0]);
				
				// if the cell is different from the last one we saw
				if (lastCell.id != currentCell.id) {
					// unhighlight the last cell
					lastCell.highlighted.Value = false;
					// and highlilght the one we are under
					currentCell.highlighted.Value = true;
					
					// update the cell tracker
					lastCell = currentCell;
				}
				
				// update our position to the appropriate part of the lerp
				gameObject.transform.position = Vector3.Lerp(
					source.transform.position,
					target.transform.position,
					i
				);
				// we're done for this tick
				yield return null;
			}
		}
	}
}
