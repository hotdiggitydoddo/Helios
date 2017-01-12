using System;
using System.Collections.Generic;

namespace Helios.Core
{
	public abstract class Subsystem : ISubsystem
	{
		private readonly List<uint> _entitiesToAdd;
		private readonly List<uint> _entitiesToRemove;

		protected EntityManager _em { get; private set; }
		protected List<uint> _relevantEntities { get; private set; }
		protected Bitset _bits;

		public Bitset ComponentMask { get { return _bits; } }

	    protected Subsystem(EntityManager em)
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

		public virtual void Update(float dt)
		{
			_relevantEntities.AddRange(_entitiesToAdd);
			_relevantEntities.RemoveAll(x => _entitiesToRemove.Contains(x));

			_entitiesToAdd.Clear();
			_entitiesToRemove.Clear();
		}

	    public abstract void CreateAspect(uint entity, List<IComponent> components);
	    public abstract bool HasAspect(uint entity);
	}
}
