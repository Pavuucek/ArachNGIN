@echo off
set year=%DATE:~10,5%
:: %DATE:~10,4%
set day=%DATE:~7,2%
set mnt=%DATE:~4,2%
set hr=%TIME:~0,2%
set min=%TIME:~3,2%

IF %day% LSS 10 SET day=0%day:~1,1%
IF %mnt% LSS 10 SET mnt=0%mnt:~1,1%
IF %hr% LSS 10 SET hr=0%hr:~1,1%
IF %min% LSS 10 SET min=0%min:~1,1%
set committime=%date% - %time%
git add *.*
git commit -m "AutoCommit: %committime%"
rem git push -v --progress "origin" master:master 
