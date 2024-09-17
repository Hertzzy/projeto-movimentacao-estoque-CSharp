# Projeto - Movimentação de Estoque -  C#

Este projeto é uma API para gerenciar a movimentação de estoque, somente operações de criação e listagem de produtos e movimentações.

## Funcionalidades

- **Criar Produtos**
- **Listar Produtos**
- **Movimentar Estoque**
- **Excluir a última movimentação**

## Estruturas de Dados

### Produto

```sql
{
  Id,               // Identificador único gerado automaticamente
  Sku,              // Código alfanumérico informado pelo usuário
  Descricao,        // Descrição do produto
  DataCadastro,     // Data de cadastro do produto
  DataMovimentacao, // Data da última movimentação de estoque registrada
  Saldo             // Quantidade atual em estoque
}

{
  Id,               // Identificador único gerado automaticamente
  IdProduto,        // Identificador do produto
  Tipo,             // Tipo da movimentação (entrada ou saída)
  Quantidade,       // Quantidade movimentada
  Data              // Data da movimentação
}
```

## Pacotes Instalados

O projeto utiliza os seguintes pacotes:

- **AutoMapper**: Mapeamento de objetos.
- **Dapper**: ORM para acesso a dados.
- **Npgsql**: Driver .NET para utilização do PostgreSQL.


### Script SQL

Script para configurar o banco de dados:

```sql
CREATE DATABASE apimovimentacaoestoque;

\c apimovimentacaoestoque

CREATE TABLE Produtos (
  Id SERIAL PRIMARY KEY, 
  Sku VARCHAR(100) NOT NULL, 
  Descricao VARCHAR(255) NOT NULL,
  DataCadastro TIMESTAMP NOT NULL, 
  DataMovimentacao TIMESTAMP NULL, 
  Saldo NUMERIC(10, 2) NOT NULL
);

CREATE TABLE Movimentacao (
  Id SERIAL PRIMARY KEY,
  IdProduto INT NOT NULL,
  Tipo VARCHAR(10) NOT NULL,
  Quantidade NUMERIC(10, 2) NOT NULL,
  Data TIMESTAMP NOT NULL,
  FOREIGN KEY (IdProduto) REFERENCES Produtos(Id)
);
```

Conexão com o banco de dados Postgres

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*", 
  "ConnectionStrings": { //Host        Nome do Usuario       Nome da Data Base
    "DefaultConnection": "Host=;Port=;Username=;Password=;Database=apimovimentacaoestoque"
  }                      //    Porta            Senha  
}


