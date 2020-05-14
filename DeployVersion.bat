cd ..\Byt3Deploy

copy ..\Byt3\Applications\Byt3.AutoUpdate\bin\Debug\Byt3.AutoUpdate.exe ..\Byt3\Applications\FLDebugger\bin\Debug\Byt3.AutoUpdate.exe
call upload.bat FLDebugger ..\Byt3\Applications\FLDebugger\bin\Debug FLDebugger

del ..\Byt3\Applications\FLDebugger\bin\Debug\Byt3.AutoUpdate.exe

copy ..\Byt3\Applications\Byt3.AutoUpdate\bin\Debug\Byt3.AutoUpdate.exe ..\Byt3\Applications\FLDebugger.Projects\bin\Debug\Byt3.AutoUpdate.exe
copy ..\Byt3\Applications\FLDebugger\bin\Debug\FLDebugger.exe ..\Byt3\Applications\FLDebugger.Projects\bin\Debug\FLDebugger.exe
call upload.bat FLDebugger_Projects ..\Byt3\Applications\FLDebugger.Projects\bin\Debug FLDebugger.Projects

del ..\Byt3\Applications\FLDebugger.Projects\bin\Debug\FLDebugger.Projects.exe