FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /code
ENV TZ=Asia/Taipei
COPY ./src/. /code

# https://stackoverflow.com/a/72727052
RUN dotnet tool install --global dotnet-ef
ENV PATH="$PATH:/root/.dotnet/tools"

RUN chmod +x /code/entrypoint.sh
RUN dotnet restore
ENTRYPOINT ["./entrypoint.sh"]