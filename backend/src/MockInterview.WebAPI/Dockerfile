FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["MockInterview.WebAPI/MockInterview.WebAPI.csproj", "MockInterview.WebAPI/"]
RUN dotnet restore "MockInterview.WebAPI/MockInterview.WebAPI.csproj"
COPY . .
WORKDIR "/src/MockInterview.WebAPI"
RUN dotnet build "MockInterview.WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MockInterview.WebAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MockInterview.WebAPI.dll"]
