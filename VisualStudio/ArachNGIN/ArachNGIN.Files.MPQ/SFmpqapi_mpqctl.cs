using Microsoft.VisualBasic;
using Microsoft.VisualBasic.Compatibility;
using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Runtime.InteropServices;
 // ERROR: Not supported in C#: OptionDeclaration
namespace ArachNGIN.Files.MPQ
{
	static class SFmpqapi
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

		public struct SFMPQVERSION
		{
			public short Major;
			public short Minor;
			public short Revision;
			public short Subrevision;
		}
		[DllImport("ArachNGIN.Files.MPQ.ocx", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern void SFMpqDestroy();
		[DllImport("ArachNGIN.Files.MPQ.ocx", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern string SFMpqGetVersionString();
		[DllImport("ArachNGIN.Files.MPQ.ocx", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int SFMpqGetVersionString2(string lpBuffer, int dwBufferLength);
		[DllImport("ArachNGIN.Files.MPQ.ocx", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern SFMPQVERSION SFMpqGetVersion();

		// This no longer needs to be called.  It is only provided for compatibility with older versions

// SFMpqGetVersionString2's return value is the required length of the buffer plus
// the terminating null, so use SFMpqGetVersionString2(ByVal 0&, 0) to get the length.

// General error codes
		public const uint MPQ_ERROR_MPQ_INVALID = 0x85200065;
		public const uint MPQ_ERROR_FILE_NOT_FOUND = 0x85200066;
			//Physical write file to MPQ failed. Not sure of exact meaning
		public const uint MPQ_ERROR_DISK_FULL = 0x85200068;
		public const uint MPQ_ERROR_HASH_TABLE_FULL = 0x85200069;
		public const uint MPQ_ERROR_ALREADY_EXISTS = 0x8520006a;
			//When MOAU_READ_ONLY is used without MOAU_OPEN_EXISTING
		public const uint MPQ_ERROR_BAD_OPEN_MODE = 0x8520006c;

		public const uint MPQ_ERROR_COMPACT_ERROR = 0x85300001;

// MpqOpenArchiveForUpdate flags
		public const int MOAU_CREATE_NEW = 0x0;
			//Was wrongly named MOAU_CREATE_NEW
		public const int MOAU_CREATE_ALWAYS = 0x8;
		public const int MOAU_OPEN_EXISTING = 0x4;
		public const int MOAU_OPEN_ALWAYS = 0x20;
			//Must be used with MOAU_OPEN_EXISTING
		public const int MOAU_READ_ONLY = 0x10;
		public const int MOAU_MAINTAIN_LISTFILE = 0x1;

// MpqAddFileToArchive flags
			//Will be added if not present
		public const uint MAFA_EXISTS = 0x80000000;
		public const int MAFA_UNKNOWN40000000 = 0x40000000;
		public const int MAFA_MODCRYPTKEY = 0x20000;
		public const int MAFA_ENCRYPT = 0x10000;
		public const int MAFA_COMPRESS = 0x200;
		public const int MAFA_COMPRESS2 = 0x100;
		public const int MAFA_REPLACE_EXISTING = 0x1;

// MpqAddFileToArchiveEx compression flags
			//Standard PKWare DCL compression
		public const int MAFA_COMPRESS_STANDARD = 0x8;
			//ZLib's deflate compression
		public const int MAFA_COMPRESS_DEFLATE = 0x2;
			//Standard wave compression
		public const int MAFA_COMPRESS_WAVE = 0x81;
			//Unused wave compression
		public const int MAFA_COMPRESS_WAVE2 = 0x41;

// Flags for individual compression types used for wave compression
			//Main compressor for standard wave compression
		public const int MAFA_COMPRESS_WAVECOMP1 = 0x80;
			//Main compressor for unused wave compression
		public const int MAFA_COMPRESS_WAVECOMP2 = 0x40;
			//Secondary compressor for wave compression
		public const int MAFA_COMPRESS_WAVECOMP3 = 0x1;

// ZLib deflate compression level constants (used with MpqAddFileToArchiveEx and MpqAddFileFromBufferEx)
		public const int Z_NO_COMPRESSION = 0;
		public const int Z_BEST_SPEED = 1;
		public const int Z_BEST_COMPRESSION = 9;
		public const int Z_DEFAULT_COMPRESSION = (-1);

// MpqAddWAVToArchive quality flags
		public const int MAWA_QUALITY_HIGH = 1;
		public const int MAWA_QUALITY_MEDIUM = 0;
		public const int MAWA_QUALITY_LOW = 2;

// SFileGetFileInfo flags
			//Block size in MPQ
		public const int SFILE_INFO_BLOCK_SIZE = 0x1;
			//Hash table size in MPQ
		public const int SFILE_INFO_HASH_TABLE_SIZE = 0x2;
			//Number of files in MPQ
		public const int SFILE_INFO_NUM_FILES = 0x3;
			//Is Long a file or an MPQ?
		public const int SFILE_INFO_TYPE = 0x4;
			//Size of MPQ or uncompressed file
		public const int SFILE_INFO_SIZE = 0x5;
			//Size of compressed file
		public const int SFILE_INFO_COMPRESSED_SIZE = 0x6;
			//File flags (compressed, etc.), file attributes if a file not in an archive
		public const int SFILE_INFO_FLAGS = 0x7;
			//Handle of MPQ that file is in
		public const int SFILE_INFO_PARENT = 0x8;
			//Position of file pointer in files
		public const int SFILE_INFO_POSITION = 0x9;
			//Locale ID of file in MPQ
		public const int SFILE_INFO_LOCALEID = 0xa;
			//Priority of open MPQ
		public const int SFILE_INFO_PRIORITY = 0xb;
			//Hash index of file in MPQ
		public const int SFILE_INFO_HASH_INDEX = 0xc;

// SFileListFiles flags
			// Specifies that lpFilelists is a file list from memory, rather than being a list of file lists
		public const int SFILE_LIST_MEMORY_LIST = 0x1;
			// Only list files that the function finds a name for
		public const int SFILE_LIST_ONLY_KNOWN = 0x2;
			// Only list files that the function does not find a name for
		public const int SFILE_LIST_ONLY_UNKNOWN = 0x4;

		public const int SFILE_TYPE_MPQ = 0x1;
		public const int SFILE_TYPE_FILE = 0x2;

		public const int INVALID_HANDLE_VALUE = -1;

		public const int FILE_BEGIN = 0;
		public const int FILE_CURRENT = 1;
		public const int FILE_END = 2;

			//Open archive without regard to the drive type it resides on
		public const int SFILE_OPEN_HARD_DISK_FILE = 0x0;
			//Open the archive only if it is on a CD-ROM
		public const int SFILE_OPEN_CD_ROM_FILE = 0x1;
			//Open file with write access
		public const int SFILE_OPEN_ALLOW_WRITE = 0x8000;

			//Used with SFileOpenFileEx; only the archive with the handle specified will be searched for the file
		public const int SFILE_SEARCH_CURRENT_ONLY = 0x0;
			//SFileOpenFileEx will look through all open archives for the file
		public const int SFILE_SEARCH_ALL_OPEN = 0x1;

		public struct FILELISTENTRY
		{
				// Nonzero if this entry is used
			public int dwFileExists;
				// Locale ID of file
			public int lcLocale;
				// Compressed size of file
			public int dwCompressedSize;
				// Uncompressed size of file
			public int dwFullSize;
				// Flags for file
			public int dwFlags;
			[VBFixedArray(259)]
			public byte[] szFileName;

			//UPGRADE_TODO: "Initialize" must be called to initialize instances of this structure. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="B4BFF9E0-8631-45CF-910E-62AB3970F27B"'
			public void Initialize()
			{
				szFileName = new byte[260];
			}
		}
		[DllImport("ArachNGIN.Files.MPQ.ocx", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern bool SFileOpenArchive(string lpFileName, int dwPriority, int dwFlags, ref int hMPQ);
		[DllImport("ArachNGIN.Files.MPQ.ocx", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern bool SFileCloseArchive(int hMPQ);
		[DllImport("ArachNGIN.Files.MPQ.ocx", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern bool SFileGetArchiveName(int hMPQ, string lpBuffer, int dwBufferLength);
		[DllImport("ArachNGIN.Files.MPQ.ocx", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern bool SFileOpenFile(string lpFileName, ref int hFile);
		[DllImport("ArachNGIN.Files.MPQ.ocx", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern bool SFileOpenFileEx(int hMPQ, string lpFileName, int dwSearchScope, ref int hFile);
		[DllImport("ArachNGIN.Files.MPQ.ocx", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern bool SFileCloseFile(int hFile);
		[DllImport("ArachNGIN.Files.MPQ.ocx", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int SFileGetFileSize(int hFile, ref int lpFileSizeHigh);
		[DllImport("ArachNGIN.Files.MPQ.ocx", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern bool SFileGetFileArchive(int hFile, ref int hMPQ);
		[DllImport("ArachNGIN.Files.MPQ.ocx", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern bool SFileGetFileName(int hMPQ, string lpBuffer, int dwBufferLength);
		[DllImport("ArachNGIN.Files.MPQ.ocx", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int SFileSetFilePointer(int hFile, int lDistanceToMove, ref int lplDistanceToMoveHigh, int dwMoveMethod);
		[DllImport("ArachNGIN.Files.MPQ.ocx", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern bool SFileReadFile(int hFile, ref object lpBuffer, int nNumberOfBytesToRead, ref int lpNumberOfBytesRead, ref object lpOverlapped);
		[DllImport("ArachNGIN.Files.MPQ.ocx", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int SFileSetLocale(int nNewLocale);
		[DllImport("ArachNGIN.Files.MPQ.ocx", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern bool SFileGetBasePath(string lpBuffer, int dwBufferLength);
		[DllImport("ArachNGIN.Files.MPQ.ocx", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern bool SFileSetBasePath(string lpNewBasePath);
		[DllImport("ArachNGIN.Files.MPQ.ocx", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int SFileGetFileInfo(int hFile, int dwInfoType);
		[DllImport("ArachNGIN.Files.MPQ.ocx", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern bool SFileSetArchivePriority(int hMPQ, int dwPriority);
		[DllImport("ArachNGIN.Files.MPQ.ocx", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int SFileFindMpqHeader(int hFile);
		[DllImport("ArachNGIN.Files.MPQ.ocx", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern bool SFileListFiles(int hMPQ, string lpFileLists, ref FILELISTENTRY lpListBuffer, int dwFlags);
		[DllImport("ArachNGIN.Files.MPQ.ocx", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int MpqOpenArchiveForUpdate(string lpFileName, int dwFlags, int dwMaximumFilesInArchive);
		[DllImport("ArachNGIN.Files.MPQ.ocx", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int MpqCloseUpdatedArchive(int hMPQ, int dwUnknown2);
		[DllImport("ArachNGIN.Files.MPQ.ocx", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern bool MpqAddFileToArchive(int hMPQ, string lpSourceFileName, string lpDestFileName, int dwFlags);
		[DllImport("ArachNGIN.Files.MPQ.ocx", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern bool MpqAddWaveToArchive(int hMPQ, string lpSourceFileName, string lpDestFileName, int dwFlags, int dwQuality);
		[DllImport("ArachNGIN.Files.MPQ.ocx", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern bool MpqRenameFile(int hMPQ, string lpcOldFileName, string lpcNewFileName);
		[DllImport("ArachNGIN.Files.MPQ.ocx", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern bool MpqDeleteFile(int hMPQ, string lpFileName);
		[DllImport("ArachNGIN.Files.MPQ.ocx", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern bool MpqCompactArchive(int hMPQ);
		[DllImport("ArachNGIN.Files.MPQ.ocx", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern bool MpqAddFileToArchiveEx(int hMPQ, string lpSourceFileName, string lpDestFileName, int dwFlags, int dwCompressionType, int dwCompressLevel);
		[DllImport("ArachNGIN.Files.MPQ.ocx", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern bool MpqAddFileFromBufferEx(int hMPQ, ref object lpBuffer, int dwLength, string lpFileName, int dwFlags, int dwCompressionType, int dwCompressLevel);
		[DllImport("ArachNGIN.Files.MPQ.ocx", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern bool MpqAddFileFromBuffer(int hMPQ, ref object lpBuffer, int dwLength, string lpFileName, int dwFlags);
		[DllImport("ArachNGIN.Files.MPQ.ocx", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern bool MpqAddWaveFromBuffer(int hMPQ, ref object lpBuffer, int dwLength, string lpFileName, int dwFlags, int dwQuality);
		[DllImport("ArachNGIN.Files.MPQ.ocx", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern bool MpqSetFileLocale(int hMPQ, string lpFileName, int nOldLocale, int nNewLocale);
		[DllImport("ArachNGIN.Files.MPQ.ocx", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern bool SFileDestroy();
		[DllImport("ArachNGIN.Files.MPQ.ocx", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern void StormDestroy();

// Storm functions implemented by this library
//UPGRADE_ISSUE: Declaring a parameter 'As Any' is not supported. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="FAE78A8D-8978-4FD4-8208-5B7324A8F795"'
//UPGRADE_ISSUE: Declaring a parameter 'As Any' is not supported. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="FAE78A8D-8978-4FD4-8208-5B7324A8F795"'

// Extra storm-related functions
//UPGRADE_WARNING: Structure FILELISTENTRY may require marshalling attributes to be passed as an argument in this Declare statement. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"'

// Archive editing functions implemented by this library

// Extra archive editing functions
//UPGRADE_ISSUE: Declaring a parameter 'As Any' is not supported. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="FAE78A8D-8978-4FD4-8208-5B7324A8F795"'
//UPGRADE_ISSUE: Declaring a parameter 'As Any' is not supported. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="FAE78A8D-8978-4FD4-8208-5B7324A8F795"'
//UPGRADE_ISSUE: Declaring a parameter 'As Any' is not supported. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="FAE78A8D-8978-4FD4-8208-5B7324A8F795"'

// These functions do nothing.  They are only provided for
// compatibility with MPQ extractors that use storm.

// Returns 0 if the dll version is equal to the version your program was compiled
// with, 1 if the dll is newer, -1 if the dll is older.
		public static int SFMpqCompareVersion()
		{
			int functionReturnValue = 0;
			SFMPQVERSION ExeVersion = default(SFMPQVERSION);
			SFMPQVERSION DllVersion = default(SFMPQVERSION);
			var _with1 = ExeVersion;
			_with1.Major = 1;
			_with1.Minor = 0;
			_with1.Revision = 7;
			_with1.Subrevision = 4;
			//UPGRADE_WARNING: Couldn't resolve default property of object DllVersion. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			DllVersion = SFMpqGetVersion();
			if (DllVersion.Major > ExeVersion.Major) {
				functionReturnValue = 1;
				return functionReturnValue;
			} else if (DllVersion.Major < ExeVersion.Major) {
				functionReturnValue = -1;
				return functionReturnValue;
			}
			if (DllVersion.Minor > ExeVersion.Minor) {
				functionReturnValue = 1;
				return functionReturnValue;
			} else if (DllVersion.Minor < ExeVersion.Minor) {
				functionReturnValue = -1;
				return functionReturnValue;
			}
			if (DllVersion.Revision > ExeVersion.Revision) {
				functionReturnValue = 1;
				return functionReturnValue;
			} else if (DllVersion.Revision < ExeVersion.Revision) {
				functionReturnValue = -1;
				return functionReturnValue;
			}
			if (DllVersion.Subrevision > ExeVersion.Subrevision) {
				functionReturnValue = 1;
				return functionReturnValue;
			} else if (DllVersion.Subrevision < ExeVersion.Subrevision) {
				functionReturnValue = -1;
				return functionReturnValue;
			}
			functionReturnValue = 0;
			return functionReturnValue;
		}
	}
}
