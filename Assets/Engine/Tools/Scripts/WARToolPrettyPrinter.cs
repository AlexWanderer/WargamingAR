using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WARToolPrettyPrinter {
	public static void PrintList<T>(List<T> list) {
		var str = list.Aggregate<T, string>("", (x, y) => x + ", " + y.ToString());
		Debug.Log("[" + str.Substring(2) + "]");
	}
}
