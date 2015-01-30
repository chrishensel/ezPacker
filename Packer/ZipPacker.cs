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
using Ionic.Zip;

namespace ezPacker.Packer
{
    class ZipPacker : IPacker
    {
        #region Fields

        private ZipFile _zip;

        #endregion

        #region Constructors

        public ZipPacker()
        {
            _zip = new ZipFile();
        }

        #endregion

        #region IPacker Members

        void IPacker.Add(string archiveFileName, Stream stream)
        {
            _zip.AddEntry(archiveFileName, stream);
        }

        void IPacker.Save(Stream destination)
        {
            _zip.Save(destination);
        }

        #endregion

        #region IDisposable Members

        void IDisposable.Dispose()
        {

        }

        #endregion
    }
}
