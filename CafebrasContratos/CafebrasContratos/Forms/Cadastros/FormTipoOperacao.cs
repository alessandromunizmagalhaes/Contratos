using SAPHelper;

namespace CafebrasContratos
{
    public class FormTipoOperacao : FormCadastroBasico
    {
        public override string FormType { get { return "FormTipoOperacao"; } }
        public override string mainDbDataSource { get { return new TabelaTipoOperacao().NomeComArroba; } }
    }
}
