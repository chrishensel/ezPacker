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

using System.IO;

namespace ezPacker.Core
{
    abstract class FileReference
    {
        public string FileName { get; set; }
        public string Extension { get; set; }
        public FileNameMode Mode { get; set; }

        public bool Equals(IFileContext context, FileInfo file)
        {
            return Equals(context, file, this.FileName, this.Extension, this.Mode);
        }

        public static bool Equals(IFileContext context, FileInfo file, string otherFileName, string otherFileExtension, FileNameMode comparisonMode)
        {
            if (!string.IsNullOrWhiteSpace(otherFileExtension))
            {
                if (!string.Equals(file.Extension, "." + otherFileExtension))
                {
                    return false;
                }
            }

            if (!string.IsNullOrWhiteSpace(otherFileName))
            {
                switch (comparisonMode)
                {
                    case FileNameMode.Default:
                        {
                            if (!string.Equals(file.Name, otherFileName))
                            {
                                return false;
                            }
                        } break;
                    case FileNameMode.NonLiteral:
                        {
                            if (file.Name.IndexOf(otherFileName, System.StringComparison.OrdinalIgnoreCase) == -1)
                            {
                                return false;
                            }
                        } break;
                    case FileNameMode.NonLiteralIncludePath:
                        {
                            if (context.GetRelativePath(file.FullName).IndexOf(otherFileName, System.StringComparison.OrdinalIgnoreCase) == -1)
                            {
                                return false;
                            }
                        } break;
                }
            }

            return true;
        }
    }
}
