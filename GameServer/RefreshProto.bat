@echo off
del /s /f ..\Proto\*.cs
cd OriginalProto
protoc ".\*" --csharp_out=..\..\Proto\