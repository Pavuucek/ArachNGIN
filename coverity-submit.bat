@echo off
echo Submitting to coverity
curl --form token=OOi0LFy4vCb4MwefMr3w --form email=michal.kuncl@gmail.com --form file=@coverity-ArachNGIN.zip --form version="$MajorVersion$.$MinorVersion$.$Revision$-$Commit$-$ShortHash$" --form description="$Branch$:$MajorVersion$.$MinorVersion$.$Revision$-$Commit$-$ShortHash$" https://scan.coverity.com/builds?project=Pavuucek%2FArachNGIN
echo FINISHED!