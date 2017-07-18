using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WAR.UI;

namespace WAR.Board {
	
	public abstract class WARMovableObject : WARGridObject {
		public void followPath(List<int> path, WARGrid grid) {
			
			// make sure the source isn't highlighted
			grid.GetCell(path[0]).highlighted.Value = false;
			
			// for now just	place the object at the last cell
			var cell = grid.GetCell(path[path.Count - 1]);
			gameObject.transform.position = cell.transform.position;
			
			// select the final cell
			cell.highlighted.Value = true;
		}
	}
}
