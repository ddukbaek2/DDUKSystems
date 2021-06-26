@echo off

set XLSX_NAME=%~n0.xlsx
set JSON_PATH=Assets\Resources\Tables
set CS_PATH=Assets\Scripts\Tables\Data

set CURRENT_PATH=%cd%
set CLIENT_PATH=%cd%\..\..\Client
set APP_PATH=%CLIENT_PATH%\ExternalTools
cd %CLIENT_PATH%
set CLIENT_PATH=%cd%
cd %APP_PATH%
set APP_PATH=%cd%

set XLSX_FULLPATH=%CURRENT_PATH%\%XLSX_NAME%
set JSON_FULLPATH=%CLIENT_PATH%\%JSON_PATH%
set CS_FULLPATH=%CLIENT_PATH%\%CS_PATH%

TableExporter %XLSX_FULLPATH% %JSON_FULLPATH% %CS_FULLPATH%

pause