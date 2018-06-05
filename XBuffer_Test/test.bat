BuildXBuffer.exe

xbuffer_parser.exe input=custom_class.xb template="templates/csharp_class.ftl" output_dir="output_cs/" suffix=".cs"
xbuffer_parser.exe input=custom_class.xb template="templates/csharp_buffer.ftl" output_dir="output_cs/" suffix="Buffer.cs"

C:/WINDOWS/Microsoft.NET/Framework/v2.0.50727/csc.exe /target:library -out:xbuffer_config.dll /r:xbuffer_runtime.dll output_cs\*.cs

BuildXBufferData.exe

pause