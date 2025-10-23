# AbacatePay Integration Tests

Este documento explica como configurar e executar os testes de integração com a API real do AbacatePay.

## 📋 Pré-requisitos

1. **API Key do AbacatePay**: Você precisa de uma chave de API válida
2. **Ambiente Sandbox**: Recomendamos usar o ambiente de sandbox para testes
3. **.NET 8.0**: SDK do .NET instalado

## 🔧 Configuração

### 1. Criar arquivo de configuração

Copie o arquivo de exemplo e configure suas credenciais:

```bash
cp tests/AbacatePay.IntegrationTests/env.example tests/AbacatePay.IntegrationTests/.env
```

### 2. Editar arquivo .env

Abra o arquivo `tests/AbacatePay.IntegrationTests/.env` e configure:

```env
# Sua chave de API do AbacatePay (obtenha no dashboard)
ABACATEPAY_API_KEY=sk_test_sua_chave_aqui

# Usar ambiente sandbox (recomendado para testes)
ABACATEPAY_SANDBOX=true

# URL base (opcional, detectada automaticamente)
# ABACATEPAY_BASE_URL=https://sandbox.abacatepay.com

# Timeout em segundos (opcional, padrão: 30)
# ABACATEPAY_TIMEOUT=30
```

### 3. Obter API Key

1. Acesse o [dashboard do AbacatePay](https://dashboard.abacatepay.com)
2. Faça login na sua conta
3. Vá para "Configurações" > "API Keys"
4. Crie uma nova chave de API para testes
5. Copie a chave e cole no arquivo `.env`

## 🚀 Executando os Testes

### Opção 1: Script automatizado (Recomendado)

```bash
./run-integration-tests.sh
```

### Opção 2: Comando direto

```bash
dotnet test tests/AbacatePay.IntegrationTests/ --verbosity normal
```

### Opção 3: Executar testes específicos

```bash
# Apenas testes de pagamento
dotnet test tests/AbacatePay.IntegrationTests/ --filter "PaymentIntegrationTests"

# Apenas testes de cliente
dotnet test tests/AbacatePay.IntegrationTests/ --filter "CustomerIntegrationTests"

# Apenas testes de webhook
dotnet test tests/AbacatePay.IntegrationTests/ --filter "WebhookIntegrationTests"
```

## 📊 Cobertura dos Testes

### ✅ Testes de Pagamento (`PaymentIntegrationTests`)
- Criação de pagamentos PIX
- Busca de pagamentos por ID
- Listagem de pagamentos com filtros
- Cancelamento de pagamentos
- Validação de dados de entrada
- Tratamento de erros

### ✅ Testes de Cliente (`CustomerIntegrationTests`)
- Criação de clientes
- Listagem de clientes
- Validação de dados obrigatórios
- Tratamento de CPFs duplicados

### ✅ Testes de Webhook (`WebhookIntegrationTests`)
- Criação de configurações de webhook
- Atualização de webhooks
- Listagem de webhooks
- Exclusão de webhooks
- Verificação de assinatura

### ✅ Testes de Loja (`StoreIntegrationTests`)
- Busca de informações da loja
- Validação de dados da loja

## 🔒 Segurança

### ⚠️ Importante
- **NUNCA** commite o arquivo `.env` no Git
- **SEMPRE** use o ambiente sandbox para testes
- **MANTENHA** suas chaves de API seguras
- **ROTACIONE** suas chaves regularmente

### 🛡️ Boas Práticas
- Use variáveis de ambiente em produção
- Configure timeouts apropriados
- Monitore o uso da API
- Limpe recursos criados durante os testes

## 🐛 Solução de Problemas

### Erro: "ABACATEPAY_API_KEY environment variable is required"
- Verifique se o arquivo `.env` existe
- Confirme se a chave está configurada corretamente
- Certifique-se de que não há espaços extras

### Erro: "Failed to connect to AbacatePay API"
- Verifique sua conexão com a internet
- Confirme se a chave de API é válida
- Verifique se está usando o ambiente correto (sandbox/produção)

### Erro: "Invalid API key"
- Verifique se a chave está correta
- Confirme se a chave não expirou
- Verifique se tem as permissões necessárias

### Timeout nos testes
- Aumente o valor de `ABACATEPAY_TIMEOUT` no arquivo `.env`
- Verifique sua conexão com a internet
- Considere executar os testes em horários de menor tráfego

## 📈 Monitoramento

### Logs dos Testes
Os testes exibem logs detalhados sobre:
- Conexão com a API
- Criação de recursos
- Validação de respostas
- Tempo de execução

### Métricas
- Tempo total de execução
- Taxa de sucesso dos testes
- Recursos criados durante os testes

## 🔄 CI/CD

### GitHub Actions
```yaml
- name: Run Integration Tests
  run: ./run-integration-tests.sh
  env:
    ABACATEPAY_API_KEY: ${{ secrets.ABACATEPAY_API_KEY }}
    ABACATEPAY_SANDBOX: true
```

### Azure DevOps
```yaml
- script: ./run-integration-tests.sh
  env:
    ABACATEPAY_API_KEY: $(ABACATEPAY_API_KEY)
    ABACATEPAY_SANDBOX: true
```

## 📞 Suporte

Se você encontrar problemas:

1. Verifique os logs de erro
2. Confirme a configuração do `.env`
3. Teste a conectividade com a API
4. Consulte a documentação da API
5. Entre em contato com o suporte do AbacatePay

---

**Nota**: Estes testes fazem chamadas reais para a API do AbacatePay. Certifique-se de usar o ambiente sandbox para evitar cobranças desnecessárias.
