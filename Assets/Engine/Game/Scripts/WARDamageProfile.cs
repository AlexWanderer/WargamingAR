using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WAR.Game {
	public class DamageProfile {
		public Damage emp;
		public Damage thermal;
		public Damage phaser;
		public Damage kinetic;
		public Damage special;
		
		// get the damage associated with a particular type
		public Damage getType(DamageType type) {
			return (Damage) GetType().GetField(type.ToString()).GetValue(this);
		}
	}
}