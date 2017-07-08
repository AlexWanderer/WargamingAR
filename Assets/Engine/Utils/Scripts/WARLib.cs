using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using WAR;
using WAR.Ships;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace WAR.Utils {
	public abstract class WARLib<T> : Manager<WARLib<T>> {
		[Required][SerializeField] string filter;
		
		[AssetList(CustomFilterMethod = "Filter")]
		[InfoBox("asset pool we can load from")]
		public List<GameObject> assets = new List<GameObject>();
		
		public bool Filter(GameObject obj) {
			#if UNITY_EDITOR
			// Filter returns true if the AssetDatabase finds obj using filter
			return AssetDatabase
				.FindAssets(filter, null)
				.ToList()
				.Where( guid => AssetDatabase.LoadMainAssetAtPath(AssetDatabase.GUIDToAssetPath(guid)) == obj ).Any();
			#endif
			return false;
		}
	}
}
