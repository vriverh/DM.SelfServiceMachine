# DM.LocalServices Solution

This solution refactors the original DM.SelfServiceMachine project into a modular, cross-platform architecture using .NET 8.

## Project Structure

### 1. DM.LocalServices.Models
Contains shared data models and DTOs used across all projects.
- **Target Framework**: .NET 8.0
- **Purpose**: Common data models for device information, printer status, read info, etc.

### 2. DM.LocalServices.Device  
Hardware abstraction layer providing device-specific implementations.
- **Target Framework**: .NET 8.0
- **Purpose**: Hardware device interfaces (ID card readers, face cameras, printers)
- **Features**: 
  - Cross-platform device support through dependency injection
  - Windows-specific printer functionality
  - ID card reader SDKs (ZKTeco, GHC, HS)
  - Face camera integration

### 3. DM.LocalServices.Repository
Business logic and data access layer.
- **Target Framework**: .NET 8.0
- **Purpose**: Repository pattern implementation for device operations
- **Features**:
  - Local and virtual repository implementations
  - PDF processing capabilities
  - Device-specific repositories for different hardware types

### 4. DM.LocalServices.API
RESTful API service providing hardware interfaces.
- **Target Framework**: .NET 8.0
- **Purpose**: Web API for hardware device control
- **Features**:
  - Auto-updater service
  - Client registration service  
  - Hardware control endpoints (face camera, ID card reader, printer)
  - Swagger/OpenAPI documentation
  - Cross-platform service (no WPF dependencies)

### 5. DM.LocalServices.WebView
Cross-platform UI application using Avalonia framework.
- **Target Framework**: .NET 8.0
- **Purpose**: WebView-based user interface
- **Features**:
  - Cross-platform support (Windows/Linux)
  - Modern Avalonia UI framework
  - WebView integration (placeholder for future implementation)
  - Responsive design

## Key Improvements

### Architecture
- **Modular Design**: Clear separation of concerns across 5 focused projects
- **Dependency Injection**: Proper DI configuration for cross-platform device support
- **Cross-Platform**: Supports both Windows and Linux environments

### Technology Upgrades
- **Upgraded to .NET 8**: Latest LTS version with performance improvements
- **Avalonia UI**: Modern, cross-platform UI framework replacing WPF
- **API-First**: Decoupled API service for better scalability

### Development Experience
- **Better Maintainability**: Clear project boundaries and responsibilities
- **Easier Testing**: Each layer can be unit tested independently
- **Flexible Deployment**: API and UI can be deployed separately

## Building and Running

### Prerequisites
- .NET 8.0 SDK
- Windows (for hardware-specific functionality) or Linux

### Build All Projects
```bash
dotnet build DM.LocalServices.sln
```

### Run API Service
```bash
dotnet run --project DM.LocalServices.API
```
API will be available at: http://127.0.0.1:9180

### Run WebView Application
```bash
dotnet run --project DM.LocalServices.WebView
```

## Configuration

Configuration files are located in the API project:
- `appsettings.json` - Base configuration
- `appsettings.Development.json` - Development overrides
- `appsettings.Production.json` - Production overrides

## Third-Party Dependencies

Some third-party libraries require additional setup:
- **brQueryPrinterLib**: Brother printer query library (placeholder stub provided)
- **Hardware SDKs**: Various ID card reader and face camera SDKs

## Future Enhancements

- **WebView Integration**: Full web browser functionality in Avalonia app
- **Real-time Communication**: SignalR for API-WebView communication
- **Enhanced Device Support**: Additional device drivers and abstractions
- **Container Support**: Docker containerization for easy deployment

## Migration Notes

This solution maintains the core functionality of the original DM.SelfServiceMachine project while providing:
- Better separation of concerns
- Cross-platform compatibility  
- Modern .NET 8 features
- Improved maintainability and testability

The original WPF-specific code has been abstracted to allow for cross-platform operation while preserving business logic and hardware integration capabilities.