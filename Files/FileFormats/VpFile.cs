using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArachNGIN.Files.FileFormats
{
    /// <summary>
    /// Class to read Volition VP files (used by Conflict Freespace and Freespace 2)
    ///
    /// File structure (copypasta from http://www.hard-light.net/wiki/index.php/*.VP)
    ///
    /// VP files are made up of three main components; the header, followed by the individual files, and finally the index for the entries.
    /// <b>The Header</b>
    /// char header[4]; //Always "VPVP"
    /// int version;    //As of this version, still 2.
    /// int diroffset;  //Offset to the file index
    /// int direntries; //Number of entries
    ///
    /// <b>The files</b>
    /// Files are simply stored in the VP, one right after the other.No spacing or null termination is necessary.
    ///
    /// <b>The index</b>
    /// The index is a series of "direntries"; each directory has the structure, as seen below.
    ///
    /// int offset; //Offset of the file data for this entry.
    /// int size; //Size of the file data for this entry
    /// char name[32]; //Null-terminated filename, directory name, or ".." for backdir
    /// int timestamp; //Time the file was last modified, in unix time.
    ///
    /// Each direntry may be a directory, a file, or a backdir.
    /// A directory entry signifies the start of a directory,
    /// and has the name entry set to the name of the directory;
    /// a backdir has the name of "..", and represents the end of a directory.
    /// Because there is no type descriptor inherent to the format,
    /// directories and backdirs are identified by the "size", and "timestamp" entries being set to 0.
    /// All valid VP files should start with the "data" directory as the toplevel.
    ///
    /// Note that it isn't necessary at all to add backdirs at the end of a VP file.
    /// </summary>
    public class VpFile
    {
    }
}