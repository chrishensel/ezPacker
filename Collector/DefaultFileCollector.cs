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
using System.Collections.Generic;
using System.IO;
using ezPacker.Core;

namespace ezPacker.Collector
{
    class DefaultFileCollector : IFileCollector
    {
        #region Fields

        private Dictionary<string, FileInfo[]> _files;

        #endregion

        #region Constructors

        internal DefaultFileCollector()
        {
            _files = new Dictionary<string, FileInfo[]>();
        }

        #endregion

        #region IFileCollector Members

        int IFileCollector.Count
        {
            get { return _files.Count; }
        }

        void IFileCollector.Add(FileInfo file)
        {
            _files.Add(file.FullName, new[] { file, file });
        }

        bool IFileCollector.Replace(IFileContext context, string file, FileInfo replacement, FileNameMode comparisonMode)
        {
            bool found = false;

            foreach (var item in _files)
            {
                if (FileReference.Equals(context, item.Value[0], file, string.Empty, comparisonMode))
                {
                    item.Value[1] = replacement;
                    found = true;
                }
            }

            return found;
        }

        IEnumerable<Tuple<FileInfo, FileInfo>> IFileCollector.GetAll()
        {
            foreach (var item in _files.Values)
            {
                yield return Tuple.Create(item[0], item[1]);
            }
        }

        #endregion
    }
}
