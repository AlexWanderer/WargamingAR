using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WAR;
using WAR.Utils;

namespace WAR.Ships {
	public class WARShipLibrary : WARLib<WARShipLibrary> {		
		public static GameObject[] ships {
			get {
				return WARShipLibrary.Instance.assets.ToArray();
			}	
		}
	}
}
