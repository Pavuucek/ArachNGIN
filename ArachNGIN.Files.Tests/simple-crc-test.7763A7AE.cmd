@echo off
cd mpc-hc
rem call build.bat Clean All Both Release
call build.bat Both packages Release vs2015
cd ..
pause