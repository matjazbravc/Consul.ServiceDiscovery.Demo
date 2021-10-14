#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/ValueService.OpenApi/ValueService.OpenApi.csproj", "src/ValueService.OpenApi/"]
RUN dotnet restore "src/ValueService.OpenApi/ValueService.OpenApi.csproj"
COPY . .
WORKDIR "/src/src/ValueService.OpenApi"
RUN dotnet build "ValueService.OpenApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ValueService.OpenApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ValueService.OpenApi.dll"]