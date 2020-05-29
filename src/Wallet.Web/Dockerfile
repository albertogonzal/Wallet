FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["src/Wallet.Web/Wallet.Web.csproj", "src/Wallet.Web/"]
RUN dotnet restore "src/Wallet.Web/Wallet.Web.csproj"
COPY . .
WORKDIR "/src/src/Wallet.Web"
RUN dotnet build "Wallet.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Wallet.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Wallet.Web.dll"]
