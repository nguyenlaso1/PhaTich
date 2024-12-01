// @sonhg: class: InControl.TinyJSON.ProxyArray
using System;
using System.Collections;
using System.Collections.Generic;

namespace InControl.TinyJSON
{
	public sealed class ProxyArray : Variant, IEnumerable<Variant>, IEnumerable
	{
		public ProxyArray()
		{
			this.list = new List<Variant>();
		}

		IEnumerator<Variant> IEnumerable<Variant>.GetEnumerator()
		{
			return this.list.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.list.GetEnumerator();
		}

		public void Add(Variant item)
		{
			this.list.Add(item);
		}

		public override Variant this[int index]
		{
			get
			{
				return this.list[index];
			}
			set
			{
				this.list[index] = value;
			}
		}

		public int Count
		{
			get
			{
				return this.list.Count;
			}
		}

		internal bool CanBeMultiRankArray(int[] rankLengths)
		{
			return this.CanBeMultiRankArray(0, rankLengths);
		}

		private bool CanBeMultiRankArray(int rank, int[] rankLengths)
		{
			int count = this.list.Count;
			rankLengths[rank] = count;
			if (rank == rankLengths.Length - 1)
			{
				return true;
			}
			ProxyArray proxyArray = this.list[0] as ProxyArray;
			if (proxyArray == null)
			{
				return false;
			}
			int count2 = proxyArray.Count;
			for (int i = 1; i < count; i++)
			{
				ProxyArray proxyArray2 = this.list[i] as ProxyArray;
				if (proxyArray2 == null)
				{
					return false;
				}
				if (proxyArray2.Count != count2)
				{
					return false;
				}
				if (!proxyArray2.CanBeMultiRankArray(rank + 1, rankLengths))
				{
					return false;
				}
			}
			return true;
		}

		private List<Variant> list;
	}
}
