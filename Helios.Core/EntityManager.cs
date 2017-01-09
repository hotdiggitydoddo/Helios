using System;
using System.Collections.Generic;
using System.Linq;

namespace Helios.Core
{
	public class EntityManager
	{
		public EntityManager()
		{
			_components = new Dictionary<uint, List<IComponent>>();
			_subsystems = new List<ISubsystem>();
			_entities = new Dictionary<uint, Bitset>();
		}

		private uint _nextId = 1;
		private Dictionary<uint, List<IComponent>> _components;
		private List<ISubsystem> _subsystems;
		private Dictionary<uint, Bitset> _entities;

		public uint CreateEntity()
		{
			var newId = _nextId;
			_entities.Add(newId, new Bitset());
			_nextId++;
		
			return newId;
		}


		public T GetComponent<T>(uint entity) where T : IComponent
		{
			return (T)_components[entity].Where(x => x.GetType() == typeof(T));
		}

		public List<IComponent> GetComponents(uint entity)
		{
			if (_components.ContainsKey(entity))
				return _components[entity];

			return new List<IComponent>();
		}

		public void RegisterSubsystem(ISubsystem subsystem)
		{
			if (!_subsystems.Select(s => s.ComponentMask.GetBits()).Contains(subsystem.ComponentMask.GetBits()))
				_subsystems.Add(subsystem);
		}

		public void AddComponent<T>(uint entity, T component) where T : IComponent
		{
			if (!_components.ContainsKey(entity))
				_components.Add(entity, new List<IComponent> { component });
			else
				_components[entity].Add(component);

			_entities[entity].SetBit(component.TypeId());

			var relevantSubsystems = _subsystems.Where(s => s.ComponentMask.IsSubsetOf(_entities[entity]));

			foreach (var s in relevantSubsystems)
				s.CreateAspect(entity, GetComponents(entity));
		}
	}
}
