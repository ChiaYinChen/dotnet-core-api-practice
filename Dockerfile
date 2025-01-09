FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /code
ENV TZ=Asia/Taipei
COPY ./src/. /code
RUN dotnet tool restore
RUN chmod +x /code/entrypoint.sh
RUN dotnet restore
ENTRYPOINT ["./entrypoint.sh"]