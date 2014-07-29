@ECHO OFF

%SystemRoot%\Microsoft.NET\Framework\v4.0.30319\msbuild "%~dp0\Bohrium.msbuild" /t:%1 /v:minimal /maxcpucount /nodeReuse:false