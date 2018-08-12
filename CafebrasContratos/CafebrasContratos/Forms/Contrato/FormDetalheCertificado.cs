using SAPbouiCOM;
using SAPHelper;
using System;

namespace CafebrasContratos
{
    public abstract class FormDetalheCertificado : SAPHelper.Form
    {
        public abstract override string FormType { get; }
        public abstract string mainDbDataSource { get; }

        protected string MatrixUID { get; private set; } = "matrix";

        public static string _fatherFormUID = "";

        #region :: Campos

        public abstract Matriz _matriz { get; }
        public ButtonForm _adicionar = new ButtonForm()
        {
            ItemUID = "btnAdd"
        };
        public ButtonForm _remover = new ButtonForm()
        {
            ItemUID = "btnRmv"
        };

        #endregion


        #region :: Eventos de Item

        public override void OnAfterFormVisible(string FormUID, ref ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            using (var formCOM = new FormCOM(FormUID))
            {
                var form = formCOM.Form;
                try
                {
                    form.Freeze(true);

                    _matriz._certificado.Popular(form, _matriz.ItemUID);

                    CarregarDadosMatriz(form, _fatherFormUID, _matriz.ItemUID, mainDbDataSource);

                    using (var fatherFormCOM = new FormCOM(_fatherFormUID))
                    {
                        var statusContratoPai = formPai.GetStatusPersistent(fatherFormCOM.Form);
                        form.Items.Item("1").Enabled = UsuarioPermitido() && formPai.ContratoPodeSerAlterado(statusContratoPai);
                    }
                }
                catch (Exception e)
                {
                    Dialogs.PopupError("Erro interno. Erro ao desenhar o form.\nErro: " + e.Message);
                }
                finally
                {
                    form.Freeze(false);
                }
            }
        }

        public override void OnBeforeItemPressed(string FormUID, ref ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            if (pVal.ItemUID == "1")
            {
                using (var formCOM = new FormCOM(FormUID))
                {
                    var form = formCOM.Form;
                    var mtx = ((Matrix)form.Items.Item(_matriz.ItemUID).Specific);
                    mtx.FlushToDataSource();

                    using (var dbdtsCOM = new DBDatasourceCOM(form, mainDbDataSource))
                    {
                        var dbdts = dbdtsCOM.Dbdts;
                        if (!CamposMatrizEstaoValidos(form, dbdts, _matriz))
                        {
                            BubbleEvent = false;
                        }
                    }
                }
            }
        }

        public override void OnAfterItemPressed(string FormUID, ref ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            if (pVal.ItemUID == _adicionar.ItemUID)
            {
                OnBotaoAdicionarClick(pVal);
            }
            else if (pVal.ItemUID == _remover.ItemUID)
            {
                OnBotaoRemoverClick(pVal);
            }
            else if (pVal.ItemUID == "1")
            {
                CarregarDataSourceFormPai(FormUID, _fatherFormUID, _matriz.ItemUID, mainDbDataSource);

                var fatherForm = GetFormIfExists(_fatherFormUID);
                if (fatherForm != null)
                {
                    ChangeFormMode(fatherForm);
                }
            }
        }

        private void OnBotaoAdicionarClick(ItemEvent pVal)
        {
            using (var formCOM = new FormCOM(pVal.FormUID))
            {
                var form = formCOM.Form;
                using (var dbdtsCOM = new DBDatasourceCOM(form, mainDbDataSource))
                {
                    var dbdts = dbdtsCOM.Dbdts;

                    _matriz.AdicionarLinha(form, dbdts);
                }
            }
        }

        private void OnBotaoRemoverClick(ItemEvent pVal)
        {
            using (var formCOM = new FormCOM(pVal.FormUID))
            {
                var form = formCOM.Form;
                using (var dbdtsCOM = new DBDatasourceCOM(form, mainDbDataSource))
                {
                    var dbdts = dbdtsCOM.Dbdts;

                    _matriz.RemoverLinhaSelecionada(form, dbdts);
                }
            }
        }

        #endregion


        #region :: Configuração da Matriz

        public class Matriz : MatrizChildForm
        {
            public ComboFormObrigatorioUnico _certificado = new ComboFormObrigatorioUnico()
            {
                ItemUID = "Certif",
                Datasource = "U_Certif",
                SQL = "SELECT Code, Name FROM [@UPD_CRTC] WHERE Canceled = 'N' ORDER BY Name",
                MensagemQuandoObrigatorio = "O certificado é obrigatório",
                MensagemQuandoUnico = "Não pode existir um certificado duplicado."
            };
        }

        #endregion


        #region :: Abstracts

        public abstract bool UsuarioPermitido();
        public abstract FormContrato formPai { get; }

        #endregion
    }
}