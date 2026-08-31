# 1. Aşama: Uygulamayı Derleme (Build)
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /source
COPY . .
RUN dotnet restore
RUN dotnet publish -c Release -o /app

# 2. Aşama: Uygulamayı Çalıştırma (Run)
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app .

# Sunucunun dışarıya açacağı portu belirliyoruz
EXPOSE 8080
ENV ASPNETCORE_HTTP_PORTS=8080
ENTRYPOINT ["dotnet", "Adalyn.API.dll"]