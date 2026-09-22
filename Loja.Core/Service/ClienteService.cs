using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Loja.Core.Contoler;
using Loja.Core.Model;

namespace Loja.Core.Service
{
    public class ClienteService
    {
        private readonly ValidadorCliente _validador;
        private readonly ClienteDAO? _dao;

        public ClienteService(ValidadorCliente validador)
        {
            _validador = validador;
            _dao = null;
        }

        public ClienteService(ValidadorCliente validador, ClienteDAO dao)
        {
            _validador = validador;
            _dao = dao;
        }

        public string ValidarCadastro(Cliente cliente)
        {
            if (_validador.EhValido(cliente))
            {
                return "CADASTRO_VALIDO";
            }
            else
            {
                return "CADASTRO_INVALIDO";
            }
        }

        public bool Cadastrar(Cliente cliente)
        {
            // Primeiro valida os dados.
            bool valido = _validador.EhValido(cliente);

            if (valido == false)
            {
                return false;
            }

            // Verifica se o DAO foi informado.
            if (_dao == null)
            {
                return false;
            }

            // Armazena o cliente.
            _dao.Adicionar(cliente);

            return true;
        }

        // Novo método.
        public Cliente? Consultar(int id)
        {
            // Verifica se existe um DAO disponível.
            if (_dao == null)
            {
                return null;
            }

            return _dao.BuscarPorId(id);
        }

    }
}
