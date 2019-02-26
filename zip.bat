@echo off
IF EXIST ".\_bin\Debug\*.*" 7z a ArachNGIN_debug-$MajorVersion$.$MinorVersion$.$Revision$-$Commit$-$ShortHash$.zip .\_bin\Debug\*.*
IF EXIST ".\_bin\Release\*.*" 7z a ArachNGIN_release-$MajorVersion$.$MinorVersion$.$Revision$-$Commit$-$ShortHash$.zip .\_bin\Release\*.*
goto :eof