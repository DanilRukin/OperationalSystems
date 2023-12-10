FROM mcr.microsoft.com/dotnet/sdk:7.0 as build

WORKDIR /app

COPY . ./

RUN dotnet restore


RUN dotnet publish --no-restore -c Release -o published

FROM mcr.microsoft.com/dotnet/aspnet:7.0 as runtime

# Uncomment the line below if running with HTTPS
# ENV ASPNETCORE_URLS=https://+:443

WORKDIR /app

COPY --from=build /app/published .

ENTRYPOINT [ "dotnet", "Messanger.Server.dll" ]
