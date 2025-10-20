# TaskProcessor - Sistema de Processamento de Tarefas

Sistema de processamento de tarefas em background desenvolvido em C#, utilizando MongoDB para persistência e RabbitMQ para comunicação assíncrona.

## Descrição do Projeto

Este projeto implementa um sistema completo de processamento de tarefas em background, atendendo aos seguintes requisitos:

- **API REST** para criação e consulta de tarefas
- **Processamento assíncrono** com workers em background
- **Sistema de retry** com limite configurável de tentativas
- **Controle de concorrência** para múltiplos workers
- **Status das tarefas** (Pendente, EmProcessamento, Concluído, Erro)
- **Escalabilidade** com RabbitMQ para filas de mensagens
- **Containerização** com Docker e Docker Compose

## Arquitetura

```
TaskProcessor/
├── TaskProcessor.API/          # Camada de apresentação (Controllers, Middlewares)
├── TaskProcessor.Application/  # Camada de aplicação (Services, Validators)
├── TaskProcessor.Domain/       # Camada de domínio (Entities, Interfaces, DTOs)
├── TaskProcessor.Infrastructure/ # Camada de infraestrutura (MongoDB, RabbitMQ)
└── TaskProcessor.Worker/      # Worker para processamento em background
```

## Tecnologias Utilizadas

- **.NET 8.0** - Framework principal
- **MongoDB** - Banco de dados NoSQL
- **RabbitMQ** - Sistema de filas para comunicação assíncrona
- **Docker** - Containerização
- **FluentValidation** - Validação de dados
- **Logging estruturado**

## Como Executar o Projeto

### Pré-requisitos

- Docker Desktop
- Docker Compose
- Git

### 1. Clone o Repositório

```bash
git clone https://github.com/G-Rochaa/TaskProcessor.git
cd TaskProcessor
```

### 2. Executar com Docker Compose

```bash
# Subir todos os serviços
docker-compose -f docker-compose.dev.yml up --build

```

### 3. Verificar se os Serviços Estão Rodando

```bash
# Verificar status dos containers
docker-compose -f docker-compose.dev.yml ps

# Verificar logs
docker-compose -f docker-compose.dev.yml logs -f
```
# se o worker não estiver rodando 

docker-compose -f docker-compose.dev.yml up worker


## 📡 Endpoints da API

### Base URL: `http://localhost:5034`

### 1. Criar Tarefa
```http
POST /api/tarefas
Content-Type: application/json

{
  "tipoTarefa": "Email",
  "dadosTarefa": "{\"email\":\"usuario@exemplo.com\",\"assunto\":\"Teste\"}"
}
```

**Resposta:**
```json
{
  "id": "123e4567-e89b-12d3-a456-426614174000",
  "tipoTarefa": "Email",
  "dadosTarefa": "{\"email\":\"usuario@exemplo.com\",\"assunto\":\"Teste\"}",
  "status": "Pendente",
  "numeroTentativas": 0,
  "maximoTentativas": 3,
  "dataCriacao": "2024-01-15T10:30:00Z",
  "dataAtualizacao": "2024-01-15T10:30:00Z"
}
```

### 2. Listar Todas as Tarefas
```http
GET /api/tarefas
```

### 3. Obter Tarefa por ID
```http
GET /api/tarefas/{id}
```

### 4. Listar Tarefas Pendentes
```http
GET /api/tarefas/pendentes
```

## Tipos de Tarefa Suportados

- **Email** - Processamento de envio de e-mails
- **Relatorio** - Geração de relatórios
- **Genérico** - Processamento genérico

## Status das Tarefas

- **Pendente** - Tarefa criada, aguardando processamento
- **EmProcessamento** - Tarefa sendo processada por um worker
- **Concluida** - Tarefa processada com sucesso
- **Erro** - Tarefa falhou após esgotar tentativas

## Serviços Docker

| Serviço | Porta | Descrição |
|---------|-------|-----------|
| **API** | 5034 | API REST para gerenciamento de tarefas |
| **Worker** | - | Processador de tarefas em background |
| **MongoDB** | 27017 | Banco de dados NoSQL |
| **RabbitMQ** | 5672, 15672 | Sistema de filas (Management UI) |

## Monitoramento

### Swagger UI
- **URL**: `http://localhost:5034/swagger`
- Documentação interativa da API

### RabbitMQ Management
- **URL**: `http://localhost:15672`
- **Usuário**: `guest`
- **Senha**: `guest`

### Logs dos Serviços
```bash
# Logs da API
docker-compose -f docker-compose.dev.yml logs -f api

# Logs do Worker
docker-compose -f docker-compose.dev.yml logs -f worker

# Logs do MongoDB
docker-compose -f docker-compose.dev.yml logs -f mongodb

# Logs do RabbitMQ
docker-compose -f docker-compose.dev.yml logs -f rabbitmq
```

## Testando a Aplicação

### 1. Via Swagger UI
1. Acesse `http://localhost:5034/swagger`
2. Use a interface para testar os endpoints

### 2. Via cURL
```bash
# Criar tarefa
curl -X POST http://localhost:5034/api/tarefas \
  -H "Content-Type: application/json" \
  -d '{"tipoTarefa":"Email","dadosTarefa":"{}","maximoTentativas":3}'

# Listar tarefas
curl http://localhost:5034/api/tarefas
```

### 3. Via Postman
- Importe a coleção de requisições
- Configure a base URL: `http://localhost:5034`


## Desenvolvimento Local

### Executar apenas infraestrutura (MongoDB + RabbitMQ)
```bash
docker-compose -f docker-compose.infrastructure.yml up -d
```

### Executar API localmente
```bash
cd TaskProcessor.API
dotnet run
```

### Executar Worker localmente
```bash
cd TaskProcessor.Worker
dotnet run
```


### Limpeza de Dados

```bash
# Parar todos os serviços
docker-compose -f docker-compose.dev.yml down

# Remover volumes (dados persistentes)
docker-compose -f docker-compose.dev.yml down -v

# Rebuild completo
docker-compose -f docker-compose.dev.yml up --build --force-recreate
```

## Desenvolvedor

Desenvolvido como parte de teste técnico, demonstrando conhecimento em:
- Arquitetura de software (Clean Architecture, DDD 'tentei')
- C# e .NET 8
- MongoDB e NoSQL
- RabbitMQ e mensageria assíncrona
- Docker e containerização
- Boas práticas de desenvolvimento

---

**Nota**: Este projeto foi desenvolvido para fins de demonstração técnica e pode ser adaptado para diferentes cenários de uso conforme necessário.
