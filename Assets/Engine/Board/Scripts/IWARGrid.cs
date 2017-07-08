using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WAR.UI;

namespace WAR.Board {
	public interface IWARGrid {
		// display the grid on the view 
		void CreateGrid();
		
		// add objects to a specific grid
		void AddObjectsToCell(int cellId, List<WARGridObject> objects);
	}
}