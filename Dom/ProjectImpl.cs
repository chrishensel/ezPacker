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

using System.Collections.Generic;
using System.IO;

namespace ezPacker.Dom
{
    class ProjectImpl : IProject
    {
        #region IProject Members

        public string Name { get; set; }
        public DirectoryInfo BasePath { get; set; }
        public DirectoryInfo OutPath { get; set; }
        public string PackedName { get; set; }

        public bool IsRecursiveMode { get; set; }
        public bool IncludeAllByDefault { get; set; }
        public IReadOnlyList<Include> Inclusions { get; set; }
        public IReadOnlyList<Exclude> Exclusions { get; set; }
        public IReadOnlyList<Replacement> Replacements { get; set; }

        #endregion
    }
}
