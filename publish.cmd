@echo off
dotnet publish -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true
echo.
mkdir %utils%\todo
xcopy /e /i /y bin\Release\net5.0\win-x64\publish %utils%\todo
echo Add %utils%\todo to the path variable
