using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;

using WAR.Board;

namespace WAR.Pathfinder {
	struct enqueued {
		public int id;
		public int cost;
	}
	
	public class WARPathAStar {
		public Dictionary<int, int> getCostMap(int cellId, WARGrid grid) {
			// the map itself
			var map = new Dictionary<int, int>();
			// Queue to store the cells to process starting with out target cellId
			var opened = new Queue<enqueued>();
			opened.Enqueue(new enqueued{id = cellId, cost=0});
			// the cells we've seen before
			var closed = new HashSet<int>();
			// make sure we dont come back to the origin
			closed.Add(cellId);
			
			// while we have cells left to process
			while(opened.Count > 0) {
				// get the next cell to process
				var cell = opened.Dequeue();
				
				// assign the cost to the map
				map.Add(cell.id, cell.cost);

				foreach(var neighbor in grid.GetCell(cell.id).neighbors){
					// if we're looking at a cell for the first time
					if (!closed.Contains(neighbor)){
						// process their children
						opened.Enqueue(new enqueued{id = neighbor, cost = cell.cost + 1});
						// and add it to the list of cells we've processed
						closed.Add(neighbor);
					}
				}
			}
			
			// return the map we just filled
			return map;
		}
	}
}
