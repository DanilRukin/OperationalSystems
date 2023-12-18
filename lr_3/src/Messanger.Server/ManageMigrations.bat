@echo off

dotnet tool update --global dotnet-ef


set /p migration_name=Migration name: 

dotnet ef migrations add %migration_name% -c MessangerDataContext -o Data/Migrations/MessangerDb
dotnet ef database update -c MessangerDataContext

echo Для продолжения нажмите любую клавишу

pause
