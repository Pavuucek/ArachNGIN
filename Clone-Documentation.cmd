@echo off
call git-new-workdir.cmd . ..\ArachNGIN_gh-pages gh-pages
pushd ..\ArachNGIN_gh-pages
git branch --set-upstream-to=remotes/origin/gh-pages
popd