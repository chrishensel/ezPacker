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

using System.Collections.ObjectModel;
using System.IO;
using ezPacker.Core;

namespace ezPacker.Collector
{
    class DefaultFileCollector : Collection<FileInfo>, IFileCollector
    {
        #region IFileCollector Members

        bool IFileCollector.Replace(IFileContext context, string file, FileInfo replacement, FileNameMode comparisonMode)
        {
            bool found = false;

            for (int i = 0; i < this.Items.Count; i++)
            {
                if (FileReference.Equals(context, this.Items[i], file, string.Empty, comparisonMode))
                {
                    this.Items[i] = replacement;
                    found = true;
                }
            }

            return found;
        }

        #endregion
    }
}
