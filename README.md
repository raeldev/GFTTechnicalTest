
## Arquitetura
![Screenshot](ReadmeFiles\Arquitechture.png)

## Net 6 API + Worker
- Necessário net 6 instalado
- É possível rodar em Docker também

</br>

## Rodando RabbitMQ Docker Image 
<code>docker run -d --hostname rabbitserver --name RabbitMQ_TaskManager -p 15672:15672 -p 5672:5672 rabbitmq:3-management</code>

</br>

## MongoDB
Essa API utiliza um Cluster free de MongoDB via Atlas Cloud e pode eventualmente estar fora do ar, se possível configure seu próprio ambiente mongo de outra forma e aponte no arquivo launchSettings.json
- DataBase: TaskManagerDB
- Collection: KabanTask

</br>

## Node
Utilize Nodejs 8.5.0 ou superior 

</br>

## Angular
Rode instale as dependências com <code>npm install</code> depois rode com <code>npm run start</code> 