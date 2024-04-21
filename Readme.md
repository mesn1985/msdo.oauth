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
This section presents an overview of all the services in this project

| Service name                                      | Purpose                                                           |Implementation                                                           |
|---------------------------------------------------|-------------------------------------------------------------------|-------------------------------------------------------------------|
| Client | Is the client service, that need access to a protected resource | [msdo.oauth.client](./msdo.oauth.client) |
| Protected Resource server | The resource server with protected resources | [msdo.oauth.protectedResource](./msdo.oauth.protectedResource)  |
| Identity Server | The Authorization server that provides access token for protected resources | [msdo.oauth.identityServer](./msdo.oauth.identityServer) |

## Identity Server Endpoints

These are the endpoints provided by the Identity Server:

| Path                                      | Purpose                                                           |
|---------------------------------------------------|-------------------------------------------------------------------|
| /.well-known/openid-configuration | Retrieves OpenID Connect configuration details for authentication. |
