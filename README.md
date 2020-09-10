## Authorization Server (Test)

Authorization server is responsible for managing accesses to other services. 
It acts as a central point of authentication where every request should be validated.

#### Check Authorization/Notes.txt

To Run Identity Server as a container using docker:
1. Go to the solution folder (authorization)
2. Build the docker image for Identity Server
```javascript
	docker build -f Dockerfile.Authorization.Api -t authapiimage .
```
3. Run the container
```javascript
	docker run --name authapi-ctr -d -p 5555:80 -e ASPNETCORE_ENVIRONMENT=Development -e ASPNETCORE_URLS=http://*:80 --rm -t authapiimage
```

Command options
	-t : TTY (displays the name of the application)
	—rm : removes older containers if exists
	-p : maps an external port to an internal port (inside the container)
	-e : key=value defines environment variables 
	-d : runs container in background and display container Id

Reference:
- docker images : lists all docker images
- docker ps -a : lists all containers
- docker inspect -f '{{range .NetworkSettings.Networks}}{{.IPAddress}}{{end}}' {containerId} : Gets information of the container
- docker stop {containerId}
- docker rm {containerId}



## Authorization Ressources Server

Resources management server is accessed through an Api that exposes a set of endpoints, allowing one to define identity resources:
* Clients: applications allowed to access the system
* Api Resources: Api's protected by Identity server
* Tenants: Companies accessing the sytem through a client application
* Users: users that belongs to a Tenant and have access to resources through a client

To Run Authorization Resources Server as a container using docker:
1. Go to the solution folder (authorization)
2. Build the docker image for Authorization Resources
```javascript
	docker build -f Dockerfile.Authorization.ResourcesApi -t authresourcesapiimage .
```
3. Run the container
```javascript
	docker run --name authresourcesapi-ctr -d -p 5556:80 -e ASPNETCORE_ENVIRONMENT=Development -e ASPNETCORE_URLS=http://*:80 --rm -t authresourcesapiimage
```

Notes:
- Authorization Ressources Server runs in a Linux Alpine image just for testing purposes and it can be replaced by other image.


## Run With Docker Compose
Both applications can be executed inside the same docker host by run the them with docker-compose (with provided configuration)
1. Build the images using the command provided above
2. Execute docker-compose
```javascript
	docker-compose up -d
```

Notes:
- By executing the previous command docker will create application containers in the same docker host, both sharing the same network. This allows a container to access another by only reference its name (Since docker creates a single network to both containers, a hostname is resolved to a private container address)
- docker-compose configuration is set to run both applications with Staging environment
