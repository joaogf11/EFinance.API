FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy csproj and restore dependencies
COPY ["EFinnance.API.csproj", "./"]
RUN dotnet restore "EFinnance.API.csproj"

# Copy the rest of the files
COPY . .

# Build and publish
RUN dotnet build "EFinnance.API.csproj" -c Release -o /app/build
RUN dotnet publish "EFinnance.API.csproj" -c Release -o /app/publish

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Set environment variables
ENV ASPNETCORE_URLS=http://+:80;http://+:5000
ENV ASPNETCORE_ENVIRONMENT=Development
ENV DOTNET_RUNNING_IN_CONTAINER=true
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=1
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

# Garante que o app.db é copiado para o contêiner
COPY --from=build /app/app.db /app/app.db
RUN chmod 644 /app/app.db

EXPOSE 80
EXPOSE 5000

ENTRYPOINT ["dotnet", "EFinnance.API.dll"] 