FROM mcr.microsoft.com/dotnet/aspnet:8.0 as base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 as build
WORKDIR /src
COPY . ./
RUN dotnet restore
COPY . .
WORKDIR /src/
RUN dotnet build Messanger.Client.csproj -c Release -o /app/build

FROM build AS publish
RUN dotnet publish Messanger.Client.csproj -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENV DOTNET_EnableDiagnostics=0
ENTRYPOINT ["dotnet", "Messanger.Client.dll"]
