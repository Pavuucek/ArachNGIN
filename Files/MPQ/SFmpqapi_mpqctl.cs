/*
 * Copyright (c) 2006-2014 Michal Kuncl <michal.kuncl@gmail.com> http://www.pavucina.info
 * Permission is hereby granted, free of charge, to any person obtaining a copy of this software and
 * associated documentation files (the "Software"), to deal in the Software without restriction,
 * including without limitation the rights to use, copy, modify, merge, publish, distribute,
 * sublicense, and/or sell copies of the Software, and to permit persons to whom the Software
 * is furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all copies or
 * substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
 * INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
 * PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
 * FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
 * ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 *
 * Original (Visual Basic) version and ArachNGIN.Files.MPQ.ocx file by ShadowFlare
 */

using System.Runtime.InteropServices;
using Microsoft.VisualBasic;

// ERROR: Not supported in C#: OptionDeclaration

namespace ArachNGIN.Files.MPQ
{

    /// <summary>
    /// ShadowFlare MPQ API
    /// </summary>
    internal static class SFmpqapi
    {
//  ShadowFlare MPQ API Library. (c) ShadowFlare Software 2002

//  All functions below are actual functions that are part of this
//  library and do not need any additional dll files.  It does not
//  even require Storm to be able to decompress or compress files.

//  This library emulates the interface of Lmpqapi and Storm MPQ
//  functions, so it may be used as a replacement for them in
//  MPQ extractors/archivers without even needing to recompile
//  the program that uses Lmpqapi or Storm.  It has a few features
//  not included in Lmpqapi and Storm, such as extra flags for some
//  functions, setting the locale ID of existing files, and adding
//  files without having to write them somewhere else first.  Also,
//  MPQ handles used by functions prefixed with "SFile" and "Mpq"
//  can be used interchangably; all functions use the same type
//  of MPQ handles.  You cannot, however, use handles from this
//  library with storm or lmpqapi or vice-versa.  Doing so will
//  most likely result in a crash.

//  Revision History:
//  06/12/2002 1.07 (ShadowFlare)
//  - No longer requires Storm.dll to compress or decompress
//    Warcraft III files
//  - Added SFileListFiles for getting names and information
//    about all of the files in an archive
//  - Fixed a bug with renaming and deleting files
//  - Fixed a bug with adding wave compressed files with
//    low compression setting
//  - Added a check in MpqOpenArchiveForUpdate for proper
//    dwMaximumFilesInArchive values (should be a number that
//    is a power of 2).  If it is not a proper value, it will
//    be rounded up to the next higher power of 2

//  05/09/2002 1.06 (ShadowFlare)
//  - Compresses files without Storm.dll!
//  - If Warcraft III is installed, this library will be able to
//    find Storm.dll on its own. (Storm.dll is needed to
//    decompress Warcraft III files)
//  - Fixed a bug where an embedded archive and the file that
//    contains it would be corrupted if the archive was modified
//  - Able to open all .w3m maps now

//  29/06/2002 1.05 (ShadowFlare)
//  - Supports decompressing files from Warcraft III MPQ archives
//    if using Storm.dll from Warcraft III
//  - Added MpqAddFileToArchiveEx and MpqAddFileFromBufferEx for
//    using extra compression types

//  29/05/2002 1.04 (ShadowFlare)
//  - Files can be compressed now!
//  - Fixed a bug in SFileReadFile when reading data not aligned
//    to the block size
//  - Optimized some of SFileReadFile's code.  It can read files
//    faster now
//  - SFile functions may now be used to access files not in mpq
//    archives as you can with the real storm functions
//  - MpqCompactArchive will no longer corrupt files with the
//    MODCRYPTKEY flag as long as the file is either compressed,
//    listed in "(listfile)", is "(listfile)", or is located in
//    the same place in the compacted archive; so it is safe
//    enough to use it on almost any archive
//  - Added MpqAddWaveFromBuffer
//  - Better handling of archives with no files
//  - Fixed compression with COMPRESS2 flag

//  15/05/2002 1.03 (ShadowFlare)
//  - Supports adding files with the compression attribute (does
//    not actually compress files).  Now archives created with
//    this dll can have files added to them through lmpqapi
//    without causing staredit to crash
//  - SFileGetBasePath and SFileSetBasePath work more like their
//    Storm equivalents now
//  - Implemented MpqCompactArchive, but it is not finished yet.
//    In its current state, I would recommend against using it
//    on archives that contain files with the MODCRYPTKEY flag,
//    since it will corrupt any files with that flag
//  - Added SFMpqGetVersionString2 which may be used in Visual
//    Basic to get the version string

//  07/05/2002 1.02 (ShadowFlare)
//  - SFileReadFile no longer passes the lpOverlapped parameter it
//    receives to ReadFile.  This is what was causing the function
//    to fail when used in Visual Basic
//  - Added support for more Storm MPQ functions
//  - GetLastError may now be used to get information about why a
//    function failed

//  01/05/2002 1.01 (ShadowFlare)
//  - Added ordinals for Storm MPQ functions
//  - Fixed MPQ searching functionality of SFileOpenFileEx
//  - Added a check for whether a valid handle is given when
//    SFileCloseArchive is called
//  - Fixed functionality of SFileSetArchivePriority when multiple
//    files are open
//  - File renaming works for all filenames now
//  - SFileReadFile no longer reallocates the buffer for each block
//    that is decompressed.  This should make SFileReadFile at least
//    a little faster

//  30/04/2002 1.00 (ShadowFlare)
//  - First version.
//  - Compression not yet supported
//  - Does not use SetLastError yet, so GetLastError will not return any
//    errors that have to do with this library
//  - MpqCompactArchive not implemented

//  This library is freeware, you can do anything you want with it but with
//  one exception.  If you use it in your program, you must specify this fact
//  in Help|About box or in similar way.  You can obtain version string using
//  SFMpqGetVersionString call.

//  THIS LIBRARY IS DISTRIBUTED "AS IS".  NO WARRANTY OF ANY KIND IS EXPRESSED
//  OR IMPLIED. YOU USE AT YOUR OWN RISK. THE AUTHOR WILL NOT BE LIABLE FOR
//  DATA LOSS, DAMAGES, LOSS OF PROFITS OR ANY OTHER KIND OF LOSS WHILE USING
//  OR MISUSING THIS SOFTWARE.

//  Any comments or suggestions are accepted at blakflare@hotmail.com (ShadowFlare)

