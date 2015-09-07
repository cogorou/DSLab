@echo off

@rem ==================================================
@rem ENVIRONMENT VARIABLE

@rem ==================================================
@rem JOB
:JOB

pushd DSLab.Core
call _build.bat
if ERRORLEVEL 1 goto :EOF
popd

pushd TestPanTilt
call _build.bat
if ERRORLEVEL 1 goto :EOF
popd

pushd TestPlayer
call _build.bat
if ERRORLEVEL 1 goto :EOF
popd

pushd TestVarious
call _build.bat
if ERRORLEVEL 1 goto :EOF
popd

@rem ==================================================
@rem common

