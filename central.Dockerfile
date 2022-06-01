FROM mcr.microsoft.com/dotnet/sdk:5.0 as build

WORKDIR /source
COPY ./Flash.sln ./Flash.sln
COPY ./nuget.config ./nuget.config

COPY ./src/Flash.Domain/Flash.Domain.csproj ./src/Flash.Domain/
COPY ./src/Flash.Central.Api/Flash.Central.Api.csproj ./src/Flash.Central.Api/
COPY ./src/Flash.Central.Data/Flash.Central.Data.csproj ./src/Flash.Central.Data/
COPY ./src/Flash.Central.Dtos/Flash.Central.Dtos.csproj ./src/Flash.Central.Dtos/
COPY ./src/Flash.Central.Core/Flash.Central.Core.csproj ./src/Flash.Central.Core/
COPY ./src/Flash.Central.ViewModel/Flash.Central.ViewModel.csproj ./src/Flash.Central.ViewModel/
COPY ./src/Flash.Central.AdminApi/Flash.Central.AdminApi.csproj ./src/Flash.Central.AdminApi/
COPY ./src/Flash.Central.Foundation/Flash.Central.Foundation.csproj ./src/Flash.Central.Foundation/
COPY ./src/Flash.Central.Jobs/Flash.Central.Jobs.csproj ./src/Flash.Central.Jobs/

RUN dotnet restore

COPY ./src ./src

RUN dotnet build ./src/Flash.Central.Api/Flash.Central.Api.csproj -c Release -o /build

############################################3

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS result
WORKDIR /app
EXPOSE 80

RUN apt-get update && apt-get install -y libgdiplus postgresql-client

ENTRYPOINT [ "/bin/bash" ]
CMD ["/app/central-start.sh"]

COPY ./deploy/central-start.sh ./deploy/wait-for-postgresql.sh /app/
RUN chmod +x /app/central-start.sh
COPY --from=build /build /app
