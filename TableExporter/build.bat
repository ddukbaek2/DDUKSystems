@echo off

:: 파이썬 설치 경로.
set MODULE_PATH=C:\Program Files (x86)\Microsoft Visual Studio\Shared\Python37_64\Scripts

:: 프로젝트 경로.
set PROJECT_PATH=%cd%

:: 빌드 결과물이 출력될 경로.
set BUILD_PATH=%PROJECT_PATH%\Build

:: 빌드 대상인 스크립트 경로.
set SCRIPT_NAME=TableExporter.py

:: 프로그램 이름
set APP_NAME=TableExporter

C:
cd %MODULE_PATH%
md %BUILD_PATH%\Spec
md %BUILD_PATH%\Work

pyinstaller^
 --onefile %PROJECT_PATH%\%SCRIPT_NAME%^
 --specpath %BUILD_PATH%\Spec^
 --distpath %BUILD_PATH%^
 --workpath %BUILD_PATH%\Work^
 --name %APP_NAME%

rmdir /s/q %PROJECT_PATH%\__pycache__

pause