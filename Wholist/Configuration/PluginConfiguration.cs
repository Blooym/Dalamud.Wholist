using System;
using System.Numerics;
using Dalamud.Configuration;
using Wholist.Common;

namespace Wholist.Configuration
{
    /// <summary>
    ///     Provides access to and determines the plugin configuration.
    /// </summary>
    [Serializable]
    internal sealed class PluginConfiguration : IPluginConfiguration
    {
        /// <summary>
        ///     The current configuration version, incremented on breaking changes.
        /// </summary>
        public int Version { get; set; }

        /// <summary>
        ///     The configuration for colours used in the plugin.
        /// </summary>
        public ColourConfiguration Colours = new();

        /// <summary>
        ///     The configuration for the "Nearby Players" window.
        /// </summary>
        public NearbyPlayersConfiguration NearbyPlayers = new();

        /// <summary>
        ///     Saves the current configuration to disk.
        /// </summary>
        internal void Save() => Services.PluginInterface.SavePluginConfig(this);

        /// <summary>
        ///     The configuration for the "Nearby Players" window.
        /// </summary>
        internal sealed class NearbyPlayersConfiguration
        {
            public enum LodestoneSearchRegion
            {
                Europe,
                Germany,
                France,
                NorthAmerica,
                Japan

            }

            /// <summary>
            ///     Whether to hide AFK players from the list.
            /// </summary>
            public bool FilterAfk;

            /// <summary>
            ///     Whether to hide the window in combat.
            /// </summary>
            public bool HideInCombat;

            /// <summary>
            ///     Whether to hide the window in instances.
            /// </summary>
            public bool HideInInstance;

            /// <summary>
            ///     The maximum number of players to show in the list.
            /// </summary>
            public int MaxPlayersToShow = 100;

            /// <summary>
            ///     Automatically open the window on login.
            /// </summary>
            public bool OpenOnLogin;

            /// <summary>
            ///     Whether the list should prioritize known players.
            /// </summary>
            public bool PrioritizeKnown = true;

            /// <summary>
            ///     Whether the list should use class abbreviations instead of full names.
            /// </summary>
            public bool UseJobAbbreviations;

            /// <summary>
            ///     Whether to show the search bar from the window.
            /// </summary>
            public bool ShowSearchBar = true;

            /// <summary>
            ///     Whether to hide blocked players from the list.
            /// </summary>
            public bool FilterBlockedPlayers = true;

            /// <summary>
            ///     Whether to hide low level characters.
            /// </summary>
            public bool FilterLowLevel = true;

            /// <summary>
            ///     Whether to show directions relative to the local player, rather than to the camera.
            /// </summary>
            public bool PlayerRelativeDirection;

            /// <summary>
            ///     The region to perform a lodestone player search lookup on.
            /// </summary>
            public LodestoneSearchRegion LodestonePlayerSearchRegion = LodestoneSearchRegion.Europe;
        }

        /// <summary>
        ///     The configuration for colours used in the plugin.
        /// </summary>
        internal sealed class ColourConfiguration
        {
            private static Vector4 tankColourDefault = new(0f, 0.6f, 1f, 1f);
            private static Vector4 healerColourDefault = new(0f, 0.8f, 0.1333333f, 1f);
            private static Vector4 dpsColourDefault = new(0.7058824f, 0f, 0f, 1f);
            private static Vector4 otherColourDefault = new(0.5f, 0.5f, 0.5f, 1f);

            /// <summary>
            ///     Represents different ways JobColours can be displayed.
            /// </summary>
            public enum JobColourMode
            {
                Role,
                Job,
            }

            public JobColourMode JobColMode = JobColourMode.Role;
            public NameColours Name = new();
            public RoleColours Role = new();
            public JobColours Job = new();

            /// <summary>
            ///     Represents the colours used for player names.
            /// </summary>
            internal sealed class NameColours
            {
                public Vector4 Default = new(1.0f, 1.0f, 1.0f, 1.0f);
                public Vector4 Friend = new(1.0f, 0.5f, 0.0f, 1.0f);
                public Vector4 Party = new(0.0f, 0.7f, 1.0f, 1.0f);
                public Vector4 SyncServicePaired = new(0.3f, 1.0f, 0.5f, 1.0f);
            }

            /// <summary>
            ///     Represents the colours used for player job roles.
            /// </summary>
            internal sealed class RoleColours
            {
                public Vector4 Tank = tankColourDefault;
                public Vector4 Healer = healerColourDefault;
                public Vector4 MeleeDps = dpsColourDefault;
                public Vector4 RangedDps = dpsColourDefault;
                public Vector4 Other = otherColourDefault;
            }

            /// <summary>
            ///     Represents the colours used for player jobs.
            /// </summary>
            internal sealed class JobColours
            {
                // Tanks
                public Vector4 Warrior = tankColourDefault;
                public Vector4 Paladin = tankColourDefault;
                public Vector4 DarkKnight = tankColourDefault;
                public Vector4 Gunbreaker = tankColourDefault;

                // Healers
                public Vector4 WhiteMage = healerColourDefault;
                public Vector4 Scholar = healerColourDefault;
                public Vector4 Astrologian = healerColourDefault;
                public Vector4 Sage = healerColourDefault;

                // Melee
                public Vector4 Monk = dpsColourDefault;
                public Vector4 Dragoon = dpsColourDefault;
                public Vector4 Ninja = dpsColourDefault;
                public Vector4 Samurai = dpsColourDefault;
                public Vector4 Reaper = dpsColourDefault;
                public Vector4 Viper = dpsColourDefault;

                // Ranged
                public Vector4 Bard = dpsColourDefault;
                public Vector4 Machinist = dpsColourDefault;
                public Vector4 Dancer = dpsColourDefault;

                // Casters
                public Vector4 BlackMage = dpsColourDefault;
                public Vector4 Summoner = dpsColourDefault;
                public Vector4 RedMage = dpsColourDefault;
                public Vector4 Pictomancer = dpsColourDefault;
                public Vector4 BlueMage = dpsColourDefault;

                // Misc
                public Vector4 Other = otherColourDefault;
            }
        }
    }
}
