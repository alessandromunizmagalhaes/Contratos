using SAPbouiCOM;
using SAPHelper;
using System;
using System.Collections.Generic;

namespace CafebrasContratos
{
    public class FormContratoFinal : FormContrato
    {
        #region :: Propriedades

        private const string SRF = "FormContratoFinal.srf";
        public MatrizDTDocumentos _matrizDocumentos = new MatrizDTDocumentos()
        {
            ItemUID = "mtxDocs",
            Datasource = "Documentos"
        };

        #endregion


        #region :: Overrides

        public override string FormType { get { return "FormContratoFinal"; } }
        public override string MainDbDataSource { get { return new TabelaContratoFinal().NomeComArroba; } }
        public override string AnexoDbDataSource { get { return new TabelaAnexosDoContratoFinal().NomeComArroba; } }
        public override AbasContrato Abas { get { return new AbasContratoFinal(); } }

        public override Type FormAberturaPorPeneiraType { get { return typeof(FormContratoFinalAberturaPorPeneira); } }
        public override Type FormComissoesType { get { return typeof(FormContratoFinalComissoes); } }
        public override Type FormDetalheCertificadoType { get { return typeof(FormContratoFinalDetalheCertificado); } }

        public override string FormAberturaPorPeneiraSRF { get { return "FormContratoFinalAberturaPorPeneira.srf"; } }
        public override string FormComissoesSRF { get { return "FormContratoFinalComissoes.srf"; } }
        public override string FormDetalheCertificadoSRF { get { return "FormContratoFinalDetalheCertificado.srf"; } }

        public static string _fatherFormUID = "";

        public override bool UsuarioPermitido()
        {
            switch (Program._grupoAprovador)
            {
                case GrupoAprovador.Planejador:
                case GrupoAprovador.Gestor:
                    return true;
                default:
                    return false;
            }
        }

        public override void _OnAdicionarNovo(SAPbouiCOM.Form form)
        {
            base._OnAdicionarNovo(form);

            form.Items.Item(_descricao.ItemUID).Click();
        }

        protected override void FormEmModoVisualizacao(SAPbouiCOM.Form form)
        {
            base.FormEmModoVisualizacao(form);

            form.Items.Item(_statusQualidade.ItemUID).Enabled = false;
        }

        public override void IniciarValoresAoAdicionarNovo(SAPbouiCOM.Form form, DBDataSource dbdts)
        {
            _status.SetValorDBDatasource(dbdts, StatusContratoFinal.Esboço);
            _statusQualidade.SetValorDBDatasource(dbdts, StatusContratoFinalQualidade.PreAprovado);
        }

