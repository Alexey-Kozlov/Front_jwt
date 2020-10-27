FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 5008

ENV ASPNETCORE_Kestrel__Endpoints__Http__Url="http://ws-pc-70:5008"
ENV ASPNETCORE_URLS=http://+:5008
ENV ServiceUrls__AuthorityApiEndpoint="http://ws-pc-70:5001"
ENV ServiceUrls__WebApiEndpoint="http://ws-pc-70:5007"
ENV ServiceUrls__DefaultRedirectUri="http://ws-pc-70:5008"
ENV Certificates__Path="WsPc70.pfx"
ENV Certificates__Password="Test"

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Front_jwt.csproj", "./"]


RUN dotnet restore "./Front_jwt.csproj"

COPY . .

WORKDIR "/src/."

RUN dotnet build "Front_jwt.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Front_jwt.csproj" -c Release -o /app


COPY ["WsPc70.pfx", "/app/WsPc70.pfx"]
#RUN update-ca-certificates

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Front_jwt.dll"]