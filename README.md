# 🧾 PartnerFlow API

**PartnerFlow** é uma API de pedidos desenvolvida como parte de um **teste técnico para a consultoria Venice**.  
Ela integra diferentes tecnologias para armazenar dados, distribuir mensagens e aplicar boas práticas de arquitetura de software.

---

## 🔧 Tecnologias Utilizadas

- **.NET 9**
- **SQL Server**
- **MongoDB**
- **Kafka**
- **Kafka UI**
- **Redis**
- **JWT**
- **Docker**

---

## 🧠 Arquitetura & Boas Práticas

Este projeto segue **principais boas práticas de desenvolvimento**:

### ✅ Clean Architecture

Separação clara de responsabilidades entre camadas:

- `Domain`: regras de negócio puras.
- `Application`: casos de uso (serviços e validações).
- `Infrastructure`: implementações específicas (banco, cache, mensageria).
- `API`: entrada/saída e controle da aplicação.
- `Tests`: testes unitários (xUnit & NSubstitute).

### ✅ DDD (Domain-Driven Design)

- Entidades ricas como `Pedido` e `ItemPedido`
- Uso de `DTOs` para comunicação
- Repositórios e serviços desacoplados por interfaces

---

## ✅ Design Patterns Aplicados

### 📨 Publisher (Observer via Kafka)
> Utilizamos um publisher para enviar eventos ao Kafka quando um pedido é criado.

**Justificativa:**

O padrão **Publisher/Subscriber** (também conhecido como Observer) garante **baixo acoplamento** entre o produtor de eventos (neste caso, a aplicação que cria pedidos) e os consumidores (outros sistemas que podem reagir a esses eventos). Isso permite:

- Escalabilidade horizontal
- Integração com múltiplos consumidores sem alteração no produtor
- Facilidade de manutenção e extensão do sistema

---

### 📂 Repository Pattern
> Cada tipo de dado (ex: `Pedido`, `ItemPedido`) possui seu repositório, responsável pela persistência em SQL Server ou MongoDB.

**Justificativa:**

O padrão **Repository** permite:

- **Abstrair a lógica de acesso a dados**, desacoplando a aplicação da tecnologia de armazenamento.
- Substituir ou evoluir a infraestrutura sem impactar a camada de negócio.
- Facilitar testes unitários por meio da injeção de dependência e uso de mocks.
- Centralizar as regras de persistência de dados.

---

## 🚀 Como Executar

### ✅ Requisitos

- Docker Desktop **ou** WSL 2
- .NET 9 SDK
- EF CLI

---

📌 Observações Finais
O projeto segue padrão Clean Architecture, com separação clara entre camadas.
Aplicação estruturada para escalabilidade, com uso de mensageria, cache e persistência híbrida.
Possui autenticação JWT integrada e documentada no Swagger para facilitar testes.
Foi desenvolvido com foco em boas práticas e clareza, voltada ao ambiente corporativo e/ou microserviços distribuídos.

### Subir a aplicação

```bash
docker-compose up --build

---

## 📦 Execução de Migrations
> ⚠️ Esse passo é necessário apenas ao rodar o projeto **fora do Docker**, usando o SDK local.

```bash
dotnet ef database update --project PartnerFlow.Infrastructure --startup-project PartnerFlow.API

🔐 Autenticação
A API possui autenticação via JWT.

📥 Endpoint de Login
POST /api/auth/login

📤 Corpo da requisição:
{
  "usuario": "admin",
  "senha": "123456"
}

🚨 Como usar no Swagger
Clique no botão "Authorize" no Swagger e cole o token nesse formato:

Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...

🔧 Ferramentas de Acesso e Apoio

🔍 Kafka UI
http://localhost:8080/ui/clusters/PartnerFlow/all-topics/pedidos/messages?keySerde=String&valueSerde=String&limit=100

💾 Redis (via RedisInsight)
Host: localhost
Porta: 6379

🍃 MongoDB (via Mongo Compass)
Conexão: mongodb://localhost:27017
Banco: PartnerFlow

🗄️ SQL Server (via DBeavear)
Servidor: localhost,1433
Usuário: sa
Senha: StrongP@ssw0rd
Banco: PartnerFlowDb



