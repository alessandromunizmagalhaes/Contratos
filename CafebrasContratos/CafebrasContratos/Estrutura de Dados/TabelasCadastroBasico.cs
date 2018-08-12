using SAPHelper;

namespace CafebrasContratos
{
    public class TabelaModalidade : TabelaCadastroBasico
    {
        public TabelaModalidade() : base("UPD_OMOD", "Cadastro de Modalidade")
        {

        }
    }

    public class TabelaUnidadeComercial : TabelaCadastroBasico
    {
        public TabelaUnidadeComercial() : base("UPD_OUCM", "Cadastro de Unidade Comercial")
        {

        }
    }

    public class TabelaTipoOperacao : TabelaCadastroBasico
    {
        public TabelaTipoOperacao() : base("UPD_OTOP", "Cadastro de Tipo de Operação")
        {

        }
    }

    public class TabelaMetodoFinanceiro : TabelaCadastroBasico
    {
        public TabelaMetodoFinanceiro() : base("UPD_OMFN", "Cadastro de Método Financeiro")
        {

        }
    }

    public class TabelaSafra : TabelaCadastroBasico
    {
        public TabelaSafra() : base("UPD_OSAF", "Cadastro de Safra do Item")
        {

        }
    }

    public class TabelaCertificado : TabelaCadastroBasico
    {
        public TabelaCertificado() : base("UPD_CRTC", "Cadastro do Certificado")
        {

        }
    }
}
