cd ..\Byt3Deploy

copy ..\Byt3\Applications\Byt3.AutoUpdate\bin\Debug\Byt3.AutoUpdate.exe ..\Byt3\Applications\FLDebugger\bin\Debug\Byt3.AutoUpdate.exe
del ..\Byt3\Applications\FLDebugger\bin\Debug\FLDebugger.exe.config
del ..\Byt3\Applications\FLDebugger\bin\Debug\FLDebugger.pdb
del ..\Byt3\Applications\FLDebugger\bin\Debug\System.Buffers.xml
del ..\Byt3\Applications\FLDebugger\bin\Debug\System.Memory.xml
del ..\Byt3\Applications\FLDebugger\bin\Debug\System.Numerics.Vectors.xml
del ..\Byt3\Applications\FLDebugger\bin\Debug\System.Runtime.CompilerServices.Unsafe.xml


call upload.bat FLDebugger ..\Byt3\Applications\FLDebugger\bin\Debug FLDebugger

del ..\Byt3\Applications\FLDebugger\bin\Debug\Byt3.AutoUpdate.exe

copy ..\Byt3\Applications\Byt3.AutoUpdate\bin\Debug\Byt3.AutoUpdate.exe ..\Byt3\Applications\FLDebugger.Projects\bin\Debug\Byt3.AutoUpdate.exe
xcopy /E /Y ..\Byt3\Applications\FLDebugger\bin\Debug ..\Byt3\Applications\FLDebugger.Projects\bin\Debug\
del ..\Byt3\Applications\FLDebugger.Projects\bin\Debug\FLDebugger.Projects.exe.config
del ..\Byt3\Applications\FLDebugger.Projects\bin\Debug\FLDebugger.Projects.pdb


call upload.bat FLDebugger_Projects ..\Byt3\Applications\FLDebugger.Projects\bin\Debug FLDebugger.Projects

del ..\Byt3\Applications\FLDebugger.Projects\bin\Debug\Byt3.AutoUpdate.exe