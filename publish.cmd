@echo off
dotnet publish -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true
echo.
echo Copy publish folder somewhere and add it to path
