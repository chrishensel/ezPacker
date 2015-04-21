﻿// ezPacker
// Copyright (C) 2015 Sascha-Christian Hensel
// 
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License along
// with this program; if not, write to the Free Software Foundation, Inc.,
// 51 Franklin Street, Fifth Floor, Boston, MA 02110-1301 USA.

using System;
using System.IO;
using ezPacker.Collector;
using ezPacker.Dom;
using ezPacker.Packer;
using ezPacker.Project;

namespace ezPacker
{
    class Program
    {
        static void Main(string[] args)
        {
            WriteInfo("".PadRight(79, '*'));
            WriteInfo("ezPacker, Copyright (C) 2015 Sascha-Christian Hensel");
            WriteInfo("ezPacker comes with ABSOLUTELY NO WARRANTY.");
            WriteInfo("This is free software, and you are welcome to redistribute it");
            WriteInfo("under certain conditions; see LICENSE.");
            WriteInfo("".PadRight(79, '*'));
            WriteInfo("");

            if (args.Length != 1)
            {
                WriteError("No configuration file given!");
            }
            else
            {
                string path = args[0];

                WriteInfo("Trying to load configuration file '{0}'...", path);

                IProject project = null;

                IProjectParser parser = new XmlProjectParser();
                using (FileStream stream = File.OpenRead(path))
                {
                    project = parser.Parse(stream);
                }

                IMatcherContext context = new MatcherContext() { Project = project };

                IFileCollector collector = new DefaultFileCollector();
                foreach (FileInfo item in Matcher.GetMatches(context))
                {
                    collector.Add(item);
                }

                WriteInfo("Success! Found  '{0}' possible file(s) to include for packing.", collector.Count);

                if (project.Replacements.Count == 0)
                {
                    WriteInfo("No files need to replaced.");
                }
                else
                {
                    WriteInfo("Trying to replace '{0}' file(s)...", project.Replacements.Count);

                    foreach (Replacement replacement in project.Replacements)
                    {
                        WriteInfo("  Attempt to replace file '{0}'...", replacement.FileName);
                        if (!replacement.Exists())
                        {
                            WriteWarning("  ... failed: replacement file '{0}' does not exist!", replacement.ReplacementFile.FullName);
                            continue;
                        }

                        bool replaced = collector.Replace(context, replacement.FileName, replacement.ReplacementFile, replacement.Mode);
                        if (replaced)
                        {
                            WriteInfo("  ... succeeded!");
                        }
                        else
                        {
                            WriteWarning("  ... failed: file to be replaced wasn't found in list of files to be packed.");
                        }
                    }

                    WriteInfo("Replacement procedure completed.");
                }

                WriteInfo("Begin packing...");

                string outputFilePath = Path.Combine(project.BasePath.FullName, project.PackedName + ".zip");

                using (IPacker packer = new ZipPacker())
                {
                    foreach (FileInfo file in collector)
                    {
                        if (file.FullName == outputFilePath)
                        {
                            continue;
                        }

                        string archiveFileName = context.GetRelativePath(file.FullName);

                        packer.Add(archiveFileName, file.OpenRead());

                        WriteInfo("  Added '{0}'.", archiveFileName);
                    }

                    WriteInfo("Writing archive to '{0}'...", outputFilePath);

                    using (FileStream output = new FileStream(outputFilePath, FileMode.Create, FileAccess.Write, FileShare.Read))
                    {
                        packer.Save(output);
                    }

                    WriteInfo("... success!");
                }
            }

            WriteInfo("");

            if (Properties.Settings.Default.InteractiveMode)
            {
                WriteInfo("");
                WriteInfo("Press any key to quit . . .");
                Console.ReadKey();
            }
        }

        static void WriteInfo(string format, params object[] args)
        {
            Console.ResetColor();
            Console.WriteLine(format, args);
        }

        static void WriteWarning(string format, params object[] args)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(format, args);
        }

        static void WriteError(string format, params object[] args)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(format, args);
        }
    }
}