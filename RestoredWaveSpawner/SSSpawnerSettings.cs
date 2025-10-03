using System;
using System.Linq;
using Exiled.API.Features;
using Respawning;
using Respawning.Waves;
using UserSettings.ServerSpecific;
using Player = LabApi.Features.Wrappers.Player;

namespace RestoredWaveSpawner
{
    public class SSSpawnerSettings
    {

        //private SSButton ntfSpawnButton;
        //private SSButton chaosSpawnButton;
        private SSTwoButtonsSetting instantSpawnToggle;
        private SSTwoButtonsSetting miniSpawnToggle;
        private SSTwoButtonsSetting sideToggle;
        private SSButton spawnButton;

        public void Activate()
        {
            instantSpawnToggle = new SSTwoButtonsSetting(null, "Spawn Type","Animated","Instant", hint:"Choose between instant spawns or animated spawns");
            miniSpawnToggle = new SSTwoButtonsSetting(null, "Wave Size","Full","Mini", hint:"Should spawn a full or mini wave");
            sideToggle = new SSTwoButtonsSetting(null, "Faction","NTF","Chaos", hint:"Choose between NTF or Chaos spawn");
            spawnButton = new SSButton(null, "Spawn Wave","Spawn", hint:"Spawns a wave based on the settings above.");
            var settings = new ServerSpecificSettingBase[5]
            {
                new SSGroupHeader("Restored Wave Spawner"),
                instantSpawnToggle,
                miniSpawnToggle,
                sideToggle,
                spawnButton
            };
            if(ServerSpecificSettingsSync.DefinedSettings == null)
                ServerSpecificSettingsSync.DefinedSettings = settings;
            else
                ServerSpecificSettingsSync.DefinedSettings = ServerSpecificSettingsSync.DefinedSettings.Concat(settings).ToArray();
            ServerSpecificSettingsSync.SendToPlayersConditionally((hub => Player.Get(hub).HasPermission(PlayerPermissions.RespawnEvents)));
            ServerSpecificSettingsSync.ServerOnSettingValueReceived += ProcessUserInput;
        }
        
        public void Deactivate()
        {
            ServerSpecificSettingsSync.ServerOnSettingValueReceived -= ProcessUserInput;
        }
        
        public void ProcessUserInput(ReferenceHub hub, ServerSpecificSettingBase setting)
        {
            if (hub == null || setting == null)
                return;
            if (setting.SettingId == spawnButton.SettingId)
            {
                Player player = Player.Get(hub);
                if (player == null || !player.HasPermission(PlayerPermissions.RespawnEvents))
                    return;
                bool instant = ServerSpecificSettingsSync
                    .GetSettingOfUser<SSTwoButtonsSetting>(hub, instantSpawnToggle.SettingId).SyncIsB;
                bool mini = ServerSpecificSettingsSync
                    .GetSettingOfUser<SSTwoButtonsSetting>(hub, miniSpawnToggle.SettingId).SyncIsB;
                bool chaos = ServerSpecificSettingsSync
                    .GetSettingOfUser<SSTwoButtonsSetting>(hub, sideToggle.SettingId).SyncIsB;
                DoSpawn(mini,chaos,instant);
            }
        }

        private void DoSpawn(bool isMini, bool isChaos, bool instant)
        {
            SpawnableWaveBase wave = null;
            if (isChaos)
            {
                if (isMini)
                {
                    WaveManager.TryGet(out ChaosMiniWave chaosMiniWave);
                    wave = chaosMiniWave;
                }
                else
                {
                    WaveManager.TryGet(out ChaosSpawnWave chaosSpawnWave);
                    wave = chaosSpawnWave;
                }
            }
            else
            {
                if (isMini)
                {
                    WaveManager.TryGet(out NtfMiniWave ntfMiniWave);
                    wave = ntfMiniWave;
                }
                else
                {
                    WaveManager.TryGet(out NtfSpawnWave ntfSpawnWave);
                    wave = ntfSpawnWave;
                }
            }

            if (wave == null) return;
            if (instant)
            {
                WaveManager.Spawn(wave);
            } else
            {
                WaveManager.InitiateRespawn(wave);
            }
        }
    }
}