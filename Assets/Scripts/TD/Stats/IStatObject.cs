using System.Collections.Generic;
namespace TowerDefense
{
    public interface IStatObject
    {
        public abstract Stats GetStats();
    }
    
    public interface IScalable
    {
        public abstract void ApplyScaling(Stats stats, float scaling);
    }

    public interface IStatComponent
    {
        public abstract Stats GetStats();
    }
}
