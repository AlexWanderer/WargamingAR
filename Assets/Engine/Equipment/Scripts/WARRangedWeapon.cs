using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using WAR.Game;

namespace WAR.Equipment {
	public class WARRangedWeapon : WARWeapon, IWARShootingModifier {
		// specific implementation of our shooting modifier
		public ShootingAttack modifyShootingAttack(ShootingAttack shootyMcScoots){
			return shootyMcScoots;
		}
	}
}
