
echo "$PWD Run dotnet build"
echo "Stop WebApplicationUnimotWork"
cd /D/Praca/Pumox/src/Pumox.Core.WebApplication
dotnet run -p Pumox.Core.WebApplication.csproj -c Debug -f net5.0
