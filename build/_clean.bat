@echo off

@rem ==================================================
@rem ENVIRONMENT VARIABLE

@rem ==================================================
@rem JOB
:JOB

pushd DSLab.Core
call _clean.bat
popd

pushd TestPanTilt
call _clean.bat
popd

pushd TestPlayer
call _clean.bat
popd

pushd TestVarious
call _clean.bat
popd

@rem ==================================================
@rem common
