using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loja.Core.Model
{
    public class Cliente
    {
        public int Id { get; set; }
        public string Nome { get; set; } = "";
        public string Email { get; set; } = "";

        // Construtor vazio.
        // Permite criar um Cliente sem informar os dados inicialmente.
        public Cliente()
        {
        }

        // Construtor que recebe os dados do cliente.
        // Permite criar o objeto já preenchendo Id, Nome e Email.
        public Cliente(int id, string nome, string email)
        {
            Id = id;
            Nome = nome;
            Email = email;
        }

        public override string ToString()
        {
            // Retorna uma string contendo os dados do objeto.
            return $"{Id} - {Nome} - {Email}";
        }

    }
}
