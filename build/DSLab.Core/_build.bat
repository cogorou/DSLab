@echo off

set PREFIX=DSLab.Core
set PROJECT=%PREFIX%.csproj

@rem ==================================================
@rem ENVIRONMENT VARIABLE

set MSBUILD_EXE=%WinDir%\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe

@if "%FRAMEWORK%"=="" set FRAMEWORK=v4.0

@rem ==================================================
@rem START
:START

call :JOB v4.0
if ERRORLEVEL 1 goto :EOF

exit /b

@rem ==================================================
@rem JOB
:JOB

set FRAMEWORK=%1

echo FrameworkVersion=%FRAMEWORK%

call :MAKE clean AnyCPU Debug
call :MAKE clean AnyCPU Release

if exist %PREFIX%.log del %PREFIX%.log

call :MAKE build AnyCPU Debug
if ERRORLEVEL 1 goto :EOF

call :MAKE build AnyCPU Release
if ERRORLEVEL 1 goto :EOF

echo [%time%]

exit /b

@rem ==================================================
@rem MAKE
:MAKE

set COMMAND=%1
set PLATFORM=%2
set SOLUTION=%3

set DEFINECONSTANTS="
if "%LINUX%"=="1"    set DEFINECONSTANTS=%DEFINECONSTANTS%LINUX;
set DEFINECONSTANTS=%DEFINECONSTANTS%"

echo [%time%] %PROJECT% - %COMMAND% %PLATFORM% %SOLUTION%
%MSBUILD_EXE% %PROJECT% /t:%COMMAND% /p:Platform=%PLATFORM% /p:Configuration=%SOLUTION% /p:DefineConstants=%DEFINECONSTANTS% >> %PREFIX%.log 2>&1
if ERRORLEVEL 1 (
	echo error occured.
	pause
	goto :EOF
)

exit /b
