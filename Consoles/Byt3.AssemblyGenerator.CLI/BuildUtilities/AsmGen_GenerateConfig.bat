cd ..
dotnet run -c Release AsmGen.assemblyconfig --create
dotnet run -c Release AsmGen.assemblyconfig -a Byt3.AssemblyGenerator.CLI.csproj
dotnet run -c Releases AsmGen.assemblyconfig -sname AsmGen
cd BuildUtilities