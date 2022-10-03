#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["DotNetExperiments.API/DotNetExperiments.API.csproj", "DotNetExperiments.API/"]
RUN dotnet restore "DotNetExperiments.API/DotNetExperiments.API.csproj"
COPY . .
WORKDIR "/src/DotNetExperiments"
RUN dotnet build "DotNetExperiments.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DotNetExperiments.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DotNetExperiments.API.dll"]