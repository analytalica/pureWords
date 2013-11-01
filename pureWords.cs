//Import various C# things.
using System;
using System.IO;
using System.Text;
using System.Reflection;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;

//Import Procon things.
using PRoCon.Core;
using PRoCon.Core.Plugin;
using PRoCon.Core.Players;

namespace PRoConEvents
{
    public class pureWords : PRoConPluginAPI, IPRoConPluginInterface
    {

        //--------------------------------------
        //Class level variables.
        //--------------------------------------

        private bool pluginEnabled = false;
        private String someString = "string";

        private String kickMessage = "";

        private String debugLevelString = "1";
        private int debugLevel = 1;

        //--------------------------------------
        //Plugin constructor. Can be left blank.
        //--------------------------------------

        public pureWords()
        {

        }

        //--------------------------------------
        //Description settings for your plugin.
        //--------------------------------------

        public string GetPluginName()
        {
            return "pureWords";
        }

        public string GetPluginVersion()
        {
            return "0.0.1";
        }

        public string GetPluginAuthor()
        {
            return "Analytalica";
        }

        public string GetPluginWebsite()
        {
            return "purebattlefield.org";
        }

        public string GetPluginDescription()
        {
            return @"pureWords is a word filter plugin that monitors server chat.";
        }

        //--------------------------------------
        //Helper Functions
        //--------------------------------------

        public void toChat(String message)
        {
            this.ExecuteCommand("procon.protected.send", "admin.say", message, "all");
        }

        public void toConsole(int msgLevel, String message)
        {
            //a message with msgLevel 1 is more important than 2
            if (debugLevel >= msgLevel)
            {
            this.ExecuteCommand("procon.protected.pluginconsole.write", message);
            }
        }

        public void kickPlayer(String playerName, String badWord)
        {
            this.ExecuteCommand("procon.protected.send", "admin.kickPlayer", playerName, this.kickMessage);
        }

        //--------------------------------------
        //These methods run when Procon does what's on the label.
        //--------------------------------------

        //Runs when the plugin is compiled.

        public void OnPluginLoaded(string strHostName, string strPort, string strPRoConVersion)
        {
            // Depending on your plugin you will need different types of events. See other plugins for examples.
            this.RegisterEvents(this.GetType().Name, "OnPluginLoaded", "OnGlobalChat", "OnTeamChat", "OnSquadChat");
        }

        public override void OnGlobalChat(string speaker, string message)
        {
        }

        public override void OnTeamChat(string speaker, string message, int teamId)
        {
        }

        public override void OnSquadChat(string speaker, string message, int teamId, int squadId)
        {
        }

        //Runs when the plugin is enabled (checked in Procon)
        //Note that this also runs right after a server restart if it was enabled before.

        public void OnPluginEnable()
        {
            //Use a variable like this one to turn on and off your plugin.
            this.pluginEnabled = true;

            //Say something in the console.
            this.toConsole(1, "pureWords Enabled!");
        }

        //Runs when the pluginh is disabled (unchecked in Procon)

        public void OnPluginDisable()
        {
            //Use a variable like this one to turn on and off your plugin.
            this.pluginEnabled = false;

            //Say something in the console.
            this.toConsole(1, "pureWords Disabled!");
        }

        //List plugin variables.
        public List<CPluginVariable> GetDisplayPluginVariables()
        {
            List<CPluginVariable> lstReturn = new List<CPluginVariable>();
            lstReturn.Add(new CPluginVariable("Settings|Some String", typeof(string), someString));
            lstReturn.Add(new CPluginVariable("Settings|Kick Message", typeof(string), kickMessage));
            lstReturn.Add(new CPluginVariable("Other|Debug Level", typeof(string), debugLevelString));
            return lstReturn;
        }

        public List<CPluginVariable> GetPluginVariables()
        {
            return GetDisplayPluginVariables();
        }

        //Set variables.
        public void SetPluginVariable(String strVariable, String strValue)
        {
            if (Regex.Match(strVariable, @"Some String").Success)
            {
                someString = strValue;
            }
            else if (Regex.Match(strVariable, @"Kick Message").Success)
            {
                kickMessage = strValue;
            }
            else if (Regex.Match(strVariable, @"Debug Level").Success)
            {
                debugLevelString = strValue;
                try
                {
                    debugLevel = Int32.Parse(debugLevelString);
                }
                catch (Exception z)
                {
                    toConsole(1, "Invalid debug level! Choose 0, 1, or 2 only.");
                    debugLevel = 1;
                    debugLevelString = "1";
                }
            }
        }

    }
}