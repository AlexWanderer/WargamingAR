using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WAR.Board;

namespace WAR.Pathfinder {
	
	public enum PATHFINDER_TYPE {
		astar,
	};
	
	public interface IWARPathfinder {
		List<int> findPath(int source, int target, WARGrid grid);
	}
}
