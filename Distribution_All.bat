@echo off

set BASE_PATH=%cd%
rem 개인프로젝트 - 무제
xcopy /Y "%BASE_PATH%\DagraacSystems\bin\Release\netstandard2.0\DagraacSystems.dll" "D:\Projects\Game\Assets\Plugins\"
rem xcopy /Y "%BASE_PATH%\DagraacSystemsUnity\bin\Release\netstandard2.0\DagraacSystemsUnity.dll" "D:\Projects\Game\Assets\Plugins\"
rem xcopy /Y "%BASE_PATH%\TableExporter\Build\TableExporter.exe" "D:\Projects\Game\ExternalTools\"

pause