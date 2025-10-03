using System;
using Exiled.API.Enums;
using LabApi.Features;
using LabApi.Loader.Features.Plugins;
using LabApi.Loader.Features.Plugins.Enums;

namespace RestoredWaveSpawner
{

    public class Plugin
    {

        // Shared constants
        public static string Name { get; } = "RestoredWaveSpawner";
        public static string Author { get; } = "TayTay";
        public static Version Version { get; } = new Version(1, 0, 0, 0);

        // These will be set depending on which plugin system is being used.
        public static LabAPIPlugin LabAPI { get; private set; } = null;
        public static ExiledPlugin Exiled { get; private set; } = null;

        /**
         * Dynamically gets the config from either LabAPI or Exiled, depending on which one is being used.
         */
        public static Config Config => LabAPI != null ? LabAPI.Config : Exiled?.Config;

        public static SSSpawnerSettings Settings => LabAPI != null ? LabAPI.settings : Exiled?.settings;


        public class LabAPIPlugin : LabApi.Loader.Features.Plugins.Plugin<Config>
        {

            public static LabAPIPlugin Instance { get; private set; }
            public SSSpawnerSettings settings;


            public override void Enable()
            {
                Instance = this;
                LabAPI = this;
                settings = new SSSpawnerSettings();
                settings.Activate();
            }

            public override void Disable()
            {
                if (settings != null)
                {
                    settings.Deactivate();
                    settings = null;
                }

                Instance = null;
                LabAPI = null;
            }

            public override string Name { get; } = Plugin.Name + ".LabAPI";

            public override string Description { get; } =
                "A plugin for LabAPI that adds a respawn wave menu back to the game.";

            public override string Author { get; } = Plugin.Author;
            public override Version Version { get; } = Plugin.Version;
            public override Version RequiredApiVersion { get; } = new Version(LabApiProperties.CompiledVersion);
        }

        public class ExiledPlugin : Exiled.API.Features.Plugin<Config>
        {
            public static ExiledPlugin Instance { get; private set; }
            public SSSpawnerSettings settings;

            public override void OnEnabled()
            {
                Instance = this;
                Exiled = this;
                settings = new SSSpawnerSettings();
                settings.Activate();
            }

            public override void OnDisabled()
            {
                if (settings != null)
                {
                    settings.Deactivate();
                    settings = null;
                }

                Instance = null;
                Exiled = null;
            }

            public override string Name { get; } = $"{Plugin.Name}.Exiled";
            public override string Author { get; } = Plugin.Author;
            public override Version Version { get; } = Plugin.Version;
            public override PluginPriority Priority { get; } = PluginPriority.High;
        }
    }
}