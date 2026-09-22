# Projeto de Testes em C# com xUnit

Projeto didático desenvolvido em **C#** para demonstrar, de forma prática, conceitos de **testes de software utilizando xUnit**, incluindo testes unitários, testes parametrizados, testes de integração e validação do estado produzido pelas operações.

O projeto utiliza exemplos simples para facilitar a compreensão dos conceitos de teste e da interação entre diferentes classes.

---

## Objetivos

O projeto tem como principais objetivos:

- compreender a estrutura de um teste automatizado;
- utilizar o padrão **AAA — Arrange, Act e Assert**;
- criar testes com `[Fact]`;
- criar testes parametrizados com `[Theory]` e `[InlineData]`;
- utilizar diferentes tipos de `Assert`;
- testar entradas válidas e inválidas;
- compreender testes positivos e negativos;
- identificar testes **Red** e **Green**;
- compreender os princípios básicos de **TDD — Test-Driven Development**;
- realizar testes de integração entre classes;
- verificar não apenas o retorno de uma operação, mas também o estado produzido por ela.

---

# Tecnologias utilizadas

- C#
- .NET
- xUnit
- Visual Studio
- Git
- GitHub

---

# Estrutura do projeto

O projeto utiliza uma separação entre o código principal da aplicação e o projeto responsável pelos testes.

```text
Projeto
│
├── Loja.Core
│   │
│   ├── Cliente.cs
│   ├── ClienteDAO.cs
│   ├── ClienteService.cs
│   ├── ValidadorCliente.cs
│   ├── CalculadoraFrete.cs
│   └── PedidoServiceSimples.cs
│
└── Loja.Tests
    │
    ├── ClienteDAOIntegracaoTests.cs
    ├── ClienteServiceIntegracaoTests.cs
    └── outros testes
```

---

# Classe Cliente

A classe `Cliente` representa os dados básicos de um cliente.

```csharp
public class Cliente
{
    public int Id { get; set; }
    public string Nome { get; set; } = "";
    public string Email { get; set; } = "";

    public Cliente()
    {
    }

    public Cliente(int id, string nome, string email)
    {
        Id = id;
        Nome = nome;
        Email = email;
    }

    public override string ToString()
    {
        return $"{Id} - {Nome} - {Email}";
    }
}
```

A classe possui dois construtores, permitindo criar um cliente vazio ou informar seus dados diretamente.

---

# ClienteDAO

A classe `ClienteDAO` é responsável pelo armazenamento e manipulação dos clientes.

Para fins didáticos, os dados são mantidos em memória utilizando um `ArrayList`.

Entre suas operações estão:

- adicionar clientes;
- listar clientes;
- buscar por ID;
- atualizar clientes;
- excluir clientes;
- limpar os dados;
- consultar a quantidade de clientes.

O uso de armazenamento em memória permite estudar os testes sem necessidade de configurar um banco de dados.

---

# Validação de clientes

A classe `ValidadorCliente` é responsável por verificar se os dados informados para um cliente são válidos.

Entre as situações que podem ser verificadas estão:

```text
Nome vazio              → inválido

E-mail válido           → válido
Exemplo: ana@email.com

E-mail inválido         → inválido
Exemplo: email-invalido
```

Essa separação permite testar as regras de validação independentemente das demais operações.

---

# ClienteService

A classe `ClienteService` representa a camada responsável por coordenar as operações relacionadas ao cliente.

Ela pode utilizar:

```text
ClienteService
      │
      ├── ValidadorCliente
      │
      └── ClienteDAO
```

O serviço pode validar um cliente e, quando os dados forem válidos, solicitar ao DAO que realize seu armazenamento.

---

# Estrutura AAA

Os testes utilizam o padrão:

## Arrange

Preparação dos objetos e dados necessários para o teste.

```csharp
ClienteDAO dao = new ClienteDAO();
ValidadorCliente validador = new ValidadorCliente();

ClienteService service =
    new ClienteService(validador, dao);
```

## Act

Execução da operação que será testada.

```csharp
bool resultado = service.Cadastrar(cliente);
```

## Assert

Verificação do resultado obtido.

```csharp
Assert.True(resultado);
```

A estrutura completa pode ser representada por:

```text
Arrange
   ↓
Preparar

Act
   ↓
Executar

Assert
   ↓
Verificar
```

---

# Testes com Fact

O atributo `[Fact]` é utilizado quando o teste possui um cenário específico.

Exemplo:

