@echo off

set PREFIX=demo
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

call :MAKE Rebuild AnyCPU Release
if ERRORLEVEL 1 goto :EOF

exit /b

@rem ==================================================
@rem MAKE
:MAKE

set COMMAND=%1
set PLATFORM=%2
set SOLUTION=%3

echo %PROJECT% - %COMMAND% %PLATFORM% %SOLUTION%
%MSBUILD_EXE% %PROJECT% /t:%COMMAND% /p:Platform=%PLATFORM% /p:Configuration=%SOLUTION% > %PREFIX%.log 2>&1
if ERRORLEVEL 1 (
	echo error occured.
	pause
	goto :EOF
)

exit /b
