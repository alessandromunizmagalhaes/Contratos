namespace CafebrasContratos
{
    public class FormPreContratoComissoes : FormComissoes
    {
        public override string FormType { get { return "FormPreContratoComissoes"; } }
        public override string corretorDbDataSource { get { return new TabelaCorretoresDoPreContrato().NomeComArroba; } }
        public override string responsavelDbDataSource { get { return new TabelaResponsaveisDoPreContrato().NomeComArroba; } }
        public override FormContrato formPai { get { return new FormPreContrato(); } }

        public override MatrizCorretores _corretores
        {
            get
            {
                return new MatrizCorretores()
                {
                    ItemUID = MatrixCorretoresUID,
                    Datasource = corretorDbDataSource
                };
            }
        }

        public override MatrizResponsaveis _responsaveis
        {
            get
            {
                return new MatrizResponsaveis()
                {
                    ItemUID = MatrixResponsaveisUID,
                    Datasource = responsavelDbDataSource
                };
            }
        }

        public override bool UsuarioPermitido()
        {
            return formPai.UsuarioPermitido();
        }
    }
}
