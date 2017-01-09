using System;
using System.Collections.Generic;
using Helios.Core;

namespace Helios.Core
{
	public abstract class Subsystem : ISubsystem
	{
		private List<uint> _entitiesToAdd;
		private List<uint> _entitiesToRemove;

		protected EntityManager _em { get; private set; }
		protected List<uint> _relevantEntities { get; private set; }
		protected Bitset _bits;

		public Bitset ComponentMask { get { return _bits; } }

		public Subsystem(EntityManager em)
		{
			_em = em;
			_bits = new Bitset();
			_relevantEntities = new List<uint>();
			_entitiesToAdd = new List<uint>();
			_entitiesToRemove = new List<uint>();
		}

		public virtual void OnComponentAdded(uint entity)
		{
			_entitiesToAdd.Add(entity);
		}

		public virtual void OnComponentRemoved(uint entity)
		{
			_entitiesToRemove.Add(entity);
		}

		public virtual void Update()
		{
			_relevantEntities.AddRange(_entitiesToAdd);
			_relevantEntities.RemoveAll(x => _entitiesToRemove.Contains(x));

			_entitiesToAdd.Clear();
			_entitiesToRemove.Clear();
		}

		public virtual void CreateAspect(uint entity, List<IComponent> components) { }
	}
}
