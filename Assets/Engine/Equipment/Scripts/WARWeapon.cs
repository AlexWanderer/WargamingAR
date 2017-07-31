using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

using WAR.Game;

namespace WAR.Equipment {
	public class WARWeapon : WAREquipment {
		[SerializeField]
		private List<Damage> damage;
	}
}