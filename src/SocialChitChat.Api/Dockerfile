#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/SocialChitChat.Api/SocialChitChat.Api.csproj", "src/SocialChitChat.Api/"]
COPY ["src/SocialChitChat.Business/SocialChitChat.Business.csproj", "src/SocialChitChat.Business/"]
COPY ["src/SocialChitChat.DataAccess/SocialChitChat.DataAccess.csproj", "src/SocialChitChat.DataAccess/"]
RUN dotnet restore "./src/SocialChitChat.Api/SocialChitChat.Api.csproj"
COPY . .
WORKDIR "/src/src/SocialChitChat.Api"
RUN dotnet build "./SocialChitChat.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./SocialChitChat.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SocialChitChat.Api.dll"]