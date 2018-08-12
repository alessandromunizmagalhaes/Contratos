
namespace CafebrasContratos
{
    public class FormContratoFinalAberturaPorPeneira : FormAberturaPorPeneira
    {
        public override string FormType { get { return "FormContratoFinalAberturaPorPeneira"; } }
        public override string mainDbDataSource { get { return new TabelaItensDoContratoFinal().NomeComArroba; } }
        public override string fatherFormMainDbDataSource { get { return new TabelaContratoFinal().NomeComArroba; } }

        public override Matriz _matriz
        {
            get
            {
                return new Matriz()
                {
                    ItemUID = MatrixUID,
                    Datasource = mainDbDataSource
                };
            }
        }

        public override bool UsuarioPermitido()
        {
            return new FormContratoFinal().UsuarioPermitido();
        }
    }
}
