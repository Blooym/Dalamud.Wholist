using System.Collections.Generic;
using System.Linq;
using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;
using Lumina.Excel.Sheets;
using Sirensong;
using Sirensong.Extensions;
using Sirensong.Game.Enums;
using Sirensong.IoC;
using Wholist.Configuration;
using Wholist.DTRHandling;
using Wholist.Resources.Localization;
using Wholist.UserInterface;

namespace Wholist.Common
{
    /// <summary>
    ///     Provides access to necessary instances and services.
    /// </summary>
    internal sealed class Services
    {
        /// <inheritdoc cref="MiniServiceContainer" />
        private static readonly MiniServiceContainer ServiceContainer = new();

        // Dalamud services
        [PluginService] internal static IDalamudPluginInterface PluginInterface { get; set; } = null!;
        [PluginService] internal static IClientState ClientState { get; set; } = null!;
        [PluginService] internal static IObjectTable ObjectTable { get; set; } = null!;
        [PluginService] internal static ICommandManager Commands { get; set; } = null!;
        [PluginService] internal static ICondition Condition { get; set; } = null!;
        [PluginService] internal static IPartyList PartyList { get; set; } = null!;
        [PluginService] internal static IPluginLog PluginLog { get; set; } = null!;
        [PluginService] internal static IGameInteropProvider GameInteropProvider { get; set; } = null!;
        [PluginService] internal static IDataManager DataManager { get; set; } = null!;
        [PluginService] internal static IDtrBar DtrBar { get; set; } = null!;
        [PluginService] internal static IFramework Framework { get; set; } = null!;

        // Data dictionaries
        internal static Dictionary<uint, string> WorldNames { get; set; } = null!;
        internal static Dictionary<uint, (string name, string abbreviation, ClassJobRole role, uint id)> JobNames { get; set; } = null!;

        // Plugin services
        internal static WindowManager WindowManager { get; private set; } = null!;
        internal static PluginConfiguration Configuration { get; private set; } = null!;
        internal static DTRManager DtrManager { get; private set; } = null!;

        /// <summary>
        ///     Initializes the service class.
        /// </summary>
        internal static void Initialize(IDalamudPluginInterface pluginInterface)
        {
            SirenCore.InjectServices<Services>();
            pluginInterface.Create<Services>();
            Configuration = PluginInterface.GetPluginConfig() as PluginConfiguration ?? new PluginConfiguration();
            ServiceContainer.GetOrCreateService<LocalizationManager>();
            InitializeDataDictionaries();
            InitializeServices();
        }

        /// <summary>
        ///     Initializes data dictionaries from Excel sheets.
        /// </summary>
        private static void InitializeDataDictionaries()
        {
            WorldNames = DataManager.GetExcelSheet<World>()
                .ToDictionary(item => item.RowId, item => item.Name.ExtractText().ToTitleCase());
            JobNames = DataManager.GetExcelSheet<ClassJob>()
                .ToDictionary(item => item.RowId, item => (
                    name: item.Name.ExtractText().ToTitleCase(),
                    abbreviation: item.Abbreviation.ExtractText().ToUpperInvariant(),
                    role: (ClassJobRole)item.Role,
                    id: item.RowId
                ));
        }

        /// <summary>
        ///     Initializes plugin services.
        /// </summary>
        private static void InitializeServices()
        {
            WindowManager = ServiceContainer.GetOrCreateService<WindowManager>();
            DtrManager = ServiceContainer.GetOrCreateService<DTRManager>();
            ServiceContainer.CreateService<CommandHandling.CommandManager>();
        }

        /// <summary>
        ///     Disposes of the service class.
        /// </summary>
        internal static void Dispose() => ServiceContainer.Dispose();
    }
}
