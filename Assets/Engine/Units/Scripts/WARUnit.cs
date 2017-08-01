using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;
using System.Linq;

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
		 
		public void takeDamage(IWARAttack attack) {
			// go over every possible damage type
			foreach (DamageType type in Enum.GetValues(typeof(DamageType))) {
				// the damage associated with this type
				var damage = attack.getAttack().getType(type);
				// the armor associated with the type
				var armor = attack.getArmor().getType(type);
				
				// take the damage associated with the type
				currentHealth -= damage.strength - (armor.strength - damage.armorPen);
			}
		}
	}
}
