using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Loja.Core.Model;

namespace Loja.Core.Service
{
    public class ClienteDAO
    {
        // ArrayList responsável por armazenar os clientes.
        private readonly ArrayList _clientes = new ArrayList();

        // Adiciona um novo cliente na lista.
        public void Adicionar(Cliente cliente)           
        {
            _clientes.Add(cliente);
        }

        // Retorna todos os clientes cadastrados.
        public ArrayList ListarTodos()
        {
            return _clientes;
        }

        // Busca um cliente pelo seu código de identificação.
        public Cliente? BuscarPorId(int id)            
        {
            // A ? indica que o método pode não encontrar o cliente e retornar null
            // Percorre todos os elementos armazenados no ArrayList.
            foreach (Cliente cliente in _clientes)
            {
                // Verifica se o cliente possui o ID procurado.
                if (cliente.Id == id)
                    // Retorna o cliente encontrado.
                    return cliente;
            }

            // Retorna null quando nenhum cliente é encontrado.
            return null;
        }

        // Atualiza os dados de um cliente já cadastrado.
        public bool Atualizar(Cliente clienteAtualizado)
        {
            for (int i = 0; i < _clientes.Count; i++)
            {
                // Converte o elemento armazenado para Cliente.
                Cliente atual = (Cliente)_clientes[i]!;

                // Verifica se encontrou o cliente que será atualizado.
                if (atual.Id == clienteAtualizado.Id)
                {
                    // Substitui o cliente antigo pelo cliente atualizado.
                    _clientes[i] = clienteAtualizado;

                    // Informa que a atualização foi realizada.
                    return true;
                }
            }

            return false;
        }

        // Exclui um cliente utilizando seu ID.
        public bool Excluir(int id)
        {
            // Procura o cliente que será excluído.
            Cliente? cliente = BuscarPorId(id);

            // Verifica se o cliente não foi encontrado.
            if (cliente is null)
                return false;

            // Remove o cliente encontrado.
            _clientes.Remove(cliente);

            // Informa que a exclusão foi realizada.
            return true;
        }

        // Remove todos os clientes do ArrayList.
        public void Limpar()
        {
            _clientes.Clear();
        }

        // Retorna a quantidade de clientes cadastrados.
        public int Quantidade
        {
            get
            {
                // Retorna a quantidade de clientes armazenados na lista.
                return _clientes.Count;
            }
        }      

    }
}
