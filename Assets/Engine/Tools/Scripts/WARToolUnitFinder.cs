using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;

namespace WAR.Tools {
	public class WARToolUnitFinder : MonoBehaviour {
		public static string PATH = "Armies/";
		
		public static List<GameObject> GetByArmy(string army) {
			return Resources.LoadAll(PATH + army + "/", typeof(GameObject))
							.Select(x => x as GameObject).ToList();
		}
		public static List<GameObject> GetByArmyUnitType(string army, string type) {
			return (Resources.LoadAll(PATH + army + "/" + type, 
									  typeof(GameObject)) as GameObject[]).ToList();		
		}
		public static GameObject GetByArmyUnitName(string army, string name) {
			return GetByArmy(army).First(x => x.name == name);
		}		
	}
}
