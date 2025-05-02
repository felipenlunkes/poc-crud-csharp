# Prova de conceito de CRUD em C#

O POC-CRUD-csharp é uma prova de conceito de aplicação web backend desenvolvida em C# que implementa o acesso e manipulação de dados em uma base MySQL.

**Funcionalidades implementadas**:
* Configuração e utilização de uma base de dados **MySQL**;
* Utilização do [Entity Framework](https://learn.microsoft.com/en-us/ef/) e [ASP.NET MVC](https://dotnet.microsoft.com/en-us/apps/aspnet/mvc);
* Aplicação de Clean Architecture sobre o modelo MVC (**MVC + Services + Repositories**): **Controller** -> **Service** -> **Repository** para melhor segregação de responsabilidades;
* Instanciação e injeção automática de dependências para classes que implementem as interfaces **IService** e **IRepository** criadas para o projeto;
* Mapeamento de exceções para respostas HTTP válidas para cada contexto;
* Uso de Web Token para acesso à endpoints que exigem autenticação. Além disso, determinados endpoints exigem que o usuário seja administrador. O suporte a essa validação também foi implementado;
* Implementação de dois CRUDs iniciais: USER e ACCOUNT. O usuário é utilizado exclusivamente para autenticação, e cada conta está vinculada a apenas um usuário.

Mais está por vir!
