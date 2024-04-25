# Visualisation of Token and authorization  grants.
This project aims to create a visualization of the flow of tokens when using OAuth, along with the flow
of the Client and Code authorization grant. This project uses [IdentityServer](https://duendesoftware.com/products/identityserver) for OAuth functionallity, but should  **never be used as an example of how to configure Identity server**.
In fact this project **disregards most security aspects of the Identity server configuration**. For examples of proper Identity server, please referer
to [IdentityServer4 documentation](https://docs.duendesoftware.com/identityserver/v7). The sole purpose of this project is to demonstrate the flows
in OAuth.

## Running the project
The services can be deployed as a unit pr. service by deploying the services directly onto the OS,
or the services can be deployed as a single unit, by using [docker compose](https://docs.docker.com/compose/). Using docker compose is the recommend approach for this project.
  
Because this project serves educational purpose only, it is only intended to be deploy to a local desktop, where the user can experiment.

### Docker compose
Executing the command ```Docker compose up``` in the root of this project folder, will use the `docker-compose.yml` file to deploy this entire project.
Afterwards the [client service API]() will be exposed on port 5003.

### Unit pr service
The application can be deployed as a unit pr. services. Each service must be deploy using the [dotnet cli tool](https://learn.microsoft.com/en-us/dotnet/core/tools/).
The services can be deployed by executing the commands shown below. The shown order of the commands, is also the recommended order for deployment:

- ```dotnet run --project .\msdo.oauth.identityServer\ ```
- ```dotnet run --project .\msdo.oauth.protectedResource\ --ConfigurationFile=Local.json```
- ```dotnet run --project .\msdo.oauth.client\ --ConfigurationFile=Local.json```

### Configuring the services
Yet to come
## Information on services in this project
This section presents an overview of all the services in this project. The table below presents a general overview of all services in this project.

| Service name                                      | Purpose                                                           |Implementation                                                           |
|---------------------------------------------------|-------------------------------------------------------------------|-------------------------------------------------------------------|
| Client | Is the client service, that need access to a protected resource | [msdo.oauth.client](./msdo.oauth.client) |
| Protected Resource server | The resource server with protected resources | [msdo.oauth.protectedResource](./msdo.oauth.protectedResource) |
| Identity Server | The Authorization server that provides access token for protected resources | [msdo.oauth.identityServer](./msdo.oauth.identityServer) |

### Services Network ports
The services usage of network ports are persistant, regardless if deployment are made to a local OS, or using docker compose,
the table below provides a full overview of ports used by each service.
| Service name                                      | Port                                                           |
|---------------------------------------------------|-------------------------------------------------------------------|
| Client | Port 5003, used for http traffic  |
| Protected Resource server | Port 5002, used for http traffic |
| Identity Server | Port 5001, used for http traffic|

### Services endpoints
This section presents all the endpoints exposed by the services. Except for identity server. Only
the Endpoints used in this project is presented for identity server

#### Client

| Path                                      | Purpose                                                           | HTTP Method | Link to service OpenApi specifications |
|-------------------------------------------|-------------------------------------------------------------------|-------------|---------------------------------------|
| `/ClientCredentialGrant`                 | Uses client credential grant to obtain a valid accessToken, and then requests a protected resource | GET         | Yet to come                           |


#### Protected resource server
| Path                  | Purpose                                                           | HTTP Method | Link to service OpenApi specifications |
|-----------------------|-------------------------------------------------------------------|-------------|---------------------------------------|
| `/Resource`           | Provides a protected resource, if a valid access token is presented | GET         | Yet to come                           |


#### Identity server
| Path                                      | Purpose                                                           | HTTP Method | Link to service OpenApi specifications |
|-------------------------------------------|-------------------------------------------------------------------|-------------|---------------------------------------|
| `/.well-known/openid-configuration`       | Retrieves OpenID Connect configuration details for authentication. See [documentation](https://identityserver4.readthedocs.io/en/latest/endpoints/discovery.html) | GET         | Yet to come                           |
| `/connect/token`                          | Used to request tokens. See [documentation](https://identityserver4.readthedocs.io/en/latest/endpoints/token.html)  | POST        | Yet to come                           |


## Testing