using System;
using Exiled.API.Enums;
using LabApi.Features;
using LabApi.Loader.Features.Plugins;
using LabApi.Loader.Features.Plugins.Enums;

namespace RestoredWaveSpawner
{
#if EXILED
    public class Plugin : Exiled.API.Features.Plugin<Config>
#else
    public class Plugin : Plugin<Config>
#endif
    {
        public static Plugin Instance { get; private set; }
        public SSSpawnerSettings settings;

#if EXILED
        public override void OnEnabled()
#else
        public override void Enable()
#endif
        {

            Instance = this;
            settings = new SSSpawnerSettings();
            settings.Activate();

        }

#if EXILED
        public override void OnDisabled()
#else
        public override void Disable()
#endif
        {
            if (settings != null)
            {
                settings.Deactivate();
                settings = null;
            }
            Instance = null;
        }


        public override string Author { get; } = "TayTay";
        public override Version Version { get; } = new Version(1, 0, 0, 0);
        
#if EXILED
            public override string Name { get; } = "RestoredWaveSpawner.EXILED";
            public override PluginPriority Priority { get; } = PluginPriority.Lowest;
#else
        public override string Name { get; } = "RestoredWaveSpawner.LabAPI";

        public override string Description { get; } =
            "Readds the wave spawner option for SCP:SL until it's officially added back into the game.";

        public override Version RequiredApiVersion { get; } = new Version(LabApiProperties.CompiledVersion);
        public override LoadPriority Priority { get; } = LoadPriority.Lowest;
#endif
    }
}