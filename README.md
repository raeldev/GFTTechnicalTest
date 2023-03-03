
## Arquitetura
<img src="https://i.imgur.com/Os7TFiI.png">

## Net 6 API + Worker
- [Necessário net 6 instalado](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
- É possível rodar em Docker também

</br>

## Rodando RabbitMQ Docker Image 
<code>docker run -d --hostname rabbitserver --name RabbitMQ_TaskManager -p 15672:15672 -p 5672:5672 rabbitmq:3-management</code>

</br>

## MongoDB
Essa API utiliza um [Cluster free de MongoDB via Atlas Cloud](https://www.mongodb.com/atlas) e pode eventualmente estar fora do ar, se possível configure seu próprio ambiente mongo de outra forma e aponte no arquivo launchSettings.json
- DataBase: TaskManagerDB
- Collection: KabanTask

</br>

## Angular
Rode instale as dependências com <code>npm install</code> depois rode com <code>npm run start</code> 
- Utilize Nodejs 14.20.0 ou superior 