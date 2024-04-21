# Visualisation of Token and authorization  grants.
This project aims to create a visualization of the flow of tokens when using OAuth, along with the flow
of the Client and Code authorization grant. This project uses [IdentityServer](https://duendesoftware.com/products/identityserver) for OAuth functionallity, but should  **never be used as an example of how to configure Identity server**.
In fact this project **disregards most security aspects of the Identity server configuration**. For examples of proper Identity server, please referer
to [IdentityServer4 documentation](https://docs.duendesoftware.com/identityserver/v7). The sole purpose of this project is to demonstrate the flows
in OAuth.

## Starting the services

### Docker compose

### Directly on the OS

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
| Identity Server | Port 5501, used for http traffic|

### Services endpoints
This section presents all the endpoints exposed by the services. Except for identity server. Only
the Endpoints used in this project is presented for identity server

#### Client
| Path                                      | Purpose                                                           | Link to service OpenApi specifications |
|---------------------------------------------------|-------------------------------------------------------------------|-------------------------------------------------------------------|
||| Yet to come|

#### Protected resource server
| Path                                      | Purpose                                                           | Link to service OpenApi specifications |
|---------------------------------------------------|-------------------------------------------------------------------|-------------------------------------------------------------------|
||| Yet to come|

#### Identity server
| Path                                      | Purpose                                                           | Link to service OpenApi specifications |
|---------------------------------------------------|-------------------------------------------------------------------|-------------------------------------------------------------------|
| /.well-known/openid-configuration |  Retrieves OpenID Connect configuration details for authentication. | Yet to come|
