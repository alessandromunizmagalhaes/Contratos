using SAPHelper;

namespace CafebrasContratos
{
    class TransactionNotification : IStoredProcedureImplementation
    {
        public void CriarFuncoes()
        {
            CriarFuncao_NomeTabela();
            CriarFuncao_CamposOrigemParaUpdate();
            CriarProcedure_AtualizaCampoFalandoQueVeioDoContrato();
            CriarProcedure_AtualizaCodigoContrato();
            CriarProcedure_AtualizaSaldoContratoFilhoConformeDocMKT();
        }

        private void CriarFuncao_NomeTabela()
        {
            using (var rsCOM = new RecordSet())
            {
                rsCOM.DoQuery(
                    $@"IF OBJECT_ID('dbo.NOMETABELA') IS NOT NULL
                        DROP FUNCTION NOMETABELA;"
                    );

                rsCOM.DoQuery(
                    $@"CREATE FUNCTION NOMETABELA(@ObjectType VARCHAR(255)) 
	                        RETURNS VARCHAR(50)
	                        BEGIN
		                        DECLARE @Res VARCHAR(50);
		                        SELECT @Res = U_Tabela FROM [@UPD_OBJ_TYPES] WHERE U_ObjType = @ObjectType
	                        RETURN @Res;
                        END"
                        );
            }
        }

        private void CriarFuncao_CamposOrigemParaUpdate()
        {
            using (var rsCOM = new RecordSet())
            {
                rsCOM.DoQuery(
                    @"IF OBJECT_ID('dbo.CAMPOSORIGEM') IS NOT NULL
                        DROP FUNCTION CAMPOSORIGEM;"
                    );

                rsCOM.DoQuery(
                    $@"CREATE FUNCTION CAMPOSORIGEM(@ObjectType VARCHAR(255)) 
	                    RETURNS VARCHAR(200)
	                    BEGIN
		                    DECLARE @ORIGEM INT;
		                    DECLARE @Res VARCHAR(100);
		                    DECLARE @OPERACAO VARCHAR(3);
		                    DECLARE @CAMPOS VARCHAR(200);
		                    SELECT @ORIGEM = U_Origem FROM [@UPD_OBJ_TYPES] WHERE U_ObjType = @ObjectType
		
		                    -- Origem debito
		                    IF @ORIGEM > 0
		                    BEGIN
			                    SET @OPERACAO = '+';
		                    END
		                    -- Origem credito
		                    ELSE IF @ORIGEM < 0
		                    BEGIN
			                    SET @OPERACAO = '-';
		                    END

		                    -- Origem Estoque
		                    IF ABS(@ORIGEM) = 1
		                    BEGIN
			                    SET @CAMPOS = 'tb2.U_SPesoRec = tb2.U_SPesoRec __OPERACAO__ tb1.Quantity, tb2.U_SPesoNCT = tb2.U_SPesoNCT __OPERACAO__ tb1.U_QtdSaca';
		                    END
		                    -- Origem Fiscal
		                    ELSE IF ABS(@ORIGEM) = 2
		                    BEGIN
			                    SET @CAMPOS = 'tb2.U_SFin = tb2.U_SFin __OPERACAO__ tb1.LineTotal';
		                    END

		                    SET @Res = REPLACE(@CAMPOS, '__OPERACAO__',@OPERACAO);
	                    RETURN @Res;
                    END"
                        );
            }
        }

        private void CriarProcedure_AtualizaCodigoContrato()
        {
            using (var rsCOM = new RecordSet())
            {
                rsCOM.DoQuery(
                    $@"IF OBJECT_ID('dbo.AtualizaCodigoContrato') IS NOT NULL
                        DROP PROCEDURE AtualizaCodigoContrato;"
                    );

                rsCOM.DoQuery(
                    $@"CREATE PROCEDURE dbo.AtualizaCodigoContrato @DocEntry NVARCHAR(30), @Tabela NVARCHAR(30)
                        AS
                        BEGIN
	                        DECLARE @UPDATE NVARCHAR(100)
	                        DECLARE @ParmDefinition nvarchar(100);
	                        SET @ParmDefinition = N'@DocEntry NVARCHAR(30)';  
	                        SET @UPDATE = 'UPDATE ' + @Tabela + ' SET U_DocNumCF = NULL WHERE DocEntry = @DocEntry';
	                        EXECUTE sp_executesql @UPDATE, @ParmDefinition, @DocEntry = @DocEntry
                        END"
                        );
            }
        }

