using System;
using System.Collections.Generic;
using Dalamud.Plugin.Ipc;
using Wholist.Common;

namespace Wholist.Interop
{
    internal sealed class IpcManager
    {
        // -- LightlessSync --
        // Active Pairs
        private readonly ICallGateSubscriber<List<nint>> lightlessActivePairsCallGateSubscriber = Services.PluginInterface.GetIpcSubscriber<List<nint>>("LightlessSync.GetHandledAddresses");
        private readonly TimeSpan lightlessActivePairsCacheDuration = TimeSpan.FromSeconds(5);
        private DateTime lightlessActivePairsLastUpdateTime = DateTime.MinValue;
        private readonly HashSet<nint> lightlessActivePairs = [];
        public bool LightlessActivePairsIpcAvailable => this.lightlessActivePairsCallGateSubscriber.HasFunction;
        public HashSet<nint> LightlessActivePairs
        {
            get
            {
                if (!this.LightlessActivePairsIpcAvailable)
                {
                    return [];
                }
                else if ((DateTime.UtcNow - this.lightlessActivePairsLastUpdateTime) > this.lightlessActivePairsCacheDuration)
                {
                    this.lightlessActivePairs.Clear();
                    foreach (var item in this.lightlessActivePairsCallGateSubscriber.InvokeFunc())
                    {
                        this.lightlessActivePairs.Add(item);
                    }
                    this.lightlessActivePairsLastUpdateTime = DateTime.UtcNow;
                }
                return this.lightlessActivePairs;
            }
        }

        private IpcManager()
        {

        }
    }
}
