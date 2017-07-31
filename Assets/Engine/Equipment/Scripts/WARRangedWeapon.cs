using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using WAR.Game;

namespace WAR.Equipment {
	public class WARRangedWeapon : WAREquipment, IWARShootingModifier {
		// specific implementation of our shooting modifier
		public void modifyShootingAttack(out ShootingAttack shootyMcScoots){
			shootyMcScoots = new ShootingAttack();
			return;
		}
	}
}
