using SAPHelper;
using System.Collections.Generic;

namespace CafebrasContratos
{
    public class Versao_Zero_Um : Versionamento
    {
        public override double Versao { get => 0.1; }

        public override void Aplicar(Database db)
        {
            var tabelas = new List<Tabela>(){
                new TabelaModalidade(),
                new TabelaUnidadeComercial(),
                new TabelaTipoOperacao(),
                new TabelaMetodoFinanceiro(),
                new TabelaSafra(),
                new TabelaCertificado(),
                new TabelaParticipante(),
                new TabelaGrupoCafe(),
                new TabelaConfiguracaoPeneira(),
                new TabelaObjectTypes(),
                new TabelaPreContrato(),
                new TabelaContratoFinal()
            };

            for (int i = 0; i < tabelas.Count; i++)
            {
                Dialogs.Info($"Criando tabelas... {i + 1} de {tabelas.Count}... Aguarde...", SAPbouiCOM.BoMessageTime.bmt_Long);

                db.CriarTabela(tabelas[i]);
            }

            Dialogs.Info($"Criando campos de usuário...");

            var camposSAP = new CamposTabelaSAP();

            db.CriarCampo("OUSR", camposSAP.grupoAprovador);
            db.CriarCampo("OPOR", camposSAP.numeroContratoFilho);
            db.CriarCampo("OPOR", camposSAP.filhoDeContrato);

            Dialogs.Info($"Atualizando procedimentos armazenados...");

            var transactionNotification = new TransactionNotification();
            transactionNotification.CriarFuncoes();
            var postTransaction = new PostTransactionNotice();
            new SBO_SP_TransactionNotification().Atualizar(transactionNotification);
            new SBO_SP_PostTransactionNotice().Atualizar(postTransaction);
        }
    }
}
