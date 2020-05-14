set projectDir=%1
set solutionDir=%2
echo %projectDir%
echo %solutionDir%
cd %solutionDir%
BuildUtilities\Console\Byt3.Console.exe fl -ss Settings.Resolution.X:256 Settings.Resolution.Y:256 -r -i Resources\Icons\IconFilter\GenerateIcon.fl -o %projectDir%Resources\OpenFL.png

BuildUtilities\Console\Byt3.Console.exe png2ico -i %projectDir%Resources\OpenFL.png -o %projectDir%OpenFL.ico