FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src

RUN cd ../. && cd src && ls -la

COPY ["./Godel.HelloWorld/Godel.HelloWorld.csproj", "Godel.HelloWorld/"]
RUN dotnet restore "Godel.HelloWorld/Godel.HelloWorld.csproj"

COPY . .
WORKDIR "/src"

RUN dotnet build "./Godel.HelloWorld/Godel.HelloWorld.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "./Godel.HelloWorld/Godel.HelloWorld.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Godel.HelloWorld.dll"]