        public override string ProximaChavePrimaria(DBDataSource dbdts)
        {
            var numPreContrato = dbdts.GetValue(_numeroDoPreContrato.Datasource, 0);
            using (var recordset = new RecordSet())
            {
                var rs = recordset.DoQuery(
                    $@"SELECT 
                        CONVERT(NVARCHAR, {numPreContrato}) + '.' + CONVERT(NVARCHAR, COUNT(*) + 1) as codigo
                    FROM [{dbdts.TableName}] WHERE {_numeroDoPreContrato.Datasource} = {numPreContrato}");
                return rs.Fields.Item("codigo").Value;
            }
        }

        public override void RegrasDeNegocioAoSalvar(SAPbouiCOM.Form form, DBDataSource dbdts)
        {
            var numPreContrato = dbdts.GetValue(_numeroDoPreContrato.Datasource, 0);
            if (string.IsNullOrEmpty(numPreContrato))
            {
                throw new BusinessRuleException("Não foi possível identificar qual o Pré-Contrato referente a este Contrato Final.");
            }
        }

        public override void QuandoPuderAdicionarObjetoFilho(SAPbouiCOM.Form form)
        {
            ToggleBotao(form, true);
        }

        public override void QuandoNaoPuderAdicionarObjetoFilho(SAPbouiCOM.Form form)
        {
            ToggleBotao(form, false);
        }

        public override bool ContratoPodeSerAlterado(string status)
        {
            return status == StatusContratoFinal.Esboço
                || status == StatusContratoFinal.Renegociacao
                || status == StatusContratoFinal.Liberado
                || String.IsNullOrEmpty(status)
            ;
        }

        public override void ValidaAlteracaoDeStatus(GestaoStatusContrato gestaoStatus, string numeroContrato)
        {

        }

        #endregion


        #region :: Campos

        public ItemForm _numeroDoPreContrato = new ItemForm()
        {
            Datasource = "U_DocNumCC"
        };

        public override ItemForm _numeroDoContrato
        {
            get
            {
                return new ItemForm()
                {
                    ItemUID = "DocNumCF",
                    Datasource = "U_DocNumCF",
                };
            }
        }
        public ItemFormContrato _statusQualidade = new ItemFormContrato()
        {
            ItemUID = "StatusQua",
            Datasource = "U_StatusQua",
            gestaoCamposEmStatus = new GestaoCamposContrato()
            {
                QuandoEmEsboco = true,
                QuandoEmLiberado = true,
                QuandoEmRenegociacao = true,
                QuandoEmAutorizado = false,
                QuandoEmEncerrado = false,
                QuandoEmCancelado = false,
            }
        };

        #endregion


        #region :: Definição de Botões

        private ComboForm _botaoComboCopiar = new ComboForm()
        {
            ItemUID = "btnCopiar",
            ValoresPadrao = new Dictionary<string, string>() {
                {"1","Pedido de Compra"},
                {"2","Adiantamento para Fornecedor"},
                {"3","Recebimento de Mercadoria"},
                {"4","Devolução de Mercadoria"},
                {"5","Nota Fiscal de Entrada"},
                {"6","Devolução de Nota Fiscal de Entrada"},
            }
        };
        private static bool _adicionou = false;

        #endregion


        #region :: Eventos de Formulários

        public override void OnAfterFormDataAdd(ref BusinessObjectInfo BusinessObjectInfo, out bool BubbleEvent)
        {
            base.OnAfterFormDataAdd(ref BusinessObjectInfo, out BubbleEvent);

            using (var formCOM = new FormCOM(BusinessObjectInfo.FormUID))
            {
                var form = formCOM.Form;
                using (var dbdtsCOM = new DBDatasourceCOM(form, MainDbDataSource))
                {
                    var dbdts = dbdtsCOM.Dbdts;
                    if (BusinessObjectInfo.ActionSuccess)
                    {
                        _adicionou = true;
                        AtualizarSaldoPreContrato(dbdts);
                    }
                }
            }
        }

        public override void OnAfterFormDataUpdate(ref BusinessObjectInfo BusinessObjectInfo, out bool BubbleEvent)
        {
            base.OnAfterFormDataUpdate(ref BusinessObjectInfo, out BubbleEvent);

            using (var dbdtsCOM = new DBDatasourceCOM(BusinessObjectInfo.FormUID, MainDbDataSource))
            {
                var dbdts = dbdtsCOM.Dbdts;

                var status = _status.GetValorDBDatasource<string>(dbdts);
                if (status == StatusContratoFinal.Cancelado)
                {
                    CancelarSaldoContratoFinal(dbdts);
                }
                else
                {
                    AtualizarSaldoPreContrato(dbdts);
                }
            }
        }

        public override void OnAfterFormDataLoad(ref BusinessObjectInfo BusinessObjectInfo, out bool BubbleEvent)
        {
            BubbleEvent = true;


            base.OnAfterFormDataLoad(ref BusinessObjectInfo, out BubbleEvent);

            using (var formCOM = new FormCOM(BusinessObjectInfo.FormUID))
            {
                var form = formCOM.Form;
                AtualizarMatriz(form);
            }
        }

        #endregion


        #region :: Eventos de Item

        public override void OnAfterFormVisible(string FormUID, ref ItemEvent pVal, out bool BubbleEvent)
        {
            base.OnAfterFormVisible(FormUID, ref pVal, out BubbleEvent);

            using (var formCOM = new FormCOM(FormUID))
            {
                var form = formCOM.Form;
                DesabilitarMenuAdicionarNovo(form);

                form.Items.Item(_botaoComboCopiar.ItemUID).AffectsFormMode = false;
                var botaoComboCopiar = (ButtonCombo)form.Items.Item(_botaoComboCopiar.ItemUID).Specific;
                _botaoComboCopiar.Popular(botaoComboCopiar.ValidValues);

                var mtx = GetMatrix(form, _matrizDocumentos.ItemUID);
                _matrizDocumentos.Bind(mtx);

                if (form.Mode == BoFormMode.fm_ADD_MODE)
                {
                    Dialogs.Success("Carregando informações do Contrato de Compra Geral... Aguarde...", BoMessageTime.bmt_Long);

                    using (var fatherFormCOM = new FormCOM(_fatherFormUID))

                    using (var dbdtsCFCOM = new DBDatasourceCOM(form, MainDbDataSource))
                    using (var dbdtsPCCOM = new DBDatasourceCOM(_fatherFormUID, new TabelaPreContrato().NomeComArroba))

                    using (var dbdtsCFCertificadoCOM = new DBDatasourceCOM(form, new TabelaCertificadosDoContratoFinal().NomeComArroba))
                    using (var dbdtsPCCertificadoCOM = new DBDatasourceCOM(_fatherFormUID, new TabelaCertificadosDoPreContrato().NomeComArroba))

                    using (var dbdtsCFResponsavelCOM = new DBDatasourceCOM(form, new TabelaResponsaveisDoContratoFinal().NomeComArroba))
                    using (var dbdtsPCResponsavelCOM = new DBDatasourceCOM(_fatherFormUID, new TabelaResponsaveisDoPreContrato().NomeComArroba))

                    using (var dbdtsCFCorretorCOM = new DBDatasourceCOM(form, new TabelaCorretoresDoContratoFinal().NomeComArroba))
                    using (var dbdtsPCCorretorCOM = new DBDatasourceCOM(_fatherFormUID, new TabelaCorretoresDoPreContrato().NomeComArroba))
                    {
                        var fatherForm = fatherFormCOM.Form;

                        var dbdtsCF = dbdtsCFCOM.Dbdts;
                        var dbdtsPC = dbdtsPCCOM.Dbdts;

                        var dbdtsCFCertificado = dbdtsCFCertificadoCOM.Dbdts;
                        var dbdtsPCCertificado = dbdtsPCCertificadoCOM.Dbdts;

                        var dbdtsCFResponsavel = dbdtsCFResponsavelCOM.Dbdts;
                        var dbdtsPCResponsavel = dbdtsPCResponsavelCOM.Dbdts;

                        var dbdtsCFCorretor = dbdtsCFCorretorCOM.Dbdts;
                        var dbdtsPCCorretor = dbdtsPCCorretorCOM.Dbdts;

                        try
                        {
                            form.Freeze(true);

                            var labelsIn = string.Empty;
                            for (int i = 0; i < _peneiras.Count; i++)
                            {
                                labelsIn += ",'" + _peneiras[i].Datasource.Replace("P", "L") + "'";
                            }

                            CopyIfFieldsMatch(dbdtsPC, ref dbdtsCF, labelsIn);
                            CopyIfFieldsMatch(dbdtsPCCertificado, ref dbdtsCFCertificado);
                            CopyIfFieldsMatch(dbdtsPCResponsavel, ref dbdtsCFResponsavel);
                            CopyIfFieldsMatch(dbdtsPCCorretor, ref dbdtsCFCorretor);

                            var saldoSacas = Helpers.ToDouble(dbdtsPC.GetValue(_saldoDeSacas.Datasource, 0));
                            var saldoPeso = Helpers.ToDouble(dbdtsPC.GetValue(_saldoDePeso.Datasource, 0));

                            saldoSacas = saldoSacas < 0 ? 0 : saldoSacas;
                            saldoPeso = saldoPeso < 0 ? 0 : saldoPeso;

                            _quantidadeDeSacas.SetValorDBDatasource(dbdtsCF, saldoSacas);
                            _quantidadeDePeso.SetValorDBDatasource(dbdtsCF, saldoPeso);

                            CalcularTotais(form, dbdtsCF);

                            _OnAdicionarNovo(form);

                            PopularPessoasDeContato(form, dbdtsPC.GetValue(_codigoPN.Datasource, 0), dbdtsPC.GetValue(_pessoasDeContato.Datasource, 0));
                            HabilitarCamposDePeneira(form, dbdtsCF, dbdtsCF.GetValue(_codigoItem.Datasource, 0));
                        }
                        finally
                        {
                            form.Freeze(false);
                        }

                        Dialogs.Success("Ok.");
                    }
                }
            }
        }

        public override void OnAfterItemPressed(string FormUID, ref ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            if (pVal.ItemUID == "1")
            {
                using (var formCOM = new FormCOM(FormUID))
                {
                    var form = formCOM.Form;
                    if (form.Mode == BoFormMode.fm_ADD_MODE && _adicionou)
                    {
                        _adicionou = false;
                        form.Close();
                    }
                }
            }
            else
            {
                base.OnAfterItemPressed(FormUID, ref pVal, out BubbleEvent);
            }
        }

        public override void OnAfterFormClose(string FormUID, ref ItemEvent pVal, out bool BubbleEvent)
        {
            base.OnAfterFormClose(FormUID, ref pVal, out BubbleEvent);

            var formTypePreContrato = new FormPreContrato().FormType;
            for (int i = 0; i < Global.SBOApplication.Forms.Count; i++)
            {
                var currentForm = Global.SBOApplication.Forms.Item(i);
                if (currentForm.TypeEx == formTypePreContrato)
                {
                    new FormPreContrato().AtualizarMatriz(currentForm);
                }
            }
        }

        public override void OnBeforeComboSelect(string FormUID, ref ItemEvent pVal, out bool BubbleEvent)
        {
            if (pVal.ItemUID == _botaoComboCopiar.ItemUID)
            {
                BubbleEvent = false;

                using (var formCOM = new FormCOM(FormUID))
                {
                    var form = formCOM.Form;
                    if (form.Mode == BoFormMode.fm_OK_MODE)
                    {
                        using (var dbdtsCOM = new DBDatasourceCOM(form, MainDbDataSource))
                        {
                            var dbdts = dbdtsCOM.Dbdts;
                            var fornecedor = _codigoPN.GetValorDBDatasource<string>(dbdts);
                            var numContratoFinal = _numeroDoContrato.GetValorDBDatasource<string>(dbdts);
                            var transportadora = _transportadora.GetValorDBDatasource<string>(dbdts);

                            var codigoItem = _codigoItem.GetValorDBDatasource<string>(dbdts);
                            var deposito = _deposito.GetValorDBDatasource<string>(dbdts);
                            var utilizacao = _utilizacao.GetValorDBDatasource<string>(dbdts);
                            var safra = _safra.GetValorDBDatasource<string>(dbdts);
                            var embalagem = _embalagem.GetValorDBDatasource<string>(dbdts);
                            var quantidade = _saldoDePeso.GetValorDBDatasource<double>(dbdts);
                            var filial = GetFilial(_deposito.GetValorDBDatasource<string>(dbdts));
                            var precoUnitario = _valorFaturado.GetValorDBDatasource<double>(dbdts);

                            var objPedidoCompra = new FormPedidoCompra();
                            var formPedidoCompra = objPedidoCompra.Abrir();
                            objPedidoCompra.PreencherPedido(formPedidoCompra, new PedidoCompraParams()
                            {
                                NumContratoFinal = numContratoFinal,
                                Fornecedor = fornecedor,
                                Item = codigoItem,
                                Utilizacao = utilizacao,
                                Transportadora = transportadora,
                                Embalagem = embalagem,
                                Deposito = deposito,
                                Quantidade = quantidade,
                                PrecoUnitario = precoUnitario,
                                Filial = filial
                            });
                        }
                    }
                    else
                    {
                        Dialogs.PopupError("Salve o Contrato antes de criar um novo Documento.");
                    }
                }
            }
            else
            {
                base.OnAfterComboSelect(FormUID, ref pVal, out BubbleEvent);
            }
        }

        private string GetFilial(string deposito)
        {
            using (var rsCOM = new RecordSet())
            {
                var rs = rsCOM.DoQuery($"SELECT BPLID FROM OWHS WHERE WhsCode = '{deposito}'");
                return rs.Fields.Item("BPLID").Value.ToString();
            }
        }

        public override void OnBeforeMatrixLinkPressed(string FormUID, ref ItemEvent pVal, out bool BubbleEvent)
        {
            base.OnBeforeMatrixLinkPressed(FormUID, ref pVal, out BubbleEvent);

            if (pVal.ColUID == _matrizDocumentos.Codigo.ItemUID)
            {
                var mtx = GetMatrix(FormUID, _matrizDocumentos.ItemUID);
                var codigo = mtx.Columns.Item(pVal.ColUID).Cells.Item(pVal.Row).Specific.Value;

                new FormPedidoCompra().Abrir(codigo);
            }
        }

        #endregion


        #region :: Eventos Internos


        public override void _OnPesquisar(SAPbouiCOM.Form form)
        {
            base._OnPesquisar(form);
            form.Items.Item(_statusQualidade.ItemUID).Enabled = true;
        }

        #endregion


        #region :: Regras de Negócio

        private void AtualizarSaldoPreContrato(DBDataSource dbdts)
        {
            var tabelaPreContrato = Global.Company.UserTables.Item(new TabelaPreContrato().NomeSemArroba);
            var numPreContrato = _numeroDoPreContrato.GetValorDBDatasource<string>(dbdts);
            var numContratoFinal = _numeroDoContrato.GetValorDBDatasource<string>(dbdts);
            var codePreContrato = FormPreContrato.GetCode(numPreContrato);
            if (tabelaPreContrato.GetByKey(codePreContrato))
            {
                // tem que ser feito o cálculo levando em consideração a soma dos contratos filhos tirando eu mesmo,
                // porque senão, toda vez que eu atualizar o contrato final vai subtrair do pre contrato.
                using (var recordset = new RecordSet())
                {
                    var rs = recordset.DoQuery(
                        $@"SELECT 
	                        SUM(U_QtdSaca) as SaldoSaca, SUM(U_QtdPeso) as SaldoPeso, SUM(U_TLivre) as SaldoLivre
	                        FROM [@UPD_OCFC] 
	                        WHERE 1 = 1
		                        AND {_status.Datasource} <> '{StatusContratoFinal.Cancelado}' AND U_DocNumCC = {numPreContrato} AND U_DocNumCF <> {numContratoFinal}"
                    );

                    double sacasPreContrato = tabelaPreContrato.UserFields.Fields.Item(_quantidadeDeSacas.Datasource).Value;
                    double pesoPreContrato = tabelaPreContrato.UserFields.Fields.Item(_quantidadeDePeso.Datasource).Value;
                    double livrePreContrato = tabelaPreContrato.UserFields.Fields.Item(_totalLivre.Datasource).Value;

                    double saldoSacas = rs.Fields.Item("SaldoSaca").Value;
                    double saldoPeso = rs.Fields.Item("SaldoPeso").Value;
                    double saldoLivre = rs.Fields.Item("SaldoLivre").Value;

                    double sacasContratoFinal = _saldoDeSacas.GetValorDBDatasource<double>(dbdts);
                    double pesoContratoFinal = _saldoDePeso.GetValorDBDatasource<double>(dbdts);
                    double livreContratoFinal = _saldoFinanceiro.GetValorDBDatasource<double>(dbdts);

                    tabelaPreContrato.UserFields.Fields.Item(_saldoDeSacas.Datasource).Value = sacasPreContrato - saldoSacas - sacasContratoFinal;
                    tabelaPreContrato.UserFields.Fields.Item(_saldoDePeso.Datasource).Value = pesoPreContrato - saldoPeso - pesoContratoFinal;
                    tabelaPreContrato.UserFields.Fields.Item(_saldoFinanceiro.Datasource).Value = livrePreContrato - saldoLivre - livreContratoFinal;

                    tabelaPreContrato.Update();
                }
            }
        }

        private void CancelarSaldoContratoFinal(DBDataSource dbdts)
        {
            var tabelaPreContrato = Global.Company.UserTables.Item(new TabelaPreContrato().NomeSemArroba);
            var numPreContrato = _numeroDoPreContrato.GetValorDBDatasource<string>(dbdts);
            var numContratoFinal = _numeroDoContrato.GetValorDBDatasource<string>(dbdts);
            var codePreContrato = FormPreContrato.GetCode(numPreContrato);
            if (tabelaPreContrato.GetByKey(codePreContrato))
            {
                double sacasPreContrato = tabelaPreContrato.UserFields.Fields.Item(_saldoDeSacas.Datasource).Value;
                double pesoPreContrato = tabelaPreContrato.UserFields.Fields.Item(_saldoDePeso.Datasource).Value;
                double livrePreContrato = tabelaPreContrato.UserFields.Fields.Item(_saldoFinanceiro.Datasource).Value;

                double sacasContratoFinal = _saldoDeSacas.GetValorDBDatasource<double>(dbdts);
                double pesoContratoFinal = _saldoDePeso.GetValorDBDatasource<double>(dbdts);
                double livreContratoFinal = _saldoFinanceiro.GetValorDBDatasource<double>(dbdts);

                tabelaPreContrato.UserFields.Fields.Item(_saldoDeSacas.Datasource).Value = sacasPreContrato + sacasContratoFinal;
                tabelaPreContrato.UserFields.Fields.Item(_saldoDePeso.Datasource).Value = pesoPreContrato + pesoContratoFinal;
                tabelaPreContrato.UserFields.Fields.Item(_saldoFinanceiro.Datasource).Value = livrePreContrato + livreContratoFinal;

                tabelaPreContrato.Update();
            }
        }

        private void ToggleBotao(SAPbouiCOM.Form form, bool habilitado)
        {
            form.Items.Item(_botaoComboCopiar.ItemUID).Enabled = habilitado;
        }

        public void AtualizarMatriz(SAPbouiCOM.Form form)
        {
            var dbdts = GetDBDatasource(form, MainDbDataSource);
            var dt = GetDatatable(form, _matrizDocumentos.Datasource);

            var numeroContrato = _numeroDoContrato.GetValorDBDatasource<string>(dbdts);

            try
            {
                form.Freeze(true);
                dt.ExecuteQuery(
                    $@"SELECT 
	                    ObjType as Tipo
	                    , tb0.DocEntry
	                    , tb0.DocStatus
	                    , tb0.DocDate
	                    , tb1.ItemCode
	                    , tb1.Dscription
	                    , tb1.Quantity
	                    , tb0.DocTotal 
                    FROM OPOR tb0
                    INNER JOIN (
	                    SELECT 
		                    DocEntry, ItemCode, Dscription, Quantity, ROW_NUMBER() OVER ( PARTITION BY DocEntry ORDER BY DocEntry ) as count
	                    FROM POR1
                    )  tb1 ON (tb1.DocEntry = tb0.DocEntry AND tb1.count = 1)
                    WHERE 1 = 1 
	                    AND tb0.U_DocNumCF = '{numeroContrato}'"
                    );

                var mtx = GetMatrix(form, _matrizDocumentos.ItemUID);
                mtx.LoadFromDataSource();

                mtx.AutoResizeColumns();
            }
            finally
            {
                form.Freeze(false);
            }
        }

        #endregion


        #region :: Métodos Estáticos

        public static void AbrirNoRegistro(string codigo)
        {
            var findParams = new CriarFormFindParams() { chavePrimariaUID = "DocNumCF", chavePrimariaValor = codigo };
            var criarFormParams = new CriarFormParams() { Mode = BoFormMode.fm_FIND_MODE, FindParams = findParams };
            CriarForm(AppDomain.CurrentDomain.BaseDirectory + SRF, criarFormParams);
        }

        public static void AbrirCriandoNovoRegistro(string fatherFormUID)
        {
            var formParams = new CriarFormParams() { Mode = BoFormMode.fm_ADD_MODE };
            CriarFormFilho(AppDomain.CurrentDomain.BaseDirectory + SRF, fatherFormUID, new FormContratoFinal(), formParams);
        }

        #endregion


        public class MatrizDTDocumentos : MatrizDatatable
        {
            public ItemForm TipoDocumento = new ItemForm()
            {
                ItemUID = "tipo",
                Datasource = "Tipo"
            };
            public ItemForm Codigo = new ItemForm()
            {
                ItemUID = "docentry",
                Datasource = "DocEntry"
            };
            public ItemForm Status = new ItemForm()
            {
                ItemUID = "docstatus",
                Datasource = "DocStatus"
            };
            public ItemForm Data = new ItemForm()
            {
                ItemUID = "docdate",
                Datasource = "DocDate"
            };
            public ItemForm CodigoItem = new ItemForm()
            {
                ItemUID = "itemcode",
                Datasource = "ItemCode"
            };
            public ItemForm NomeItem = new ItemForm()
            {
                ItemUID = "itemname",
                Datasource = "ItemName"
            };
            public ItemForm ValorTotal = new ItemForm()
            {
                ItemUID = "doctotal",
                Datasource = "DocTotal"
            };
        }

        public class AbasContratoFinal : AbasContrato
        {
            public TabForm Documentos = new TabForm()
            {
                ItemUID = "AbaCFinal",
                PaneLevel = 3
            };
        }
    }
}
