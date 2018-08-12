using SAPbouiCOM;
using SAPHelper;
using System;

namespace CafebrasContratos
{
    public abstract class FormAberturaPorPeneira : SAPHelper.Form
    {
        public abstract override string FormType { get; }
        public abstract string mainDbDataSource { get; }
        public abstract string fatherFormMainDbDataSource { get; }
        public abstract Matriz _matriz { get; }

        public string MatrixUID { get; private set; } = "mtxItem";

        public static string _fatherFormUID = "";

        private const string chooseItemUID = "Item";

        #region :: Campos

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

                    _matriz.CriarColunaSumAuto(form, _matriz._percentual.ItemUID);
                    _matriz.CriarColunaSumAuto(form, _matriz._diferencial.ItemUID);

                    CarregarDadosMatriz(form, _fatherFormUID, _matriz.ItemUID, mainDbDataSource);

                    var mtx = ((Matrix)form.Items.Item(_matriz.ItemUID).Specific);
                    ClicarParaCalcularOsTotalizadores(mtx);

                    form.Items.Item("1").Enabled = UsuarioPermitido();

                    using (var fatherFormCOM = new FormCOM(_fatherFormUID))
                    {
                        var fatherForm = fatherFormCOM.Form;
                        using (var dbdtsCOM = new DBDatasourceCOM(fatherForm, fatherFormMainDbDataSource))
                        {
                            var fatherDbdts = dbdtsCOM.Dbdts;
                            var itemcodeBase = fatherDbdts.GetValue("U_ItemCode", 0).Trim();
                            ConditionsParaItens(form, itemcodeBase);
                        }
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

                        if (!CamposMatrizEstaoValidos(form, dbdts, _matriz) || !SomaDosPercentuaisEstaCorreta(mtx))
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

        public override void OnBeforeChooseFromList(SAPbouiCOM.Form form, ChooseFromListEvent chooseEvent, ChooseFromList choose, ref ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = FormContrato.TemGrupoDeItemConfiguradoParaChoose();
        }

        public override void OnAfterChooseFromList(SAPbouiCOM.Form form, ChooseFromListEvent chooseEvent, ChooseFromList choose, ref ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            var dataTable = chooseEvent.SelectedObjects;
            var mtx = ((Matrix)form.Items.Item(_matriz.ItemUID).Specific);

            var itemcode = dataTable.GetValue("ItemCode", 0);
            var itemname = dataTable.GetValue("ItemName", 0);
            if (ItemJaFoiUsado(itemcode, mtx))
            {
                Dialogs.PopupError($"O Item '{itemcode}' - '{itemname}' já foi usado.");
            }
            else
            {
                mtx.SetCellWithoutValidation(pVal.Row, _matriz._codigoItem.ItemUID, itemcode);
                mtx.SetCellWithoutValidation(pVal.Row, _matriz._nomeItem.ItemUID, itemname);

                ChangeFormMode(form);
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
                    var dbdts = GetDBDatasource(form, mainDbDataSource);

                    _matriz.RemoverLinhaSelecionada(form, dbdts);
                }
            }
        }

        #endregion


        #region :: Regras de Negócio

        private bool SomaDosPercentuaisEstaCorreta(Matrix mtx)
        {
            var percentual = SomaDosPercentuais(mtx);
            if (percentual == 100)
            {
                return true;
            }
            else
            {
                Dialogs.PopupError("A coluna percentual deve conter o total de 100%");
            }
            return false;
        }

        private double SomaDosPercentuais(Matrix mtx)
        {
            var soma_percentual = 0.0;
            for (int i = 1; i <= mtx.RowCount; i++)
            {
                var percentual = Helpers.ToDouble(mtx.GetCellSpecific(_matriz._percentual.ItemUID, i).Value);
                soma_percentual += percentual;
            }

            return soma_percentual;
        }

        private void ClicarParaCalcularOsTotalizadores(Matrix mtx)
        {
            for (int i = 1; i <= mtx.RowCount; i++)
            {
                mtx.Columns.Item(_matriz._percentual.ItemUID).Cells.Item(i).Click();
            }
        }

        private bool ItemJaFoiUsado(string itemcode, Matrix mtx)
        {
            for (int i = 1; i <= mtx.RowCount; i++)
            {
                if (mtx.GetCellSpecific(_matriz._codigoItem.ItemUID, i).Value == itemcode)
                {
                    return true;
                }
            }
            return false;
        }

