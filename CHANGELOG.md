# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## 1.3.0

### How to migrate

Some interfaces were changed to hide the library implementation.
If you have implemented any of these interfaces, you will need to update your implementation.

### Changed

- Update dependencies.
- Hide library implementation from interfaces.
- Split `TelegramDataStorage.Core`, `TelegramDataStorage.TelegramBot` and `TelegramDataStorage.WTelegramBot`.

## 1.2.1

### How to migrate

No migration is needed.

### Fixed

- Fixed a bug where updating an existing entity throw an exception due to no changes being detected.

## 1.2.0

### How to migrate

In most cases you can just update the package and everything should work.
If you have custom converters, you will need to update them to use the new `System.Text.Json` API.

See the Microsoft migration guide for more information:
https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/migrate-from-newtonsoft

### Changed

- Migrated from Newtonsoft.Json to System.Text.Json.
- Updated dependencies.

## 1.1.0

### Changed

- Exceptions and interfaces are now in the Abstractions project.

### 1.0.0

Initial release.
