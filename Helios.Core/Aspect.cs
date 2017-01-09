using System;
namespace Helios.Core
{
	public abstract class Aspect
	{
		public readonly uint Owner;

		public Aspect(uint entity)
		{
			Owner = entity;
		}
	}
}