        // This no longer needs to be called.  It is only provided for compatibility with older versions

// SFMpqGetVersionString2's return value is the required length of the buffer plus
// the terminating null, so use SFMpqGetVersionString2(ByVal 0&, 0) to get the length.

// General error codes        
        /// <summary>
        /// General error codes: Invalid MPQ file
        /// </summary>
        public const uint MpqErrorMpqInvalid = 0x85200065;
        /// <summary>
        /// General error codes: file not found
        /// </summary>
        public const uint MpqErrorFileNotFound = 0x85200066;
        /// <summary>
        /// General error codes:
        /// Physical write file to MPQ failed. Not sure of exact meaning
        /// </summary>
        public const uint MpqErrorDiskFull = 0x85200068;
        /// <summary>
        /// General error codes: hash table full
        /// </summary>
        public const uint MpqErrorHashTableFull = 0x85200069;
        /// <summary>
        /// General error codes: already exists
        /// </summary>
        public const uint MpqErrorAlreadyExists = 0x8520006a;
        /// <summary>
        /// General error codes:
        /// When MOAU_READ_ONLY is used without MOAU_OPEN_EXISTING
        /// </summary>
        public const uint MpqErrorBadOpenMode = 0x8520006c;
        /// <summary>
        /// General error codes: compact error
        /// </summary>
        public const uint MpqErrorCompactError = 0x85300001;

// MpqOpenArchiveForUpdate flags        
        /// <summary>
        /// MpqOpenArchiveForUpdate flags: create new
        /// </summary>
        public const int MoauCreateNew = 0x0;
        /// <summary>
        /// MpqOpenArchiveForUpdate flags: create always
        /// Was wrongly named MOAU_CREATE_NEW
        /// </summary>
        public const int MoauCreateAlways = 0x8;
        /// <summary>
        /// MpqOpenArchiveForUpdate flags: open existing
        /// </summary>
        public const int MoauOpenExisting = 0x4;
        /// <summary>
        /// MpqOpenArchiveForUpdate flags: open always
        /// </summary>
        public const int MoauOpenAlways = 0x20;
        /// <summary>
        /// MpqOpenArchiveForUpdate flags: read only
        /// Must be used with MOAU_OPEN_EXISTING
        /// </summary>
        public const int MoauReadOnly = 0x10;
        /// <summary>
        /// MpqOpenArchiveForUpdate flags: maintain listfile
        /// </summary>
        public const int MoauMaintainListfile = 0x1;

// MpqAddFileToArchive flags
        /// <summary>
        /// MpqAddFileToArchive flags: exists
        /// Will be added if not present
        /// </summary>
        public const uint MafaExists = 0x80000000;
        /// <summary>
        /// MpqAddFileToArchive flags: unknown40000000
        /// (as in nobody knows what this does)
        /// </summary>
        public const int MafaUnknown40000000 = 0x40000000;
        /// <summary>
        /// MpqAddFileToArchive flags: modcryptkey
        /// </summary>
        public const int MafaModcryptkey = 0x20000;
        /// <summary>
        /// MpqAddFileToArchive flags: encrypt
        /// </summary>
        public const int MafaEncrypt = 0x10000;
        /// <summary>
        /// MpqAddFileToArchive flags: compress
        /// </summary>
        public const int MafaCompress = 0x200;
        /// <summary>
        /// MpqAddFileToArchive flags: compress2
        /// </summary>
        public const int MafaCompress2 = 0x100;
        /// <summary>
        /// MpqAddFileToArchive flags: replace existing
        /// </summary>
        public const int MafaReplaceExisting = 0x1;

// MpqAddFileToArchiveEx compression flags
        /// <summary>
        /// MpqAddFileToArchiveEx compression flags: compress standard
        /// Standard PKWare DCL compression
        /// </summary>
        public const int MafaCompressStandard = 0x8;      
        /// <summary>
        /// MpqAddFileToArchiveEx compression flags: compress deflate
        /// ZLib's deflate compression
        /// </summary>
        public const int MafaCompressDeflate = 0x2;
        /// <summary>
        /// MpqAddFileToArchiveEx compression flags: compress wave
        /// Standard wave compression
        /// </summary>
        public const int MafaCompressWave = 0x81;      
        /// <summary>
        /// MpqAddFileToArchiveEx compression flags: compress wave2
        /// Unused wave compression
        /// </summary>
        public const int MafaCompressWave2 = 0x41;

// Flags for individual compression types used for wave compression
        
