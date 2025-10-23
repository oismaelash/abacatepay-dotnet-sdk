#!/bin/bash

echo "Building AbacatePay .NET SDK..."

# Clean previous builds
echo "Cleaning previous builds..."
dotnet clean

# Restore packages
echo "Restoring packages..."
dotnet restore

# Build the solution
echo "Building solution..."
dotnet build --configuration Release

# Run unit tests (always)
echo "Running unit tests..."
dotnet test tests/AbacatePay.Tests/ --configuration Release --no-build

# Run integration tests (only if API key is provided)
if [ -f "tests/AbacatePay.IntegrationTests/.env" ] && grep -q "ABACATEPAY_API_KEY=" tests/AbacatePay.IntegrationTests/.env && ! grep -q "ABACATEPAY_API_KEY=your_api_key_here" tests/AbacatePay.IntegrationTests/.env; then
    echo "Running integration tests..."
    dotnet test tests/AbacatePay.IntegrationTests/ --configuration Release --no-build
else
    echo "Skipping integration tests (no API key configured)"
    echo "To run integration tests, configure your API key in tests/AbacatePay.IntegrationTests/.env"
fi

# Pack the NuGet package
echo "Packing NuGet package..."
dotnet pack src/AbacatePay/AbacatePay.csproj --configuration Release --no-build --output ./packages

echo "Build completed successfully!"
echo "NuGet package created in ./packages directory"
