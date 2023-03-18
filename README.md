
## Desafio
[![desafio.png](https://i.postimg.cc/LXvCQPCs/desafio.png)](https://postimg.cc/8JJmj7V8)

Neste caso, para exemplo, decidi usar o worker para separação de operações de persistencia de dados.

</br>

## Arquitetura
[![Arquitechture.png](https://i.postimg.cc/BQF5XDT1/Arquitechture.png)](https://postimg.cc/N5g2nKPg)

</br>

# QuickStart
Execute o seguinte comando no diretorio da solution (.sln):

<code>docker-compose -f docker-compose.yml up</code>

</br>

# Manual Mode With Docker 

### **Criando rede**
<code>docker network create taskmanagernet</code>

</br>

### **Start RabbitMQ Container**
<code>docker run -d --hostname rabbitserver --name RabbitMQ_TaskManager --network taskmanagernet -p 15672:15672 -p 5672:5672 rabbitmq:3-management</code>

</br>

### **Net 6 WebAPI Container**
- Execute os comandos abaixo pasta da solution (.sln)
    - Revise as configuracoes de embiente do Dockerfile
    - WebAPI Build: <code>docker build -t taskmanager-webapi -f TaskManager.WebAPI/Dockerfile .</code> 
    - WebAPI Run: <code>docker run -d --name TaskManager.WebPI -p 5020:5020 --network taskmanagernet taskmanager-webapi </code> 
    - http://localhost:5020/swagger

</br>

### **Net 6 Worker Container**
- Execute os comandos abaixo pasta da solution (.sln)
    - Revise as configuracoes de embiente do Dockerfile
    - Worker Build: <code>docker build -t taskmanager-worker -f TaskManager.Worker/Dockerfile .</code> 
    - Worker Run: <code>docker run -d --name TaskManager.Worker --network taskmanagernet taskmanager-worker </code>

</br>

### **Net 6 API + Worker (Without Docker)**
- [É necessario ter Net 6 Instalado](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
- Execute no diretório do .csproj do TaskManager.WebAPI <code>dotnet run</code>
- Execute no diretório do .csproj do TaskManager.Worker <code>dotnet run</code>

</br>

### **MongoDB (Atlas Cloud)**
Esse projeto utiliza um [Cluster free de MongoDB via Atlas Cloud](https://www.mongodb.com/atlas) e pode eventualmente estar fora do ar, se possível configure seu próprio ambiente mongo de outra forma e aponte no arquivo launchSettings.json e DockerFile do projeto WebAPI e Worker
- DataBase: TaskManagerDB
- Collection: KabanTask

</br>

### **Frontend Angular (with NPM)**
- É necessario ter Nodejs 14.20.0 ou superior
- Rode instale as dependências com <code>npm install</code> depois rode com <code>npm run start</code>

</br>

### **To Do**
- :white_square_button: Add Angular Project to docker compose file