@echo off
md ..\ArachNGIN_gh-pages >nul
call gitversioner.bat w Doxyfile
doxygen
call gitversioner.bat r Doxyfile