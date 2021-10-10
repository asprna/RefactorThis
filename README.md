# refactor-this
The attached project is a poorly written products API in C#.

Please evaluate and refactor areas where you think can be improved. 

Consider all aspects of good software engineering and show us how you'll make it #beautiful and make it a production ready code.

## Getting started for applicants

There should be these endpoints:

1. `GET /products` - gets all products.
2. `GET /products?name={name}` - finds all products matching the specified name.
3. `GET /products/{id}` - gets the project that matches the specified ID - ID is a GUID.
4. `POST /products` - creates a new product.
5. `PUT /products/{id}` - updates a product.
6. `DELETE /products/{id}` - deletes a product and its options.
7. `GET /products/{id}/options` - finds all options for a specified product.
8. `GET /products/{id}/options/{optionId}` - finds the specified product option for the specified product.
9. `POST /products/{id}/options` - adds a new product option to the specified product.
10. `PUT /products/{id}/options/{optionId}` - updates the specified product option.
11. `DELETE /products/{id}/options/{optionId}` - deletes the specified product option.

All models are specified in the `/Models` folder, but should conform to:

**Product:**
```
{
  "Id": "01234567-89ab-cdef-0123-456789abcdef",
  "Name": "Product name",
  "Description": "Product description",
  "Price": 123.45,
  "DeliveryPrice": 12.34
}
```

**Products:**
```
{
  "Items": [
    {
      // product
    },
    {
      // product
    }
  ]
}
```

**Product Option:**
```
{
  "Id": "01234567-89ab-cdef-0123-456789abcdef",
  "Name": "Product name",
  "Description": "Product description"
}
```

**Product Options:**
```
{
  "Items": [
    {
      // product option
    },
    {
      // product option
    }
  ]
}
```
# Code refactor stage by stage
I have divided this project into a few phases, which are explained below.

## Update the .net framework
The project currently uses .net core 2.1 which is no longer supported by Microsoft [[Ref]](https://dotnet.microsoft.com/platform/support/policy/dotnet-core "[Ref]". Hence, I have  upgraded the .net framework to .net 5.
1. Upgraded .net framework to .net 5
2. Upgraded Sqlite package to latest 5.0.10
3. Added missing Newtonsoft package
4. Refactor the Startup.cs according to .net 5 standard

## Added Integration Test Suite to the project (TDD)
I have implemented the integration tests before any code changes. By following TDD approch, the developer will be able to refactor the existing code and provide the same output as the old code.

## Implemented Clean Architecture Pattern
The clean architechture pattern allows to seperate the project into unique layers. There are many advantages of this pattern [[Ref]](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html "[Ref]") . Overtime, when new features are added to this project, this pattern will certainly help developers to write a clean code and it will be easy for them to debug.

## Added EntityFramework Support - Database first model
As this simple CURD application does not use complex queries, EntityFramework is the best candidate for this project. EntityFramework provides greater support for Unit Testing. The effort to migrate from SQlite to SQL server or any other database server using EntityFramework is very minimal. The other option is to use Dapper but it is not as supportive with unit tests.

`dotnet CLI command: dotnet ef dbcontext scaffold “Data source=../API/App_Data/products.db” Microsoft.EntityFrameworkCore.Sqlite —context DataContext —context-dir .\Persistence —output-dir ..\Domain —data-annotations —force -p .\Persistence -f`

## Implemeted CQRS Pattern using Mediator
The CQRS allows developers to seperate queries and commands. This pattern allows developers to write clean code as well as thin controllers in the API project. Another benefit is, if a need arises to configure two database, one for the read (read optimised database) and one for the write (write optimised database),  it can be done using this pattern with ease.

## Refactor existing API
After all the necessary ground work mentioned above, I started refactoring the existing API. I have also added unit tests for each of the components. Both Unit and Integration tests covers 95% of the code base.

# Improvement Suggestions
This simple database can be normalised using the following methods.
1. Adding primary keys and foreign keys to Product and ProductOption tables.
2. Database performance can be improved by adding Cluster/Non-Cluster indexes.