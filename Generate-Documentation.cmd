@echo off
md ..\ArachNGIN_gh-pages >nul
call gitversioner.bat w -f=Doxyfile
doxygen
call gitversioner.bat r -f=Doxyfile