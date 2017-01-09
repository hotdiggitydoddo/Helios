using System;
using System.Collections.Generic;

namespace Helios.Core
{
	public class BitsetEqualityComparer : IEqualityComparer<Bitset>
	{
		public BitsetEqualityComparer()
		{
		}

		public bool Equals(Bitset x, Bitset y)
		{
			return x.GetBits() == y.GetBits();
		}

		public int GetHashCode(Bitset obj)
		{
			return (int)obj.GetBits() * 125;
		}
	}
}
