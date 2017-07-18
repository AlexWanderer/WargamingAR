using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using WAR.UI;

namespace WAR.Board {

	public abstract class WARMovableObject : WARGridObject {
		// the amount of time between cells
		public float speed = 0.5f;
		
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
				
				// move between this cell and the next
				yield return moveBetweenCells(source, target);
				
				// make sure we move between this cell next time
				lastCell = cell;
			}
			
		}

		// coroutine to move object between cells
		private IEnumerator moveBetweenCells(WARActorCell source, WARActorCell target) {
			// the moment in time along the path between the two cells 
			var i = 0.0f;
			// the amount to move per tick
			var rate = 1.0f/speed;

			// if the cell and the target are the same then we're done
			if (source == target) {
				yield break;
			}

			// until we reach the end
			while (i < 1.0) {
				// increment the distance counter to the next tick
				i += Time.deltaTime * rate;

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
