using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;

using WAR.Board;
using WAR.Game;

namespace WAR.Units {
	public abstract class WARUnit : WARMovableObject {
		// the id of the player that owns this unit
		public int owner;
		
		[SerializeField]
		public int maxHealth;
		public int currentHealth;
		
		public void Start() {
		// start off at max health
			currentHealth = maxHealth;
		}
		
		public void takeDamage(DamageProfile damage) {
			// go over every possible damage type
			foreach (DamageType foo in Enum.GetValues(typeof(DamageType))) {
				// the damage associated with this type
				var dmg = damage.GetType().GetField(foo.ToString()).GetValue(damage);
				// take the damage associated with the type
				currentHealth -= ((Damage)dmg).strength;
			}
		}
	}
}
