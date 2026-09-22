using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loja.Core.Contoler
{
    public class PedidoServiceSimples
    {
        private readonly CalculadoraFrete _frete;

        public PedidoServiceSimples(CalculadoraFrete frete)
        {
            _frete = frete;
        }

        public decimal FecharPedido(decimal subtotal)
        {
            decimal valorFrete = _frete.Calcular(subtotal);
            return subtotal + valorFrete;
        }

    }
}
