@echo off
cd BuildUtilities
call AsmGen_GenerateConfig.bat
call AsmGen_Build.bat
pause