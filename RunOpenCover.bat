@echo off
setlocal

set OpenCoverPath=C:\Users\%USERNAME%\.nuget\packages\opencover\4.6.519\tools
set ReportGeneratorPath=C:\Users\%USERNAME%\.nuget\packages\reportgenerator\3.1.2\tools
REM #########################################
REM ### Edit the two paths above this block
REM #########################################

dotnet restore

if not exist %OpenCoverPath%\ goto :noDirectoryError
if not exist %ReportGeneratorPath%\ goto :noDirectoryError

if not exist coverage\unit mkdir coverage\unit
echo Running OpenCover
%OpenCoverPath%\OpenCover.Console.exe -target:"dotnet.exe" -targetargs:"test -f netcoreapp2.1 -c Release test/ReactAdvantage.Tests.Unit/ReactAdvantage.Tests.Unit.csproj" -hideskipped:File -output:coverage/unit/coverage.xml -oldStyle -filter:"+[ReactAdvantage*]* -[ReactAdvantage.Tests*]* -[ReactAdvantage.Api]*Program -[ReactAdvantage.Api]*Startup -[ReactAdvantage.Api.Views]* -[ReactAdvantage.Data]*.Migrations.*" -searchdirs:"Tests/ReactAdvantage.Tests.Unit/bin/Release/netcoreapp2.1" -register:user || goto :error
echo Running ReportGenerator
%ReportGeneratorPath%\ReportGenerator.exe -reports:coverage/unit/coverage.xml -targetdir:coverage/unit -verbosity:Error || goto :error
echo Opening the report file
start "" .\coverage\unit\index.htm || goto :error
echo Finished successfully
exit

:error
echo Stopped on error
pause
goto :EOF

:noDirectoryError
echo ####################################################
echo ### Please edit the file and set the correct path 
echo ### to OpenCover and ReportGenerator tools
echo ### before the first run
echo ####################################################
pause
goto :EOF