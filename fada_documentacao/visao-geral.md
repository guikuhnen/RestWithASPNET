# Documentação Técnica - RestWithASPNET

## 1. O que o sistema faz

O **RestWithASPNET** é uma API RESTful completa desenvolvida em ASP.NET Core 8 que implementa operações CRUD (Create, Read, Update, Delete) para gerenciamento de pessoas e livros. O sistema resolve o problema de fornecer uma solução backend robusta e escalável com funcionalidades modernas como:

- Autenticação e autorização baseada em JWT (JSON Web Tokens)
- Paginação e ordenação de resultados
- Upload e download de arquivos
- HATEOAS (Hypermedia as the Engine of Application State) para navegação dinâmica da API
- Versionamento de API
- Integração com banco de dados MySQL
- Containerização com Docker

## 2. Arquitetura

O sistema segue uma arquitetura em camadas bem definida:

### **Controllers (Camada de Apresentação)**
- `AuthController`: Gerencia autenticação (login, refresh token, revogação)
- `PersonController`: Endpoints CRUD para pessoas
- `BookController`: Endpoints CRUD para livros
- `FileController`: Gerenciamento de upload/download de arquivos

### **Business (Camada de Negócio)**
- `IPersonBusiness` / `PersonBusiness`: Lógica de negócio para pessoas
- `IBookBusiness` / `BookBusiness`: Lógica de negócio para livros
- `ILoginBusiness` / `LoginBusiness`: Validação de credenciais e gestão de tokens
- `IFileBusiness` / `FileBusiness`: Processamento de arquivos

### **Repository (Camada de Dados)**
- `IBaseRepository`: Interface genérica para operações CRUD
- `IPersonRepository`: Operações específicas de pessoa
- `IUserRepository`: Gerenciamento de usuários e autenticação

### **Data/VO (Value Objects)**
- DTOs para transferência de dados entre camadas
- `PersonVO`, `BookVO`, `TokenVO`, `UserVO`, `FileDetailVO`

### **Converters**
- Conversão entre entidades de domínio e Value Objects
- `PersonConverter`, `BookConverter`

### **HATEOAS**
- Enriquecimento automático de respostas com links de navegação
- `PersonEnricher`, `BookEnricher`

## 3. Fluxo de dados

```
Cliente → Controller → Business → Repository → Database
                ↓           ↓
            Validation  Converter (Entity ↔ VO)
                ↓
        HATEOAS Enricher → Response
```

### Exemplo de fluxo completo (Consulta de Pessoa):

1. Cliente faz requisição GET `/api/person/v1/1`
2. `PersonController.Get(1)` recebe a requisição
3. Controller chama `PersonBusiness.FindById(1)`
4. Business consulta `PersonRepository.FindById(1)`
5. Repository executa query no MySQL e retorna `Person` entity
6. `PersonConverter` converte `Person` → `PersonVO`
7. `HyperMediaFilter` enriquece `PersonVO` com links HATEOAS
8. Response JSON é retornado ao cliente

## 4. Tecnologias utilizadas

| Tecnologia | Função |
|------------|--------|
| **ASP.NET Core 8** | Framework principal para construção da API |
| **Entity Framework Core** | ORM para acesso ao banco de dados |
| **MySQL 8.3.0** | Banco de dados relacional |
| **JWT (JSON Web Tokens)** | Autenticação e autorização |
| **Asp.Versioning** | Versionamento de API |
| **Docker** | Containerização da aplicação e banco |
| **React 18** | Frontend cliente (pasta `/client`) |
| **Axios** | Cliente HTTP no frontend |
| **React Router DOM** | Roteamento no frontend |

### Bibliotecas principais do Backend:
- `Microsoft.IdentityModel.JsonWebTokens` - Geração e validação de JWT
- `Microsoft.AspNetCore.Authentication.JwtBearer` - Middleware de autenticação
- HATEOAS customizado - Implementação própria para hypermedia

## 5. Estrutura de pastas

