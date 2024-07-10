@echo off
del /s /f ..\Common\Proto\*.cs
cd OriginalProto
protoc ".\*" --csharp_out=..\..\Common\Proto\