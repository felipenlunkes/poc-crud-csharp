# Prova de conceito de CRUD em C#

O POC-CRUD-csharp é uma prova de conceito de aplicação web backend desenvolvida em C# que implementa o acesso e manipulação de dados em uma base MySQL.

**Funcionalidades já implementadas**:

✅ Integração com banco de dados MySQL;
✅ [Entity Framework](https://learn.microsoft.com/en-us/ef/) + [ASP.NET MVC](https://dotnet.microsoft.com/en-us/apps/aspnet/mvc);
✅ Clean Architecture aplicada ao MVC (Controller → Service → Repository);
✅ Injeção automática de dependências via interfaces IService e IRepository;
✅ Mapeamento de exceções para respostas HTTP apropriadas;
✅ Autenticação via JWT (incluindo verificação de admin para alguns endpoints);
✅ CRUDs completos de User e Account (com vinculação entre eles).

🔐 A aplicação já permite login autenticado, gerenciamento de usuários e contas e segue boas práticas de organização e separação de responsabilidades, utilizando um modelo **MVC + Services + Repositories**, não o modelo MVC convencional;

Mais está por vir!
