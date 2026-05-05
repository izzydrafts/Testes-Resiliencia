# Testes-Resiliencia - Testes Automatizados

Projeto de estudos da disciplina de Arquitetura de Aplicações Web, focado em testes automatizados em uma API de pedidos.

##  Tecnologias utilizadas

- .NET 8
- C#
- xUnit
- Moq
- FluentAssertions
- RestSharp
- Polly

---

##  Objetivo

Implementar testes unitários e de integração para validar o funcionamento de uma API de pedidos, garantindo:

- Regras de negócio corretas
- Respostas corretas da API
- Tratamento de erros
- Resiliência com tentativas automáticas

---

##  Tipos de testes

### ✔ Unit Tests
- Cálculo de total com desconto
- Validação de regras de negócio
- Verificação de exceções
- Uso de mocks (Moq)

### ✔ Integration Tests
- GET /api/orders
- POST /api/orders (válido)
- POST /api/orders (inválido)

### ✔ Bônus (Polly)
- Política de retry com 3 tentativas
- Simulação de falha e reexecução automática

---

##  Como executar o projeto

```bash
dotnet restore
dotnet build
dotnet test