        private void CriarProcedure_AtualizaSaldoContratoFilhoConformeDocMKT()
        {
            using (var rsCOM = new RecordSet())
            {
                rsCOM.DoQuery(
                    $@"IF OBJECT_ID('dbo.AtualizaSaldoContratoConformeDocMkt') IS NOT NULL
                        DROP PROCEDURE AtualizaSaldoContratoConformeDocMkt;"
                    );

                rsCOM.DoQuery(
                    $@"CREATE PROCEDURE dbo.AtualizaSaldoContratoConformeDocMkt @DocEntry NVARCHAR(30), @ObjectType VARCHAR(255)
                        AS
                        BEGIN
	                        DECLARE @UPDATE NVARCHAR(500)
	                        DECLARE @ParmDefinition nvarchar(100);
	                        DECLARE @CAMPOS VARCHAR(200) = (SELECT DBO.CAMPOSORIGEM(@ObjectType))
	                        IF @CAMPOS IS NOT NULL
	                        BEGIN
		                        DECLARE @Tabela VARCHAR(50) = (SELECT DBO.NOMETABELA(@ObjectType))
		                        SET @ParmDefinition = N'@DocEntry NVARCHAR(30)';  
		                        SET @UPDATE = 'UPDATE tb2 SET ' + @CAMPOS + '
						                        FROM ' + @Tabela + ' tb0
						                        INNER JOIN ' + RIGHT(@Tabela,3) + '1' + ' tb1 ON ( tb1.DocEntry = tb0.DocEntry )
						                        INNER JOIN [@UPD_OCFC] tb2 ON (tb2.U_DocNumCF = tb0.U_DocNumCF)
						                        WHERE tb0.DocEntry = @DocEntry';
		                        --SET @UPDATE = 'UPDATE ' + @Tabela + ' SET U_SonOfContract = ''N'' WHERE DocEntry = @DocEntry';
		                        EXECUTE sp_executesql @UPDATE, @ParmDefinition, @DocEntry = @DocEntry
	                        END
                        END"
                        );
            }
        }

        private void CriarProcedure_AtualizaCampoFalandoQueVeioDoContrato()
        {
            using (var rsCOM = new RecordSet())
            {
                rsCOM.DoQuery(
                    $@"IF OBJECT_ID('dbo.AtualizaCampoFalandoQueVeioDoContrato') IS NOT NULL
                        DROP PROCEDURE AtualizaCampoFalandoQueVeioDoContrato;"
                    );

                rsCOM.DoQuery(
                    $@"CREATE PROCEDURE dbo.AtualizaCampoFalandoQueVeioDoContrato @DocEntry NVARCHAR(30), @Tabela NVARCHAR(30)
                        AS
                        BEGIN
	                        DECLARE @UPDATE NVARCHAR(100)
	                        DECLARE @ParmDefinition nvarchar(100);
	                        SET @ParmDefinition = N'@DocEntry NVARCHAR(30)';  
	                        SET @UPDATE = 'UPDATE ' + @Tabela + ' SET U_SonOfContract = ''N'' WHERE DocEntry = @DocEntry';
	                        EXECUTE sp_executesql @UPDATE, @ParmDefinition, @DocEntry = @DocEntry
                        END"
                );
            }
        }

        public string Codigo()
        {
            return
                $@"IF (SELECT count(*) FROM [@UPD_OBJ_TYPES] WHERE U_objtype = @object_type) > 0 AND @transaction_type = 'A'
                    BEGIN
		                DECLARE @VeioDeContrato VARCHAR(3);
		                DECLARE @Tabela VARCHAR(50) = (SELECT DBO.NOMETABELA(@object_type))
		                DECLARE @SELECT NVARCHAR(600)
		                DECLARE @ParmDefinition nvarchar(100);

		                SET @ParmDefinition = N'@list_of_cols_val_tab_del NVARCHAR(255), @VeioDeContratoOUT NVARCHAR(3) OUTPUT';  
		                SET @SELECT = 'SELECT @VeioDeContratoOUT = U_SonOfContract FROM ' + @Tabela + ' WHERE DocEntry = @list_of_cols_val_tab_del';
		                EXECUTE sp_executesql @SELECT, @ParmDefinition, @list_of_cols_val_tab_del = @list_of_cols_val_tab_del, @VeioDeContratoOUT = @VeioDeContrato OUTPUT;

		                IF @VeioDeContrato = 'N'
		                BEGIN
			                EXEC AtualizaCodigoContrato @DocEntry = @list_of_cols_val_tab_del, @Tabela = @Tabela
		                END

		                DECLARE @TemItemDeCafe INT
		                SET @ParmDefinition = N'@DocEntry NVARCHAR(255), @ResOUT BIT OUTPUT';  
		                SET @SELECT = 'SELECT @ResOUT = COUNT(*) 
		                FROM ' + RIGHT(@Tabela,3) + '1' + ' tb0 
		                INNER JOIN [@UPD_DENIED_USAGE] tb2 ON (tb2.U_Usage = tb0.Usage)
		                WHERE tb0.DocEntry = @DocEntry';
		                EXECUTE sp_executesql @SELECT, @ParmDefinition, @DocEntry = @list_of_cols_val_tab_del, @ResOUT = @TemItemDeCafe OUTPUT;
	                    IF @TemItemDeCafe > 0 AND (@VeioDeContrato = 'N' OR @VeioDeContrato IS NULL)
			                BEGIN
				                select @error = '1407'
				                select @error_message = 'O Item escolhido só pode ser gerado apartir de um Contrato de Compra de Café.'
			                END
                    END";
        }
    }
}
