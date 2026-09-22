using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Loja.Core.Model;

namespace Loja.Core.Contoler
{
    public class ValidadorCliente
    {
        public bool EhValido(Cliente cliente)
        {
            if (cliente is null)
                return false;

            if (string.IsNullOrWhiteSpace(cliente.Nome))
                return false;

            if (string.IsNullOrWhiteSpace(cliente.Email))
                return false;

            return cliente.Email.Contains("@");
        }


    }
}
