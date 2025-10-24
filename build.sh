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

# Pack the NuGet package
echo "Packing NuGet package..."
dotnet pack src/AbacatePay/AbacatePay.csproj --configuration Release --no-build --output ./packages

echo "Build completed successfully!"
echo "NuGet package created in ./packages directory"
