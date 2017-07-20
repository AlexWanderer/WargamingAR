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
	
	public class WARPathAStar : IWARPathfinder {
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
		
		
		// calculate the final cell cost given our source, target and grid
		private int cellCost(int cell, Dictionary<int,int> sourceCostMap, Dictionary<int,int> targetCostMap, WARGrid grid) {
			var cost = sourceCostMap[cell] + targetCostMap[cell];
			return cost;// + grid.GetCell(cell).pathFindingCost
		}
		// Node container for our cell id and the parent node
		class Node {
			public int id;
			public Node parent;
			
			// if we only have the ID then we parent ourself
			public Node(int id) {
				this.id = id;
				this.parent = new Node(id, this);
			}
			public Node(int id, Node parent) {
				this.id = id;
				this.parent = parent;
			}
		}
		private List<Node> getPathFromNodes(Node node){
			List<Node> path = new List<Node>{node};
			while(node.parent.id != node.id) {
				path.Add(node.parent);
				node = node.parent;
			}
			return path;
		}
		
		public List<int> findPath(int source, int target, WARGrid grid) {
			// create the cost map for our source and target
			var sourceCostMap = getCostMap(source, grid);
			var targetCostMap = getCostMap(target, grid);
			
			// traverse our grid using cost maps to determine path
			
			// path to return
			var path = new List<int>();
			
			// map from cellId to the Node representing it
			var nodeMap = new Dictionary<int, Node>();
			nodeMap.Add(source, new Node(source));
			
			// map to store the cell costs
			var finalCostMap = new Dictionary<int,int>();
			finalCostMap.Add(source, cellCost(source, sourceCostMap, targetCostMap, grid));
			
			// set of nodes we need to process
			var opened = new List<int>{source};
			// set of nodes we have processed
			var closed = new List<int>();
			
			// while there are still cells to process
			while (opened.Count > 0) {				
				// find the cell with the lowest cost
				var cell = opened.Aggregate(
					(min, id) => {
						return finalCostMap[id] < finalCostMap[min] ? id : min;
					}
				);
				
				// remove the cell with the lowest cost and add it to closed
				opened.Remove(cell);
				closed.Add(cell);
				
				// if we found the current target then we have done our job
				if (cell == target){
					// create the path by walking parents of final node
					path = getPathFromNodes(nodeMap[cell]).Select(node => node.id).ToList();
					// then reverse it because we calculated the path from target to start
					path.Reverse();
					return path;
				}
				
				// determine if we add our neighbors to the traversal path
				foreach (var neighbor in grid.GetCell(cell).neighbors.Where(id => !closed.Contains(id))) {
					// if we have not opened this cell, or the path to this cell is shorter than previously found
					if(!opened.Contains(neighbor) || getPathFromNodes(nodeMap[cell]).Count + 1 < getPathFromNodes(nodeMap[neighbor]).Count) {
						// add the neighbor to the nodeMap, linking its parent to the node at cell
						nodeMap.Add(neighbor,new Node(neighbor, nodeMap[cell]));
						// add  the neighbor to the final cost map using cellCost
						finalCostMap.Add(neighbor, cellCost(neighbor, sourceCostMap, targetCostMap, grid));
						// if we haven't opened this neighbor already, open it
						if (!opened.Contains(neighbor)) {
							opened.Add(neighbor);
						}
					}
				}
			}
			Debug.LogError("Reached end of Astar traversal without finding target");
			return path;
		}
	}
}
