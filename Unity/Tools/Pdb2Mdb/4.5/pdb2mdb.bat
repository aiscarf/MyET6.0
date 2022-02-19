@echo Generate mdb.dll
cd /d %~dp0
set UnityCodePath=%cd%\..\..\..\Assets\Bundles\Code
D:\Unity\2020.3.3f1c1\Unity\Editor\Data\MonoBleedingEdge\bin\mono.exe .\pdb2mdb.exe Hotfix.dll
copy .\Hotfix.dll.mdb %UnityCodePath%\Hotfix.mdb.bytes
pause