        #endregion


        #region :: Conditions

        public void ConditionsParaItens(SAPbouiCOM.Form form, string itemcodeBase)
        {
            /* - COMENTANDO PORQUE ESSE FORM NÃO É UTILIZADO MAIS.
             * AS PENEIRAS AGORA SÃO CAMPOS NO CONTRATO.
            ChooseFromList oCFL = form.ChooseFromLists.Item(chooseItemUID);
            Conditions oConds = oCFL.GetConditions();

            using (var recordset = new RecordSet())
            {
                var rsGrupo = recordset.DoQuery(FormContrato.SQLGrupoDeItensPermitidos);
                if (rsGrupo.RecordCount > 0)
                {
                    int i = 0;
                    while (!rsGrupo.EoF)
                    {
                        i++;
                        string grupoDeItem = rsGrupo.Fields.Item("U_ItmsGrpCod").Value;

                        Condition oCond = oConds.Add();

                        if (i == 1)
                        {
                            oCond.BracketOpenNum = 1;
                        }

                        oCond.Alias = "ItmsGrpCod";
                        oCond.Operation = BoConditionOperation.co_EQUAL;
                        oCond.CondVal = grupoDeItem;

                        if (i == rsGrupo.RecordCount)
                        {
                            oCond.BracketCloseNum = 1;
                            oCond.Relationship = BoConditionRelationship.cr_AND;
                        }
                        else
                        {
                            oCond.Relationship = BoConditionRelationship.cr_OR;
                        }

                        rsGrupo.MoveNext();
                    }
                }
            }

            // só trazer itens ativos
            Condition oCondAtivo = oConds.Add();
            oCondAtivo.BracketOpenNum = 2;
            oCondAtivo.Alias = "frozenFor";
            oCondAtivo.Operation = BoConditionOperation.co_EQUAL;
            oCondAtivo.CondVal = "N";
            oCondAtivo.BracketCloseNum = 2;
            oCondAtivo.Relationship = BoConditionRelationship.cr_AND;

            using (var recordset = new RecordSet())
            {
                // só trazer os itens de peneira do item base
                var rsItens = recordset.DoQuery($"SELECT ItemCode FROM OITM WHERE U_UPD_ITEMBASE = '{itemcodeBase}' AND ItemCode <> '{itemcodeBase}'");
                if (rsItens.RecordCount > 0)
                {
                    int i = 0;
                    while (!rsItens.EoF)
                    {
                        i++;
                        string itemcode = rsItens.Fields.Item("ItemCode").Value;

                        Condition oCond = oConds.Add();

                        if (i == 1)
                        {
                            oCond.BracketOpenNum = 3;
                        }

                        oCond.Alias = "ItemCode";
                        oCond.Operation = BoConditionOperation.co_EQUAL;
                        oCond.CondVal = itemcode;

                        if (i == rsItens.RecordCount)
                        {
                            oCond.BracketCloseNum = 3;
                        }
                        else
                        {
                            oCond.Relationship = BoConditionRelationship.cr_OR;
                        }

                        rsItens.MoveNext();
                    }

                    oCFL.SetConditions(oConds);
                }
            }
            */
        }

        #endregion


        #region :: Configuração da Matriz

        public class Matriz : MatrizChildForm
        {
            public ItemFormObrigatorio _codigoItem = new ItemFormObrigatorio()
            {
                ItemUID = "ItemCode",
                Datasource = "U_ItemCode",
                Mensagem = "O Código do Item é obrigatório",
            };
            public ItemForm _nomeItem = new ItemForm()
            {
                ItemUID = "ItemName",
                Datasource = "U_ItemName"
            };
            public ItemFormObrigatorio _percentual = new ItemFormObrigatorio()
            {
                ItemUID = "PercItem",
                Datasource = "U_PercItem",
                Mensagem = "O percentual é obrigatório",
            };
            public ItemForm _diferencial = new ItemForm()
            {
                ItemUID = "Difere",
                Datasource = "U_Difere"
            };
        }

        #endregion


        #region :: Abstracts

        public abstract bool UsuarioPermitido();

        #endregion
    }
}
