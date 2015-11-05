//	Copyright © 2015 Alexander Isakov. All rights reserved.
//  This file is a part of Converter.NET and is licensed under MS-PL.
//  See LICENSE for details or visit http://opensource.org/licenses/MS-PL.
//	----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CommandShell;
using CommandShell.Metadata;


namespace Converter.Net.Commands
{
    [ShellCommand("convert", Description = "Converts C#/VB.NET code snippets.", Options = typeof(ConvertCommand))]
    public sealed class ConvertCommand
    {
        #region Options

        [ValueList("Files to be converted.")]
        public IList<string> Files { get; set; }

        [Switch('v', "verbose", "Provides detailed messages.")]
        public bool Verbosity { get; set; }

        #endregion

        #region Methods

        public int Run()
        {
            try
            {
                if (Files == null || !Files.Any()) return 0;
                foreach (var file in Files)
                {
                    switch (DefineFileType(file))
                    {
                        case FileType.CS:
                            ProcessCS(file);
                            break;
                        case FileType.VBNET:
                            ProcessVBNET(file);
                            break;
                        default:
                            WriteLine("Unknown file extension: {0}", file);
                            break;
                    }
                }
                return 0;
            }
            catch (Exception error)
            {
                // internal error, see windows events log
                WriteError(1, "Unexpected error occurred ({0}).", error.Message);
                return 1;
            }
        }

        #endregion

        #region Helpers

        private static FileType DefineFileType(string fileName)
        {
            if (fileName.EndsWith(".cs"))
                return FileType.CS;
            return fileName.EndsWith(".vb") ? FileType.VBNET : FileType.Unknown;
        }

        private void ProcessCS(string fileName)
        {
            if (!File.Exists(fileName))
            {
                WriteLine("File doesn't exist: {0}", fileName);
                return;
            }
            File.WriteAllText(Path.ChangeExtension(fileName, "vb"), Converter.ConvertCS2VBNET(File.ReadAllText(fileName)));
        }

        private void ProcessVBNET(string fileName)
        {
            if (!File.Exists(fileName))
            {
                WriteLine("File doesn't exist: {0}", fileName);
                return;
            }
            File.WriteAllText(Path.ChangeExtension(fileName, "cs"), Converter.ConvertVBNET2CS(File.ReadAllText(fileName)));
        }

        private void WriteLine(string format, params object[] parameters)
        {
            if (Verbosity) Shell.Output.WriteLine(format, parameters);
        }

        private static void WriteError(int code, string format, params object[] parameters)
        {
            var prefix = "convert : error " + code.ToString("D2") + " : ";
            Shell.Error.WriteLine(prefix + format, parameters);
        }

        #endregion
    }
}