```csharp
[Fact]
public void Cadastrar_ClienteValido_DeveSalvarNoDAO()
{
    // Arrange
    ClienteDAO dao = new ClienteDAO();
    ValidadorCliente validador = new ValidadorCliente();

    ClienteService service =
        new ClienteService(validador, dao);

    Cliente cliente = new Cliente
    {
        Id = 1,
        Nome = "Ana",
        Email = "ana@email.com"
    };

    // Act
    bool cadastrou = service.Cadastrar(cliente);

    Cliente? recuperado = service.Consultar(1);

    // Assert
    Assert.True(cadastrou);
    Assert.NotNull(recuperado);

    Assert.Equal(1, recuperado.Id);
    Assert.Equal("Ana", recuperado.Nome);
    Assert.Equal("ana@email.com", recuperado.Email);
}
```

---

# Verificação do resultado e do estado

Um método pode retornar um resultado correto e ainda produzir um estado incorreto.

Por esse motivo, alguns testes verificam dois níveis.

Primeiro, o resultado informado pela operação:

```csharp
Assert.True(cadastrou);
```

Depois, o estado realmente produzido:

```csharp
Assert.NotNull(recuperado);

Assert.Equal("Ana", recuperado.Nome);
Assert.Equal("ana@email.com", recuperado.Email);
```

Essa abordagem aumenta a capacidade do teste de identificar falhas de integração.

---

# Testes com Theory

O atributo `[Theory]` permite executar o mesmo teste utilizando diferentes conjuntos de dados.

Exemplo:

```csharp
[Theory]
[InlineData("Ana", "ana@email.com", true)]
[InlineData("", "bia@email.com", false)]
[InlineData("Carlos", "email-invalido", false)]
public void Cadastrar_DeveValidarEntradasDiferentes(
    string nome,
    string email,
    bool esperado)
{
    // Arrange
    ClienteDAO dao = new ClienteDAO();
    ValidadorCliente validador = new ValidadorCliente();

    ClienteService service =
        new ClienteService(validador, dao);

    Cliente cliente =
        new Cliente(1, nome, email);

    // Act
    bool resultado =
        service.Cadastrar(cliente);

    // Assert
    Assert.Equal(esperado, resultado);
}
```

Nesse exemplo, o xUnit executa o mesmo teste três vezes.

| Nome | E-mail | Resultado esperado |
|---|---|---|
| Ana | ana@email.com | `true` |
| vazio | bia@email.com | `false` |
| Carlos | email-invalido | `false` |

O `[Theory]` reduz a duplicação de código e permite testar várias combinações de entrada.

---

# Testando uma atualização

Também é importante verificar o estado do objeto depois de uma atualização.

Exemplo:

```csharp
[Fact]
public void Atualizar_DevePreservarIdENomeEAlterarEmail()
{
    // Arrange
    ClienteDAO dao = new ClienteDAO();

    Cliente clienteOriginal =
        new Cliente(
            1,
            "Ana",
            "antigo@email.com"
        );

    dao.Adicionar(clienteOriginal);

    Cliente clienteAtualizado =
        new Cliente(
            1,
            "Ana",
            "novo@email.com"
        );

    // Act
    bool atualizou =
        dao.Atualizar(clienteAtualizado);

    Cliente? recuperado =
        dao.BuscarPorId(1);

    // Assert
    Assert.True(atualizou);

    Assert.NotNull(recuperado);

    Assert.Equal(1, recuperado.Id);
    Assert.Equal("Ana", recuperado.Nome);
    Assert.Equal(
        "novo@email.com",
        recuperado.Email
    );
}
```

O teste verifica que:

```text
Id       → preservado
Nome     → preservado
E-mail   → alterado
```

---

# Testes positivos e negativos

Os testes não devem verificar apenas entradas corretas.

Também devemos verificar como o sistema reage a entradas inválidas.

Exemplo de cliente válido:

```text
Nome: Ana
E-mail: ana@email.com

Resultado esperado:
CADASTRO_VALIDO
```

Exemplo de cliente inválido:

```text
Nome:
E-mail: ana_email.com

Resultado esperado:
CADASTRO_INVALIDO
```

Um teste fica **Green** quando o comportamento observado corresponde ao comportamento esperado.

Portanto, um teste com dados inválidos também pode ficar Green quando o sistema identifica corretamente que esses dados são inválidos.

---

# Testes de integração

Os testes de integração verificam a colaboração entre diferentes componentes.

Um dos fluxos utilizados no projeto é:

```text
Teste
  │
  ▼
ClienteService
  │
  ├──────────────► ValidadorCliente
  │                     │
  │                     ▼
  │                 EhValido()
  │
  └──────────────► ClienteDAO
                        │
                        ├── Adicionar()
                        └── BuscarPorId()
```

