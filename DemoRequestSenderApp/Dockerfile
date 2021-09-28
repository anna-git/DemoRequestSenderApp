#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 84

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["DemoRequestSenderApp/DemoRequestSenderApp.csproj", "DemoRequestSenderApp/"]
RUN dotnet restore "DemoRequestSenderApp/DemoRequestSenderApp.csproj"
COPY . .
WORKDIR "/src/DemoRequestSenderApp"
RUN dotnet build "DemoRequestSenderApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DemoRequestSenderApp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DemoRequestSenderApp.dll"]