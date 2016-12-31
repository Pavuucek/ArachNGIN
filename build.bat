@ECHO OFF
nuget restore
call GitVersioner.bat a SharedAssemblyInfo.cs
call "%VS140COMNTOOLS%..\..\VC\vcvarsall.bat"
msbuild ArachNGIN.sln /m /property:Configuration=Debug /property:Platform="Any CPU"
msbuild ArachNGIN.sln /m /property:Configuration=Release /property:Platform="Any CPU"

if ERRORLEVEL 1 pause

call gitversioner.bat w zip.bat
call zip.bat
call gitversioner.bat r zip.bat