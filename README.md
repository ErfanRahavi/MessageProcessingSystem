# Message Processing System

## Project Overview

The **Message Processing System** is a .NET-based solution designed to process messages from a simulated queue. The solution contains multiple microservices that perform specific tasks such as message distribution, message processing, and system health checking.

This project showcases skills in building scalable, modular .NET services with features like gRPC communication, regular expression analysis, and distributed system management.

## Components

### 1. **Management System**
   - Built with ASP.NET Core, this service handles health check requests.
   - Exposes an endpoint:
     - `POST /api/module/health`
     - This API returns the system's status, including the number of active clients, the expiration time, and whether the system is enabled.

### 2. **Message Distributor**
   - This service:
     - Reads messages from a queue (simulated by generating random messages).
     - Sends messages to the message processor service.
     - Logs the processed message results.
     - Handles the periodic health check request to the management system.

### 3. **Message Processor**
   - A gRPC service responsible for analyzing and processing messages sent by the distributor.
   - Performs regex analysis on messages based on predefined rules.
   - Returns processed results including message validity, length, and any regex match details.

## API Documentation

### **Management System API**
- **Endpoint**: `/api/module/health`
- **Method**: `POST`
- **Request Body**:
  ```json
  {
    "Id": "string", 
    "SystemTime": "string", 
    "NumberofConnectedClients": 5
  }
- **Response Body**:
  ```json
  {
  "IsEnabled": true, 
  "NumberOfActiveClients": 3, 
  "ExpirationTime": "string"
  }
  
## Setup Instructions

1. **Clone the repository**:
   ```bash
   git clone https://github.com/ErfanRahavi/MessageProcessingSystem.git
   cd MessageProcessingSystem
   
2. **Restore dependencies**:
   ```bash
   dotnet restore
   
3. **Build the solution**:
   ```bash
   dotnet build
   
4. **Run the services: Navigate to each service directory (MessageDistributor, MessageProcessor, ManagementSystem) and run**:
   ```bash
   dotnet run
   
5. **Run tests**:
   ```bash
   dotnet test


