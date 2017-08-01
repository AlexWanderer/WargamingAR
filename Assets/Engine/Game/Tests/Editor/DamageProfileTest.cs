using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;

using WAR.Tools;
using WAR.Units;

namespace WAR.Game.Tests {
	public class DamageProfileTest {
		
		[Test]
		public void GetType() {
			// a profile to test with
			var profile = new DamageProfile{emp = new Damage{strength=1}};
			
			// make sure we can retrieve the type
			Assert.AreEqual(1, profile.getType(DamageType.emp).strength, 1);
		}
	}
}
