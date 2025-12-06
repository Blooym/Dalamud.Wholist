using System;
using System.Collections.Generic;
using Dalamud.Plugin.Ipc;
using Wholist.Common;

namespace Wholist.Interop
{
    internal sealed class IpcManager
    {
        private IpcManager()
        {

        }

        // -- LightlessSync --
        // Active Pairs
        private readonly ICallGateSubscriber<List<nint>> lightlessActivePairsCallGateSubscriber = Services.PluginInterface.GetIpcSubscriber<List<nint>>("LightlessSync.GetHandledAddresses");
        private static readonly TimeSpan LightlessActivePairsCacheDuration = TimeSpan.FromSeconds(5);
        private DateTime lightlessActivePairsLastUpdateTime = DateTime.MinValue;
        private readonly HashSet<nint> lightlessActivePairs = [];

        public bool LightlessActivePairsIpcAvailable => this.lightlessActivePairsCallGateSubscriber.HasFunction;
        public HashSet<nint> LightlessActivePairs
        {
            get
            {
                try
                {
                    if (!this.LightlessActivePairsIpcAvailable)
                    {
                        return [];
                    }

                    if ((DateTime.UtcNow - this.lightlessActivePairsLastUpdateTime) > LightlessActivePairsCacheDuration)
                    {
                        this.lightlessActivePairs.Clear();
                        var pairs = this.lightlessActivePairsCallGateSubscriber.InvokeFunc();
                        this.lightlessActivePairs.UnionWith(pairs);
                        this.lightlessActivePairsLastUpdateTime = DateTime.UtcNow;
                    }

                    return this.lightlessActivePairs;
                }
                catch
                {
                    return [];
                }
            }
        }
    }
}
