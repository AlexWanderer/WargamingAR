using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using WAR.Board;

namespace WAR.Units {
	public abstract class WARUnit : WARMovableObject {
		// the id of the player that owns this unit
		public int owner;
	}
}
