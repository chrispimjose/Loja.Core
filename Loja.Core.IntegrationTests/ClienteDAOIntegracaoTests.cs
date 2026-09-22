using Loja.Core;
using Loja.Core.Contoler;
using Loja.Core.Model;
using Loja.Core.Service;

namespace Loja.Core.IntegrationTests
{
    public class ClienteDAOIntegracaoTests
    {
        [Fact (DisplayName ="Verifica a Integração com o Data Object Access - DAO")]
        public void Adicionar_Cliente_DeveSerRecuperadoPorId()
        {
            // Arrange
            // Cria o DAO que será utilizado no teste.
            var dao = new ClienteDAO();

            // Cria o cliente que será adicionado ao DAO.
            var cliente = new Cliente
            {
                Id = 10,
                Nome = "Carlos",
                Email = "carlos@email.com"
            };

            // Act
            // Adiciona o cliente e depois tenta recuperá-lo pelo ID.
            dao.Adicionar(cliente);
            Cliente? recuperado = dao.BuscarPorId(10);

            // Assert
            // Verifica se o cliente foi encontrado.
            Assert.NotNull(recuperado);

            // Verifica se o cliente recuperado possui o nome esperado.
            Assert.Equal(10, recuperado.Id);
            Assert.Equal("Carlos", recuperado.Nome);
            Assert.Equal("carlos@email.com", recuperado.Email);

        }

        [Fact (DisplayName ="Verifica se o Cliente válido é salvo no DAO")]
        public void Cadastrar_ClienteValido_SalvaNoDAO()
        {
            // Arrange
            // Cria o objeto responsável pelo armazenamento.
            ClienteDAO dao = new ClienteDAO();

            // Cria o objeto responsável pela validação.
            ValidadorCliente validador = new ValidadorCliente();

            // Cria o serviço passando o validador.
            ClienteService service = new ClienteService(validador);

            // Cria o cliente utilizado no teste.
            Cliente cliente = new Cliente
            {
                Id = 1,
                Nome = "Bia",
                Email = "bia@email.com"
            };

            // Act
            // Solicita ao serviço que valide o cadastro.
            string resultado = service.ValidarCadastro(cliente);

            // Se o cadastro for válido, adiciona o cliente ao DAO.
            if (resultado == "CADASTRO_VALIDO")
            {
                dao.Adicionar(cliente);
            }

            // Busca no DAO o cliente que foi adicionado.
            Cliente? recuperado = dao.BuscarPorId(1);

            // Assert
            // Verifica se o serviço considerou o cadastro válido.
            Assert.Equal("CADASTRO_VALIDO", resultado);

            // Verifica se o cliente realmente foi armazenado.
            Assert.NotNull(recuperado);

            // Verifica se o cliente armazenado possui o nome esperado.
            Assert.Equal("Bia", recuperado.Nome);

            // Verifica também o e-mail armazenado.
            Assert.Equal("bia@email.com", recuperado.Email);

        }

        [Fact (DisplayName ="Verifica se a atualização de um cliente preserva o ID e o nome, mas altera o e-mail")]
        public void Atualizar_DevePreservarIdENomeEAlterarEmail()
        {
            // Arrange
            // Cria o DAO responsável pelo armazenamento dos clientes.
            ClienteDAO dao = new ClienteDAO();

            // Cria e adiciona o cliente original.
            Cliente clienteOriginal = new Cliente(
                1,
                "Ana",
                "antigo@email.com"
            );

            dao.Adicionar(clienteOriginal);

            // Cria uma nova versão do cliente com o e-mail alterado.
            Cliente clienteAtualizado = new Cliente(
                1,
                "Ana",
                "novo@email.com"
            );

            // Act
            // Atualiza o cliente armazenado no DAO.
            dao.Atualizar(clienteAtualizado);

            // Recupera o cliente após a atualização.
            Cliente? recuperado = dao.BuscarPorId(1);

            // Assert
            // Verifica se o cliente continua armazenado.
            Assert.NotNull(recuperado);

            // Verifica se o ID foi preservado.
            Assert.Equal(1, recuperado.Id);

            // Verifica se o nome foi preservado.
            Assert.Equal("Ana", recuperado.Nome);

            // Verifica se o e-mail foi alterado.
            Assert.Equal("novo@email.com", recuperado.Email);

            /* Esse é um bom teste porque não verifica 
             * simplesmente se Atualizar() foi executado. 
             * Ele verifica o estado do objeto depois da operação.
             */
        }

    }
}
