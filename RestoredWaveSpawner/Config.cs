using Exiled.API.Interfaces;

namespace RestoredWaveSpawner
{
    public class Config : Exiled.API.Interfaces.IConfig
    {
        
        public bool IsEnabled { get; set; } = true;
        public bool Debug { get; set; } = false;
    }
}