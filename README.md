# TaskProcessor - Sistema de Processamento de Tarefas em Background

Sistema de processamento assíncrono de tarefas desenvolvido com .NET 8, MongoDB e RabbitMQ.

Tecnologias

.NET 8 - Framework principal
ASP.NET Core - API REST
MongoDB - Banco de dados NoSQL
RabbitMQ - Sistema de filas/mensageria
Docker - Containerização

Pré-requisitos
Antes de começar, você precisa ter instalado:

Docker Desktop (versão 24.x ou superior)
.NET 8 SDK (apenas para desenvolvimento)
Git

Como Executar
1. Clone o repositório:

bashgit clone https://github.com/seu-usuario/TaskProcessor.git
cd TaskProcessor

2. Suba os serviços de infraestrutura (MongoDB + RabbitMQ):

bashdocker-compose -f docker-compose.dev.yml up -d
Aguarde alguns segundos até os containers estarem prontos.

3. Verifique se os serviços estão rodando
bashdocker ps

Você deve ver dois containers:

taskprocessor-mongodb
taskprocessor-rabbitmq

4. Execute a API:
bashcd src/TaskProcessor.API
dotnet run

5.Execute o Worker (em outro terminal):
bashcd src/TaskProcessor.Worker
dotnet run


