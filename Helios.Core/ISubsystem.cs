using System.Collections.Generic;

namespace Helios.Core
{
	public interface ISubsystem
	{
		Bitset ComponentMask { get; }

		void OnComponentAdded(uint entity);
		void OnComponentRemoved(uint entity);
		void Update();
		void CreateAspect(uint entity, List<IComponent> components);
	}
}
