// ezPacker
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
using System.Reflection;
using ezPacker.Dom;

namespace ezPacker.Project
{
    class DefaultProjectContext : IProjectContext
    {
        #region Constants

        private const string PlaceRelativeToProjectFileIndicator = @"$\";

        #endregion

        #region Fields

        private FileInfo _projectFile;

        #endregion

        #region Constructors

        internal DefaultProjectContext(FileInfo projectFile)
        {
            if (projectFile == null)
            {
                throw new ArgumentNullException("projectFile");
            }

            _projectFile = projectFile;
        }

        #endregion

        #region IProjectContext Members

        DirectoryInfo IProjectContext.GetPhysicalDirectory(string path)
        {
            if (path.StartsWith(PlaceRelativeToProjectFileIndicator))
            {
                path = path.Replace(PlaceRelativeToProjectFileIndicator, _projectFile.Directory.FullName);
            }

            if (!Path.IsPathRooted(path))
            {
                path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), path);
            }

            return new DirectoryInfo(path);
        }

        FileInfo IProjectContext.GetPhysicalFile(string path, IProject project)
        {
            if (path.StartsWith(PlaceRelativeToProjectFileIndicator))
            {
                path = path.Replace(PlaceRelativeToProjectFileIndicator, _projectFile.Directory.FullName + "\\");
            }

            if (!Path.IsPathRooted(path))
            {
                path = Path.Combine(project.BasePath.FullName, path);
            }

            return new FileInfo(path);
        }

        #endregion
    }
}
