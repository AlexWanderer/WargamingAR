using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using WAR.Board;

namespace WAR.Board.Tests {
	public class FindCellNeighborIDsTest {
		//               ___     ___     ___
		//           ___/8  \___/17 \___/26 \
		//          /7  \___/16 \___/25 \___/ 
		//          \___/6  \___/15 \___/24 \ 
		//          /5  \___/14 \___/23 \___/
		//          \___/4  \___/13 \___/22 \ 
		//          /3	\___/12 \___/21 \___/
		//          \___/2  \___/11 \___/20 \
		//          /1  \___/10 \___/19 \___/
		//          \___/0  \___/9  \___/18 \
		//              \___/   \___/   \___/
		//
		
		private int nRows = 9;
		private int nColumns = 3;
		
		private void RunTest(int cellId, List<int> target) {
			// compute the result
			var result = WARHexGrid.FindCellNeighborIDs(cellId, nColumns, nRows);
			
			// make sure we got the right amount
			Assert.AreEqual(target.Count, result.Count);
			target.Sort();
			result.Sort();
			// make sure each element is what we expect
			Assert.AreEqual(target,result);
		}
		
		[Test]
		public void OddMiddle() {
			
			// a middle cell is something like row 4 column 5
			var cellId = 13;
			// the 6 spots we care about
			var target = new List<int>{14,15,23,21,11,12};
			RunTest(cellId,target);
					
		}
		
		[Test]
		public void EvenMiddle() {
			RunTest(14,new List<int>{6,16,15,13,12,4});
		}
		
		[Test]
		public void LeftEdge() {
			// a middle cell that is on the left edge
			var cellId = 3;
			// the 6 spots we care about
			var target = new List<int>{1,2,4,5};
			RunTest(cellId,target);
			
		}
		
		[Test]
		public void RightEdge() {
			// a middle cell that is on the left edge
			var cellId = 22;
			// the 6 spots we care about
			var target = new List<int>{20,21,23,24};
	
			RunTest(cellId,target);
			
		}
		[Test]
		public void TopOddEdge() {
			// a middle cell that is on the left edge
			var cellId = 9;
			// the 6 spots we care about
			var target = new List<int>{10,11,19};
			RunTest(cellId,target);
		}
		
		[Test]
		public void TopEvenEdge() {
			// a middle cell that is on the left edge
			var cellId = 16;
			// the 6 spots we care about
			var target = new List<int>{8,6,14,15,17};
			RunTest(cellId,target);
		}
		[Test]
		public void BottomOddEdge() {
			// a middle cell that is on the left edge
			var cellId = 17;
			// the 6 spots we care about
			var target = new List<int>{16,15,25};
			RunTest(cellId,target);
		}
		
		[Test]
		public void BottomEvenEdge() {
			// a middle cell that is on the left edge
			var cellId = 10;
			// the 6 spots we care about
			var target = new List<int>{0,2,12,11,9};
			RunTest(cellId,target);
		}
		[Test]
		public void TopLeftEdge() {
			// a middle cell that is on the left edge
			var cellId = 7;
			// the 6 spots we care about
			var target = new List<int>{8,6,5};
			RunTest(cellId,target);
		}
		[Test]
		public void BottomLeftEdge() {
			// a middle cell that is on the left edge
			var cellId = 1;
			// the 6 spots we care about
			var target = new List<int>{0,2,3};
			RunTest(cellId,target);
		}
		[Test]
		public void BottomRightEdge() {
			// a middle cell that is on the left edge
			var cellId = 18;
			// the 6 spots we care about
			var target = new List<int>{19,20};
			RunTest(cellId,target);	
		}
		[Test]
		public void TopRightEdge() {
			// a middle cell that is on the left edge
			var cellId = 26;
			// the 6 spots we care about
			var target = new List<int>{24,25};
			RunTest(cellId,target);	
		}
		[Test]
		public void TopRightCornerEven() {
			// compute the result
			var result = WARHexGrid.FindCellNeighborIDs(28, 3, 10);
			var target = new List<int>{29,27,26};
			// make sure we got the right amount
			Assert.AreEqual(target.Count, result.Count);
			target.Sort();
			result.Sort();
			// make sure each element is what we expect
			Assert.AreEqual(target,result);
		}
		
	}
}
