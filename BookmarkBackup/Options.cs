using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using CommandLine.Text;
using System.IO;

namespace BookmarkBackup
{
    class Options
    {
        [Option('u', "user", Required = true, HelpText = "The user who's bookmarks need to be backed up")]
        public string User { get; set; }

        [Option('c', "chrome", HelpText = "Only copies bookmarks from Google Chrome")]
        public bool Chrome { get; set; }

        [Option('f', "firefox", HelpText = "Only copies bookmarks from FireFox")]
        public bool FireFox { get; set; }

        [Option('a', "all", DefaultValue = true, HelpText = "Copies bookmarks from both FireFox and Chrome")]
        public bool All { get; set; }

        [Option('d', "drive", DefaultValue = "Current location", HelpText = "Drive in which to save the bookmarks")]
        public string Drive { get; set; }

        [VerbOption("restore", HelpText = "Restore the bookmarks. If you saved the bookmarks in a non-default drive, please specifiy the drive to retrive the bookmarks from")]
        public bool Restore { get; set; }

        [Option('m', "multiple", HelpText = "Use if you'd like to backup bookmarks from multiple users")]
        public bool Multiple { get; set; }
        
        [ParserState]
        public IParserState LastParserState { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            Console.Write(HelpText.AutoBuild(this,
              (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current)));
            Console.ReadLine();
            return HelpText.AutoBuild(this,
              (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }
}