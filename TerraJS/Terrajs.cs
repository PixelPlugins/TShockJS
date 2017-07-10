using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TShockAPI;
using Terraria;
using TerrariaApi.Server;
using Jint;
using Microsoft.Xna.Framework;
using System.IO;

namespace Terrajs
{
    [ApiVersion(2, 0)]
    public class Terrajs : TerrariaPlugin
    {
        Engine e;

        /// <summary>
        /// Gets the author(s) of this plugin
        /// </summary>
        public override string Author
        {
            get { return "Anonymous"; }
        }

        /// <summary>
        /// Gets the description of this plugin.
        /// A short, one lined description that tells people what your plugin does.
        /// </summary>
        public override string Description
        {
            get { return "A sample test plugin"; }
        }

        /// <summary>
        /// Gets the name of this plugin.
        /// </summary>
        public override string Name
        {
            get { return "Test Plugin"; }
        }

        /// <summary>
        /// Gets the version of this plugin.
        /// </summary>
        public override Version Version
        {
            get { return new Version(1, 0, 0, 0); }
        }

        /// <summary>
        /// Initializes a new instance of the TestPlugin class.
        /// This is where you set the plugin's order and perfrom other constructor logic
        /// </summary>
        public Terrajs(Main game) : base(game)
        {

        }

        /// <summary>
        /// Handles plugin initialization. 
        /// Fired when the server is started and the plugin is being loaded.
        /// You may register hooks, perform loading procedures etc here.
        /// </summary>
        public override void Initialize()
        {
            e = new Engine();

            e.SetValue("log", new Action<object>(Console.WriteLine));

            e.SetValue("mkCmd", new Action<string, string>(mkCmd));

            e.SetValue("tellPlayer", new Action<string, string>(tellPlr));

            e.SetValue("killPlayer", new Action<string>(killPlr));

            if (!Directory.Exists("jsplugins"))
            {
                Directory.CreateDirectory("jsplugins");
            }

            foreach(string entry in Directory.GetFiles("jsplugins"))
            {
                Console.WriteLine("Loading " + entry + "...");
                e.Execute(File.ReadAllText(entry));
            }
        }

        void killPlr(string plr)
        {
            foreach(TSPlayer p in TShock.Players)
            {
                if(p.Name == plr)
                {
                    p.KillPlayer();
                }
            }
        }

        void tellPlr(string plr, string msg)
        {
            foreach(TSPlayer p in TShock.Players)
            {
                if(p.Name == plr)
                {
                    p.SendMessage(msg, Color.Yellow);
                }
            }
        }

        void mkCmd(string cmdname, string callback)
        {
            Console.WriteLine("makingcmd");
            Commands.ChatCommands.Add(new Command("terrajs.run", (CommandArgs args) => { e.Execute(callback + "({player: \"" + args.Player.Name + "\"})");}, cmdname));
        }

        /// <summary>
        /// Handles plugin disposal logic.
        /// *Supposed* to fire when the server shuts down.
        /// You should deregister hooks and free all resources here.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Deregister hooks here
            }
            base.Dispose(disposing);
        }
    }
}