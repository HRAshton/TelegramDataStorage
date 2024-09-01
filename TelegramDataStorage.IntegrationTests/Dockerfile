FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

ARG BUILD_CONFIGURATION=Release

WORKDIR /src
COPY ["TelegramDataStorage.IntegrationTests/TelegramDataStorage.IntegrationTests.csproj", "TelegramDataStorage.IntegrationTests/"]
COPY ["TelegramDataStorage/TelegramDataStorage.csproj", "TelegramDataStorage/"]
RUN dotnet restore "TelegramDataStorage.IntegrationTests/TelegramDataStorage.IntegrationTests.csproj"

FROM build AS test

ARG BOT_TOKEN
ARG CHAT_ID

ENV TelegramDataStorage__BotToken=$BOT_TOKEN
ENV TelegramDataStorage__ChatId=$CHAT_ID

COPY . .
WORKDIR "/src/TelegramDataStorage.IntegrationTests"
RUN dotnet test "TelegramDataStorage.IntegrationTests.csproj" -c $BUILD_CONFIGURATION -o /app/build
