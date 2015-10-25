@echo off
7z a ArachNGIN_debug-$MajorVersion$.$MinorVersion$.$Revision$-$Commit$-$ShortHash$.zip .\_bin\Debug\*.*
7z a ArachNGIN_release-$MajorVersion$.$MinorVersion$.$Revision$-$Commit$-$ShortHash$.zip .\_bin\Release\*.*
goto :eof