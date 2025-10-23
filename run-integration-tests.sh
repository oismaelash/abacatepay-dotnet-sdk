#!/bin/bash

echo "Running AbacatePay Integration Tests..."

# Verificar se o arquivo .env existe
if [ ! -f "tests/AbacatePay.IntegrationTests/.env" ]; then
    echo "❌ Error: .env file not found in tests/AbacatePay.IntegrationTests/"
    echo "Please create a .env file with your API credentials."
    echo "You can copy env.example to .env and fill in your details:"
    echo "  cp tests/AbacatePay.IntegrationTests/env.example tests/AbacatePay.IntegrationTests/.env"
    echo ""
    echo "Required variables:"
    echo "  ABACATEPAY_API_KEY=your_api_key_here"
    echo "  ABACATEPAY_SANDBOX=true"
    exit 1
fi

# Verificar se a API key está definida
if ! grep -q "ABACATEPAY_API_KEY=" tests/AbacatePay.IntegrationTests/.env || grep -q "ABACATEPAY_API_KEY=your_api_key_here" tests/AbacatePay.IntegrationTests/.env; then
    echo "❌ Error: ABACATEPAY_API_KEY not configured in .env file"
    echo "Please set your real API key in tests/AbacatePay.IntegrationTests/.env"
    exit 1
fi

# Verificar se está em modo sandbox
if ! grep -q "ABACATEPAY_SANDBOX=true" tests/AbacatePay.IntegrationTests/.env; then
    echo "⚠️  Warning: ABACATEPAY_SANDBOX is not set to true"
    echo "Integration tests should run in sandbox mode to avoid charges"
    echo "Please set ABACATEPAY_SANDBOX=true in your .env file"
    read -p "Do you want to continue anyway? (y/N): " -n 1 -r
    echo
    if [[ ! $REPLY =~ ^[Yy]$ ]]; then
        exit 1
    fi
fi

echo "✅ Environment configuration validated"
echo ""

# Restaurar pacotes
echo "Restoring packages..."
dotnet restore

# Executar testes de integração
echo "Running integration tests..."
dotnet test tests/AbacatePay.IntegrationTests/ --verbosity normal

# Verificar resultado
if [ $? -eq 0 ]; then
    echo ""
    echo "✅ Integration tests completed successfully!"
    echo "All tests passed against the real AbacatePay API"
else
    echo ""
    echo "❌ Integration tests failed!"
    echo "Please check the test output above for details"
    exit 1
fi
