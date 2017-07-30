using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using WAR.Board;

namespace WAR.Units {
	public abstract class WARUnit : WARMovableObject {
		// the id of the player that owns this unit
		public int owner;
		
		[SerializeField]
		public int maxHealth;
		private int currentHealth;
		
		public void Start() {
		// start off at max health
			currentHealth = maxHealth;
		}
		
		public void takeDamage(int damage) {
		// for now, just decrement the health
			maxHealth -= damage;
		}
	}
}
