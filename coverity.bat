@echo off
if exist "cov-int" rd /q /s "cov-int"
if exist "coverity-ArachNGIN.zip" del "coverity-ArachNGIN.zip"
cov-build.exe --dir cov-int %windir%\Microsoft.Net\Framework\v4.0.30319\msbuild ArachNGIN.sln /m /t:Rebuild /property:Configuration=Release /property:Platform="Any CPU"
7z a -tzip "coverity-ArachNGIN.zip" "cov-int"
call GitVersioner.bat w coverity-submit.bat
call coverity-submit.bat
call GitVersioner.bat r coverity-submit.bat
if exist "cov-int" rd /q /s "cov-int"
pause