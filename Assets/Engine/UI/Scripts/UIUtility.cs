using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WAR.UI {
	public struct UIPlane {
		public Vector3 center; // center of the plane
		public Vector3 extent; // full (width,height)
		public Quaternion rotation;
	
		public Vector3 start {
			get {return center - extent * 0.5f;}
		}
		public Vector3 end {
			get {return center + extent * 0.5f;}
		}
		
		public UIPlane (Vector3 start, Vector3 end, Quaternion? rotation = null) {
			this.center = (start + end)/2;
			Vector3 extent = 2 * (end - this.center);
			this.extent = new Vector3 (Mathf.Abs (extent.x), Mathf.Abs (extent.y), Mathf.Abs (extent.z));
			this.rotation = rotation ?? Quaternion.identity;
		}
	}
}
