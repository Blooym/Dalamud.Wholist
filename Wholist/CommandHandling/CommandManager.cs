using System;
using Wholist.CommandHandling.Commands;
using Wholist.CommandHandling.Interfaces;
using Wholist.Common;

namespace Wholist.CommandHandling
{
    internal sealed class CommandManager : IDisposable
    {
        private bool disposedValue;

        /// <summary>
        ///     All  commands to register with the <see cref="Dalamud.Game.Command.CommandManager" />, holds all references.
        /// </summary>
        private IDalamudCommand[] commands = { new WhoCommand(), new WhoSettingsCommand() };

        /// <summary>
        ///     Initializes a new instance of the <see cref="CommandManager" /> class.
        /// </summary>
        private CommandManager()
        {
            foreach (var command in this.commands)
            {
                Services.Commands.AddHandler(command.Name, command.Command);
            }
        }

        /// <summary>
        ///     Disposes of the command manager.
        /// </summary>
        public void Dispose()
        {
            if (this.disposedValue)
            {
                return;
            }

            foreach (var command in this.commands)
            {
                Services.Commands.RemoveHandler(command.Name);
            }
            this.commands = Array.Empty<IDalamudCommand>();

            this.disposedValue = true;
        }
    }
}
