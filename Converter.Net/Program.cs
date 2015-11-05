//	Copyright © 2015 Alexander Isakov. All rights reserved.
//  This file is a part of Converter.NET and is licensed under MS-PL.
//  See LICENSE for details or visit http://opensource.org/licenses/MS-PL.
//	----------------------------------------------------------------------

using System.Linq;
using CommandShell;
using CommandShell.Helpers;
using CommandShell.Infrastucture;
using Converter.Net.Commands;

namespace Converter.Net
{
    class Program
    {
        static int Main(string[] args)
        {
            if (args.Any() && args[0] != "/?") return Shell.RunCommand(new ConvertCommand(), args);
            HelpBuilder.Default.PrintCommandHelp(Shell.Output, CommandsResolver.GetCommandMetadata(typeof(ConvertCommand)));
            return 0;
        }
    }
}
