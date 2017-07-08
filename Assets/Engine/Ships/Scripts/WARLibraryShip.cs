using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WAR;
using WAR.Utils;
using Sirenix.OdinInspector;

namespace WAR.Ships {
	public class WARLibraryShip : WARLibrary<WARLibraryShip> {		
		string filter = "l: warship";
		
		
		public static GameObject[] ships {
			get {
				return WARLibraryShip.Instance.assets.ToArray();
			}	
		}
	}
}
