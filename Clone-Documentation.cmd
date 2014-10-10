@echo off
md ..\ArachNGIN_gh-pages >nul
pushd ..\ArachNGIN_gh-pages
git clone https://github.com/Pavuucek/ArachNGIN.git .
git checkout origin/gh-pages -b gh-pages
git branch -d master
git branch
popd
pause