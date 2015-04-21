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
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml.Linq;
using ezPacker.Core;
using ezPacker.Dom;

namespace ezPacker.Project
{
    class XmlProjectParser : IProjectParser
    {
        #region IProjectParser Members

        IProject IProjectParser.Parse(Stream stream)
        {
            XDocument doc = XDocument.Load(stream);
            XElement root = doc.Root;

            ProjectImpl project = new ProjectImpl();
            project.Name = root.Element("name").Value;
            project.BasePath = GetDirectoryInfo(root.Element("basePath").Value);
            project.PackedName = root.Element("packedName").Value;
            project.IsRecursiveMode = bool.Parse(root.Element("basePath").Attribute("recursive").Value);
            project.IncludeAllByDefault = bool.Parse(root.Element("inclusions").Attribute("all").Value);

            ParseExclusions(root, project);
            ParseInclusions(root, project);
            ParseReplacements(root, project);

            return project;
        }

        private static void ParseExclusions(XElement root, ProjectImpl project)
        {
            List<Exclude> exclusions = new List<Exclude>();
            foreach (XElement item in root.Element("exclusions").Elements("exclude"))
            {
                XAttribute tmp = null;

                Exclude v = new Exclude();

                if ((tmp = item.Attribute("extension")) != null)
                {
                    v.Extension = tmp.Value;
                }
                if ((tmp = item.Attribute("file")) != null)
                {
                    v.FileName = tmp.Value;
                }
                if ((tmp = item.Attribute("mode")) != null)
                {
                    v.Mode = (FileNameMode)Enum.Parse(typeof(FileNameMode), tmp.Value, true);
                }

                exclusions.Add(v);
            }
            project.Exclusions = exclusions;
        }

        private static void ParseInclusions(XElement root, ProjectImpl project)
        {
            List<Include> inclusions = new List<Include>();
            foreach (XElement item in root.Element("inclusions").Elements("include"))
            {
                XAttribute tmp = null;

                Include v = new Include();

                if ((tmp = item.Attribute("extension")) != null)
                {
                    v.Extension = tmp.Value;
                }
                if ((tmp = item.Attribute("file")) != null)
                {
                    v.FileName = tmp.Value;
                }
                if ((tmp = item.Attribute("force")) != null)
                {
                    v.ForceInclude = bool.Parse(tmp.Value);
                }
                if ((tmp = item.Attribute("mode")) != null)
                {
                    v.Mode = (FileNameMode)Enum.Parse(typeof(FileNameMode), tmp.Value, true);
                }

                inclusions.Add(v);
            }
            project.Inclusions = inclusions;
        }

        private static void ParseReplacements(XElement root, ProjectImpl project)
        {
            List<Replacement> replacements = new List<Replacement>();

            XElement node = root.Element("replacements");
            if (node != null)
            {
                foreach (XElement item in node.Elements("replace"))
                {
                    XAttribute tmp = null;

                    Replacement v = new Replacement();

                    if ((tmp = item.Attribute("file")) != null)
                    {
                        v.FileName = tmp.Value;
                    }
                    if ((tmp = item.Attribute("with")) != null)
                    {
                        v.ReplacementFile = new FileInfo(Path.Combine(project.BasePath.FullName, tmp.Value));
                    }
                    if ((tmp = item.Attribute("mode")) != null)
                    {
                        v.Mode = (FileNameMode)Enum.Parse(typeof(FileNameMode), tmp.Value, true);
                    }

                    replacements.Add(v);
                }
            }

            project.Replacements = replacements;
        }


        private DirectoryInfo GetDirectoryInfo(string path)
        {
            if (!Path.IsPathRooted(path))
            {
                path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), path);
            }

            return new DirectoryInfo(path);
        }

        #endregion
    }
}