        /// <summary>
        /// Flags for individual compression types used for wave compression: wavecomp1
        /// Main compressor for standard wave compression
        /// </summary>
        public const int MafaCompressWavecomp1 = 0x80;      
        /// <summary>
        /// Flags for individual compression types used for wave compression: wavecomp2
        /// Main compressor for unused wave compression
        /// </summary>
        public const int MafaCompressWavecomp2 = 0x40;      
        /// <summary>
        /// Flags for individual compression types used for wave compression: wavecomp3
        /// Secondary compressor for wave compression
        /// </summary>
        public const int MafaCompressWavecomp3 = 0x1;

// ZLib deflate compression level constants (used with MpqAddFileToArchiveEx and MpqAddFileFromBufferEx)
        public const int ZNoCompression = 0;
        public const int ZBestSpeed = 1;
        public const int ZBestCompression = 9;
        public const int ZDefaultCompression = (-1);

// MpqAddWAVToArchive quality flags
        public const int MawaQualityHigh = 1;
        public const int MawaQualityMedium = 0;
        public const int MawaQualityLow = 2;

// SFileGetFileInfo flags
        //Block size in MPQ
        public const int SfileInfoBlockSize = 0x1;
        //Hash table size in MPQ
        public const int SfileInfoHashTableSize = 0x2;
        //Number of files in MPQ
        public const int SfileInfoNumFiles = 0x3;
        //Is Long a file or an MPQ?
        public const int SfileInfoType = 0x4;
        //Size of MPQ or uncompressed file
        public const int SfileInfoSize = 0x5;
        //Size of compressed file
        public const int SfileInfoCompressedSize = 0x6;
        //File flags (compressed, etc.), file attributes if a file not in an archive
        public const int SfileInfoFlags = 0x7;
        //Handle of MPQ that file is in
        public const int SfileInfoParent = 0x8;
        //Position of file pointer in files
        public const int SfileInfoPosition = 0x9;
        //Locale ID of file in MPQ
        public const int SfileInfoLocaleid = 0xa;
        //Priority of open MPQ
        public const int SfileInfoPriority = 0xb;
        //Hash index of file in MPQ
        public const int SfileInfoHashIndex = 0xc;

// SFileListFiles flags
        // Specifies that lpFilelists is a file list from memory, rather than being a list of file lists
        public const int SfileListMemoryList = 0x1;
        // Only list files that the function finds a name for
        public const int SfileListOnlyKnown = 0x2;
        // Only list files that the function does not find a name for
        public const int SfileListOnlyUnknown = 0x4;

