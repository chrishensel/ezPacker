# ezPacker
Simple-to-use-program to create packages from build outputs

# Introduction
I had a relatively simple use-case, which is very tedious to do manually:

> Grab the build output - but not all files - and pack them in a ZIP archive.

I didn't want to write batch scripts, since for this use-case it was just too much, the error handling is annoying to do and quite frankly, I simply shouldn't have to do that. Also I didn't want to rely on (proprietary) tools, be dependent on TFSBuild or write complicated MSBuild-target files for this.

So I wrote this little tool, which can do the following...

# Features

- Specify a "base directory" in which to look for files to include
- Allow to scan either only the base directory or include subdirectories
- Allow for inclusion and exclusion of files by their pattern (for example, ignore all .pdb files)
- Allow for manually including particular excluded files (for example, exclude all .pdb files but include *SpecialPdbFile.pdb* anyway)
- Replace some files by other files (for example, replace a dummy "config.xml", which is used during development by a file that is used in a live deployment).

# Requirements

- Microsoft .Net Framework 4.5
- Microsoft Visual Studio 2012

Maybe it runs on Mono as well, and/or can be opened with Mono/SharpDevelop, but I haven't checked it yet. 

# File format

The file required to specify the rules is pretty straightforward:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
  <name>Sample project</name>
  <basePath recursive="true">.\</basePath>
  <outPath>$\</outPath>
  <packedName>SamplePackedArchive</packedName>
  <exclusions>
    <exclude extension="pdb" />
    <exclude extension="xml" />
    <exclude file="vshost.exe" mode="nonLiteral" />
    <exclude file="exe.config" mode="nonLiteral" />
    <exclude extension="txt" />
  </exclusions>
  <inclusions all="true">
    <include file="ezPacker.pdb" force="true" />
    <include extension="pdb" force="true" mode="nonLiteralIncludePath" />
  </inclusions>
  <replacements>
    <replace file="ezPacker.pdb" with="Sample\SomeOtherFile.txt" mode="nonLiteralIncludePath" />
  </replacements>
</configuration>
```

## Directory path handling

### Base/Out paths

The *basePath* states where the files to be packed are found. The *outPath* is optional and states where the packed archive will be put - this path can be omitted, in which case the packed archive will be stored in the base path.

These two paths allow to specify "$\" at the beginning, which causes the path to be interpreted *relative to the directory of the project file*.

### Inclusion, Exclusion & Replacement lists

By default, *ezPacker* interprets files in the *inclusion*, *exclusion* and *replacement* lists as if they are relative to the base directory, since that is what this tool shall do.
