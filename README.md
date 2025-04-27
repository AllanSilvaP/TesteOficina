# 🛒 Rota das Oficinas — E-commerce Web API

Este projeto é uma API completa de e-commerce desenvolvida para o teste técnico da **Rota das Oficinas**.

---

## ✨ Tecnologias Utilizadas

- **.NET 8.0** — Plataforma principal
- **Entity Framework Core** — ORM para persistência de dados
- **PostgreSQL** — Banco de dados relacional
- **CQRS Pattern** — Separação de leitura e escrita
- **Repository Pattern** — Abstração de acesso a dados
- **Xunit** + **Bogus** + **FluentAssertions** — Testes automatizados

---

## 📋 Funcionalidades Implementadas

- Cadastro, atualização, consulta e remoção de produtos
- Integração completa com banco de dados PostgreSQL
- Ambiente de testes com cobertura para unidades e integração
- Padrão CQRS para comandos e queries
- Padrão Repository para acesso a dados desacoplado

---

## 🚀 Como rodar o projeto

1. Clone o repositório:
   ```bash
   git clone https://github.com/AllanSilvaP/TesteOficina

2. Acesse a pasta do projeto (COLOCAR NOME DO SEU DIRETORIO!):
   ```bash
   cd rota-das-oficinas-api 
   ```

3. Restaure os pacotes:
   ```bash
   dotnet restore
   ```

4. Atualize o banco de dados (Lembre-se do migrations antes):
   ```bash
   dotnet ef database update
   ```

5. Inicie a aplicação:
   ```bash
   dotnet run
   ```

A API ficará disponível! caso esteja desenvolvendo recomendo usar o swager

---

## 🧪 Como rodar os testes

Para executar todos os testes automatizados:

```bash
dotnet test
```


---

## 📚 Referências

- [Documentação CQRS (Microsoft)](https://learn.microsoft.com/en-us/azure/architecture/patterns/cqrs)
- [Repository Pattern (Microsoft)](https://learn.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/infrastructure-persistence-layer-design)
- [Entity Framework Core Docs](https://learn.microsoft.com/en-us/ef/core/)

---

## 👨‍💻 Autor

Desenvolvido por Allan da Silva.  
Teste técnico para a **Rota das Oficinas**.
```
