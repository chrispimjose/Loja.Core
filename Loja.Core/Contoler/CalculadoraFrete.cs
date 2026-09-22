using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loja.Core.Contoler
{
    public class CalculadoraFrete
    {
        public decimal Calcular(decimal subtotal)
        {
            if (subtotal >= 300m)
                return 0m;

            return 25m;
        }

    }
}
