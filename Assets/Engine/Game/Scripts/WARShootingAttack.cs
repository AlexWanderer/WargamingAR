using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using WAR.Units;

namespace WAR.Game {
	public enum DamageType {
		emp,
		thermal,
		phaser,
		kinetic,
		special,
	}
	
	public struct Damage {
		public DamageType type;
		[Range(1,4)]
		public int damage;
		[Range(1,4)]
		public int armorPen;
	}
	
	public struct ShootingAttack {
		List<Damage> damage;		
	}
	
	public class WARShootingAttack {
		// who is making this shooting attack
		private WARUnit shooter;
		private ShootingAttack attack;
		
		// make the initial modification with the weapon that was fired
		public WARShootingAttack(WARUnit shooter, IWARShootingModifier fired) {
			this.shooter = shooter;
			fired.modifyShootingAttack(out this.attack);
		}
	}
}
