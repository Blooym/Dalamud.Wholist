using System;
using System.Collections.Generic;
using System.Numerics;
using Dalamud.Game.ClientState.Objects.SubKinds;
using FFXIVClientStructs.FFXIV.Client.Game.Character;
using FFXIVClientStructs.FFXIV.Client.Game.Control;
using FFXIVClientStructs.FFXIV.Client.Game.Object;
using FFXIVClientStructs.FFXIV.Client.UI.Agent;
using Wholist.Common;
using Wholist.Game;

namespace Wholist.DataStructures
{
    /// <summary>
    ///     Represents a pre-formatted version of a <see cref="PlayerCharacter" /> with slimmed down information.
    /// </summary>
    internal readonly struct PlayerInfoSlim(IPlayerCharacter basePlayer)
    {
        /// <summary>
        ///     The name of the player.
        /// </summary>
        internal readonly string Name => basePlayer.Name.TextValue;

        /// <summary>
        ///     The level of the player.
        /// </summary>
        internal readonly unsafe byte Level
        {
            get
            {
                var forayLevel = ((Character*)basePlayer.Address)->GetForayInfo()->Level;
                return forayLevel is 0 ? basePlayer.Level : forayLevel;
            }
        }

        /// <summary>
        ///     The company tag of the player.
        /// </summary>
        internal readonly unsafe string CompanyTag => ((Character*)basePlayer.Address)->FreeCompanyTagString;

        /// <summary>
        ///     The HomeWorld of the player.
        /// </summary>
        internal readonly string HomeWorld => Services.WorldNames.GetValueOrDefault(basePlayer.HomeWorld.RowId)!;

        /// <summary>
        ///     The distance of the player from the local player.
        /// </summary>
        internal readonly unsafe double Distance
        {
            get
            {
                var localPlayer = Services.ObjectTable.LocalPlayer;
                if (localPlayer == null)
                {
                    return 0;
                }
                return Math.Round((((GameObject*)localPlayer.Address)->Position - this.Position).Magnitude, MidpointRounding.ToEven);
            }
        }

        /// <summary>
        ///     The colour of the player's name.
        /// </summary>
        internal readonly Vector4 NameColour => PlayerManager.GetPlayerNameColour(this);

        /// <summary>
        ///     The job information of the player.
        /// </summary>
        internal readonly JobInfoSlim Job => new(basePlayer.ClassJob.RowId);

        /// <summary>
        ///     Whether the player is a friend of the local player.
        /// </summary>
        internal readonly unsafe bool IsFriend => ((Character*)basePlayer.Address)->IsFriend;

        /// <summary>
        ///     Whether the player is in the local player's party.
        /// </summary>
        internal readonly unsafe bool IsInParty => ((Character*)basePlayer.Address)->IsPartyMember;

        /// <summary>
        ///     Whether the player is paired and actively shown via a Mod Sync Service.
        /// </summary>
        internal readonly bool IsSyncServicePair => Services.IpcManager.LightlessActivePairsIpcAvailable && Services.IpcManager.LightlessActivePairs.Contains(basePlayer.Address);

        /// <summary>
        ///     Whether the player is known to the local player (i.e. in party or friend).
        /// </summary>
        internal readonly bool IsKnownPlayer => this.IsInParty || this.IsFriend || this.IsSyncServicePair;

        /// <summary>
        ///     The pointer for the character data.
        /// </summary>
        internal readonly nint CharacterPtr => basePlayer.Address;

        /// <summary>
        ///     The location of the player.
        /// </summary>
        internal readonly FFXIVClientStructs.FFXIV.Common.Math.Vector3 Position => basePlayer.Position;

        /// <summary>
        ///     Targets the player.
        /// </summary>
        internal readonly unsafe void Target() => TargetSystem.Instance()->Target = (GameObject*)basePlayer.Address;

        /// <summary>
        ///     Focus targets the player.
        /// </summary>
        internal readonly unsafe void FocusTarget() => TargetSystem.Instance()->FocusTarget = (GameObject*)basePlayer.Address;

        /// <summary>
        ///     Opens the examine window for the player.
        /// </summary>
        internal readonly unsafe void OpenExamine() => AgentInspect.Instance()->ExamineCharacter(basePlayer.EntityId);

        /// <summary>
        ///     Opens the character card for the player.
        /// </summary>
        internal readonly unsafe void OpenCharaCard() => AgentCharaCard.Instance()->OpenCharaCard((GameObject*)basePlayer.Address);
    }
}
