 (Simple) Search Query Engine [![Build status](https://ci.appveyor.com/api/projects/status/08nfnb02rvo9syx7/branch/develop?svg=true)](https://ci.appveyor.com/project/larais/sqe/branch/develop)
==================

## What is this?

> Have you ever thought about letting users search data in the frontend by writing search queries like SQL, but didn't because validation was too complicated?

SQE implements a [parser](src/SQE.Grammar/SQE.g4) using [Antlr4](https://github.com/antlr/antlr4) that allows users to type in queries. 
This project is primarily used to allow frontend searching Serilog Logs provided by the [Serilog Microsoft SQL Server Sink](https://github.com/serilog/serilog-sinks-mssqlserver) and is used in our [seriView](https://github.com/larais/seriView) WebApp.

#### Here are some examples of valid search strings:
```
Id = 3
Id = 3 OR Message = "Hello"
Id = 3 AND (Message = "Hello" OR Level != 4)
SomeProperty >= 5
```
> Feel free to try some queries in our [Demo](https://larais.github.io/SQE/)!

#### Can I use this in my project that doesn't have anything to do with Serilog?

Sure! The [grammar](src/SQE.Grammar/SQE.g4) itself is not limited to Serilog, so you can use it as a starting point and generate your own parser and implement your own validation.

## Contributing

Feel free to contribute by creating issues or pull requests!

## License
This project is licensed under the [MIT License](LICENSE)

## Maintainers
SQE is maintained by [Sebastian Lang](https://github.com/SebastianLng) and [David Morais Ferreira](https://github.com/DavidMoraisFerreira).
