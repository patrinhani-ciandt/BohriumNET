@ECHO OFF

%SystemRoot%\Microsoft.NET\Framework\v4.0.30319\msbuild "%~dp0\%1.msbuild" /t:%2 %3 /v:minimal /maxcpucount /nodeReuse:false