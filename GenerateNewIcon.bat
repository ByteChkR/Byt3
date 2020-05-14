set projectDir=%1
set solutionDir=%2
echo %projectDir%
echo %solutionDir%

cd %solutionDir%

call BuildUtilities\Console\Byt3.Console.exe fl -wd %solutionDir% -ss Settings.Resolution.X:512 Settings.Resolution.Y:512 -r -i Resources\Icons\IconFilter\GenerateIcon.fl -o %projectDir%Resources\OpenFL.png


call BuildUtilities\Console\Byt3.Console.exe png2ico -i %projectDir%Resources\OpenFL.png -o %projectDir%OpenFL.ico