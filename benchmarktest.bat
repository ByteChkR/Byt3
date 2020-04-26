:a
D:\Users\Tim\Documents\Byt3\BuildUtilities\Console\Byt3.Console.exe vh -i D:\Users\Tim\Documents\Byt3\Libraries\Byt3.OpenFL\Byt3.OpenFL.Common\Byt3.OpenFL.Common.csproj "X.X.+.{HHmm}"

dotnet build

cd D:\Users\Tim\Documents\Byt3\Consoles\TestingProject\bin\Debug\netcoreapp2.1
dotnet TestingProject.dll -flbench gen -ea -out performance_no_checks_no_extra --no-checks
dotnet TestingProject.dll -flbench gen -ea -out performance_with_checks_no_extra
dotnet TestingProject.dll -flbench gen -ea -out performance_with_checks_zip_extra --extra zip
dotnet TestingProject.dll -flbench gen -ea -out performance_no_checks_zip_extra --extra zip --no-checks
cd D:\Users\Tim\Documents\Byt3
goto a