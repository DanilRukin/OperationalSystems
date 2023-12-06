@echo off

dotnet tool update --global dotnet-ef


set /p magration_name=Migration name: 

dotnet ef migrations add %magration_name% -c MessangerDataContext -o Data/Migrations/MessangerDb
dotnet ef database update -c MessangerDataContext

echo Для продолжения нажмите любую клавишу

pause
