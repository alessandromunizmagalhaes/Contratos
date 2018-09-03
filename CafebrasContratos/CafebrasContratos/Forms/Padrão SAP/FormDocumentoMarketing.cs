using SAPbouiCOM;
using SAPHelper;
using System;

namespace CafebrasContratos
{
    public abstract class FormDocumentoMarketing : SAPHelper.Form
    {
        public abstract override string FormType { get; }
        public abstract string mainDbDataSource { get; }
        public abstract BoFormObjectEnum formEnum { get; }

        public FormDocumentoMarketing()
        {
            var camposSAP = new CamposTabelaSAP();
            _numeroContratoFinal.ItemUID = camposSAP.numeroContratoFilho.NomeComU_NaFrente;
            _numeroContratoFinal.Datasource = _numeroContratoFinal.ItemUID;

            _qtdSaca.ItemUID = camposSAP.QuantidadeSacas.NomeComU_NaFrente;
            _qtdSaca.Datasource = _qtdSaca.ItemUID;

            _embalagem.ItemUID = camposSAP.Embalagem.NomeComU_NaFrente;
            _embalagem.Datasource = _embalagem.ItemUID;
            _embalagem.SQL = "SELECT PkgCode, PkgType FROM OPKG ORDER BY PkgType";

            _filhoDeContrato.Datasource = camposSAP.filhoDeContrato.NomeComU_NaFrente;
        }

        #region :: Campos

        public ComboForm _embalagem = new ComboForm();
        public ItemForm _numeroContratoFinal = new ItemForm();
        public ItemForm _qtdSaca = new ItemForm();
        public ItemForm _filhoDeContrato = new ItemForm()
        {
            ItemUID = "SOContract"
        };

        #endregion


        #region :: Eventos de Item

        public override void OnBeforeFormLoad(string FormUID, ref ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            using (var formCOM = new FormCOM(FormUID))
            {
                var form = formCOM.Form;
                CriarEDesenharCampos(form);
                CriarEDesenharCampoEmbalagem(form);
            }
        }

        private void CriarEDesenharCampos(SAPbouiCOM.Form form)
        {
            var itemRefUID = "46";
            var itemRef = form.Items.Item(itemRefUID);

            var itemLabelRefUID = "86";
            var itemLabelRef = form.Items.Item(itemLabelRefUID);

            var editNumeroContratoFinal = form.Items.Add(_numeroContratoFinal.ItemUID, BoFormItemTypes.it_EDIT);

            editNumeroContratoFinal.Enabled = false;
            editNumeroContratoFinal.FromPane = 0;
            editNumeroContratoFinal.ToPane = 0;

            editNumeroContratoFinal.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False);

            int comboTop = itemRef.Top + 23;
            editNumeroContratoFinal.Top = comboTop;
            editNumeroContratoFinal.Left = itemRef.Left;
            editNumeroContratoFinal.Width = itemRef.Width;
            editNumeroContratoFinal.DisplayDesc = true;

            ((EditText)editNumeroContratoFinal.Specific).DataBind.SetBound(true, mainDbDataSource, _numeroContratoFinal.Datasource);

            var labelGrupoAprovador = form.Items.Add("L_DocNumCF", BoFormItemTypes.it_STATIC);

            labelGrupoAprovador.FromPane = 0;
            labelGrupoAprovador.ToPane = 0;
            labelGrupoAprovador.Top = comboTop;
            labelGrupoAprovador.Left = itemLabelRef.Left;
            labelGrupoAprovador.Width = itemLabelRef.Width;
            labelGrupoAprovador.LinkTo = editNumeroContratoFinal.UniqueID;

            ((StaticText)labelGrupoAprovador.Specific).Caption = "Nº do Contrato";

            var linkedButton = form.Items.Add("B_DocNumCF", BoFormItemTypes.it_LINKED_BUTTON);

            linkedButton.Top = editNumeroContratoFinal.Top - 1;
            linkedButton.Left = editNumeroContratoFinal.Left - 19;
            linkedButton.LinkTo = editNumeroContratoFinal.UniqueID;

            ((LinkedButton)linkedButton.Specific).LinkedObject = BoLinkedObject.lf_BusinessPartner;

            var editVeioContrato = form.Items.Add(_filhoDeContrato.ItemUID, BoFormItemTypes.it_EDIT);

            editVeioContrato.Visible = false;
            editVeioContrato.FromPane = 0;
            editVeioContrato.ToPane = 0;

            editVeioContrato.Top = editNumeroContratoFinal.Top;
            editVeioContrato.Left = editNumeroContratoFinal.Left - 120;
            editVeioContrato.Width = 15;

            ((EditText)editVeioContrato.Specific).DataBind.SetBound(true, mainDbDataSource, _filhoDeContrato.Datasource);
        }

        private void CriarEDesenharCampoEmbalagem(SAPbouiCOM.Form form)
        {
            var itemRefUID = "2034";
            var itemRef = form.Items.Item(itemRefUID);

            var itemLabelRefUID = "2051";
            var itemLabelRef = form.Items.Item(itemLabelRefUID);

            var comboEmbalagem = form.Items.Add(_embalagem.ItemUID, BoFormItemTypes.it_COMBO_BOX);
            const int Pane = 8;
            comboEmbalagem.FromPane = Pane;
            comboEmbalagem.ToPane = Pane;

            int comboTop = itemRef.Top + 15;
            comboEmbalagem.Top = comboTop;
            comboEmbalagem.Left = itemRef.Left;
            comboEmbalagem.Width = itemRef.Width;
            comboEmbalagem.DisplayDesc = true;

            ((ComboBox)comboEmbalagem.Specific).DataBind.SetBound(true, mainDbDataSource, _embalagem.Datasource);

            var labelGrupoAprovador = form.Items.Add("L_Packg", BoFormItemTypes.it_STATIC);

            labelGrupoAprovador.FromPane = Pane;
            labelGrupoAprovador.ToPane = Pane;
            labelGrupoAprovador.Top = comboTop;
            labelGrupoAprovador.Left = itemLabelRef.Left;
            labelGrupoAprovador.Width = itemLabelRef.Width;
            labelGrupoAprovador.LinkTo = comboEmbalagem.UniqueID;

            ((StaticText)labelGrupoAprovador.Specific).Caption = "Embalagem";

            _embalagem.Popular(form);
        }

