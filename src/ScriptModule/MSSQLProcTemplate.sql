/*<��д���ܵ�����>
���� : 

�޸�����		�޸���		����
--------------------------------------------------------------------------------------------------------------
20200502		ligy		create


set quoted_identifier off

*/
create proc sp_�������_����
( 
--...����
@UserCode varchar(20)
,@UserName nvarchar(50)
,@Success bit =0 output -- ���������,ָʾ������ִ�гɹ���
,@Msg nvarchar(2000) = '' output --���������,���ر�����ִ�н����Ϣ
,@NeedTrans bit =1 --�Ƿ��ڱ�sp�п�������,����ⲿ�Ѿ�����,�ڲ���������
,@RtnTblResult bit =1 --sp����󷵻�һ��fok,fmsgָʾ���̵ĳɹ�/ʧ��
,@Debug char(1) ='0'
)
as
begin

	set nocount on

	select @Success=0,@Msg=''

	declare @Trans1 int , @Trans2 int
	if @@TRANCOUNT>0 select @NeedTrans=0 -- ����Ѿ�������֮��,���ؿ�������

	if @NeedTrans = 1 --�����Ҫ��������
		begin tran

		select @Trans1 = @@TRANCOUNT

			begin try
/*****************************************�淶*****************************************************/
			/*A. ��������:  1.������ʱ�������������ĳ�ͻ,������Ҫ��������
						  2.���������Ƿ�������
						  3.�����������ֲ���
						  4.Union all ���ܳ���2���Ӳ�ѯ
						  5.����ʹ�ú���
						  6.����ʹ������ת��
						  7.������ֶβ�λnull,��Ҫʹ��isnull����
						  8.���÷��� group by

		      B.��������: 1.null������λ,��null���null֮������ʼ��Ϊnull
						2.�����ֶδ���
						3.����������ȫ���������ظ�/����
						4.���ݹ���
						5.��ѯδ�� with(nolocak)   */

						  

/***************************************ʵ��ҵ���߼�**************************************************/
					select @Msg='������ʾ' --ÿһ������ǰ,�ȼٶ�����
					--insert into ........


					goto Success
			end try

			begin catch
				select @Msg=@Msg+CHAR(13)+CHAR(10)
					+'sp'+Coalesce(Error_Procedure(),object_name(@@procid),'')+CHAR(13)+CHAR(10)
					+'line' +convert(varchar(10),error_line())+CHAR(13)+CHAR(10)
					+'msg'+ERROR_MESSAGE()
				goto Rtn --��׽���
			end catch

	Success:
	select @Success=1 ,@Msg='�ɹ���Ϣ'
	goto Rtn

	Rtn:
	Select @Trans2=@@TRANCOUNT

	if @NeedTrans=1
	begin
		if @Success=1 and @Trans1=@Trans2 and @Trans2>0 --�ɹ�,�ύ����
		begin
			commit tran
		end

		else if @Trans2>0
		begin
			rollback tran
		end
	end

	if @RtnTblResult=1
	begin
		select fok=@Success,fmsg=@Msg
	end

end;
	