#!/bin/bash

#delete old proto files
rm -rf ..\Proto\*.cs
#generate new proto files

cd OriginalProto
protoc ".\*" --csharp_out=..\..\Proto\