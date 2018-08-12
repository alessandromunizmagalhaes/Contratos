using SAPbobsCOM;
using SAPHelper;
using System.Collections.Generic;

namespace CafebrasContratos
{
    public class TabelaParticipante : TabelaUDO
    {
        public Coluna Tipo
        {
            get
            {
                return new ColunaVarchar("Tipo", "Tipo", 1, false, "C", new List<ValorValido>(){
                    new ValorValido("C","Corretor"),
                    new ValorValido("R","Responsável"),
                });
            }
        }

        public Coluna PercentualComissao { get { return new ColunaPercent("PercCom", "Percentual de Comissão"); } }

        public TabelaParticipante() : base("UPD_PART", "Cadastro de Participantes", BoUTBTableType.bott_MasterData, new UDOParams() { CanDelete = BoYesNoEnum.tNO, EnableEnhancedForm = BoYesNoEnum.tNO })
        {

        }
    }
}
