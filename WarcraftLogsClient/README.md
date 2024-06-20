# WarcraftLogs GraphQL Client

## Usage

### Adding

- Add the client to your services in the Startup.cs file:
```c#
services.AddWarcraftLogsClient("yourClientId", "yourClientSecret");
```
OR
```c#
services.AddWarcraftLogsClient("yourClientId", "yourClientSecret", "accessToken");
```
Ensure to replace "yourClientId" and "yourClientSecret" with your WarcraftLogs OAuth2 credentials.

### Sending Query
There are two ways to send queries using the client:
1. Non-Typed Response:
```c#
var nonTypedResponse = warcraftLogsClient.SendQuery("yourGraphQLQuery");
```
2. Typed Response:
```c#
var typedResponse = warcraftLogsClient.SendQuery(new AbilityQuery());
```
This method returns a typed response where YourResponseType represents the type of the object returned in the "data" section of the GraphQL response.

### Creating Custom Queries
To create custom queries, you can inherit from BaseQuery<T> and define your GraphQL query and response model:
```c#
public class YourCustomQuery : BaseQuery<YourResponseType>
{
    public YourCustomQuery()
    {
        Query = "query { }";
    }
}
```
Replace Query with your actual GraphQL query, and YourResponseType with the corresponding response type.