@ECHO OFF
@REM - DAGUR
FOR /F "tokens=1,2,3,4 delims=." %%A IN ('echo %DATE%') DO SET D1=%%B
FOR /F "tokens=1 delims= " %%A IN ('echo %D1%') DO SET D=%%A

@REM - MÁNUÐUR
FOR /F "tokens=1,2,3,4 delims=." %%A IN ('echo %DATE%') DO SET M=%%C

@REM - ÁR
FOR /F "tokens=1,2,3,4 delims=." %%A IN ('echo %DATE%') DO SET Y=%%D

@REM - KLUKKUSTUND
FOR /F "tokens=1,2,3,4 delims=:" %%A IN ('echo %TIME%') DO SET HH=%%A

@REM - MÍNÚTA
FOR /F "tokens=1,2,3,4 delims=:" %%A IN ('echo %TIME%') DO SET MM=%%B

@REM - SEKÚNDA
FOR /F "tokens=1,2,3,4 delims=:" %%A IN ('echo %TIME%') DO SET SS1=%%C
FOR /F "tokens=1,2,3,4 delims= " %%A IN ('echo %SS1%') DO SET SS=%%A

IF EXIST C:\Kaos_TTT\BuildLogs GOTO ALREADYEXISTS
MD C:\Kaos_TTT\BuildLogs

:ALREADYEXISTS
SET LOGFILENAME=C:\Kaos_TTT\BuildLogs\BuildLog_%Y%.%M%.%D%-%HH%.%MM%.%SS%.txt

PowerShell .\Build-Kaos_TicTacToe.ps1 > %LOGFILENAME%
