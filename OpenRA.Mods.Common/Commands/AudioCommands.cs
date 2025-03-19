#region Copyright & License Information
/*
 * Copyright (c) The OpenRA Developers and Contributors
 * This file is part of OpenRA, which is free software. It is made
 * available to you under the terms of the GNU General Public License
 * as published by the Free Software Foundation, either version 3 of
 * the License, or (at your option) any later version. For more
 * information, see COPYING.
 */
#endregion

using System.Collections.Generic;
using System.Linq;
using OpenRA.Graphics;
using OpenRA.Traits;

namespace OpenRA.Mods.Common.Commands
{
	[TraitLocation(SystemActors.World)]
	[Desc("Shows the voice helper in the chatbox. Attach this to the world actor.")]
	public class AudioCommandsInfo : TraitInfo<AudioCommands> { }

	public class AudioCommands : IChatCommand, IWorldLoaded
	{
		[FluentReference]
		const string AvailableCommands = "notification-available-commands";

		[FluentReference]
		const string NoDescription = "description-no-description";

		[FluentReference]
		const string HelpDescription = "description-voice-description";

		readonly Dictionary<string, string> helpDescriptions;

		World world;
		ChatCommands console;

		public AudioCommands()
		{
			helpDescriptions = new Dictionary<string, string>();
		}

		public void WorldLoaded(World w, WorldRenderer wr)
		{
			world = w;
			console = world.WorldActor.Trait<ChatCommands>();

			console.RegisterCommand("voice", this);
			RegisterVoice("voice", HelpDescription);
		}

		public void InvokeCommand(string name, string arg)
		{
			TextNotificationsManager.Debug(FluentProvider.GetMessage(AvailableCommands));

			foreach (var key in console.Commands.Keys.OrderBy(k => k))
			{
				if (!helpDescriptions.TryGetValue(key, out var description))
					description = FluentProvider.GetMessage(NoDescription);

				TextNotificationsManager.Debug($"{key}: {description}");
			}
		}

		public void RegisterVoice(string name, string description)
		{
			helpDescriptions[name] = FluentProvider.GetMessage(description);
		}
	}
}
