/*<填写功能的名称>
作者 : 

修改日期		修改人		描述
--------------------------------------------------------------------------------------------------------------
20200502		ligy		create


set quoted_identifier off

*/
create proc sp_程序代号_动作
( 
--...参数
@UserCode varchar(20)
,@UserName nvarchar(50)
,@Success bit =0 output -- 可输出参数,指示本过程执行成功否
,@Msg nvarchar(2000) = '' output --可输出参数,返回本过程执行结果消息
,@NeedTrans bit =1 --是否在本sp中开启事务,如果外部已经开启,内部不必启动
,@RtnTblResult bit =1 --sp结果后返回一个fok,fmsg指示过程的成功/失败
,@Debug char(1) ='0'
)
as
begin

	set nocount on

	select @Success=0,@Msg=''

	declare @Trans1 int , @Trans2 int
	if @@TRANCOUNT>0 select @NeedTrans=0 -- 如果已经在事务之中,不必开启事务

	if @NeedTrans = 1 --如果需要开启事务
		begin tran

		select @Trans1 = @@TRANCOUNT

			begin try
/*****************************************规范*****************************************************/
			/*A. 性能问题:  1.建立临时表名避免上下文冲突,并且需要建立索引
						  2.检查物理表是否建立索引
						  3.复杂数据区分步骤
						  4.Union all 不能超过2个子查询
						  5.避免使用函数
						  6.避免使用类型转换
						  7.如果表字段部位null,不要使用isnull函数
						  8.少用分组 group by

		      B.常见问题: 1.null处理不到位,如null与非null之运算结果始终为null
						2.日期字段处理
						3.关联条件不全导致数据重复/减少
						4.内容过长
						5.查询未加 with(nolocak)   */

						  

/***************************************实际业务逻辑**************************************************/
					select @Msg='错误提示' --每一步处理前,先假定出错
					--insert into ........


					goto Success
			end try

			begin catch
				select @Msg=@Msg+CHAR(13)+CHAR(10)
					+'sp'+Coalesce(Error_Procedure(),object_name(@@procid),'')+CHAR(13)+CHAR(10)
					+'line' +convert(varchar(10),error_line())+CHAR(13)+CHAR(10)
					+'msg'+ERROR_MESSAGE()
				goto Rtn --捕捉完成
			end catch

	Success:
	select @Success=1 ,@Msg='成功信息'
	goto Rtn

	Rtn:
	Select @Trans2=@@TRANCOUNT

	if @NeedTrans=1
	begin
		if @Success=1 and @Trans1=@Trans2 and @Trans2>0 --成功,提交事务
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
	