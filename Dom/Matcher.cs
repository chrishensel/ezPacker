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

using System.Collections.Generic;
using System.IO;

namespace ezPacker.Dom
{
    static class Matcher
    {
        internal static IEnumerable<FileInfo> GetMatches(IMatcherContext context)
        {
            foreach (var file in context.Project.BasePath.GetFiles("*.*", SearchOption.AllDirectories))
            {
                MatchResult fileResult = MatchResult.Inconclusive;

                if (context.Project.IncludeAllByDefault)
                {
                    fileResult = MatchResult.Include;
                }

                foreach (IMatcher exc in context.Project.Exclusions)
                {
                    MatchResult result = exc.GetMatchResultFor(context, file);

                    if (result == MatchResult.Exclude)
                    {
                        fileResult = result;
                        break;
                    }
                }

                foreach (IMatcher inc in context.Project.Inclusions)
                {
                    MatchResult result = inc.GetMatchResultFor(context, file);

                    if (fileResult == MatchResult.Exclude && result == MatchResult.IncludeForce)
                    {
                        fileResult = MatchResult.Include;
                        break;
                    }
                    else if (result == MatchResult.Include)
                    {
                        fileResult = result;
                        break;
                    }
                }

                if (fileResult == MatchResult.Include)
                {
                    yield return file;
                }
            }
        }
    }
}
