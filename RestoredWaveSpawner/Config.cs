using Exiled.API.Interfaces;

namespace RestoredWaveSpawner
{
#if EXILED
    public class Config : Exiled.API.Interfaces.IConfig
#else
    public class Config 
#endif
    {
        
        
        
        
#if EXILED
        public bool IsEnabled { get; set; } = true;
        public bool Debug { get; set; } = false;
#endif
    }
}