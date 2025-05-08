# Prova de conceito de CRUD em C#

O POC-CRUD-csharp é uma prova de conceito de aplicação web backend desenvolvida em C# que implementa o acesso e manipulação de dados em uma base MySQL.

**Funcionalidades já implementadas**:

* [x] Integração com banco de dados MySQL;
* [x] [Entity Framework](https://learn.microsoft.com/en-us/ef/) + [ASP.NET MVC](https://dotnet.microsoft.com/en-us/apps/aspnet/mvc);
* [x] Clean Architecture aplicada ao MVC (Controller → Service → Repository);
* [x] Injeção automática de dependências via interfaces IService e IRepository;
* [x] Mapeamento de exceções para respostas HTTP apropriadas;
* [x] Autenticação via JWT (incluindo verificação de admin para alguns endpoints);
* [x] CRUDs completos de User e Account (com vínculo entre eles).

🔐 A aplicação já permite login autenticado, gerenciamento de usuários e contas e segue boas práticas de organização e separação de responsabilidades, utilizando um modelo **MVC + Services + Repositories**, não o modelo MVC convencional;

# Instruções para subir a aplicação localmente

Para subir a aplicação localmente, você precisa das seguintes dependências:

* **Docker** (Linux) ou **Docker Desktop** (Windows, casovocê não esteja usando o Docker no WSL);
* **JetBrains Rider** (Linux e Windows) ou **Visual Studio Community 2022** (Windows);
* **Postman**.

Caso tenha todas as dependências satisfeitas, siga os passos à seguir:

* Abra o projeto em `POC-CRUD/` com usa IDE de escolha;
* Localize o arquivo `docker-compose.yml`. Ele será utilizado para subir um contêiner com uma imagem do MySQL;

Agora, vamos subir as dependências, utilizando, no shell ou terminal de sua escolha:

```shell
docker compose up
```

Após, inicie a aplicação em modo Release/http. Uma janela do seu navegador padrão deve se abrir, mostrando o status da aplicação (`Healthy`, `Unhealthy`). Caso esteja como `Unhealthy`, alguma dependência não foi satisfeita. Verifique o log no console.

> A aplicação será iniciada na porta 8080.

:warning: A aplicação automaticamente irá executar as *migrations*, isto é, criar o *schema* no banco, bem como todas as tabelas. Nenhuma intervenção é necessária.

Importe o arquivo `Postman.json`, disponível dentro do diretório do projeto, no Postman, para acessar a API já implementada.

# Instruções para finalizar a aplicação

Após encerrar a execução pela IDE, basta, no shell usado para executar o comando anterior, usar a combinação `Ctrl-C`. Caso tenha subido os contêiners com:

```shell
docker compose up -d
``` 

insira, no shell, no mesmo diretório do arquivo `docker-compose.yml`, inserir:

```shell
docker compose down
``` 

# Bugs, sugestões e contato

Caso encontre algum bug ou tenha alguma sugestão, não exite em entrar em contato comigo pelo email `felipenldev@gmail.com` ou
pelas minhas redes sociais!

Mais está por vir!
