FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

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
ENV ASPNETCORE_URLS=http://+:5000
ENV ASPNETCORE_ENVIRONMENT=Production

EXPOSE 5000

ENTRYPOINT ["dotnet", "EFinnance.API.dll"] 