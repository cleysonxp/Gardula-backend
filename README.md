# Gardula Backend

Backend da aplicação **Gardula**, um sistema de gerenciamento financeiro pessoal desenvolvido com **C# e .NET 8**.

O projeto foi desenvolvido com uma arquitetura modular, separando responsabilidades entre API, regras de aplicação, domínio e infraestrutura.

---

## Sobre o projeto

O Gardula tem como objetivo centralizar o controle da vida financeira do usuário, permitindo o gerenciamento de contas, cartões, transações e outras movimentações financeiras.

O backend é responsável por disponibilizar uma API REST para o frontend da aplicação, além de concentrar as regras de negócio, autenticação, persistência e acesso aos dados.

---

## Funcionalidades

Atualmente, o backend possui suporte para:

- Cadastro de usuários
- Autenticação de usuários
- Login
- Refresh Token
- Logout
- Consulta do usuário autenticado
- Gerenciamento de contas
- Gerenciamento de cartões
- Criação de transações
- Edição de transações
- Exclusão de transações
- Transferências entre contas
- Compras utilizando cartão de crédito
- Compras parceladas
- Pagamento de fatura
- Gerenciamento de parcelas
- Resumos financeiros
- Filtros por período
- Filtros de transações
- Paginação
- Ordenação de transações
- Visões consolidadas para dashboard

---

## Arquitetura

O backend está organizado em diferentes projetos, separando as responsabilidades da aplicação.

```text
Gardula
│
├── Gardula.Api
├── Gardula.Application
├── Gardula.Domain
├── Gardula.Infrastructure
└── Gardula.UnitTests
