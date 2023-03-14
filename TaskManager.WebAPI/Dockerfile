#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 5020

RUN apt update && apt install -y curl

ENV ASPNETCORE_ENVIRONMENT="Development"
ENV ASPNETCORE_URLS="http://*:5020"
ENV RABBITMQ_CONNECTION_STRING="rabbitserver"
ENV RABBITMQ_CONNECTION_USER="guest"
ENV RABBITMQ_CONNECTION_PASS="guest"
ENV RABBITMQ_CONNECTION_RETRIES="3"
ENV RABBITMQ_QUEUE_NAME="KanbanTaskQueue"
ENV MONGODB_CONNECTION_STRING="mongodb+srv://default:guestp@mycluster.ql0r2.gcp.mongodb.net?retryWrites=true&w=majority"
ENV MONGODB_DATABASE_NAME="TaskManagerDB"

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["TaskManager.WebAPI/TaskManager.WebAPI.csproj", "TaskManager.WebAPI/"]
RUN dotnet restore "TaskManager.WebAPI/TaskManager.WebAPI.csproj"
COPY . .
WORKDIR "/src/TaskManager.WebAPI"
RUN dotnet build "TaskManager.WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TaskManager.WebAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TaskManager.WebAPI.dll"]