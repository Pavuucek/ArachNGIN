@echo off
if exist "cov-int" rd /q /s "cov-int"
if exist "coverity-ArachNGIN.zip" del "coverity-ArachNGIN.zip"
call "%VS140COMNTOOLS%..\..\VC\vcvarsall.bat"
cov-build.exe --dir cov-int msbuild ArachNGIN.sln /m /t:Rebuild /property:Configuration=Release /property:Platform="Any CPU"
7z a -tzip "coverity-ArachNGIN.zip" "cov-int"
call GitVersioner.bat w -f=coverity-submit.bat --no-utf
call coverity-submit.bat
call GitVersioner.bat r coverity-submit.bat --no-utf
if exist "cov-int" rd /q /s "cov-int"
pause