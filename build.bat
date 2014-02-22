call "%VS110COMNTOOLS%"\\vsvars32.bat
call "%VS100COMNTOOLS%"\\vsvars32.bat
call "%VS120COMNTOOLS%"\\vsvars32.bat
msbuild CSCore.sln /p:Configuration=Release /p:Platform="Any CPU"
pause