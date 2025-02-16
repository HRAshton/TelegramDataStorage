FROM mcr.microsoft.com/dotnet/sdk:8.0 AS restore

WORKDIR /repo

COPY .editorconfig .editorconfig
COPY TelegramDataStorage.sln TelegramDataStorage.sln
COPY src/TelegramDataStorage.Abstractions/TelegramDataStorage.Abstractions.csproj src/TelegramDataStorage.Abstractions/
COPY src/TelegramDataStorage.Core/TelegramDataStorage.Core.csproj src/TelegramDataStorage.Core/
COPY src/TelegramDataStorage.TelegramBot/TelegramDataStorage.TelegramBot.csproj src/TelegramDataStorage.TelegramBot/
COPY src/TelegramDataStorage.WTelegramBot/TelegramDataStorage.WTelegramBot.csproj src/TelegramDataStorage.WTelegramBot/
COPY tests/TelegramDataStorage.Core.Tests/TelegramDataStorage.Core.Tests.csproj tests/TelegramDataStorage.Core.Tests/
COPY tests/TelegramDataStorage.IntegrationTests.SharedData/TelegramDataStorage.IntegrationTests.SharedData.csproj tests/TelegramDataStorage.IntegrationTests.SharedData/
COPY tests/TelegramDataStorage.TelegramBot.Tests/TelegramDataStorage.TelegramBot.Tests.csproj tests/TelegramDataStorage.TelegramBot.Tests/
COPY tests/TelegramDataStorage.WTelegramBot.Tests/TelegramDataStorage.WTelegramBot.Tests.csproj tests/TelegramDataStorage.WTelegramBot.Tests/

RUN dotnet restore TelegramDataStorage.sln


FROM restore AS build

ARG BUILD_CONFIGURATION=Release

COPY src/ src/
COPY tests/ tests/

RUN dotnet build -c $BUILD_CONFIGURATION --no-restore TelegramDataStorage.sln


FROM build AS test

ARG BUILD_CONFIGURATION=Release

RUN --mount=type=secret,id=bot_token,env=TelegramDataStorage__BotToken \
    --mount=type=secret,id=chat_id,env=TelegramDataStorage__ChatId \
    dotnet test -c $BUILD_CONFIGURATION --no-build "tests/TelegramDataStorage.Core.Tests/TelegramDataStorage.Core.Tests.csproj" \
    && dotnet test -c $BUILD_CONFIGURATION --no-build "tests/TelegramDataStorage.TelegramBot.Tests/TelegramDataStorage.TelegramBot.Tests.csproj" \
    && dotnet test -c $BUILD_CONFIGURATION --no-build "tests/TelegramDataStorage.WTelegramBot.Tests/TelegramDataStorage.WTelegramBot.Tests.csproj"


FROM build AS pack

ARG BUILD_CONFIGURATION=Release

COPY _readme.md .
COPY _license.txt .

RUN dotnet pack -c $BUILD_CONFIGURATION --artifacts-path /output TelegramDataStorage.sln


FROM pack AS result

VOLUME /output

COPY --from=pack /output /output