using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WAR.Game {
	public interface IWARShootingTargetModifier {
		// handler to modify a shooting attack 
		ShootingAttack modifyShootingAttackTarget(ShootingAttack attack);
	}
}