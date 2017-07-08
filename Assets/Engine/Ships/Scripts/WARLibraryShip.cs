using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WAR;
using WAR.Utils;

namespace WAR.Ships {
	public class WARLibraryShip : WARLibrary<WARLibraryShip> {		
		public static GameObject[] ships {
			get {
				return WARLibraryShip.Instance.assets.ToArray();
			}	
		}
	}
}