Nesse cenário, o teste não verifica apenas uma classe isoladamente.

Ele verifica se diferentes componentes conseguem trabalhar corretamente em conjunto.

---

# Teste de integração do DAO

Os testes que acessam diretamente operações como:

```csharp
dao.Adicionar(cliente);

dao.BuscarPorId(1);

dao.Atualizar(cliente);

dao.Excluir(1);
```

podem ser organizados na classe:

```text
ClienteDAOIntegracaoTests
```

---

# Teste de integração do Service

Quando o teste parte do `ClienteService` e verifica sua colaboração com outras classes, pode ser organizado em:

```text
ClienteServiceIntegracaoTests
```

Por exemplo:

```text
ClienteService
      │
      ├── ValidadorCliente
      │
      └── ClienteDAO
```

---

# Exemplo de integração com cálculo de frete

O projeto também utiliza um exemplo envolvendo cálculo de frete.

Uma regra possível é:

```text
Subtotal menor que R$ 300
        ↓
Frete = R$ 25

Subtotal igual ou superior a R$ 300
        ↓
Frete grátis
```

Um teste parametrizado pode verificar os limites da regra:

```csharp
[Theory]
[InlineData(100, 125)]
[InlineData(299.99, 324.99)]
[InlineData(300, 300)]
public void FecharPedido_IntegraComFrete(
    double subtotal,
    double esperado)
{
    // Arrange
    CalculadoraFrete frete =
        new CalculadoraFrete();

    PedidoServiceSimples service =
        new PedidoServiceSimples(frete);

    // Act
    decimal resultado =
        service.FecharPedido(
            (decimal)subtotal
        );

    // Assert
    Assert.Equal(
        (decimal)esperado,
        resultado
    );
}
```

Esse teste também verifica valores próximos ao limite da regra de negócio.

---

# Red e Green

Durante o desenvolvimento orientado a testes, dois estados aparecem frequentemente:

## RED

O teste falha.

```text
Expected: Carlos
Actual:   Ana

TESTE → RED
```

A falha pode indicar que o comportamento esperado ainda não foi implementado ou que existe um defeito no código.

## GREEN

Depois que o comportamento necessário é implementado corretamente:

```text
Expected: Ana
Actual:   Ana

TESTE → GREEN
```

O teste passa.

---

# TDD

O **Test-Driven Development — TDD** utiliza um ciclo baseado em:

```text
RED
 ↓
Criar um teste para um comportamento
que ainda não está implementado.

GREEN
 ↓
Implementar o mínimo necessário
para fazer o teste passar.

REFACTOR
 ↓
Melhorar a estrutura do código
mantendo todos os testes passando.
```

Representação resumida:

```text
        ┌──────────────┐
        │     RED      │
        │ Teste falha  │
        └──────┬───────┘
               │
               ▼
        ┌──────────────┐
        │    GREEN     │
        │ Teste passa  │
        └──────┬───────┘
               │
               ▼
        ┌──────────────┐
        │   REFACTOR   │
        │ Melhorar     │
        └──────┬───────┘
               │
               └──────────► RED
```

---

# Executando os testes

Os testes podem ser executados pelo **Test Explorer** do Visual Studio.

Também é possível executar pelo terminal:

```bash
dotnet test
```

O .NET localizará os testes do projeto xUnit e apresentará o resultado da execução.

Exemplo:

```text
Passed: 8
Failed: 0
Skipped: 0
```

---

# Conceitos trabalhados

Ao concluir os exemplos deste projeto, são trabalhados conceitos como:

- testes automatizados;
- testes unitários;
- testes de integração;
- Arrange, Act e Assert;
- `[Fact]`;
- `[Theory]`;
- `[InlineData]`;
- `Assert.Equal`;
- `Assert.NotEqual`;
- `Assert.True`;
- `Assert.False`;
- `Assert.Null`;
- `Assert.NotNull`;
- testes positivos;
- testes negativos;
- validação de entradas;
- verificação de estado;
- testes parametrizados;
- dependência entre classes;
- Red e Green;
- ciclo Red–Green–Refactor;
- fundamentos de TDD.

---

## Finalidade acadêmica

Este projeto possui finalidade didática e foi estruturado para apoiar o aprendizado de **Qualidade de Software e Testes Automatizados em C#**, utilizando exemplos simples para demonstrar como testes podem identificar defeitos, validar regras de negócio e verificar a integração entre diferentes componentes de uma aplicação.
