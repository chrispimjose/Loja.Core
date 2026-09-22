using Loja.Core.Contoler;
using Loja.Core.Model;
using Loja.Core.Service;

namespace Loja.Core.IntegrationTests
{
    public class ClienteServiceIntegracaoTests
    {
        [Fact(DisplayName = "Teste de Cliente Válido")]
        // Teste de integração com ValidadorCliente real
        public void ClienteService_ComValidadorReal_ClienteValido()
        {
            // Arrange
            var validador = new ValidadorCliente();
            var service = new ClienteService(validador);

            var cliente = new Cliente
            {
                Id = 1,
                Nome = "Ana",
                Email = "ana@email.com"
            };

            // Act
            string resultado = service.ValidarCadastro(cliente);

            // Assert
            Assert.Equal("CADASTRO_VALIDO", resultado);

        }


        [Fact(DisplayName = "Teste de Cliente Inválido")]
        //Teste de integração com o validador real para um cliente inválido
        public void ClienteService_ComValidadorReal_ClienteInValido()
        {
            // Arrange
            // Cria o validador.
            ValidadorCliente validador = new ValidadorCliente();

            // Cria o serviço utilizando o validador.
            ClienteService service = new ClienteService(validador);

            // Cria propositalmente um cliente com dados inválidos.
            Cliente cliente = new Cliente
            {
                Id = 1,
                Nome = "",
                Email = "ana_email.com"
            };

            // Act
            // Solicita ao serviço que valide o cadastro.
            string resultado = service.ValidarCadastro(cliente);

            // Assert
            // O teste espera que o sistema identifique o cadastro como inválido.
            Assert.Equal("CADASTRO_INVALIDO", resultado);

        }

        [Fact (DisplayName = "Teste de Cliente Inválido - Email Invalido")]
        public void ValidarCadastro_EmailInvalido_DeveRetornarCadastroInvalido()
        {
            // Arrange
            ValidadorCliente validador = new ValidadorCliente();
            ClienteService service = new ClienteService(validador);

            Cliente cliente = new Cliente
            {
                Id = 1,
                Nome = "Ana",
                Email = "ana_email.com"
            };

            // Act
            string resultado = service.ValidarCadastro(cliente);

            // Assert
            Assert.Equal("CADASTRO_INVALIDO", resultado);
        }


        [Theory (DisplayName = "Teste de Fechar Pedido Integra Com Frete")]
        [InlineData(100, 125)]
        [InlineData(299.99, 324.99)]
        [InlineData(300, 300)]
        // Teste de integração de Falha PedidoService com a CalculadoraFrete real
        public void FecharPedido_IntegraComFrete(double subtotal, double esperado)
        {
            // Arrange
            var frete = new CalculadoraFrete();
            var service = new PedidoServiceSimples(frete);

            // Act
            decimal resultado = service.FecharPedido((decimal)subtotal);

            // Assert
            Assert.Equal((decimal)esperado, resultado);
        }

        [Theory (DisplayName = "Teste de Fechar Pedido Integra Com Frete - Teste Com Falha")]
        [InlineData(100, 125)]
        [InlineData(299.99, 324.99)]
        [InlineData(300, 325)] // Valor propositalmente incorreto para provocar falha
        public void FecharPedido_IntegraComFrete_TesteComFalha(double subtotal, double esperado)
        {
            // Arrange
            CalculadoraFrete frete = new CalculadoraFrete();
            PedidoServiceSimples service = new PedidoServiceSimples(frete);

            // Act
            decimal resultado = service.FecharPedido((decimal)subtotal);

            // Assert
            Assert.Equal((decimal)esperado, resultado);
        }

        [Fact(DisplayName = "Cadastrar cliente válido deve salvar no DAO")]
        public void Cadastrar_ClienteValido_DeveSalvarNoDAO()
        {
            // Arrange
            // Cria o objeto responsável pelo armazenamento.
            ClienteDAO dao = new ClienteDAO();

            // Cria o objeto responsável pela validação.
            ValidadorCliente validador = new ValidadorCliente();

            // Utiliza o NOVO construtor que recebe
            // o ValidadorCliente e o ClienteDAO.
            ClienteService service = new ClienteService(validador, dao);

            // Cria um cliente válido.
            Cliente cliente = new Cliente
            {
                Id = 1,
                Nome = "Ana",
                Email = "ana@email.com"
            };

            // Act
            // O ClienteService valida o cliente e,
            // sendo válido, solicita ao DAO que faça o cadastro.
            bool cadastrou = service.Cadastrar(cliente);

            // Consulta o cliente que deveria ter sido armazenado.
            Cliente? recuperado = service.Consultar(1);

            // Assert

            // Verifica o resultado informado pela operação.
            Assert.True(cadastrou);

            // Verifica se o cliente realmente foi armazenado.
            Assert.NotNull(recuperado);

            // Verifica os dados recuperados.
            Assert.Equal(1, recuperado.Id);
            Assert.Equal("Ana", recuperado.Nome);
            Assert.Equal("ana@email.com", recuperado.Email);
        }

        [Fact(DisplayName = "Cadastrar cliente válido no DAO - Teste com falha")]
        public void Cadastrar_ClienteValido_DeveSalvarNoDAO_TesteComFalha()
        {
            // Arrange
            ClienteDAO dao = new ClienteDAO();
            ValidadorCliente validador = new ValidadorCliente();

            ClienteService service = new ClienteService(validador, dao);

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

            // ERRO PROPOSITAL:
            // O cliente cadastrado chama-se "Ana",
            // mas o teste espera encontrar "Carlos".
            Assert.Equal("Carlos", recuperado.Nome);
        }

        [Theory(DisplayName = "Cadastrar deve validar entradas diferentes")]
        [InlineData("Ana", "ana@email.com", true)]
        [InlineData("", "bia@email.com", false)]
        [InlineData("Carlos", "email-invalido", false)]
        public void Cadastrar_DeveValidarEntradasDiferentes(string nome, string email, bool esperado)
        {
            // Arrange
            ClienteDAO dao = new ClienteDAO();
            ValidadorCliente validador = new ValidadorCliente();

            ClienteService service = new ClienteService(validador, dao);

            // Cria o cliente utilizando os dados recebidos pelo InlineData.
            Cliente cliente = new Cliente(1, nome, email);

            // Act
            bool resultado = service.Cadastrar(cliente);

            // Assert
            Assert.Equal(esperado, resultado);
        }

    }
}