        public override void OnBeforeItemPressed(string FormUID, ref ItemEvent pVal, out bool BubbleEvent)
        {
            base.OnAfterItemPressed(FormUID, ref pVal, out BubbleEvent);

            if (pVal.ItemUID == "B_DocNumCF")
            {
                using (var formCOM = new FormCOM(FormUID))
                {
                    var form = formCOM.Form;
                    using (var dbdtsCOM = new DBDatasourceCOM(form, mainDbDataSource))
                    {
                        BubbleEvent = false;
                        var dbdts = dbdtsCOM.Dbdts;
                        var codigo = dbdts.GetValue("U_DocNumCF", 0).Trim();
                        FormContratoFinal.AbrirNoRegistro(codigo);
                    }
                }
            }
        }

        #endregion


        #region :: Regras de Negócio

        public void PreencherDocumento(SAPbouiCOM.Form form, DocMKTParams param)
        {
            try
            {
                form.Freeze(true);
                form.Mode = BoFormMode.fm_ADD_MODE;
                form.Items.Item(_numeroContratoFinal.ItemUID).Specific.Value = param.NumContratoFinal;
                AtualizarCampoFalandoQueVeioDeContrato(form);
                form.Items.Item("4").Specific.Value = param.Fornecedor;
                form.Items.Item(_numeroContratoFinal.ItemUID).Enabled = false;
                ((ComboBox)form.Items.Item(_embalagem.ItemUID).Specific).Select(param.Embalagem, BoSearchKey.psk_ByValue);

                if (!String.IsNullOrEmpty(param.Filial))
                {
                    ((ComboBox)form.Items.Item("2001").Specific).Select(param.Filial, BoSearchKey.psk_ByValue);
                }

                // aba imposto
                form.Items.Item("2013").Click();

                form.Items.Item("2022").Specific.Value = param.Transportadora;

                // aba geral
                form.Items.Item("112").Click();

                var matrix = GetMatrix(form, "38");
                matrix.Columns.Item("1").Cells.Item(1).Specific.Value = param.Item;
                if (param.Quantidade > 0)
                {
                    matrix.Columns.Item("11").Cells.Item(1).Specific.Value = Helpers.ToString(param.Quantidade);
                }

                form.Freeze(true);
                form.Freeze(false);

                ((ComboBox)matrix.Columns.Item("2011").Cells.Item(1).Specific).Select(param.Utilizacao, BoSearchKey.psk_ByValue);
                matrix.Columns.Item("24").Cells.Item(1).Specific.Value = param.Deposito;
                matrix.Columns.Item("14").Cells.Item(1).Specific.Value = Helpers.ToString(param.PrecoUnitario);
                matrix.Columns.Item(_qtdSaca.ItemUID).Cells.Item(1).Specific.Value = Helpers.ToString(param.QuantidadeSacas);

                matrix.Columns.Item("1").Cells.Item(1).Click();
                form.Items.Item("14").Click();
            }
            catch (Exception e)
            {
                Dialogs.PopupError("Erro interno. Erro ao preencher dados do pedido de compra.\nErro: " + e.Message);
            }
            finally
            {
                form.Freeze(false);
            }
        }

        protected void AtualizarCampoFalandoQueVeioDeContrato(SAPbouiCOM.Form form)
        {
            form.Items.Item(_filhoDeContrato.ItemUID).Specific.Value = "S";
        }

        public virtual SAPbouiCOM.Form Abrir(string codigo = "")
        {
            return Global.SBOApplication.OpenForm(formEnum, "DocEntry", codigo);
        }

        public virtual SAPbouiCOM.Form AbrirNaMao(string menuUID, string codigo = "")
        {
            Global.SBOApplication.ActivateMenuItem(menuUID);
            var form = Global.SBOApplication.Forms.GetFormByTypeAndCount(Int32.Parse(FormType), -1);
            if (!string.IsNullOrEmpty(codigo))
            {
                try
                {
                    form.Freeze(true);
                    form.Mode = BoFormMode.fm_FIND_MODE;
                    form.Items.Item("8").Specific.Value = codigo;
                    form.Items.Item("1").Click();
                }
                catch (Exception e)
                {
                    Dialogs.PopupError("erro ao encontrar registro. Erro " + e.Message);
                }
                finally
                {
                    form.Freeze(false);
                }
            }
            return form;
        }

        public void CopiarPara(SAPbouiCOM.Form formDe)
        {
            try
            {
                Dialogs.Info("Aguarde...");
                formDe.Freeze(true);
                var botaoSelecao = (ComboBox)formDe.Items.Item("10000329").Specific;
                botaoSelecao.Select(0, BoSearchKey.psk_Index);
            }
            catch (Exception e)
            {
                Dialogs.PopupError("Erro interno. Erro ao copiar documento base para documento destino. Erro: " + e.Message);
            }
            finally
            {
                formDe.Freeze(false);
                formDe.Close();
            }
        }

        #endregion


        #region :: Eventos Internos

        public override void _OnDuplicar(SAPbouiCOM.Form form)
        {
            form.Items.Item(_numeroContratoFinal.ItemUID).Specific.Value = "";
        }

        #endregion
    }
}
