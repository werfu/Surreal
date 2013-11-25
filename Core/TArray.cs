using System;
using System.Collections;
using System.Collections.Generic;

namespace Surreal.Core
{
	public class TArray<t> : IEnumerable
	{
		public void AddItem (t item)
		{
			innerList.Add(item);
		}

		public void Empty ()
		{
			innerList.Clear();
		}

		public t Pop ()
		{
			if (innerList.Count > 0) {
				t last = innerList [innerList.Count - 1];
				innerList.Remove (last);
				return last;
			}
			else
				return default(t);
		}

		public t Last ()
		{
			if (innerList.Count > 0)
				return innerList [innerList.Count - 1];
			else
				return default(t);
		}

		public void Insert (t i)
		{
			innerList.Insert( 0, i ); 
			// TODO : Note quite sure if Insert must insert at the begining
		}

		public void Add (t a)
		{
			innerList.Add(a);
		}

		private List<t> innerList;

		public TArray ()
		{
			innerList = new List<t>();
		}

		public TArray (int size)
		{
			innerList = new List<t>(size);
		}

		public t[] ToArray()
		{
			return innerList.ToArray();
		}


		public int Num ()
		{
			return innerList.Count;
		}

		public t this [int indexer] {
			get {
				return innerList[indexer];
			}
			set {
				innerList[indexer] = value;
			}
		}

		#region IEnumerable implementation
		public IEnumerator GetEnumerator ()
		{
			return innerList.GetEnumerator();
		}
		#endregion
	}
}