        public const int SfileTypeMpq = 0x1;
        public const int SfileTypeFile = 0x2;

        public const int InvalidHandleValue = -1;

        public const int FileBegin = 0;
        public const int FileCurrent = 1;
        public const int FileEnd = 2;

        //Open archive without regard to the drive type it resides on
        public const int SfileOpenHardDiskFile = 0x0;
        //Open the archive only if it is on a CD-ROM
        public const int SfileOpenCdRomFile = 0x1;
        //Open file with write access
        public const int SfileOpenAllowWrite = 0x8000;

        //Used with SFileOpenFileEx; only the archive with the handle specified will be searched for the file
        public const int SfileSearchCurrentOnly = 0x0;
        //SFileOpenFileEx will look through all open archives for the file
        public const int SfileSearchAllOpen = 0x1;

        [DllImport("ArachNGIN.Files.MPQ.ocx", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern void SFMpqDestroy();

        [DllImport("ArachNGIN.Files.MPQ.ocx", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern string SFMpqGetVersionString();

        [DllImport("ArachNGIN.Files.MPQ.ocx", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int SFMpqGetVersionString2(string lpBuffer, int dwBufferLength);

        [DllImport("ArachNGIN.Files.MPQ.ocx", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern SfMpqVersion SFMpqGetVersion();

        [DllImport("ArachNGIN.Files.MPQ.ocx", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern bool SFileOpenArchive(string lpFileName, int dwPriority, int dwFlags, ref int hMpq);

        [DllImport("ArachNGIN.Files.MPQ.ocx", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern bool SFileCloseArchive(int hMpq);

        [DllImport("ArachNGIN.Files.MPQ.ocx", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern bool SFileGetArchiveName(int hMpq, string lpBuffer, int dwBufferLength);

        [DllImport("ArachNGIN.Files.MPQ.ocx", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern bool SFileOpenFile(string lpFileName, ref int hFile);

        [DllImport("ArachNGIN.Files.MPQ.ocx", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern bool SFileOpenFileEx(int hMpq, string lpFileName, int dwSearchScope, ref int hFile);

        [DllImport("ArachNGIN.Files.MPQ.ocx", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern bool SFileCloseFile(int hFile);

        [DllImport("ArachNGIN.Files.MPQ.ocx", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int SFileGetFileSize(int hFile, ref int lpFileSizeHigh);

        [DllImport("ArachNGIN.Files.MPQ.ocx", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern bool SFileGetFileArchive(int hFile, ref int hMpq);

        [DllImport("ArachNGIN.Files.MPQ.ocx", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern bool SFileGetFileName(int hMpq, string lpBuffer, int dwBufferLength);

        [DllImport("ArachNGIN.Files.MPQ.ocx", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int SFileSetFilePointer(int hFile, int lDistanceToMove, ref int lplDistanceToMoveHigh,
                                                     int dwMoveMethod);

        [DllImport("ArachNGIN.Files.MPQ.ocx", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern bool SFileReadFile(int hFile, ref object lpBuffer, int nNumberOfBytesToRead,
                                                ref int lpNumberOfBytesRead, ref object lpOverlapped);

        [DllImport("ArachNGIN.Files.MPQ.ocx", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int SFileSetLocale(int nNewLocale);

        [DllImport("ArachNGIN.Files.MPQ.ocx", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern bool SFileGetBasePath(string lpBuffer, int dwBufferLength);

        [DllImport("ArachNGIN.Files.MPQ.ocx", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern bool SFileSetBasePath(string lpNewBasePath);

        [DllImport("ArachNGIN.Files.MPQ.ocx", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int SFileGetFileInfo(int hFile, int dwInfoType);

        [DllImport("ArachNGIN.Files.MPQ.ocx", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern bool SFileSetArchivePriority(int hMpq, int dwPriority);

        [DllImport("ArachNGIN.Files.MPQ.ocx", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int SFileFindMpqHeader(int hFile);

        [DllImport("ArachNGIN.Files.MPQ.ocx", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern bool SFileListFiles(int hMpq, string lpFileLists, ref FileListEntry lpListBuffer,
                                                 int dwFlags);

        [DllImport("ArachNGIN.Files.MPQ.ocx", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int MpqOpenArchiveForUpdate(string lpFileName, int dwFlags, int dwMaximumFilesInArchive);

        [DllImport("ArachNGIN.Files.MPQ.ocx", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int MpqCloseUpdatedArchive(int hMpq, int dwUnknown2);

        [DllImport("ArachNGIN.Files.MPQ.ocx", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern bool MpqAddFileToArchive(int hMpq, string lpSourceFileName, string lpDestFileName,
                                                      int dwFlags);

        [DllImport("ArachNGIN.Files.MPQ.ocx", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern bool MpqAddWaveToArchive(int hMpq, string lpSourceFileName, string lpDestFileName,
                                                      int dwFlags, int dwQuality);

        [DllImport("ArachNGIN.Files.MPQ.ocx", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern bool MpqRenameFile(int hMpq, string lpcOldFileName, string lpcNewFileName);

        [DllImport("ArachNGIN.Files.MPQ.ocx", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern bool MpqDeleteFile(int hMpq, string lpFileName);

        [DllImport("ArachNGIN.Files.MPQ.ocx", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern bool MpqCompactArchive(int hMpq);

        [DllImport("ArachNGIN.Files.MPQ.ocx", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern bool MpqAddFileToArchiveEx(int hMpq, string lpSourceFileName, string lpDestFileName,
                                                        int dwFlags, int dwCompressionType, int dwCompressLevel);

        [DllImport("ArachNGIN.Files.MPQ.ocx", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern bool MpqAddFileFromBufferEx(int hMpq, ref object lpBuffer, int dwLength, string lpFileName,
                                                         int dwFlags, int dwCompressionType, int dwCompressLevel);

        [DllImport("ArachNGIN.Files.MPQ.ocx", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern bool MpqAddFileFromBuffer(int hMpq, ref object lpBuffer, int dwLength, string lpFileName,
                                                       int dwFlags);

        [DllImport("ArachNGIN.Files.MPQ.ocx", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern bool MpqAddWaveFromBuffer(int hMpq, ref object lpBuffer, int dwLength, string lpFileName,
                                                       int dwFlags, int dwQuality);

        [DllImport("ArachNGIN.Files.MPQ.ocx", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern bool MpqSetFileLocale(int hMpq, string lpFileName, int nOldLocale, int nNewLocale);

        [DllImport("ArachNGIN.Files.MPQ.ocx", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern bool SFileDestroy();

        [DllImport("ArachNGIN.Files.MPQ.ocx", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern void StormDestroy();

// Storm functions implemented by this library
//UPGRADE_ISSUE: Declaring a parameter 'As Any' is not supported. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="FAE78A8D-8978-4FD4-8208-5B7324A8F795"'
//UPGRADE_ISSUE: Declaring a parameter 'As Any' is not supported. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="FAE78A8D-8978-4FD4-8208-5B7324A8F795"'

// Extra storm-related functions
//UPGRADE_WARNING: Structure FileListEntry may require marshalling attributes to be passed as an argument in this Declare statement. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"'

// Archive editing functions implemented by this library

// Extra archive editing functions
//UPGRADE_ISSUE: Declaring a parameter 'As Any' is not supported. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="FAE78A8D-8978-4FD4-8208-5B7324A8F795"'
//UPGRADE_ISSUE: Declaring a parameter 'As Any' is not supported. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="FAE78A8D-8978-4FD4-8208-5B7324A8F795"'
//UPGRADE_ISSUE: Declaring a parameter 'As Any' is not supported. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="FAE78A8D-8978-4FD4-8208-5B7324A8F795"'

// These functions do nothing.  They are only provided for
// compatibility with MPQ extractors that use storm.

// Returns 0 if the dll version is equal to the version your program was compiled
// with, 1 if the dll is newer, -1 if the dll is older.
        public static int SfMpqCompareVersion()
        {
            var functionReturnValue = 0;
            var exeVersion = default(SfMpqVersion);
            var DllVersion = default(SfMpqVersion);
            var _with1 = exeVersion;
            _with1.Major = 1;
            _with1.Minor = 0;
            _with1.Revision = 7;
            _with1.Subrevision = 4;
            //UPGRADE_WARNING: Couldn't resolve default property of object DllVersion. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            DllVersion = SFMpqGetVersion();
            if (DllVersion.Major > exeVersion.Major)
            {
                functionReturnValue = 1;
                return functionReturnValue;
            }
            if (DllVersion.Major < exeVersion.Major)
            {
                functionReturnValue = -1;
                return functionReturnValue;
            }
            if (DllVersion.Minor > exeVersion.Minor)
            {
                functionReturnValue = 1;
                return functionReturnValue;
            }
            if (DllVersion.Minor < exeVersion.Minor)
            {
                functionReturnValue = -1;
                return functionReturnValue;
            }
            if (DllVersion.Revision > exeVersion.Revision)
            {
                functionReturnValue = 1;
                return functionReturnValue;
            }
            if (DllVersion.Revision < exeVersion.Revision)
            {
                functionReturnValue = -1;
                return functionReturnValue;
            }
            if (DllVersion.Subrevision > exeVersion.Subrevision)
            {
                functionReturnValue = 1;
                return functionReturnValue;
            }
            if (DllVersion.Subrevision < exeVersion.Subrevision)
            {
                functionReturnValue = -1;
                return functionReturnValue;
            }
            functionReturnValue = 0;
            return functionReturnValue;
        }

        #region Nested type: FileListEntry


        /// <summary>
        /// Filelist entry
        /// </summary>
        public struct FileListEntry
        {
            // Nonzero if this entry is used
            // Compressed size of file
            public int DwCompressedSize;
            public int DwFileExists;
            // Uncompressed size of file
            // Flags for file
            public int DwFlags;
            public int DwFullSize;
            public int LcLocale;
            [VBFixedArray(259)] public byte[] SzFileName;

            //UPGRADE_TODO: "Initialize" must be called to initialize instances of this structure. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="B4BFF9E0-8631-45CF-910E-62AB3970F27B"'
            public void Initialize()
            {
                SzFileName = new byte[260];
            }
        }

        #endregion

        #region Nested type: SfMpqVersion


        /// <summary>
        /// Mpq dll/ocx version
        /// </summary>
        public struct SfMpqVersion
        {

            public short Major;
            public short Minor;
            public short Revision;
            public short Subrevision;
        }

        #endregion
    }
}