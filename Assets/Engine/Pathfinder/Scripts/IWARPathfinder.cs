using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WAR.Board;

namespace WAR.Pathfinder {
	public interface IWARPathfinder {
		List<int> findPath(int source, int target, WARGrid grid);
	}
}