```
RestWithASPNET/
├── RestWithASPNET/               # Projeto principal da API
│   ├── Business/                 # Camada de lógica de negócio
│   │   ├── IPersonBusiness.cs
│   │   ├── PersonBusiness.cs
│   │   ├── IBookBusiness.cs
│   │   ├── BookBusiness.cs
│   │   ├── ILoginBusiness.cs
│   │   ├── LoginBusiness.cs
│   │   ├── IFileBusiness.cs
│   │   └── FileBusiness.cs
│   ├── Controllers/              # Controllers da API
│   │   ├── PersonController.cs
│   │   ├── BookController.cs
│   │   ├── AuthController.cs
│   │   └── FileController.cs
│   ├── Data/
│   │   ├── Converter/           # Conversores Entity ↔ VO
│   │   │   ├── Business/
│   │   │   │   ├── PersonConverter.cs
│   │   │   │   └── BookConverter.cs
│   │   │   └── Contract/
│   │   │       └── IParser.cs
│   │   └── VO/                  # Value Objects (DTOs)
│   │       ├── PersonVO.cs
│   │       ├── BookVO.cs
│   │       ├── TokenVO.cs
│   │       ├── UserVO.cs
│   │       └── FileDetailVO.cs
│   ├── Repository/              # Camada de acesso a dados
│   │   └── Base/
│   │       └── IBaseRepository.cs
│   ├── Model/                   # Entidades de domínio
│   │   ├── Person.cs
│   │   ├── Book.cs
│   │   └── User.cs
│   ├── Services/                # Serviços auxiliares
│   │   └── ITokenService.cs
│   ├── Configurations/          # Configurações
│   │   └── TokenConfiguration.cs
│   ├── Hypermedia/              # Implementação HATEOAS
│   │   └── Enricher/
│   │       ├── PersonEnricher.cs
│   │       └── BookEnricher.cs
│   ├── db/                      # Scripts de banco de dados
│   │   ├── migrations/          # Scripts DDL
│   │   └── dataset/             # Dados iniciais
│   ├── UploadDir/               # Diretório de uploads
│   └── Dockerfile               # Configuração Docker da API
├── client/                       # Frontend React
│   ├── src/
│   ├── public/
│   └── package.json
├── db/
│   └── Dockerfile               # Configuração Docker do MySQL
├── docker-compose.yml           # Orquestração de containers
└── README.md
```

## 6. Como executar localmente

### **Pré-requisitos**

- .NET 8 SDK
- Docker e Docker Compose
- Node.js 18+ (para o cliente React)
- MySQL 8+ (opcional, pode usar Docker)

### **Instalação e Execução**

#### **Opção 1: Com Docker (Recomendado)**

```bash
# Clone o repositório
git clone https://github.com/guikuhnen/RestWithASPNET.git
cd RestWithASPNET

# Inicie os containers (API + MySQL)
docker-compose up -d

# A API estará disponível em:
# - http://localhost:55000
# - https://localhost:55001

# O MySQL estará disponível na porta 3308
```

#### **Opção 2: Execução local sem Docker**

```bash
# 1. Configure a connection string no appsettings.json
# Aponte para sua instância MySQL local

# 2. Execute as migrations do banco
# (scripts em db/migrations/)

# 3. Restaure os pacotes e execute
cd RestWithASPNET
dotnet restore
dotnet run

# A API estará em https://localhost:7XXX
```

#### **Executar o Cliente React**

```bash
cd client
npm install
npm start

# Cliente disponível em http://localhost:3000
```

### **Comandos principais**

```bash
# Build da aplicação
dotnet build

# Executar testes
dotnet test

# Publicar aplicação
dotnet publish -c Release

# Build das imagens Docker
docker-compose build

# Parar containers
docker-compose down

# Ver logs
docker-compose logs -f rest-with-asp-net
```

## 7. Integrações externas

### **Banco de Dados**

**MySQL 8.3.0**
- **Porta**: 3308
- **Database**: `rest_with_asp_net`
- **Usuário**: `docker` / `docker`
- **Root**: `admin123`

**Tabelas principais**:
- `person` - Armazena dados de pessoas
- `books` - Armazena dados de livros
- `users` - Usuários do sistema para autenticação

### **Configuração de Autenticação JWT**

Configurado via `appsettings.json`:

```json
{
  "TokenConfiguration": {
    "Audience": "ExempleAudience",
    "Issuer": "ExempleIssuer",
    "Secret": "MY_SUPER_SECRET_KEY",
    "Minutes": 60,
    "DaysToExpire": 7
  }
}
```

### **Endpoints da API**

Base URL: `http://localhost:55000/api`

**Autenticação**:
- `POST /auth/v1/signin` - Login
- `POST /auth/v1/refresh` - Renovar token
- `GET /auth/v1/revoke` - Revogar token

**Pessoas**:
- `GET /person/v1/{sortDirection}/{pageSize}/{currentPage}` - Listar com paginação
- `GET /person/v1/{id}` - Buscar por ID
- `POST /person/v1` - Criar
- `PUT /person/v1` - Atualizar
- `DELETE /person/v1/{id}` - Deletar
- `PATCH /person/v1/{id}` - Alterar status

**Livros**:
- Similar à estrutura de pessoas

**Arquivos**:
- `POST /file/v1/uploadFile` - Upload único
- `POST /file/v1/uploadFiles` - Upload múltiplo
- `GET /file/v1/downloadFile/{fileName}` - Download

### **CI/CD**

O projeto utiliza **GitHub Actions** para integração contínua, com badge de status visível no README principal.

---

**Observação**: Este é um projeto educacional baseado no curso da Udemy sobre REST APIs com ASP.NET Core, demonstrando boas práticas de desenvolvimento de APIs RESTful modernas.