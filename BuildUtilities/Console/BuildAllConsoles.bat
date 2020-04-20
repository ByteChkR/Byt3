rmdir /Q /S consoles\OpenFL
rmdir /Q /S consoles\ExtPP
rmdir /Q /S consoles\VersionHelper


Byt3.Console.exe asmgen OpenFL.assemblyconfig -o ..\OpenFL -c -sname OpenFL.Console -sruntime none -a D:\Users\Tim\Documents\MasterServer\Byt3\Libraries\Byt3.OpenFL\Byt3.OpenFL.Console\Byt3.OpenFL.Console.csproj -b
Byt3.Console.exe asmgen ExtPP.assemblyconfig -o ..\ExtPP -c -sname ExtPP.Console -sruntime none -a D:\Users\Tim\Documents\MasterServer\Byt3\Libraries\Byt3.ExtPP\Byt3.ExtPP.Console\Byt3.ExtPP.Console.csproj -b
Byt3.Console.exe asmgen VersionHelper.assemblyconfig -o ..\VersionHelper -c -sname VersionHelper.Console -sruntime none -a D:\Users\Tim\Documents\MasterServer\Byt3\Libraries\Byt3.VersionHelper.Console\Byt3.VersionHelper.Console.csproj -b

move consoles\AsmGen consoles\AsmGen.Old
del RunnerConfig.xml

Byt3.Console.exe asmgen AsmGen.assemblyconfig -o ..\AsmGen -c -sname AsmGen.Console -sruntime none -a D:\Users\Tim\Documents\MasterServer\Byt3\Libraries\Byt3.AssemblyGenerator\Byt3.AssemblyGenerator.Console\Byt3.AssemblyGenerator.Console.csproj -b

rmdir /Q /S consoles\AsmGen.Old
del RunnerConfig.xml

Byt3.Console.exe

pause