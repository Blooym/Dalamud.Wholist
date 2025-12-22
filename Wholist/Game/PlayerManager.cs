using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Dalamud.Game.ClientState.Objects.SubKinds;
using FFXIVClientStructs.FFXIV.Client.Game.Character;
using Sirensong.Game.Enums;
using Wholist.Common;
using Wholist.DataStructures;

namespace Wholist.Game
{
    internal static class PlayerManager
    {
        /// <summary>
        ///     Get all nearby players from the object table
        /// </summary>
        /// <param name="filterBlocked">Whether to remove all client blocked players</param>
        internal static unsafe IEnumerable<IPlayerCharacter> GetNearbyPlayers(bool filterBlocked)
            => Services.ObjectTable
                .OfType<IPlayerCharacter>()
                .Where(x => x.GameObjectId != Services.ObjectTable.LocalPlayer?.GameObjectId)
                .Where(x => x.Level != 0 && x.ObjectIndex < 240)
                .Where(x => !filterBlocked || !BlockedCharacterHandler.IsCharacterBlocked((BattleChara*)x.Address));

        /// <summary>
        ///     Gets nearby players from the <see cref="Dalamud.Game.ClientState.Objects.ObjectTable" /> that meet the given
        ///     criteria and turns them into <see cref="PlayerInfoSlim" />s.
        /// </summary>
        /// <param name="maxPlayers">The maximum number of players to return.</param>
        /// <param name="prioritizeKnownPlayers">Whether to prioritize the client known players.</param>
        /// <param name="filterAfk">Whether to filter out AFK players.</param>
        /// <returns></returns>
        internal static List<PlayerInfoSlim> GetNearbyPlayersSlim(int maxPlayers = 100, bool filterAfk = false, bool prioritizeKnownPlayers = false, bool filterLowLevel = false)
        {
            var players = new List<PlayerInfoSlim>(maxPlayers);

            // Get nearby players from the object table and order by them by distance to the local player
            // so that when the list is truncated, the closest players are kept.
            foreach (var player in GetNearbyPlayers(Services.Configuration.NearbyPlayers.FilterBlockedPlayers).OrderBy(p => p.YalmDistanceX))
            {
                if (players.Count >= maxPlayers)
                {
                    break;
                }

                if (filterLowLevel && player.Level <= 3)
                {
                    continue;
                }

                if (filterAfk && player.OnlineStatus.RowId == (byte)OnlineStatusType.Afk)
                {
                    continue;
                }

                players.Add(new(player));
            }

            // Filter the list alphabetically and prioritize known players if necessary.
            // We sort alphabetically so the list doesn't jump around when players move.
            if (prioritizeKnownPlayers)
            {
                return [.. players.OrderByDescending(p => p.IsKnownPlayer).ThenBy(p => p.Name)];
            }
            return [.. players.OrderBy(p => p.Name)];
        }

        /// <summary>
        ///     Checks if the given <see cref="PlayerCharacter" /> is in the party from its ObjectId.
        /// </summary>
        /// <param name="playerCharacter">The <see cref="PlayerCharacter" /> to check.</param>
        /// <returns>True if the <see cref="PlayerCharacter" /> is in the party, otherwise false.</returns>
        internal static bool IsPlayerInParty(IPlayerCharacter playerCharacter)
        {
            foreach (var member in Services.PartyList)
            {
                if (member.EntityId == playerCharacter.EntityId)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        ///     Gets the colour for the given player.
        /// </summary>
        /// <param name="playerInfo"></param>
        /// <returns>The colour for the player.</returns>
        internal static Vector4 GetPlayerNameColour(PlayerInfoSlim playerInfo)
        {
            if (playerInfo.IsInParty)
            {
                return Services.Configuration.Colours.Name.Party;
            }
            if (playerInfo.IsFriend)
            {
                return Services.Configuration.Colours.Name.Friend;
            }
            if (playerInfo.IsSyncServicePair)
            {
                return Services.Configuration.Colours.Name.SyncServicePaired;
            }
            return Services.Configuration.Colours.Name.Default;
        }
    }
}
