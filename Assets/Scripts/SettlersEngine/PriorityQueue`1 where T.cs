// @sonhg: class: SettlersEngine.PriorityQueue`1 where T
using System;
using System.Collections.Generic;

namespace SettlersEngine
{
	internal class PriorityQueue<T> where T : IIndexedObject
	{
		public PriorityQueue()
		{
			this.mComparer = Comparer<T>.Default;
		}

		public PriorityQueue(IComparer<T> comparer)
		{
			this.mComparer = comparer;
		}

		public PriorityQueue(IComparer<T> comparer, int capacity)
		{
			this.mComparer = comparer;
			this.InnerList.Capacity = capacity;
		}

		protected void SwitchElements(int i, int j)
		{
			T value = this.InnerList[i];
			this.InnerList[i] = this.InnerList[j];
			this.InnerList[j] = value;
			T t = this.InnerList[i];
			t.Index = i;
			T t2 = this.InnerList[j];
			t2.Index = j;
		}

		protected virtual int OnCompare(int i, int j)
		{
			return this.mComparer.Compare(this.InnerList[i], this.InnerList[j]);
		}

		public int Push(T item)
		{
			int num = this.InnerList.Count;
			item.Index = this.InnerList.Count;
			this.InnerList.Add(item);
			while (num != 0)
			{
				int num2 = (num - 1) / 2;
				if (this.OnCompare(num, num2) >= 0)
				{
					return num;
				}
				this.SwitchElements(num, num2);
				num = num2;
			}
			return num;
		}

		public T Pop()
		{
			T result = this.InnerList[0];
			int num = 0;
			this.InnerList[0] = this.InnerList[this.InnerList.Count - 1];
			T t = this.InnerList[0];
			t.Index = 0;
			this.InnerList.RemoveAt(this.InnerList.Count - 1);
			result.Index = -1;
			for (;;)
			{
				int num2 = num;
				int num3 = 2 * num + 1;
				int num4 = 2 * num + 2;
				if (this.InnerList.Count > num3 && this.OnCompare(num, num3) > 0)
				{
					num = num3;
				}
				if (this.InnerList.Count > num4 && this.OnCompare(num, num4) > 0)
				{
					num = num4;
				}
				if (num == num2)
				{
					break;
				}
				this.SwitchElements(num, num2);
			}
			return result;
		}

		public void Update(T item)
		{
			int count = this.InnerList.Count;
			while (item.Index - 1 >= 0 && this.OnCompare(item.Index - 1, item.Index) > 0)
			{
				this.SwitchElements(item.Index - 1, item.Index);
			}
			while (item.Index + 1 < count && this.OnCompare(item.Index + 1, item.Index) < 0)
			{
				this.SwitchElements(item.Index + 1, item.Index);
			}
		}

		public T Peek()
		{
			if (this.InnerList.Count > 0)
			{
				return this.InnerList[0];
			}
			return default(T);
		}

		public void Clear()
		{
			this.InnerList.Clear();
		}

		public int Count
		{
			get
			{
				return this.InnerList.Count;
			}
		}

		protected List<T> InnerList = new List<T>();

		protected IComparer<T> mComparer;
	}
}
