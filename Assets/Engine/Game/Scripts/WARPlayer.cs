using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WAR.Game {
	public class WARPlayer {
		// the unique id of the player in the game
		public int id;
		// how many points the player has earned
		public int victoryPoints = 0;
		
		public WARPlayer (int id) {
			// construct a player object with the desired id
			this.id = id;
		}
	}
}
