@echo Sync Hotfix
cd /d %~dp0

set SourceDebugDir=%cd%\..\Temp\Bin\Debug
set UnityCodePath=%cd%\..\Assets\Bundles\Code
set ToolPath=%cd%\..\Tools\Pdb2Mdb\4.5

copy %SourceDebugDir%\Hotfix.dll %UnityCodePath%\Hotfix.dll.bytes
copy %SourceDebugDir%\Hotfix.pdb %UnityCodePath%\Hotfix.pdb.bytes
copy %SourceDebugDir%\Hotfix.dll %ToolPath%\Hotfix.dll
copy %SourceDebugDir%\Hotfix.pdb %ToolPath%\Hotfix.pdb

%ToolPath%\pdb2mdb.bat

pause