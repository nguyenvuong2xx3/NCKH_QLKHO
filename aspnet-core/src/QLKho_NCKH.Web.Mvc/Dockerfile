FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

RUN dotnet tool install -g Microsoft.Web.LibraryManager.Cli
RUN ln -s ~/.dotnet/tools/libman /usr/local/bin/libman

WORKDIR /src
COPY ["src/QLKho_NCKH.Web.Mvc/QLKho_NCKH.Web.Mvc.csproj", "src/QLKho_NCKH.Web.Mvc/"]
COPY ["src/QLKho_NCKH.Web.Core/QLKho_NCKH.Web.Core.csproj", "src/QLKho_NCKH.Web.Core/"]
COPY ["src/QLKho_NCKH.Application/QLKho_NCKH.Application.csproj", "src/QLKho_NCKH.Application/"]
COPY ["src/QLKho_NCKH.Core/QLKho_NCKH.Core.csproj", "src/QLKho_NCKH.Core/"]
COPY ["src/QLKho_NCKH.EntityFrameworkCore/QLKho_NCKH.EntityFrameworkCore.csproj", "src/QLKho_NCKH.EntityFrameworkCore/"]
WORKDIR "/src/src/QLKho_NCKH.Web.Mvc"
RUN dotnet restore 

WORKDIR /src
COPY ["src/QLKho_NCKH.Web.Mvc", "src/QLKho_NCKH.Web.Mvc"]
COPY ["src/QLKho_NCKH.Web.Core", "src/QLKho_NCKH.Web.Core"]
COPY ["src/QLKho_NCKH.Application", "src/QLKho_NCKH.Application"]
COPY ["src/QLKho_NCKH.Core", "src/QLKho_NCKH.Core"]
COPY ["src/QLKho_NCKH.EntityFrameworkCore", "src/QLKho_NCKH.EntityFrameworkCore"]
WORKDIR "/src/src/QLKho_NCKH.Web.Mvc"
RUN libman restore
RUN dotnet publish -c Release -o /publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0
EXPOSE 80
WORKDIR /app
COPY --from=build /publish .
ENTRYPOINT ["dotnet", "QLKho_NCKH.Web.Mvc.dll"]
