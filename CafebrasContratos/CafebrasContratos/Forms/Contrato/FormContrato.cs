using SAPbouiCOM;
using SAPHelper;
using System;
using System.Collections.Generic;

namespace CafebrasContratos
{
    public abstract class FormContrato : SAPHelper.Form
    {
        #region :: Propriedades

        public abstract override string FormType { get; }
        public abstract string MainDbDataSource { get; }
        public abstract string AnexoDbDataSource { get; }
        public abstract AbasContrato Abas { get; }

        private const string abaGeralUID = "AbaGeral";
        private const string abaItemUID = "AbaItem";
        private const string abaContratoFinalUID = "AbaCFinal";
        private const string abaObsUID = "AbaObs";

        private const string choosePNUID = "PN";
        private const string chooseItemUID = "Item";
        private const string chooseDepositoUID = "Warehouse";

        #endregion


        #region :: Campos


        public abstract ItemForm _numeroDoContrato { get; }

        public ItemFormContrato _status = new ItemFormContrato()
        {
            ItemUID = "StatusCtr",
            Datasource = "U_StatusCtr",
            gestaoCamposEmStatus = new GestaoCamposContrato()
            {
                QuandoEmEsboco = true,
                QuandoEmLiberado = true,
                QuandoEmRenegociacao = true,
                QuandoEmAutorizado = true,
                QuandoEmEncerrado = false,
                QuandoEmCancelado = false,
            }
        };
        public ItemFormObrigatorioContrato _descricao = new ItemFormObrigatorioContrato()
        {
            ItemUID = "Descricao",
            Datasource = "U_Descricao",
            Mensagem = "A Descrição Geral é obrigatória.",
            gestaoCamposEmStatus = new GestaoCamposContrato()
            {
                QuandoEmEsboco = true,
                QuandoEmLiberado = true,
                QuandoEmRenegociacao = true,
                QuandoEmAutorizado = true,
                QuandoEmEncerrado = false,
                QuandoEmCancelado = false,
            }
        };
        public ItemFormObrigatorioContrato _dataInicio = new ItemFormObrigatorioContrato()
        {
            ItemUID = "DataIni",
            Datasource = "U_DataIni",
            Mensagem = "A Data de Início é obrigatória.",
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
        public ItemFormObrigatorioContrato _dataFim = new ItemFormObrigatorioContrato()
        {
            ItemUID = "DataFim",
            Datasource = "U_DataFim",
            Mensagem = "A Data Final é obrigatória.",
            gestaoCamposEmStatus = new GestaoCamposContrato()
            {
                QuandoEmEsboco = true,
                QuandoEmLiberado = true,
                QuandoEmRenegociacao = true,
                QuandoEmAutorizado = true,
                QuandoEmEncerrado = false,
                QuandoEmCancelado = false,
            }
        };

        public ItemFormObrigatorioContrato _codigoPN = new ItemFormObrigatorioContrato()
        {
            ItemUID = "CardCode",
            Datasource = "U_CardCode",
            Mensagem = "O Parceiro de Negócios é obrigatório.",
            AbaUID = abaGeralUID,
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
        public ItemForm _nomePN = new ItemForm()
        {
            ItemUID = "CardName",
            Datasource = "U_CardName",
        };
        public ItemForm _nomeEstrangeiro = new ItemForm()
        {
            ItemUID = "FrgnName",
            Datasource = "U_FrgnName",
        };
        public ItemForm _telefone = new ItemForm()
        {
            ItemUID = "Tel1",
            Datasource = "U_Tel1",
        };
        public ItemForm _email = new ItemForm()
        {
            ItemUID = "EMail",
            Datasource = "U_EMail",
        };
        public ComboFormObrigatorioContrato _pessoasDeContato = new ComboFormObrigatorioContrato()
        {
            ItemUID = "CtName",
            Datasource = "U_CtName",
            Mensagem = "A pessoa de contato é obrigatória.",
            AbaUID = abaGeralUID,
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

        public ItemFormContrato _previsaoEntrega = new ItemFormContrato()
        {
            ItemUID = "DtPrEnt",
            Datasource = "U_DtPrEnt",
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
        public ItemFormContrato _previsaoPagamento = new ItemFormContrato()
        {
            ItemUID = "DtPrPgt",
            Datasource = "U_DtPrPgt",
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

        public ComboFormObrigatorioContrato _modalidade = new ComboFormObrigatorioContrato()
        {
            ItemUID = "ModCtto",
            Datasource = "U_ModCtto",
            Mensagem = "A modalidade do contrato é obrigatória",
            SQL = "SELECT Code, Name FROM [@UPD_OMOD] WHERE Canceled = 'N' ORDER BY Name",
            AbaUID = abaGeralUID,
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
        public ComboFormObrigatorioContrato _unidadeComercial = new ComboFormObrigatorioContrato()
        {
            ItemUID = "UnidCom",
            Datasource = "U_UnidCom",
            Mensagem = "A unidade de comercial é obrigatória.",
            SQL = "SELECT Code, Name FROM [@UPD_OUCM] WHERE Canceled = 'N' ORDER BY Name",
            AbaUID = abaGeralUID,
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
        public ComboFormObrigatorioContrato _tipoDeOperacao = new ComboFormObrigatorioContrato()
        {
            ItemUID = "TipoOper",
            Datasource = "U_TipoOper",
            Mensagem = "O tipo de operação é obrigatório.",
            SQL = "SELECT Code, Name FROM [@UPD_OTOP] WHERE Canceled = 'N' ORDER BY Name",
            AbaUID = abaGeralUID,
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
        public ComboFormContrato _metodoFinanceiro = new ComboFormContrato()
        {
            ItemUID = "MtdFin",
            Datasource = "U_MtdFin",
            SQL = "SELECT Code, Name FROM [@UPD_OMFN] WHERE Canceled = 'N' ORDER BY Name",
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
        public ItemFormObrigatorioContrato _codigoItem = new ItemFormObrigatorioContrato()
        {
            ItemUID = "ItemCode",
            Datasource = "U_ItemCode",
            Mensagem = "O Item é obrigatório.",
            AbaUID = abaItemUID,
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
        public ItemForm _nomeItem = new ItemForm()
        {
            ItemUID = "ItemName",
            Datasource = "U_ItemName"
        };
        public ItemFormObrigatorioContrato _deposito = new ItemFormObrigatorioContrato()
        {
            ItemUID = "WhsCode",
            Datasource = "U_WhsCode",
            Mensagem = "O depósito é obrigatório.",
            AbaUID = abaItemUID,
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
        public ComboFormObrigatorioContrato _utilizacao = new ComboFormObrigatorioContrato()
        {
            ItemUID = "Usage",
            Datasource = "U_Usage",
            Mensagem = "A utilização é obrigatória.",
            SQL = "SELECT ID, Usage FROM OUSG ORDER BY Usage",
            AbaUID = abaItemUID,
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
        public ComboFormObrigatorioContrato _safra = new ComboFormObrigatorioContrato()
        {
            ItemUID = "Safra",
            Datasource = "U_Safra",
            Mensagem = "A safra é obrigatória.",
            SQL = "SELECT Code, Name FROM [@UPD_OSAF] WHERE Canceled = 'N' ORDER BY Name",
            AbaUID = abaItemUID,
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
        public ComboFormObrigatorioContrato _embalagem = new ComboFormObrigatorioContrato()
        {
            ItemUID = "Packg",
            Datasource = "U_Packg",
            Mensagem = "A embalagem é obrigatória.",
            SQL = "SELECT PkgCode, PkgType FROM OPKG ORDER BY PkgType",
            AbaUID = abaItemUID,
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
        public ItemFormContrato _bebida = new ItemFormContrato()
        {
            ItemUID = "Bebida",
            Datasource = "U_Bebida",
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
        public ItemFormContrato _diferencial = new ItemFormContrato()
        {
            ItemUID = "Difere",
            Datasource = "U_Difere",
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
        public ItemFormContrato _taxaNY = new ItemFormContrato()
        {
            ItemUID = "RateNY",
            Datasource = "U_RateNY",
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
        public ItemFormContrato _taxaDollar = new ItemFormContrato()
        {
            ItemUID = "RateUSD",
            Datasource = "U_RateUSD",
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

        public ItemFormContrato _quantidadeDePeso = new ItemFormContrato()
        {
            ItemUID = "QtdPeso",
            Datasource = "U_QtdPeso",
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
        public ItemForm _saldoDePeso = new ItemForm()
        {
            ItemUID = "SPesoRec",
            Datasource = "U_SPesoRec",
        };
        public ItemFormContrato _quantidadeDeSacas = new ItemFormContrato()
        {
            ItemUID = "QtdSaca",
            Datasource = "U_QtdSaca",
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
        public ItemForm _saldoDeSacas = new ItemForm()
        {
            ItemUID = "SPesoNCT",
            Datasource = "U_SPesoNCT",
        };
        public ItemFormContrato _valorLivre = new ItemFormContrato()
        {
            ItemUID = "VLivre",
            Datasource = "U_VLivre",
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
        public ItemForm _totalLivre = new ItemForm()
        {
            ItemUID = "TLivre",
            Datasource = "U_TLivre"
        };
        public ItemFormContrato _valorICMS = new ItemFormContrato()
        {
            ItemUID = "VICMS",
            Datasource = "U_VICMS",
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
        public ItemForm _totalICMS = new ItemForm()
        {
            ItemUID = "TICMS",
            Datasource = "U_TICMS"
        };
        public ItemFormContrato _valorSENAR = new ItemFormContrato()
        {
            ItemUID = "VSenar",
            Datasource = "U_VSenar",
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
        public ItemForm _totalSENAR = new ItemForm()
        {
            ItemUID = "TSenar",
            Datasource = "U_TSenar"
        };
        public ItemFormContrato _valorFaturado = new ItemFormContrato()
        {
            ItemUID = "VFat",
            Datasource = "U_VFat",
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
        public ItemForm _totalFaturado = new ItemForm()
        {
            ItemUID = "TFat",
            Datasource = "U_TFat"
        };
        public ItemFormContrato _valorBruto = new ItemFormContrato()
        {
            ItemUID = "VBruto",
            Datasource = "U_VBruto",
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
        public ItemForm _totalBruto = new ItemForm()
        {
            ItemUID = "TBruto",
            Datasource = "U_TBruto"
        };

        public ItemFormContrato _valorFrete = new ItemFormContrato()
        {
            ItemUID = "VlrFrete",
            Datasource = "U_VlrFrete",
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
        public ItemFormContrato _valorSeguro = new ItemFormContrato()
        {
            ItemUID = "VSeguro",
            Datasource = "U_VSeguro",
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
        public ItemFormContrato _transportadora = new ItemFormContrato()
        {
            ItemUID = "Transp",
            Datasource = "U_Transp",
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
        public ItemFormContrato _localRetirada = new ItemFormContrato()
        {
            ItemUID = "LocalRet",
            Datasource = "U_LocalRet",
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
        public ItemFormContrato _CidadeEntrega = new ItemFormContrato()
        {
            ItemUID = "CidadeEn",
            Datasource = "U_CidadeEn",
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
        public ItemForm _saldoFinanceiro = new ItemForm()
        {
            ItemUID = "SFin",
            Datasource = "U_SFin"
        };

        #endregion


        #region :: Campos Peneira

        public readonly List<ItemFormContrato> _peneiras = new List<ItemFormContrato>() {
            new ItemFormContrato(){
                ItemUID = "P01"
                , Datasource = "U_P01",
                gestaoCamposEmStatus = new GestaoCamposContrato()
                {
                    QuandoEmEsboco = true,
                QuandoEmLiberado = true,
                QuandoEmRenegociacao = true,
                    QuandoEmAutorizado = false,
                    QuandoEmEncerrado = false,
                    QuandoEmCancelado = false,
                }
            },
            new ItemFormContrato(){
                ItemUID = "P02"
                , Datasource = "U_P02",
                gestaoCamposEmStatus = new GestaoCamposContrato()
                {
                    QuandoEmEsboco = true,
                QuandoEmLiberado = true,
                QuandoEmRenegociacao = true,
                    QuandoEmAutorizado = false,
                    QuandoEmEncerrado = false,
                    QuandoEmCancelado = false,
                }
            },
            new ItemFormContrato(){
                ItemUID = "P03"
                , Datasource = "U_P03",
                gestaoCamposEmStatus = new GestaoCamposContrato()
                {
                    QuandoEmEsboco = true,
                QuandoEmLiberado = true,
                QuandoEmRenegociacao = true,
                    QuandoEmAutorizado = false,
                    QuandoEmEncerrado = false,
                    QuandoEmCancelado = false,
                }
            },
            new ItemFormContrato(){
                ItemUID = "P04"
                , Datasource = "U_P04",
                gestaoCamposEmStatus = new GestaoCamposContrato()
                {
                    QuandoEmEsboco = true,
                QuandoEmLiberado = true,
                QuandoEmRenegociacao = true,
                    QuandoEmAutorizado = false,
                    QuandoEmEncerrado = false,
                    QuandoEmCancelado = false,
                }
            },
            new ItemFormContrato(){
                ItemUID = "P05"
                , Datasource = "U_P05",
                gestaoCamposEmStatus = new GestaoCamposContrato()
                {
                    QuandoEmEsboco = true,
                QuandoEmLiberado = true,
                QuandoEmRenegociacao = true,
                    QuandoEmAutorizado = false,
                    QuandoEmEncerrado = false,
                    QuandoEmCancelado = false,
                }
            },
            new ItemFormContrato(){
                ItemUID = "P06"
                , Datasource = "U_P06",
                gestaoCamposEmStatus = new GestaoCamposContrato()
                {
                    QuandoEmEsboco = true,
                QuandoEmLiberado = true,
                QuandoEmRenegociacao = true,
                    QuandoEmAutorizado = false,
                    QuandoEmEncerrado = false,
                    QuandoEmCancelado = false,
                }
            },
            new ItemFormContrato(){
                ItemUID = "P07"
                , Datasource = "U_P07",
                gestaoCamposEmStatus = new GestaoCamposContrato()
                {
                    QuandoEmEsboco = true,
                QuandoEmLiberado = true,
                QuandoEmRenegociacao = true,
                    QuandoEmAutorizado = false,
                    QuandoEmEncerrado = false,
                    QuandoEmCancelado = false,
                }
            },
            new ItemFormContrato(){
                ItemUID = "P08"
                , Datasource = "U_P08",
                gestaoCamposEmStatus = new GestaoCamposContrato()
                {
                    QuandoEmEsboco = true,
                QuandoEmLiberado = true,
                QuandoEmRenegociacao = true,
                    QuandoEmAutorizado = false,
                    QuandoEmEncerrado = false,
                    QuandoEmCancelado = false,
                }
            },
            new ItemFormContrato(){
                ItemUID = "P09"
                , Datasource = "U_P09",
                gestaoCamposEmStatus = new GestaoCamposContrato()
                {
                    QuandoEmEsboco = true,
                QuandoEmLiberado = true,
                QuandoEmRenegociacao = true,
                    QuandoEmAutorizado = false,
                    QuandoEmEncerrado = false,
                    QuandoEmCancelado = false,
                }
            },
            new ItemFormContrato(){
                ItemUID = "P10"
                , Datasource = "U_P10",
                gestaoCamposEmStatus = new GestaoCamposContrato()
                {
                    QuandoEmEsboco = true,
                QuandoEmLiberado = true,
                QuandoEmRenegociacao = true,
                    QuandoEmAutorizado = false,
                    QuandoEmEncerrado = false,
                    QuandoEmCancelado = false,
                }
            },
            new ItemFormContrato(){
                ItemUID = "P11"
                , Datasource = "U_P11",
                gestaoCamposEmStatus = new GestaoCamposContrato()
                {
                    QuandoEmEsboco = true,
                QuandoEmLiberado = true,
                QuandoEmRenegociacao = true,
                    QuandoEmAutorizado = false,
                    QuandoEmEncerrado = false,
                    QuandoEmCancelado = false,
                }
            },
            new ItemFormContrato(){
                ItemUID = "P12"
                , Datasource = "U_P12",
                gestaoCamposEmStatus = new GestaoCamposContrato()
                {
                    QuandoEmEsboco = true,
                QuandoEmLiberado = true,
                QuandoEmRenegociacao = true,
                    QuandoEmAutorizado = false,
                    QuandoEmEncerrado = false,
                    QuandoEmCancelado = false,
                }
            },
            new ItemFormContrato(){
                ItemUID = "P13"
                , Datasource = "U_P13",
                gestaoCamposEmStatus = new GestaoCamposContrato()
                {
                    QuandoEmEsboco = true,
                QuandoEmLiberado = true,
                QuandoEmRenegociacao = true,
                    QuandoEmAutorizado = false,
                    QuandoEmEncerrado = false,
                    QuandoEmCancelado = false,
                }
            },
            new ItemFormContrato(){
                ItemUID = "P14"
                , Datasource = "U_P14",
                gestaoCamposEmStatus = new GestaoCamposContrato()
                {
                    QuandoEmEsboco = true,
                QuandoEmLiberado = true,
                QuandoEmRenegociacao = true,
                    QuandoEmAutorizado = false,
                    QuandoEmEncerrado = false,
                    QuandoEmCancelado = false,
                }
            },
            new ItemFormContrato(){
                ItemUID = "P15"
                , Datasource = "U_P15",
                gestaoCamposEmStatus = new GestaoCamposContrato()
                {
                    QuandoEmEsboco = true,
                QuandoEmLiberado = true,
                QuandoEmRenegociacao = true,
                    QuandoEmAutorizado = false,
                    QuandoEmEncerrado = false,
                    QuandoEmCancelado = false,
                }
            },
        };

        public ItemForm _totalPeneira = new ItemForm() { ItemUID = "totalP", Datasource = "totalP" };
        public ItemForm _totalDiferencial = new ItemForm() { ItemUID = "totalD", Datasource = "totalD" };

        #endregion


        #region :: Matrizes

        public MatrizAnexos _matrixAnexos = new MatrizAnexos() { ItemUID = "mtxAnexos" };

        #endregion


        #region :: Botões

        public ButtonForm _aberturaPorPeneira = new ButtonForm()
        {
            ItemUID = "btnAbertur",
        };
        public ButtonForm _certificado = new ButtonForm()
        {
            ItemUID = "btnCertif",
        };
        public ButtonForm _comissoes = new ButtonForm()
        {
            ItemUID = "btnComiss",
        };

        #endregion


        #region :: Eventos de Formulário

        public override void OnBeforeFormDataAdd(ref BusinessObjectInfo BusinessObjectInfo, out bool BubbleEvent)
        {
            if (UsuarioPermitido())
            {
                using (var formCOM = new FormCOM(BusinessObjectInfo.FormUID))
                {
                    var form = formCOM.Form;
                    using (var dbdtsCOM = new DBDatasourceCOM(form, MainDbDataSource))
                    {
                        var dbdts = dbdtsCOM.Dbdts;

                        Dialogs.Info("Adicionando pré contrato... Aguarde...", BoMessageTime.bmt_Medium);

                        BubbleEvent = FormEstaValido(form, dbdts);
                        if (BubbleEvent)
                        {
                            string next_code = GetNextCode(MainDbDataSource);

                            dbdts.SetValue("Code", 0, next_code);
                            dbdts.SetValue("Name", 0, next_code);

                            _numeroDoContrato.SetValorDBDatasource(dbdts, ProximaChavePrimaria(dbdts));
                            SalvarLabelPeneiras(form, dbdts);
                        }
                    }
                }
            }
            else
            {
                BubbleEvent = false;
                Dialogs.PopupError("O usuário corrente não pode criar um novo contrato.");
            }
        }

        public override void OnBeforeFormDataUpdate(ref BusinessObjectInfo BusinessObjectInfo, out bool BubbleEvent)
        {
            if (UsuarioPermitido())
            {
                using (var formCOM = new FormCOM(BusinessObjectInfo.FormUID))
                {
                    var form = formCOM.Form;
                    using (var dbdtsCOM = new DBDatasourceCOM(form, MainDbDataSource))
                    {
                        var dbdts = dbdtsCOM.Dbdts;

                        Dialogs.Info("Atualizando pré contrato... Aguarde...", BoMessageTime.bmt_Medium);

                        BubbleEvent = FormEstaValido(form, dbdts);
                        if (!BubbleEvent)
                        {
                            return;
                        }

                        var gestaoStatus = new GestaoStatusContrato(GetStatusPersistent(form), GetStatusVolatil(form.UniqueID));
                        if (gestaoStatus.EstaTrocandoStatus())
                        {
                            try
                            {
                                ValidaAlteracaoDeStatus(gestaoStatus, _numeroDoContrato.GetValorDBDatasource<string>(dbdts));
                            }
                            catch (BusinessRuleException e)
                            {
                                Dialogs.PopupError(e.Message);
                                BubbleEvent = false;
                            }
                            catch (Exception e)
                            {
                                Dialogs.PopupError("Erro interno. Erro ao tentar validar a troca de status do contrato.\nErro: " + e.Message);
                                BubbleEvent = false;
                            }

                            if (!BubbleEvent)
                            {
                                return;
                            }

                            GerenciarCamposQuandoEmStatus(form, gestaoStatus.StatusVolatil);
                            GerenciarQuandoPodeAdicionarObjetoFilho(form, gestaoStatus.StatusVolatil);
                        }
                    }
                }
            }
            else
            {
                BubbleEvent = false;
                Dialogs.PopupError("O usuário corrente não pode atualizar os dados de um contrato.");
            }
        }

        public override void OnAfterFormDataLoad(ref BusinessObjectInfo BusinessObjectInfo, out bool BubbleEvent)
        {
            BubbleEvent = true;
            using (var formCOM = new FormCOM(BusinessObjectInfo.FormUID))
            {
                var form = formCOM.Form;

                if (form.Items.Item(_numeroDoContrato.ItemUID).Enabled)
                {
                    form.Items.Item(_numeroDoContrato.ItemUID).Enabled = false;
                }

                form.Items.Item(_status.ItemUID).Enabled = UsuarioPermitido();

                using (var dbdtsCOM = new DBDatasourceCOM(form, MainDbDataSource))
                {
                    var dbdts = dbdtsCOM.Dbdts;

                    GerirCamposPeneiraPegandoDoBanco(form, dbdts);

                    var codigoPN = _codigoPN.GetValorDBDatasource<string>(dbdts);
                    var pessoasDeContato = _pessoasDeContato.GetValorDBDatasource<string>(dbdts);
                    PopularPessoasDeContato(form, codigoPN, pessoasDeContato);


                    var status = _status.GetValorDBDatasource<string>(dbdts);
                    GerenciarCamposQuandoEmStatus(form, status);

                    bool contratoPodeSerAlterado = ContratoPodeSerAlterado(status);
                    if (contratoPodeSerAlterado)
                    {
                        var itemcode = _codigoItem.GetValorDBDatasource<string>(dbdts);
                        HabilitarBotaoAberturaPorPeneira(form, itemcode);
                        HabilitarCamposDePeneira(form, dbdts, itemcode);
                    }

                    GerenciarQuandoPodeAdicionarObjetoFilho(form, status);
                }
            }
        }

        #endregion


        #region :: Eventos de Item

        public override void OnAfterFormVisible(string FormUID, ref ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            using (var formCOM = new FormCOM(FormUID))
            {
                var form = formCOM.Form;
                if (String.IsNullOrEmpty(Program._grupoAprovador))
                {
                    Dialogs.PopupError("Nenhum Grupo Aprovador foi configurado para este usúario.\nNão será possível abrir a tela de contratos.");

                    form.Close();
                    BubbleEvent = false;
                }
                else
                {
                    try
                    {
                        form.Freeze(true);

                        _modalidade.Popular(form);
                        _unidadeComercial.Popular(form);
                        _tipoDeOperacao.Popular(form);
                        _metodoFinanceiro.Popular(form);
                        _utilizacao.Popular(form);
                        _embalagem.Popular(form);
                        _safra.Popular(form);

                        if (!UsuarioPermitido())
                        {
                            FormEmModoVisualizacao(form);
                        }

                        // clicando para a primeira aba já vir selecionada
                        form.Items.Item(Abas.DefinicoesGerais.ItemUID).Click();

                        ConditionsParaFornecedores(form);
                        ConditionsParaDeposito(form);
                        ConditionsParaItens(form);
                    }
                    catch (Exception e)
                    {
                        Dialogs.PopupError("Erro interno. Erro ao iniciar dados do formulário\nErro: " + e.Message);
                    }
                    finally
                    {
                        form.Freeze(false);
                    }
                }
            }
        }

        public override void OnAfterComboSelect(string FormUID, ref ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            if (pVal.ItemUID == _pessoasDeContato.ItemUID)
            {
                var dbdts = GetDBDatasource(FormUID, MainDbDataSource);
                string pessoaDeContato = dbdts.GetValue(_pessoasDeContato.Datasource, 0);
                string cardcode = dbdts.GetValue(_codigoPN.Datasource, 0);
                AtualizarDadosPessoaDeContato(cardcode, pessoaDeContato, dbdts);
            }/*
            else if (pVal.ItemUID == _status.ItemUID)
            {
                var form = GetForm(FormUID);
                var dbdts = GetDBDatasource(form, MainDbDataSource);

                var status = _status.GetValorDBDatasource<string>(dbdts);
                GerenciarCamposQuandoEmStatus(form, status);
            }*/
        }

        public override void OnAfterItemPressed(string FormUID, ref ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            if (pVal.ItemUID == _aberturaPorPeneira.ItemUID)
            {
                var formAberturaPorPeneira = Activator.CreateInstance(FormAberturaPorPeneiraType);
                CriarFormFilho(baseDirectory + FormAberturaPorPeneiraSRF, FormUID, (SAPHelper.Form)formAberturaPorPeneira);
            }
            else if (pVal.ItemUID == _certificado.ItemUID)
            {
                var formDetalheCertificado = Activator.CreateInstance(FormDetalheCertificadoType);
                CriarFormFilho(baseDirectory + FormDetalheCertificadoSRF, FormUID, (SAPHelper.Form)formDetalheCertificado);
            }
            else if (pVal.ItemUID == _comissoes.ItemUID)
            {
                var formComissoes = Activator.CreateInstance(FormComissoesType);
                CriarFormFilho(baseDirectory + FormComissoesSRF, FormUID, (SAPHelper.Form)formComissoes);
            }
            else if (pVal.ItemUID == _matrixAnexos._adicionar.ItemUID)
            {
                using (var formCOM = new FormCOM(FormUID))
                {
                    var form = formCOM.Form;
                    using (var dbdtsCOM = new DBDatasourceCOM(form, AnexoDbDataSource))
                    {
                        var dbdts = dbdtsCOM.Dbdts;
                        _matrixAnexos.AdicionarLinha(form, dbdts);
                    }
                }
            }
            else if (pVal.ItemUID == _matrixAnexos._remover.ItemUID)
            {
                using (var formCOM = new FormCOM(FormUID))
                {
                    var form = formCOM.Form;
                    using (var dbdtsCOM = new DBDatasourceCOM(form, AnexoDbDataSource))
                    {
                        var dbdts = dbdtsCOM.Dbdts;
                        _matrixAnexos.RemoverLinhaSelecionada(form, dbdts);
                    }
                }
            }
        }

        public override void OnAfterDoubleClick(string FormUID, ref ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            if (pVal.ItemUID == _matrixAnexos.ItemUID)
            {
                using (var formCOM = new FormCOM(FormUID))
                {
                    var form = formCOM.Form;
                    var matrix = GetMatrix(form, _matrixAnexos.ItemUID);

                    var data = matrix.GetCellSpecific(_matrixAnexos._data.ItemUID, pVal.Row).Value.ToString();
                    var arquivo = matrix.GetCellSpecific(_matrixAnexos._arquivoAnexado.ItemUID, pVal.Row).Value.ToString();
                    if (String.IsNullOrEmpty(data) && !String.IsNullOrEmpty(arquivo))
                    {
                        matrix.SetCellWithoutValidation(pVal.Row, _matrixAnexos._data.ItemUID, DateTime.Now.ToString("yyyyMMdd"));
                    }
                }
            }
        }

        public override void OnAfterValidate(string FormUID, ref ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            if (EventoEmCampoDeValor(pVal.ItemUID))
            {
                using (var formCOM = new FormCOM(FormUID))
                {
                    var form = formCOM.Form;
                    using (var dbdtsCOM = new DBDatasourceCOM(form, MainDbDataSource))
                    {
                        var dbdts = dbdtsCOM.Dbdts;
                        CalcularTotais(form, dbdts);
                    }
                }
            }
            else if (EventoEmCampoDePeneira(pVal.ItemUID))
            {
                using (var formCOM = new FormCOM(FormUID))
                {
                    var form = formCOM.Form;
                    using (var dbdtsCOM = new DBDatasourceCOM(form, MainDbDataSource))
                    {
                        var dbdts = GetDBDatasource(form, MainDbDataSource);
                        AtualizarSomaDosPercentuaisDePeneira(form, dbdts);
                    }
                }
            }
            else if (EventoEmCampoDeDiferencial(pVal.ItemUID))
            {
                using (var formCOM = new FormCOM(FormUID))
                {
                    var form = formCOM.Form;
                    using (var dbdtsCOM = new DBDatasourceCOM(form, MainDbDataSource))
                    {
                        var dbdts = GetDBDatasource(form, MainDbDataSource);
                        AtualizarSomaDosDiferenciais(form, dbdts);
                    }
                }
            }
        }

        public override void OnBeforeChooseFromList(SAPbouiCOM.Form form, ChooseFromListEvent chooseEvent, ChooseFromList choose, ref ItemEvent pVal, out bool BubbleEvent)
        {
            if (pVal.ItemUID == _codigoItem.ItemUID)
            {
                BubbleEvent = TemGrupoDeItemConfiguradoParaChoose();
            }
            else
            {
                BubbleEvent = true;
            }
        }

        public override void OnAfterChooseFromList(SAPbouiCOM.Form form, ChooseFromListEvent chooseEvent, ChooseFromList choose, ref ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            if (!choose.IsSystem && form.Mode != BoFormMode.fm_FIND_MODE)
            {
                var dbdts = GetDBDatasource(pVal.FormUID, MainDbDataSource);
                var dataTable = chooseEvent.SelectedObjects;

                if (chooseEvent.ItemUID == _codigoPN.ItemUID)
                {
                    OnCardCodeChoose(form, dbdts, dataTable);
                }
                else if (chooseEvent.ItemUID == _codigoItem.ItemUID)
                {
                    OnItemCodeChoose(form, dbdts, dataTable);
                }
                else if (chooseEvent.ItemUID == _deposito.ItemUID)
                {
                    OnWhsCodeChoose(dbdts, dataTable);
                }

                ChangeFormMode(form);
            }
        }

        #endregion


        #region :: Eventos Internos

        public override void _OnAdicionarNovo(SAPbouiCOM.Form form)
        {
            form.Items.Item(_numeroDoContrato.ItemUID).Enabled = false;

            var dbdts = GetDBDatasource(form, MainDbDataSource);

            IniciarValoresAoAdicionarNovo(form, dbdts);

            _numeroDoContrato.SetValorDBDatasource(dbdts, ProximaChavePrimaria(dbdts));
            _dataInicio.SetValorDBDatasource(dbdts, DateTime.Now);

            AtualizarSomaDosPercentuaisDePeneira(form, dbdts);
            AtualizarSomaDosDiferenciais(form, dbdts);

            PopularPessoasDeContato(form, "", "");

            GerirCamposPeneiraPegandoDaConfiguracao(form);

            GerenciarCamposQuandoEmStatus(form, StatusPreContrato.Esboço);
            QuandoNaoPuderAdicionarObjetoFilho(form);

            form.Items.Item(_status.ItemUID).Enabled = false;

            var mtx = GetMatrix(form, _matrixAnexos.ItemUID);
            mtx.AddRow();
        }

        public override void _OnPesquisar(SAPbouiCOM.Form form)
        {
            form.Items.Item(_numeroDoContrato.ItemUID).Enabled = true;
            form.Items.Item(_status.ItemUID).Enabled = true;

            form.Items.Item(_numeroDoContrato.ItemUID).Click();
        }

        #endregion


        #region :: Choose From Lists

        private void OnItemCodeChoose(SAPbouiCOM.Form form, DBDataSource dbdts, DataTable dataTable)
        {
            string itemcode = dataTable.GetValue("ItemCode", 0);
            string itemname = dataTable.GetValue("ItemName", 0);

            _codigoItem.SetValorDBDatasource(dbdts, itemcode);
            _nomeItem.SetValorDBDatasource(dbdts, itemname);

            HabilitarBotaoAberturaPorPeneira(form, itemcode);
            HabilitarCamposDePeneira(form, dbdts, itemcode);
        }

        private void OnWhsCodeChoose(DBDataSource dbdts, DataTable dataTable)
        {
            string whscode = dataTable.GetValue("WhsCode", 0);
            _deposito.SetValorDBDatasource(dbdts, whscode);
        }

        private void OnCardCodeChoose(SAPbouiCOM.Form form, DBDataSource dbdts, DataTable dataTable)
        {
            string cardcode = dataTable.GetValue("CardCode", 0);
            string cardname = dataTable.GetValue("CardName", 0);
            string pessoaDeContato = dataTable.GetValue("CntctPrsn", 0);
            string nomeEstrangeiro = dataTable.GetValue("CardFName", 0);

            _codigoPN.SetValorDBDatasource(dbdts, cardcode);
            _nomePN.SetValorDBDatasource(dbdts, cardname);
            _nomeEstrangeiro.SetValorDBDatasource(dbdts, nomeEstrangeiro);

            PopularPessoasDeContato(form, cardcode, pessoaDeContato);
        }

        #endregion


        #region :: Conditions

        private void ConditionsParaFornecedores(SAPbouiCOM.Form form)
        {
            ChooseFromList oCFL = form.ChooseFromLists.Item(choosePNUID);
            Conditions oConds = oCFL.GetConditions();

            Condition oCond = oConds.Add();

            oCond.Alias = "CardType";
            oCond.Operation = BoConditionOperation.co_EQUAL;
            oCond.CondVal = "S";

            oCFL.SetConditions(oConds);
        }

        private void ConditionsParaDeposito(SAPbouiCOM.Form form)
        {
            ChooseFromList oCFL = form.ChooseFromLists.Item(chooseDepositoUID);
            Conditions oConds = oCFL.GetConditions();

            Condition oCond = oConds.Add();

            oCond.Alias = "Inactive";
            oCond.Operation = BoConditionOperation.co_EQUAL;
            oCond.CondVal = "N";

            oCFL.SetConditions(oConds);
        }

        public void ConditionsParaItens(SAPbouiCOM.Form form)
        {
            ChooseFromList oCFL = form.ChooseFromLists.Item(chooseItemUID);
            Conditions oConds = oCFL.GetConditions();

            if (Program._gruposDeItensPermitidos.Count > 0)
            {
                for (int i = 0; i < Program._gruposDeItensPermitidos.Count; i++)
                {
                    string grupoDeItem = Program._gruposDeItensPermitidos[i];

                    Condition oCond = oConds.Add();

                    if (i == 0)
                    {
                        oCond.BracketOpenNum = 1;
                    }

                    oCond.Alias = "ItmsGrpCod";
                    oCond.Operation = BoConditionOperation.co_EQUAL;
                    oCond.CondVal = grupoDeItem;

                    if (i == (Program._gruposDeItensPermitidos.Count - 1))
                    {
                        oCond.BracketCloseNum = 1;
                        oCond.Relationship = BoConditionRelationship.cr_AND;
                    }
                    else
                    {
                        oCond.Relationship = BoConditionRelationship.cr_OR;
                    }
                }

                // só trazer itens ativos
                Condition oCondAtivo = oConds.Add();
                oCondAtivo.BracketOpenNum = 2;
                oCondAtivo.Alias = "frozenFor";
                oCondAtivo.Operation = BoConditionOperation.co_EQUAL;
                oCondAtivo.CondVal = "N";
                oCondAtivo.BracketCloseNum = 2;

                oCFL.SetConditions(oConds);
            }

        }

        #endregion


        #region :: Regras de negócio

        protected virtual void FormEmModoVisualizacao(SAPbouiCOM.Form form)
        {
            form.Items.Item(_descricao.ItemUID).Enabled = false;
            form.Items.Item(_codigoPN.ItemUID).Enabled = false;
            form.Items.Item(_dataInicio.ItemUID).Enabled = false;
            form.Items.Item(_dataFim.ItemUID).Enabled = false;
            form.Items.Item(_pessoasDeContato.ItemUID).Enabled = false;
            form.Items.Item(_previsaoEntrega.ItemUID).Enabled = false;
            form.Items.Item(_previsaoPagamento.ItemUID).Enabled = false;
            form.Items.Item(_modalidade.ItemUID).Enabled = false;
            form.Items.Item(_tipoDeOperacao.ItemUID).Enabled = false;
            form.Items.Item(_unidadeComercial.ItemUID).Enabled = false;
            form.Items.Item(_metodoFinanceiro.ItemUID).Enabled = false;
            form.Items.Item(_quantidadeDePeso.ItemUID).Enabled = false;
            form.Items.Item(_quantidadeDeSacas.ItemUID).Enabled = false;
            form.Items.Item(_valorLivre.ItemUID).Enabled = false;
            form.Items.Item(_valorICMS.ItemUID).Enabled = false;
            form.Items.Item(_valorSENAR.ItemUID).Enabled = false;
            form.Items.Item(_valorFaturado.ItemUID).Enabled = false;
            form.Items.Item(_valorBruto.ItemUID).Enabled = false;
            form.Items.Item(_valorFrete.ItemUID).Enabled = false;
            form.Items.Item(_valorSeguro.ItemUID).Enabled = false;
            form.Items.Item(_transportadora.ItemUID).Enabled = false;
            form.Items.Item(_localRetirada.ItemUID).Enabled = false;

            form.Items.Item(_codigoItem.ItemUID).Enabled = false;
            form.Items.Item(_deposito.ItemUID).Enabled = false;
            form.Items.Item(_utilizacao.ItemUID).Enabled = false;
            form.Items.Item(_safra.ItemUID).Enabled = false;
            form.Items.Item(_embalagem.ItemUID).Enabled = false;
            form.Items.Item(_bebida.ItemUID).Enabled = false;
            form.Items.Item(_diferencial.ItemUID).Enabled = false;
            form.Items.Item(_taxaNY.ItemUID).Enabled = false;
            form.Items.Item(_taxaDollar.ItemUID).Enabled = false;

            foreach (var peneira in _peneiras)
            {
                form.Items.Item(peneira.ItemUID).Enabled = false;
                form.Items.Item(peneira.ItemUID.Replace("P", "D")).Enabled = false;
            }
        }

        protected void PopularPessoasDeContato(SAPbouiCOM.Form form, string cardcode, string pessoaDeContatoSelecionada)
        {
            _pessoasDeContato.SQL =
                    $@"SELECT 
	                    Name, ISNULL(FirstName,Name)
                    FROM OCPR 
                    WHERE CardCode = '{cardcode}'
                    ORDER BY ISNULL(FirstName,Name)";
            _pessoasDeContato.Popular(form);

            var dbdts = GetDBDatasource(form, MainDbDataSource);
            _pessoasDeContato.SetValorDBDatasource(dbdts, pessoaDeContatoSelecionada);
            AtualizarDadosPessoaDeContato(cardcode, pessoaDeContatoSelecionada, dbdts);
        }

        private void AtualizarDadosPessoaDeContato(string cardcode, string pessoaDeContato, DBDataSource dbdts)
        {
            var sql =
                $@"SELECT
                        Tel1
	                    , E_MailL
                    FROM OCPR 
                    WHERE CardCode = '{cardcode}' AND Name = '{pessoaDeContato}'";
            using (var recordset = new RecordSet())
            {
                var rs = recordset.DoQuery(sql);

                var telefone = "";
                var email = "";

                if (rs.RecordCount > 0)
                {
                    telefone = rs.Fields.Item("Tel1").Value;
                    email = rs.Fields.Item("E_MailL").Value;
                }

                _telefone.SetValorDBDatasource(dbdts, telefone);
                _email.SetValorDBDatasource(dbdts, email);
            }
        }

        private bool EventoEmCampoDeValor(string itemUID)
        {
            return
                itemUID == _quantidadeDePeso.ItemUID
                || itemUID == _quantidadeDeSacas.ItemUID
                || itemUID == _valorLivre.ItemUID
                || itemUID == _valorICMS.ItemUID
                || itemUID == _valorSENAR.ItemUID
                || itemUID == _valorFaturado.ItemUID
                || itemUID == _valorBruto.ItemUID
            ;
        }

        private bool EventoEmCampoDePeneira(string itemUID)
        {
            return _peneiras.Find(p => p.ItemUID == itemUID) != null;
        }

        private bool EventoEmCampoDeDiferencial(string itemUID)
        {
            return _peneiras.Find(p => p.ItemUID.Replace("P", "D") == itemUID) != null;
        }

        protected void CalcularTotais(SAPbouiCOM.Form form, DBDataSource dbdts)
        {
            double qtdPeso = _quantidadeDePeso.GetValorDBDatasource<double>(dbdts);
            double qtdSacas = _quantidadeDeSacas.GetValorDBDatasource<double>(dbdts);
            double valorLivre = _valorLivre.GetValorDBDatasource<double>(dbdts);
            double valorICMS = _valorICMS.GetValorDBDatasource<double>(dbdts);
            double valorSENAR = _valorSENAR.GetValorDBDatasource<double>(dbdts);
            double valorFaturado = _valorFaturado.GetValorDBDatasource<double>(dbdts);
            double valorBruto = _valorBruto.GetValorDBDatasource<double>(dbdts);

            try
            {
                form.Freeze(true);
                var totalLivre = valorLivre * qtdSacas;
                _totalLivre.SetValorDBDatasource(dbdts, totalLivre);
                _totalICMS.SetValorDBDatasource(dbdts, valorICMS * qtdSacas);
                _totalSENAR.SetValorDBDatasource(dbdts, valorSENAR * qtdSacas);
                _totalFaturado.SetValorDBDatasource(dbdts, valorFaturado * qtdSacas);
                _totalBruto.SetValorDBDatasource(dbdts, valorBruto * qtdSacas);

                _saldoDePeso.SetValorDBDatasource(dbdts, qtdPeso);
                _saldoDeSacas.SetValorDBDatasource(dbdts, qtdSacas);
                _saldoFinanceiro.SetValorDBDatasource(dbdts, totalLivre);
            }
            finally
            {
                form.Freeze(false);
            }
        }

        private void HabilitarBotaoAberturaPorPeneira(SAPbouiCOM.Form form, string itemcode)
        {
            /* -- código comentado porque agora a abertura de peneira são campos na tela.
            var botao_habilitado = false;
            var rs = Helpers.DoQuery($"SELECT U_UPD_TIPO_ITEM FROM OITM WHERE ItemCode = '{itemcode}'");
            if (rs.Fields.Item("U_UPD_TIPO_ITEM").Value == "B")
            {
                botao_habilitado = true;
            }
            form.Items.Item(_aberturaPorPeneira.ItemUID).Enabled = botao_habilitado;
            */
        }

        protected void HabilitarCamposDePeneira(SAPbouiCOM.Form form, DBDataSource dbdts, string itemcode)
        {
            var deve_habilitar = ItemTipoBica(itemcode);

            foreach (var peneira in _peneiras)
            {
                var diferencialItemUID = peneira.ItemUID.Replace("P", "D");
                form.Items.Item(peneira.ItemUID).Enabled = deve_habilitar;
                form.Items.Item(diferencialItemUID).Enabled = deve_habilitar;

                if (!deve_habilitar)
                {
                    peneira.SetValorDBDatasource(dbdts, 0);
                    new ItemForm() { ItemUID = diferencialItemUID, Datasource = "U_" + diferencialItemUID }.SetValorDBDatasource(dbdts, 0);
                }
            }

            AtualizarSomaDosPercentuaisDePeneira(form, dbdts);
            AtualizarSomaDosDiferenciais(form, dbdts);
        }

        private bool FormEstaValido(SAPbouiCOM.Form form, DBDataSource dbdts)
        {
            return CamposFormEstaoPreenchidos(form, dbdts) && RegrasDeNegocioEstaoValidas(form, dbdts);
        }

        private bool RegrasDeNegocioEstaoValidas(SAPbouiCOM.Form form, DBDataSource dbdts)
        {
            try
            {
                var dataInicial = _dataInicio.GetValorDBDatasource<DateTime>(dbdts);
                var dataFinal = _dataFim.GetValorDBDatasource<DateTime>(dbdts);

                if (dataFinal < dataInicial)
                {
                    throw new FormValidationException("Data final do contrato não pode ser menor do que a Data inicial.", _dataFim.ItemUID);
                }

                RegrasDeNegocioAoSalvar(form, dbdts);
                if (ItemTipoBica(dbdts.GetValue(_codigoItem.Datasource, 0)))
                {
                    ValidarSomaDosPercentuaisDePeneira(dbdts);
                }
            }
            catch (BusinessRuleException e)
            {
                Dialogs.MessageBox(e.Message);

                return false;
            }
            catch (FormValidationException e)
            {
                Dialogs.MessageBox(e.Message);
                if (!String.IsNullOrEmpty(e.AbaUID))
                {
                    form.Items.Item(e.AbaUID).Click();
                }
                form.Items.Item(e.Campo).Click();
                return false;
            }
            catch (Exception e)
            {
                Dialogs.PopupError("Erro interno. Erro ao realizar as validações de regras de negócio do formulário.\nErro: " + e.Message);
                return false;
            }

            return true;
        }

        private void ValidarSomaDosPercentuaisDePeneira(DBDataSource dbdts)
        {
            if (SomaDosPercentuaisDePeneira(dbdts) != 100.00)
            {
                throw new FormValidationException("A soma dos percentuais de peneira, devem ser 100%", "P01", abaItemUID);
            }
        }

        private int SomaDosPercentuaisDePeneira(DBDataSource dbdts)
        {
            var totalPeneiras = 0;
            foreach (var peneira in _peneiras)
            {
                string valor = dbdts.GetValue(peneira.Datasource, 0);
                if (!string.IsNullOrEmpty(valor))
                {
                    totalPeneiras += Helpers.ToInt(valor);
                }
            }

            return totalPeneiras;
        }

        private double SomaDosDiferenciais(DBDataSource dbdts)
        {
            var total = 0.0;
            foreach (var peneira in _peneiras)
            {
                var diferencialDataSource = peneira.Datasource.Replace("P", "D");
                total += Helpers.ToDouble(dbdts.GetValue(diferencialDataSource, 0));
            }

            return total;
        }

        private void AtualizarSomaDosPercentuaisDePeneira(SAPbouiCOM.Form form, DBDataSource dbdts)
        {
            _totalPeneira.SetValorUserDataSource(form, SomaDosPercentuaisDePeneira(dbdts));
        }

        private void AtualizarSomaDosDiferenciais(SAPbouiCOM.Form form, DBDataSource dbdts)
        {
            _totalDiferencial.SetValorUserDataSource(form, SomaDosDiferenciais(dbdts));
        }

        private void PosicionarCamposDeTotaisDePeneiras(SAPbouiCOM.Form form)
        {
            try
            {
                var topFromLast = 21;
                var topLast = form.Items.Item(_peneiras[_peneiras.Count - 1].ItemUID).Top;

                if (form.Items.Item(_peneiras[0].ItemUID).Visible)
                {
                    for (int i = 0; i < _peneiras.Count; i++)
                    {
                        var peneira = _peneiras[i];
                        if (!form.Items.Item(peneira.ItemUID).Visible)
                        {
                            topLast = form.Items.Item(_peneiras[i - 1].ItemUID).Top;
                            break;
                        }
                    }
                }

                var totalTop = topFromLast + topLast;

                form.Items.Item("lbl" + _totalPeneira.ItemUID).Top = totalTop;
                form.Items.Item(_totalPeneira.ItemUID).Top = totalTop;
                form.Items.Item(_totalDiferencial.ItemUID).Top = totalTop;
            }
            catch (Exception e)
            {
                Dialogs.PopupError("Erro interno. Erro ao posicionar campos totalizadores de peneiras.\nErro:" + e.Message);
            }
        }

        private void GerirCamposPeneiraPegandoDaConfiguracao(SAPbouiCOM.Form form)
        {
            try
            {
                // tem que por esse try/finally porque precisa de dar um freeze
                // tem que dar o freeze porque eu só consigo fazer o item ficar invisivel
                // se a aba estiver visivel.
                // estou clicando na aba, deixando os itens invisiveis e voltando pra aba normal.
                form.Freeze(true);
                var ultimaAbaSelecionada = Abas.ActiveFolder(form);
                form.Items.Item(Abas.Itens.ItemUID).Click();

                for (int i = 0; i < Program._peneiras.Count; i++)
                {
                    string peneiraUID = Program._peneiras[i].UID;
                    bool ativo = Program._peneiras[i].Ativo;
                    var itemVisivel = form.Items.Item(peneiraUID).Visible;
                    var labelPeneiraUID = "lbl" + peneiraUID;
                    var diferencialUID = peneiraUID.Replace("P", "D");

                    if (ativo)
                    {
                        ((StaticText)form.Items.Item(labelPeneiraUID).Specific).Caption = Program._peneiras[i].Nome;
                    }

                    if (ativo && !itemVisivel)
                    {
                        form.Items.Item(peneiraUID).Visible = true;
                        form.Items.Item(labelPeneiraUID).Visible = true;
                        form.Items.Item(diferencialUID).Visible = true;
                    }
                    else if (!ativo && itemVisivel)
                    {
                        form.Items.Item(peneiraUID).Visible = false;
                        form.Items.Item(labelPeneiraUID).Visible = false;
                        form.Items.Item(diferencialUID).Visible = false;
                    }
                }

                // para posicionar o campo de total, tem que estar com a aba clicada, senão o visible sempre retorna false
                PosicionarCamposDeTotaisDePeneiras(form);

                form.Items.Item(ultimaAbaSelecionada).Click();
            }
            finally
            {
                form.Freeze(false);
            }
        }

        private void GerirCamposPeneiraPegandoDoBanco(SAPbouiCOM.Form form, DBDataSource dbdts)
        {
            try
            {
                // tem que por esse try/finally porque precisa de dar um freeze
                // tem que dar o freeze porque eu só consigo fazer o item ficar invisivel
                // se a aba estiver visivel.
                // estou clicando na aba, deixando os itens invisiveis e voltando pra aba normal.
                form.Freeze(true);
                var ultimaAbaSelecionada = Abas.ActiveFolder(form);
                form.Items.Item(Abas.Itens.ItemUID).Click();

                for (int i = 0; i < _peneiras.Count; i++)
                {
                    var itemPeneira = _peneiras[i];
                    var labelPeneiraDataSource = itemPeneira.Datasource.Replace("P", "L");
                    var labelPeneira = dbdts.GetValue(labelPeneiraDataSource, 0).Trim();
                    var ativo = !String.IsNullOrEmpty(labelPeneira);
                    var itemVisivel = form.Items.Item(itemPeneira.ItemUID).Visible;
                    var labelPeneiraUID = "lbl" + itemPeneira.ItemUID;
                    var diferencialUID = itemPeneira.ItemUID.Replace("P", "D");

                    if (ativo)
                    {
                        ((StaticText)form.Items.Item(labelPeneiraUID).Specific).Caption = labelPeneira;
                    }

                    if (ativo && !itemVisivel)
                    {
                        form.Items.Item(itemPeneira.ItemUID).Visible = true;
                        form.Items.Item(labelPeneiraUID).Visible = true;
                        form.Items.Item(diferencialUID).Visible = true;
                    }
                    else if (!ativo && itemVisivel)
                    {
                        form.Items.Item(itemPeneira.ItemUID).Visible = false;
                        form.Items.Item(labelPeneiraUID).Visible = false;
                        form.Items.Item(diferencialUID).Visible = false;
                    }
                }

                // para posicionar o campo de total, tem que estar com a aba clicada, senão o visible sempre retorna false
                PosicionarCamposDeTotaisDePeneiras(form);

                form.Items.Item(ultimaAbaSelecionada).Click();
            }
            catch (Exception e)
            {
                Dialogs.PopupError("Erro interno. Erro ao gerir campos gravados deste formulário.\nErro: " + e.Message);
            }
            finally
            {
                form.Freeze(false);
            }
        }

        private void SalvarLabelPeneiras(SAPbouiCOM.Form form, DBDataSource dbdts)
        {
            var lastAbaUID = Abas.ActiveFolder(form);

            form.Items.Item(abaItemUID).Click();

            for (int i = 0; i < _peneiras.Count; i++)
            {
                var lblUID = "lbl" + _peneiras[i].ItemUID;

                if (form.Items.Item(lblUID).Visible)
                {
                    var label = ((StaticText)form.Items.Item(lblUID).Specific).Caption;
                    var lblDataSource = _peneiras[i].Datasource.Replace("P", "L");
                    dbdts.SetValue(lblDataSource, 0, label);
                }
            }

            form.Items.Item(lastAbaUID).Click();
        }

        private bool ItemTipoBica(string itemcode)
        {
            using (var recordset = new RecordSet())
            {
                var rs = recordset.DoQuery($"SELECT U_UPD_TIPO_ITEM FROM OITM WHERE ItemCode = '{itemcode}'");
                return rs.Fields.Item("U_UPD_TIPO_ITEM").Value == "B";
            }
        }

        public static bool TemGrupoDeItemConfiguradoParaChoose()
        {
            if (Program._gruposDeItensPermitidos.Count == 0)
            {
                Dialogs.PopupInfo("Nenhum grupo de item foi configurado para filtrar esta apresentação de itens.");
                return false;
            }
            else
            {
                return true;
            }
        }

        private void GerenciarCamposQuandoEmStatus(SAPbouiCOM.Form form, string status)
        {
            try
            {
                form.Items.Item("focus").Click();

                form.Freeze(true);
                var fields = GetType().GetFields();
                foreach (var field in fields)
                {
                    if (field.FieldType == typeof(ItemFormContrato)
                        || field.FieldType == typeof(ItemFormObrigatorioContrato)
                        || field.FieldType == typeof(ComboFormContrato)
                        || field.FieldType == typeof(ComboFormObrigatorioContrato))
                    {
                        var prop = (ItemForm)field.GetValue(this);
                        var propInterface = (IItemFormContrato)field.GetValue(this);

                        var enable = DefinirStatusCampo(status, propInterface);
                        form.Items.Item(prop.ItemUID).Enabled = enable;
                    }
                    else if (field.FieldType == typeof(List<ItemFormContrato>))
                    {
                        var prop = (List<ItemFormContrato>)field.GetValue(this);
                        foreach (var item in prop)
                        {
                            var propInterface = (IItemFormContrato)item;
                            var enable = DefinirStatusCampo(status, propInterface);
                            form.Items.Item(item.ItemUID).Enabled = enable;
                            form.Items.Item(item.ItemUID.Replace("P", "D")).Enabled = enable;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Dialogs.PopupError("Erro ao desabilitar campos ao trocar status do contrato. Erro: " + e.Message);
            }
            finally
            {
                form.Freeze(false);
            }
        }

        private bool DefinirStatusCampo(string status, IItemFormContrato propInterface)
        {
            switch (status)
            {
                case StatusContratoFinal.Esboço:
                    return propInterface.gestaoCamposEmStatus.QuandoEmEsboco;
                case StatusContratoFinal.Renegociacao:
                    return propInterface.gestaoCamposEmStatus.QuandoEmRenegociacao;
                case StatusContratoFinal.Liberado:
                    return propInterface.gestaoCamposEmStatus.QuandoEmLiberado;
                case StatusContratoFinal.Autorizado:
                    return propInterface.gestaoCamposEmStatus.QuandoEmAutorizado;
                case StatusContratoFinal.Encerrado:
                    return propInterface.gestaoCamposEmStatus.QuandoEmEncerrado;
                case StatusContratoFinal.Cancelado:
                    return propInterface.gestaoCamposEmStatus.QuandoEmCancelado;
                default:
                    Dialogs.PopupError($"Erro interno. Tipo de situação {status} não encontrado");
                    return false;
            }
        }

        private bool CamposAutorizaveisEstaoAtivos(SAPbouiCOM.Form form)
        {
            return form.Items.Item(_codigoPN.ItemUID).Enabled;
        }

        private bool CamposAutorizaveisNaoEstaoAtivos(SAPbouiCOM.Form form)
        {
            return !CamposAutorizaveisEstaoAtivos(form);
        }

        public string GetStatusVolatil(string formUID)
        {
            var dbdts = GetDBDatasource(formUID, MainDbDataSource);
            var status = _status.GetValorDBDatasource<string>(dbdts);

            return status;
        }

        public string GetStatusPersistent(SAPbouiCOM.Form form)
        {
            var dbdts = GetDBDatasource(form, MainDbDataSource);
            var code = dbdts.GetValue("Code", 0).Trim();
            using (var recordset = new RecordSet())
            {
                var rs = recordset.DoQuery($"SELECT {_status.Datasource} FROM [{MainDbDataSource}] WHERE Code = '{code}'");
                return rs.Fields.Item(_status.Datasource).Value;
            }
        }

        private void GerenciarQuandoPodeAdicionarObjetoFilho(SAPbouiCOM.Form form, string status)
        {
            if (status == StatusPreContrato.Autorizado)
            {
                QuandoPuderAdicionarObjetoFilho(form);
            }
            else
            {
                QuandoNaoPuderAdicionarObjetoFilho(form);
            }
        }

        #endregion


        #region :: Abstracts

        public abstract bool UsuarioPermitido();
        public abstract Type FormAberturaPorPeneiraType { get; }
        public abstract Type FormComissoesType { get; }
        public abstract Type FormDetalheCertificadoType { get; }
        public abstract string FormAberturaPorPeneiraSRF { get; }
        public abstract string FormComissoesSRF { get; }
        public abstract string FormDetalheCertificadoSRF { get; }
        public abstract void IniciarValoresAoAdicionarNovo(SAPbouiCOM.Form form, DBDataSource dbdts);
        public abstract string ProximaChavePrimaria(DBDataSource dbdts);
        public abstract void RegrasDeNegocioAoSalvar(SAPbouiCOM.Form form, DBDataSource dbdts);
        public abstract void QuandoPuderAdicionarObjetoFilho(SAPbouiCOM.Form form);
        public abstract void QuandoNaoPuderAdicionarObjetoFilho(SAPbouiCOM.Form form);
        public abstract bool ContratoPodeSerAlterado(string status);
        public abstract void ValidaAlteracaoDeStatus(GestaoStatusContrato gestaoStatus, string numeroContrato);

        #endregion


        public interface IItemFormContrato
        {
            GestaoCamposContrato gestaoCamposEmStatus { get; set; }
        }

        public class ItemFormContrato : ItemForm, IItemFormContrato
        {
            public GestaoCamposContrato gestaoCamposEmStatus { get; set; }
        }

        public class ItemFormObrigatorioContrato : ItemFormObrigatorio, IItemFormContrato
        {
            public GestaoCamposContrato gestaoCamposEmStatus { get; set; }
        }

        public class ComboFormContrato : ComboForm, IItemFormContrato
        {
            public GestaoCamposContrato gestaoCamposEmStatus { get; set; }
        }

        public class ComboFormObrigatorioContrato : ComboFormObrigatorio, IItemFormContrato
        {
            public GestaoCamposContrato gestaoCamposEmStatus { get; set; }
        }

        public class AbasContrato : TabsForm
        {
            public TabForm DefinicoesGerais = new TabForm()
            {
                ItemUID = abaGeralUID,
                PaneLevel = 1
            };
            public TabForm Itens = new TabForm()
            {
                ItemUID = abaItemUID,
                PaneLevel = 2
            };

            public TabForm Observacoes = new TabForm()
            {
                ItemUID = abaObsUID,
                PaneLevel = 4
            };
            public TabForm Anexos = new TabForm()
            {
                ItemUID = "AbaAnexos",
                PaneLevel = 5
            };
        }

        public class MatrizAnexos : MatrizChildForm
        {
            public ItemForm _arquivoAnexado = new ItemForm()
            {
                ItemUID = "U_Path",
                Datasource = "U_Path"
            };
            public ItemForm _data = new ItemForm()
            {
                ItemUID = "U_Date",
                Datasource = "U_Date"
            };

            public ButtonForm _adicionar = new ButtonForm()
            {
                ItemUID = "btAddAnexo"
            };
            public ButtonForm _remover = new ButtonForm()
            {
                ItemUID = "btRmvAnexo"
            };
        }
    }
}
