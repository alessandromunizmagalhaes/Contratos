namespace CafebrasContratos
{
    public class PedidoCompraParams
    {
        public string NumContratoFinal { get; set; }
        public string Fornecedor { get; set; }
        public string PessoaDeContato { get; set; }
        public string Transportadora { get; set; }
        public string Modalidade { get; set; }
        public string TipoOperacao { get; set; }
        public string MetodoFinanceiro { get; set; }
        public string UnidadeComercial { get; set; }
        public string Filial { get; set; }
        public string TipoDeEmbalagem { get; set; }

        public string Item { get; set; }
        public string Utilizacao { get; set; }
        public string Embalagem { get; set; }
        public string Deposito { get; set; }
        public double Quantidade { get; set; }
        public double QuantidadeSacas { get; set; }
        public double PrecoUnitario { get; set; }
    }
}
