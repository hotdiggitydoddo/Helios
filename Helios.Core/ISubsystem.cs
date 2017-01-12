using System.Collections.Generic;

namespace Helios.Core
{
	public interface ISubsystem
	{
		Bitset ComponentMask { get; }

		void OnComponentAdded(uint entity);
		void OnComponentRemoved(uint entity);
		void Update(float dt);
		void CreateAspect(uint entity, List<IComponent> components);
	    bool HasAspect(uint entity);
	}

    public interface ILateUpdateable
    {
        void LateUpdate(float dt);
    }
}
