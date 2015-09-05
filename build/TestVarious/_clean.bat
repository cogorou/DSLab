@echo off

@rem ==================================================
@rem ENVIRONMENT VARIABLE

@rem ==================================================
@rem JOB
:JOB

del /q /s *.user
del /q /s *.aps
del /q /s *.ncb
del /q /s *.log
del /q /s *.suo
del /q /s /AH *.suo
del /q /s BuildLog.htm

rmdir /s /q obj
rmdir /s /q bin
rmdir /s /q v2.0
rmdir /s /q v4.0
rmdir /s /q v4.5
