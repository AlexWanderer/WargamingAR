using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using WAR.Game;

public interface IWARAttack {

	DamageProfile getArmor();
	DamageProfile getAttack();
}
