Byt3.Console.exe asmgen AsmGen.assemblyconfig -c -sname AsmGen.Console -sruntime none -a D:\Users\Tim\Documents\MasterServer\Byt3\Libraries\Byt3.AssemblyGenerator\Byt3.AssemblyGenerator.Console\Byt3.AssemblyGenerator.Console.csproj -b
Byt3.Console.exe asmgen OpenFL.assemblyconfig -c -sname OpenFL.Console -sruntime none -a D:\Users\Tim\Documents\MasterServer\Byt3\Libraries\Byt3.OpenFL\Byt3.OpenFL.Console\Byt3.OpenFL.Console.csproj -b
Byt3.Console.exe asmgen ExtPP.assemblyconfig -c -sname ExtPP.Console -sruntime none -a D:\Users\Tim\Documents\MasterServer\Byt3\Libraries\Byt3.ExtPP\Byt3.ExtPP.Console\Byt3.ExtPP.Console.csproj -b
Byt3.Console.exe asmgen VersionHelper.assemblyconfig -c -sname VersionHelper.Console -sruntime none -a D:\Users\Tim\Documents\MasterServer\Byt3\Libraries\Byt3.VersionHelper.Console\Byt3.VersionHelper.Console.csproj -b

xcopy AsmGen.Console_build\AsmGen.Console.dll consoles\ /Y
xcopy OpenFL.Console_build\OpenFL.Console.dll consoles\ /Y
xcopy ExtPP.Console_build\ExtPP.Console.dll consoles\ /Y
xcopy VersionHelper.Console_build\VersionHelper.Console.dll consoles\ /Y
del AsmGen.assemblyconfig
del OpenFL.assemblyconfig
del ExtPP.assemblyconfig
del VersionHelper.assemblyconfig
rmdir /S /Q AsmGen.Console_build
rmdir /S /Q OpenFL.Console_build
rmdir /S /Q ExtPP.Console_build
rmdir /S /Q VersionHelper.Console_build