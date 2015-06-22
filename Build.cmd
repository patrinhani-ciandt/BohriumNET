@ECHO OFF

%SystemRoot%\Microsoft.NET\Framework\v4.0.30319\msbuild "%~dp0\%1.msbuild" /t:%2 %3 /v:n /maxcpucount /nodeReuse:false

rem for Debug purposes
rem %SystemRoot%\Microsoft.NET\Framework\v4.0.30319\msbuild "%~dp0\%1.msbuild" /t:%2 %3 /fl /flp:logfile=%~dp0\%1.msbuild.log /maxcpucount /nodeReuse:false