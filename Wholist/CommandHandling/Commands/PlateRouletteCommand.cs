using System;
using System.Linq;
using Dalamud.Game.Command;
using Sirensong.Extensions;
using Sirensong.Game.Helpers;
using Wholist.CommandHandling.Interfaces;
using Wholist.Common;
using Wholist.Game;
using Wholist.Resources.Localization;


namespace Wholist.CommandHandling.Commands
{
    internal sealed class RandomPlateCommand : IDalamudCommand
    {

        /// <inheritdoc />
        public string Name { get; } = "/randplate";

        /// <inheritdoc />
        public CommandInfo Command => new(this.OnExecute) { HelpMessage = Strings.Commands_WhoSettings_RandPlate, ShowInHelp = true };

        /// <inheritdoc />
        public IReadOnlyCommandInfo.HandlerDelegate OnExecute => (command, _) =>
        {
            if (command != this.Name)
            {
                return;
            }

            if (Services.ClientState.IsPvP)
            {
                ChatHelper.PrintError(Strings.Errors_NoUseInPvP);
                return;
            }

            var players = PlayerManager.GetNearbyPlayers(true)
                .Where(x => x.GameObjectId != Services.ObjectTable.LocalPlayer?.GameObjectId);
            if (!players.Any())
            {
                ToastHelper.ShowErrorToast(Strings.Commands_WhoSettings_RandPlate_NoPlayers);
                return;
            }
            var r = new Random();
            players.ElementAt(r.Next(players.Count())).OpenCharaCard();
        };
    }
}
