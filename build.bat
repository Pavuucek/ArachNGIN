@ECHO OFF
nuget restore
call GitVersioner.bat a -f=SharedAssemblyInfo.cs
call "%VS140COMNTOOLS%..\..\VC\vcvarsall.bat"
msbuild ArachNGIN.sln /m /property:Configuration=Debug /property:Platform="Any CPU"
msbuild ArachNGIN.sln /m /property:Configuration=Release /property:Platform="Any CPU"

if ERRORLEVEL 1 pause

rem build zip package
call gitversioner.bat w -f=zip.bat --no-utf
call zip.bat
call gitversioner.bat r -f=zip.bat

rem build nuget package
call gitversioner.bat w -f=Nuget\ArachNGIN.nuspec
nuget pack Nuget\ArachNGIN.nuspec
call gitversioner.bat r -f=Nuget\ArachNGIN.nuspec