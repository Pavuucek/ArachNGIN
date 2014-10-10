@echo off
md ..\ArachNGIN_gh-pages >nul
gitversioner w Doxyfile
doxygen
gitversioner r Doxyfile