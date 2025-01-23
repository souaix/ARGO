using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Core.Entities.LtCimLtEdc;
namespace Infrastructure.Data.DbContexts;

public partial class LtCimLtEdcProdDbContext : DbContext
{
	// 建議保留無參數建構子 (給某些工具或動態建置使用)
	public LtCimLtEdcProdDbContext()
	{
	}

	// EF Core 最佳實務：在外部 (Program/Startup) 用 AddDbContext 註冊，
	// 並注入已含有連線字串的 DbContextOptions
	public LtCimLtEdcProdDbContext(DbContextOptions<LtCimLtEdcProdDbContext> options)
		: base(options)
	{
	}

	public virtual DbSet<ArgoCimCimAlarmchecklog> ArgoCimCimAlarmchecklogs { get; set; }

	public virtual DbSet<ArgoCimCimAlarmdevicedatalog> ArgoCimCimAlarmdevicedatalogs { get; set; }

	public virtual DbSet<ArgoCimCimScadadevicebasis> ArgoCimCimScadadevicebases { get; set; }

	public virtual DbSet<ArgoCimCimScadadevicegroup> ArgoCimCimScadadevicegroups { get; set; }

	public virtual DbSet<ArgoCimCimScadadevicelimit> ArgoCimCimScadadevicelimits { get; set; }

	public virtual DbSet<ArgoCimCimSystemmenulist> ArgoCimCimSystemmenulists { get; set; }

	public virtual DbSet<ArgoCimCimUserbasis> ArgoCimCimUserbases { get; set; }

	public virtual DbSet<ArgoCimCimUserrole> ArgoCimCimUserroles { get; set; }

	public virtual DbSet<ArgoCimCimUserrolebasis> ArgoCimCimUserrolebases { get; set; }

	public virtual DbSet<ArgoCimCimUserroledetail> ArgoCimCimUserroledetails { get; set; }

	//public virtual DbSet<ArgoCimCimUsrUserbasis> ArgoCimCimUsrUserbases { get; set; }

	//public virtual DbSet<ArgoCimCimUsrUserrole> ArgoCimCimUsrUserroles { get; set; }

	public virtual DbSet<CuMapBu2019> CuMapBu2019s { get; set; }

	public virtual DbSet<CuMapBu2019Bin> CuMapBu2019Bins { get; set; }

	public virtual DbSet<CuMapBu2019Dt> CuMapBu2019Dts { get; set; }

	public virtual DbSet<EngBu2019InkBinmap> EngBu2019InkBinmaps { get; set; }

	public virtual DbSet<EngBu2019InkBinmapDt> EngBu2019InkBinmapDts { get; set; }

	public virtual DbSet<Tbleqpequipmentbasis> Tbleqpequipmentbases { get; set; }

	public virtual DbSet<Tblprdproductrecipeidbasis> Tblprdproductrecipeidbases { get; set; }

	//	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
	//		=> optionsBuilder.UseOracle("Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=10.21.151.37)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=ltedc)));User Id=ltcim;Password=cimlt2401;");

	// ---- 改寫 OnConfiguring，移除硬編碼 UseOracle() ----
	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		// 如果外部 (Program.cs) 已經透過 AddDbContext 進行配置，通常就會是 IsConfigured=true
		// 因此您可以選擇完全刪除此方法，或留個 fallback 機制 (不建議再硬編碼)
		if (!optionsBuilder.IsConfigured)
		{
			// 這裡可以做最後的 fallback (不建議再寫死連線字串)
			// 例如:
			// optionsBuilder.UseOracle("Data Source=...;User Id=...;Password=...");
		}
	}


	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder
			.HasDefaultSchema("LTCIM")
			.UseCollation("USING_NLS_COMP");

		modelBuilder.Entity<ArgoCimCimAlarmchecklog>(entity =>
		{
			entity.HasKey(e => new { e.Deviceno, e.Colname, e.Createdate }).HasName("ARGO_CIM_CIM_ALARMCHECKLOG_PK");

			entity.ToTable("ARGO_CIM_CIM_ALARMCHECKLOG");

			entity.Property(e => e.Deviceno)
				.HasMaxLength(20)
				.IsUnicode(false)
				.HasColumnName("DEVICENO");
			entity.Property(e => e.Colname)
				.HasMaxLength(10)
				.IsUnicode(false)
				.HasColumnName("COLNAME");
			entity.Property(e => e.Createdate)
				.HasColumnType("DATE")
				.HasColumnName("CREATEDATE");
			entity.Property(e => e.Checkcount)
				.HasColumnType("NUMBER(38)")
				.HasColumnName("CHECKCOUNT");
			entity.Property(e => e.Checkstatus)
				.HasColumnType("NUMBER(38)")
				.HasColumnName("CHECKSTATUS");
			entity.Property(e => e.Colcname)
				.HasMaxLength(100)
				.IsUnicode(false)
				.HasColumnName("COLCNAME");
			entity.Property(e => e.Importdate)
				.HasColumnType("DATE")
				.HasColumnName("IMPORTDATE");
			entity.Property(e => e.Value)
				.HasMaxLength(50)
				.IsUnicode(false)
				.HasColumnName("VALUE");
		});

		modelBuilder.Entity<ArgoCimCimAlarmdevicedatalog>(entity =>
		{
			entity.HasKey(e => new { e.Recorddate, e.Deviceno }).HasName("ARGO_CIM_CIM_ALARMDEVICEDATALOG_PK");

			entity.ToTable("ARGO_CIM_CIM_ALARMDEVICEDATALOG");

			entity.Property(e => e.Recorddate)
				.HasColumnType("DATE")
				.HasColumnName("RECORDDATE");
			entity.Property(e => e.Deviceno)
				.HasMaxLength(20)
				.IsUnicode(false)
				.HasColumnName("DEVICENO");
			entity.Property(e => e.Devicegroupno)
				.HasMaxLength(50)
				.IsUnicode(false)
				.HasColumnName("DEVICEGROUPNO");
			entity.Property(e => e.V001)
				.HasMaxLength(50)
				.IsUnicode(false);
			entity.Property(e => e.V002)
				.HasMaxLength(50)
				.IsUnicode(false);
			entity.Property(e => e.V003)
				.HasMaxLength(50)
				.IsUnicode(false);
			entity.Property(e => e.V004)
				.HasMaxLength(50)
				.IsUnicode(false);
			entity.Property(e => e.V005)
				.HasMaxLength(50)
				.IsUnicode(false);
			entity.Property(e => e.V006)
				.HasMaxLength(50)
				.IsUnicode(false);
			entity.Property(e => e.V007)
				.HasMaxLength(50)
				.IsUnicode(false);
			entity.Property(e => e.V008)
				.HasMaxLength(50)
				.IsUnicode(false);
			entity.Property(e => e.V009)
				.HasMaxLength(50)
				.IsUnicode(false);
		});

		modelBuilder.Entity<ArgoCimCimScadadevicebasis>(entity =>
		{
			entity.HasKey(e => e.Deviceno).HasName("ARGO_CIM_CIM_SCADADEVICEBASIS_PK");

			entity.ToTable("ARGO_CIM_CIM_SCADADEVICEBASIS");

			entity.Property(e => e.Deviceno)
				.HasMaxLength(20)
				.IsUnicode(false)
				.HasColumnName("DEVICENO");
			entity.Property(e => e.Alarm)
				.HasMaxLength(1)
				.IsUnicode(false)
				.HasColumnName("ALARM");
			entity.Property(e => e.Com)
				.HasMaxLength(4)
				.IsUnicode(false)
				.HasComment("COM port or RTU Address")
				.HasColumnName("COM");
			entity.Property(e => e.Createdate)
				.HasColumnType("DATE")
				.HasColumnName("CREATEDATE");
			entity.Property(e => e.Creator)
				.HasMaxLength(20)
				.IsUnicode(false)
				.HasColumnName("CREATOR");
			entity.Property(e => e.Devicegroupid)
				.HasMaxLength(10)
				.IsUnicode(false)
				.HasColumnName("DEVICEGROUPID");
			entity.Property(e => e.Devicename)
				.HasMaxLength(50)
				.IsUnicode(false)
				.HasColumnName("DEVICENAME");
			entity.Property(e => e.Devicetype)
				.HasMaxLength(20)
				.IsUnicode(false)
				.HasComment("MQTT,MODBUS,WEBSOCKET")
				.HasColumnName("DEVICETYPE");
			entity.Property(e => e.Enable)
				.HasComment("1:Enable")
				.HasColumnType("NUMBER(38)")
				.HasColumnName("ENABLE");
			entity.Property(e => e.Ip)
				.HasMaxLength(20)
				.IsUnicode(false)
				.HasColumnName("IP");
			entity.Property(e => e.Location)
				.HasMaxLength(20)
				.IsUnicode(false)
				.HasColumnName("LOCATION");
			entity.Property(e => e.Mac)
				.HasMaxLength(22)
				.IsUnicode(false)
				.HasColumnName("MAC");
			entity.Property(e => e.Offset)
				.HasColumnType("FLOAT")
				.HasColumnName("OFFSET");
			entity.Property(e => e.Remark)
				.HasMaxLength(150)
				.IsUnicode(false)
				.HasColumnName("REMARK");
			entity.Property(e => e.Serialno)
				.HasMaxLength(20)
				.IsUnicode(false)
				.HasColumnName("SERIALNO");
			entity.Property(e => e.Status)
				.HasComment("-1(OverSpec)、-2(Alarm)、0(Idle)、1(Run)、9(UserHold)")
				.HasColumnType("NUMBER(38)")
				.HasColumnName("STATUS");
			entity.Property(e => e.Updatedate)
				.HasColumnType("DATE")
				.HasColumnName("UPDATEDATE");
		});

		modelBuilder.Entity<ArgoCimCimScadadevicegroup>(entity =>
		{
			entity.HasKey(e => e.Devicegroupno).HasName("ARGO_CIM_CIM_SCADADEVICEGROUP_PK");

			entity.ToTable("ARGO_CIM_CIM_SCADADEVICEGROUP");

			entity.Property(e => e.Devicegroupno)
				.HasMaxLength(50)
				.IsUnicode(false)
				.HasColumnName("DEVICEGROUPNO");
			entity.Property(e => e.Createdate)
				.HasColumnType("DATE")
				.HasColumnName("CREATEDATE");
			entity.Property(e => e.Creator)
				.HasMaxLength(10)
				.IsUnicode(false)
				.HasColumnName("CREATOR");
			entity.Property(e => e.Devicegroupname)
				.HasMaxLength(100)
				.IsUnicode(false)
				.HasColumnName("DEVICEGROUPNAME");
			entity.Property(e => e.Devicegrouptype)
				.HasMaxLength(40)
				.IsUnicode(false)
				.HasColumnName("DEVICEGROUPTYPE");
			entity.Property(e => e.Updatedate)
				.HasColumnType("DATE")
				.HasColumnName("UPDATEDATE");
			entity.Property(e => e.Updater)
				.HasMaxLength(10)
				.IsUnicode(false)
				.HasColumnName("UPDATER");
		});

		modelBuilder.Entity<ArgoCimCimScadadevicelimit>(entity =>
		{
			entity.HasKey(e => e.Deviceno).HasName("ARGO_CIM_CIM_SCADADEVICELIMIT_PK");

			entity.ToTable("ARGO_CIM_CIM_SCADADEVICELIMIT");

			entity.Property(e => e.Deviceno)
				.HasMaxLength(20)
				.IsUnicode(false)
				.HasColumnName("DEVICENO");
			entity.Property(e => e.Createdate)
				.HasColumnType("DATE")
				.HasColumnName("CREATEDATE");
			entity.Property(e => e.Creator)
				.HasMaxLength(10)
				.IsUnicode(false)
				.HasColumnName("CREATOR");
			entity.Property(e => e.Dvlowercontrollimit)
				.HasColumnType("FLOAT")
				.HasColumnName("DVLOWERCONTROLLIMIT");
			entity.Property(e => e.Dvlowerspeclimit)
				.HasColumnType("FLOAT")
				.HasColumnName("DVLOWERSPECLIMIT");
			entity.Property(e => e.Dvparam)
				.HasMaxLength(50)
				.IsUnicode(false)
				.HasColumnName("DVPARAM");
			entity.Property(e => e.Dvparamname)
				.HasMaxLength(100)
				.IsUnicode(false)
				.HasColumnName("DVPARAMNAME");
			entity.Property(e => e.Dvuppercontrollimit)
				.HasColumnType("FLOAT")
				.HasColumnName("DVUPPERCONTROLLIMIT");
			entity.Property(e => e.Dvupperspeclimit)
				.HasColumnType("FLOAT")
				.HasColumnName("DVUPPERSPECLIMIT");
			entity.Property(e => e.Updatedate)
				.HasColumnType("DATE")
				.HasColumnName("UPDATEDATE");
		});


		modelBuilder.Entity<ArgoCimCimSystemmenulist>(entity =>
		{
			entity.HasKey(e => new { e.Id, e.Level03no }).HasName("ARGO_CIM_CIM_SYSTEMMENULIST_PK");

			entity.ToTable("ARGO_CIM_CIM_SYSTEMMENULIST");

			entity.Property(e => e.Id)
				.ValueGeneratedOnAdd()
				.HasColumnType("NUMBER(22,0)")
				.HasColumnName("ID");
			entity.Property(e => e.Level03no)
				.HasMaxLength(13)
				.IsUnicode(false)
				.HasComment("處級代碼(3碼)+模組代碼(3碼)+功能代碼(3碼)+流水號(4碼)")
				.HasColumnName("LEVEL03NO");
			entity.Property(e => e.Action)
				.HasMaxLength(100)
				.IsUnicode(false)
				.HasColumnName("ACTION");
			entity.Property(e => e.Belongbu)
				.HasMaxLength(10)
				.IsUnicode(false)
				.HasComment("BU0, BU1, BU2, BU3, BU4, BU5,CIM…")
				.HasColumnName("BELONGBU");
			entity.Property(e => e.Belongdept)
				.HasMaxLength(50)
				.IsUnicode(false)
				.HasColumnName("BELONGDEPT");
			entity.Property(e => e.Controller)
				.HasMaxLength(100)
				.IsUnicode(false)
				.HasColumnName("CONTROLLER");
			entity.Property(e => e.Createdate)
				.HasColumnType("DATE")
				.HasColumnName("CREATEDATE");
			entity.Property(e => e.Creator)
				.HasMaxLength(10)
				.IsUnicode(false)
				.HasColumnName("CREATOR");
			entity.Property(e => e.Enabled)
				.HasMaxLength(1)
				.IsUnicode(false)
				.HasComment("Y/N")
				.HasColumnName("ENABLED");
			entity.Property(e => e.Icon)
				.HasMaxLength(100)
				.IsUnicode(false)
				.HasColumnName("ICON");
			entity.Property(e => e.Imgname)
				.HasMaxLength(100)
				.IsUnicode(false)
				.HasColumnName("IMGNAME");
			entity.Property(e => e.Keyword)
				.HasMaxLength(100)
				.IsUnicode(false)
				.HasColumnName("KEYWORD");
			entity.Property(e => e.Level01)
				.HasMaxLength(100)
				.IsUnicode(false)
				.HasColumnName("LEVEL01");
			entity.Property(e => e.Level01no)
				.HasMaxLength(3)
				.IsUnicode(false)
				.HasColumnName("LEVEL01NO");
			entity.Property(e => e.Level02)
				.HasMaxLength(100)
				.IsUnicode(false)
				.HasColumnName("LEVEL02");
			entity.Property(e => e.Level02no)
				.HasMaxLength(3)
				.IsUnicode(false)
				.HasColumnName("LEVEL02NO");
			entity.Property(e => e.Level03)
				.HasMaxLength(100)
				.IsUnicode(false)
				.HasColumnName("LEVEL03");
			entity.Property(e => e.Remark)
				.HasMaxLength(100)
				.IsUnicode(false)
				.HasColumnName("REMARK");
			entity.Property(e => e.Sequence)
				.HasColumnType("NUMBER(22)")
				.HasColumnName("SEQUENCE");
			entity.Property(e => e.Updatedate)
				.HasColumnType("DATE")
				.HasColumnName("UPDATEDATE");
			entity.Property(e => e.Updater)
				.HasMaxLength(10)
				.IsUnicode(false)
				.HasColumnName("UPDATER");
		});

		modelBuilder.Entity<ArgoCimCimUserbasis>(entity =>
		{
			entity.HasKey(e => e.Userno).HasName("ARGO_CIM_CIM_USERBASIS_PK");

			entity.ToTable("ARGO_CIM_CIM_USERBASIS");

			entity.Property(e => e.Userno)
				.HasMaxLength(15)
				.IsUnicode(false)
				.HasColumnName("USERNO");
			entity.Property(e => e.Birthday)
				.HasMaxLength(100)
				.IsUnicode(false)
				.HasColumnName("BIRTHDAY");
			entity.Property(e => e.Comeday)
				.HasColumnType("DATE")
				.HasColumnName("COMEDAY");
			entity.Property(e => e.Dndesc)
				.HasMaxLength(50)
				.IsUnicode(false)
				.HasColumnName("DNDESC");
			entity.Property(e => e.Dnno)
				.HasMaxLength(20)
				.IsUnicode(false)
				.HasColumnName("DNNO");
			entity.Property(e => e.Email)
				.HasMaxLength(70)
				.IsUnicode(false)
				.HasColumnName("EMAIL");
			entity.Property(e => e.Emailoutside)
				.HasMaxLength(100)
				.IsUnicode(false)
				.HasColumnName("EMAILOUTSIDE");
			entity.Property(e => e.Hirestatus)
				.HasMaxLength(20)
				.IsUnicode(false)
				.HasComment("0-已離職,1-未啟用(留職停薪),2-退休人員,3-啟用中")
				.HasColumnName("HIRESTATUS");
			entity.Property(e => e.Leaveday)
				.HasColumnType("DATE")
				.HasColumnName("LEAVEDAY");
			entity.Property(e => e.Nowinjob)
				.HasMaxLength(50)
				.IsUnicode(false)
				.HasComment("Y/N")
				.HasColumnName("NOWINJOB");
			entity.Property(e => e.Username)
				.HasMaxLength(30)
				.IsUnicode(false)
				.HasColumnName("USERNAME");
		});

		modelBuilder.Entity<ArgoCimCimUserrole>(entity =>
		{
			entity.HasKey(e => e.Userno).HasName("ARGO_CIM_CIM_USERROLE_PK");

			entity.ToTable("ARGO_CIM_CIM_USERROLE");

			entity.Property(e => e.Userno)
				.HasMaxLength(10)
				.IsUnicode(false)
				.HasColumnName("USERNO");
			entity.Property(e => e.Createdate)
				.HasColumnType("DATE")
				.HasColumnName("CREATEDATE");
			entity.Property(e => e.Creator)
				.HasMaxLength(10)
				.IsUnicode(false)
				.HasColumnName("CREATOR");
			entity.Property(e => e.Updatedate)
				.HasColumnType("DATE")
				.HasColumnName("UPDATEDATE");
			entity.Property(e => e.Updater)
				.HasMaxLength(10)
				.IsUnicode(false)
				.HasColumnName("UPDATER");
			entity.Property(e => e.Userrole)
				.HasMaxLength(1000)
				.IsUnicode(false)
				.HasColumnName("USERROLE");
		});

		modelBuilder.Entity<ArgoCimCimUserrolebasis>(entity =>
		{
			entity.HasKey(e => e.Roleno).HasName("ARGO_CIM_CIM_USERROLEBASIS_PK");

			entity.ToTable("ARGO_CIM_CIM_USERROLEBASIS");

			entity.Property(e => e.Roleno)
				.HasMaxLength(13)
				.IsUnicode(false)
				.HasComment("處級代碼(3碼)+部門/模組代碼(3碼)+課級代碼(3碼)+流水號(4碼)")
				.HasColumnName("ROLENO");
			entity.Property(e => e.Createdate)
				.HasColumnType("DATE")
				.HasColumnName("CREATEDATE");
			entity.Property(e => e.Creator)
				.HasMaxLength(10)
				.IsUnicode(false)
				.HasColumnName("CREATOR");
			entity.Property(e => e.Rolename)
				.HasMaxLength(100)
				.IsUnicode(false)
				.HasColumnName("ROLENAME");
			entity.Property(e => e.Roletype)
				.HasComment("0-依部門預設,1-依模組,9-系統人員/主任以上")
				.HasColumnType("NUMBER(22)")
				.HasColumnName("ROLETYPE");
			entity.Property(e => e.Updatedate)
				.HasColumnType("DATE")
				.HasColumnName("UPDATEDATE");
			entity.Property(e => e.Updater)
				.HasMaxLength(10)
				.IsUnicode(false)
				.HasColumnName("UPDATER");
		});

		modelBuilder.Entity<ArgoCimCimUserroledetail>(entity =>
		{
			entity.HasKey(e => new { e.Roleno, e.Level03no }).HasName("ARGO_CIM_CIM_USERROLEDETAIL_PK");

			entity.ToTable("ARGO_CIM_CIM_USERROLEDETAIL");

			entity.Property(e => e.Roleno)
				.HasMaxLength(13)
				.IsUnicode(false)
				.HasColumnName("ROLENO");
			entity.Property(e => e.Level03no)
				.HasMaxLength(13)
				.IsUnicode(false)
				.HasColumnName("LEVEL03NO");
			entity.Property(e => e.Createdate)
				.HasColumnType("DATE")
				.HasColumnName("CREATEDATE");
			entity.Property(e => e.Creator)
				.HasMaxLength(10)
				.IsUnicode(false)
				.HasColumnName("CREATOR");
			entity.Property(e => e.Updatedate)
				.HasColumnType("DATE")
				.HasColumnName("UPDATEDATE");
			entity.Property(e => e.Updater)
				.HasMaxLength(10)
				.IsUnicode(false)
				.HasColumnName("UPDATER");
		});

		//modelBuilder.Entity<ArgoCimCimUsrUserbasis>(entity =>
		//{
		//	entity
		//		.HasNoKey()
		//		.ToTable("ARGO_CIM_CIM_USR_USERBASIS");

		//	entity.Property(e => e.Comeday)
		//		.HasColumnType("DATE")
		//		.HasColumnName("COMEDAY");
		//	entity.Property(e => e.Dndesc)
		//		.HasMaxLength(50)
		//		.IsUnicode(false)
		//		.HasColumnName("DNDESC");
		//	entity.Property(e => e.Dnno)
		//		.HasMaxLength(20)
		//		.IsUnicode(false)
		//		.HasColumnName("DNNO");
		//	entity.Property(e => e.Email)
		//		.HasMaxLength(70)
		//		.IsUnicode(false)
		//		.HasColumnName("EMAIL");
		//	entity.Property(e => e.Hirestatus)
		//		.HasMaxLength(20)
		//		.IsUnicode(false)
		//		.HasColumnName("HIRESTATUS");
		//	entity.Property(e => e.Leaveday)
		//		.HasColumnType("DATE")
		//		.HasColumnName("LEAVEDAY");
		//	entity.Property(e => e.Nowinjob)
		//		.HasMaxLength(50)
		//		.IsUnicode(false)
		//		.HasColumnName("NOWINJOB");
		//	entity.Property(e => e.Username)
		//		.HasMaxLength(30)
		//		.IsUnicode(false)
		//		.HasColumnName("USERNAME");
		//	entity.Property(e => e.Userno)
		//		.HasMaxLength(15)
		//		.IsUnicode(false)
		//		.HasColumnName("USERNO");
		//});

		//modelBuilder.Entity<ArgoCimCimUsrUserrole>(entity =>
		//{
		//	entity
		//		.HasNoKey()
		//		.ToTable("ARGO_CIM_CIM_USR_USERROLE");

		//	entity.Property(e => e.Role)
		//		.HasMaxLength(100)
		//		.IsUnicode(false)
		//		.HasColumnName("ROLE");
		//	entity.Property(e => e.Userno)
		//		.HasMaxLength(10)
		//		.IsUnicode(false)
		//		.HasColumnName("USERNO");
		//});

		modelBuilder.Entity<CuMapBu2019>(entity =>
		{
			entity.HasKey(e => new { e.CNo, e.MpNo }).HasName("CU_MAP_BU2019_PK");

			entity.ToTable("CU_MAP_BU2019", tb => tb.HasComment("客戶_MAP_BU2019_頭檔"));

			entity.Property(e => e.CNo)
				.HasMaxLength(10)
				.IsUnicode(false)
				.HasComment("公司別")
				.HasColumnName("C_NO");
			entity.Property(e => e.MpNo)
				.HasMaxLength(12)
				.IsUnicode(false)
				.HasComment("MAP編號")
				.HasColumnName("MP_NO");
			entity.Property(e => e.MpBmNo)
				.HasMaxLength(12)
				.IsUnicode(false)
				.HasComment("對照編號")
				.HasColumnName("MP_BM_NO");
			entity.Property(e => e.MpCtime)
				.HasComment("建立時間")
				.HasColumnType("DATE")
				.HasColumnName("MP_CTIME");
			entity.Property(e => e.MpCuser)
				.HasMaxLength(30)
				.IsUnicode(false)
				.HasComment("建立人員")
				.HasColumnName("MP_CUSER");
			entity.Property(e => e.MpDevice)
				.HasMaxLength(50)
				.IsUnicode(false)
				.HasComment("MAP DEVICE")
				.HasColumnName("MP_DEVICE");
			entity.Property(e => e.MpIfEnable)
				.HasMaxLength(1)
				.IsUnicode(false)
				.HasDefaultValueSql("'N' ")
				.HasComment("啟/停用")
				.HasColumnName("MP_IF_ENABLE");
			entity.Property(e => e.MpLotId)
				.HasMaxLength(50)
				.IsUnicode(false)
				.HasComment("LOT ID")
				.HasColumnName("MP_LOT_ID");
			entity.Property(e => e.MpMtime)
				.HasComment("異動時間")
				.HasColumnType("DATE")
				.HasColumnName("MP_MTIME");
			entity.Property(e => e.MpMuser)
				.HasMaxLength(30)
				.IsUnicode(false)
				.HasComment("異動人員")
				.HasColumnName("MP_MUSER");
			entity.Property(e => e.MpNotch)
				.HasMaxLength(20)
				.IsUnicode(false)
				.HasComment("NOTCH")
				.HasColumnName("MP_NOTCH");
			entity.Property(e => e.MpThSumFilename)
				.HasMaxLength(50)
				.IsUnicode(false)
				.HasComment("Th SUMMARY檔名")
				.HasColumnName("MP_TH_SUM_FILENAME");
			entity.Property(e => e.MpThSumZipFiledata)
				.HasComment("Th SUMMARY壓縮檔內容")
				.HasColumnType("BLOB")
				.HasColumnName("MP_TH_SUM_ZIP_FILEDATA");
			entity.Property(e => e.MpThSumZipFilename)
				.HasMaxLength(50)
				.IsUnicode(false)
				.HasComment("Th SUMMARY壓縮檔名")
				.HasColumnName("MP_TH_SUM_ZIP_FILENAME");
			entity.Property(e => e.MpTotBqty)
				.HasComment("總壞品數")
				.HasColumnType("NUMBER(22)")
				.HasColumnName("MP_TOT_BQTY");
			entity.Property(e => e.MpTotGqty)
				.HasComment("總好品數")
				.HasColumnType("NUMBER(22)")
				.HasColumnName("MP_TOT_GQTY");
			entity.Property(e => e.MpWfLotNo)
				.HasMaxLength(50)
				.IsUnicode(false)
				.HasComment("晶圓批號")
				.HasColumnName("MP_WF_LOT_NO");
			entity.Property(e => e.MpWfQty)
				.HasComment("片數")
				.HasColumnType("NUMBER(22)")
				.HasColumnName("MP_WF_QTY");
		});

		modelBuilder.Entity<CuMapBu2019Bin>(entity =>
		{
			entity.HasKey(e => new { e.CNo, e.MpNo, e.MpdItem, e.MpdBinNo }).HasName("CU_MAP_BU2019_BIN_PK");

			entity.ToTable("CU_MAP_BU2019_BIN", tb => tb.HasComment("客戶_MAP_BU2019_BIN檔"));

			entity.Property(e => e.CNo)
				.HasMaxLength(10)
				.IsUnicode(false)
				.HasComment("公司別")
				.HasColumnName("C_NO");
			entity.Property(e => e.MpNo)
				.HasMaxLength(12)
				.IsUnicode(false)
				.HasComment("MAP編號")
				.HasColumnName("MP_NO");
			entity.Property(e => e.MpdItem)
				.HasMaxLength(3)
				.IsUnicode(false)
				.HasComment("項次")
				.HasColumnName("MPD_ITEM");
			entity.Property(e => e.MpdBinNo)
				.HasMaxLength(8)
				.IsUnicode(false)
				.HasComment("BIN NO (同欣BIN)")
				.HasColumnName("MPD_BIN_NO");
			entity.Property(e => e.MpdBinQty)
				.HasComment("BIN數量")
				.HasColumnType("NUMBER(22)")
				.HasColumnName("MPD_BIN_QTY");
			entity.Property(e => e.MpdBinType)
				.HasMaxLength(20)
				.IsUnicode(false)
				.HasComment("BIN類別")
				.HasColumnName("MPD_BIN_TYPE");
		});

		modelBuilder.Entity<CuMapBu2019Dt>(entity =>
		{
			entity.HasKey(e => new { e.CNo, e.MpNo, e.MpdItem }).HasName("CU_MAP_BU2019_DT_PK");

			entity.ToTable("CU_MAP_BU2019_DT", tb => tb.HasComment("客戶_MAP_BU2019_尾檔"));

			entity.Property(e => e.CNo)
				.HasMaxLength(10)
				.IsUnicode(false)
				.HasComment("公司別")
				.HasColumnName("C_NO");
			entity.Property(e => e.MpNo)
				.HasMaxLength(12)
				.IsUnicode(false)
				.HasComment("MAP編號")
				.HasColumnName("MP_NO");
			entity.Property(e => e.MpdItem)
				.HasMaxLength(3)
				.IsUnicode(false)
				.HasComment("項次")
				.HasColumnName("MPD_ITEM");
			entity.Property(e => e.MpdFilename)
				.HasMaxLength(50)
				.IsUnicode(false)
				.HasComment("客戶MAP檔名")
				.HasColumnName("MPD_FILENAME");
			entity.Property(e => e.MpdTestQty)
				.HasComment("TEST QTY")
				.HasColumnType("NUMBER(22)")
				.HasColumnName("MPD_TEST_QTY");
			entity.Property(e => e.MpdThFilename)
				.HasMaxLength(50)
				.IsUnicode(false)
				.HasComment("Th MAP檔名")
				.HasColumnName("MPD_TH_FILENAME");
			entity.Property(e => e.MpdThZipFiledata)
				.HasComment("Th MAP壓縮檔內容")
				.HasColumnType("BLOB")
				.HasColumnName("MPD_TH_ZIP_FILEDATA");
			entity.Property(e => e.MpdThZipFilename)
				.HasMaxLength(50)
				.IsUnicode(false)
				.HasComment("Th MAP壓縮檔名")
				.HasColumnName("MPD_TH_ZIP_FILENAME");
			entity.Property(e => e.MpdVer)
				.HasMaxLength(10)
				.IsUnicode(false)
				.HasComment("MAP版本")
				.HasColumnName("MPD_VER");
			entity.Property(e => e.MpdWfBqty)
				.HasComment("FAIL QTY")
				.HasColumnType("NUMBER(22)")
				.HasColumnName("MPD_WF_BQTY");
			entity.Property(e => e.MpdWfGqty)
				.HasComment("PASS QTY")
				.HasColumnType("NUMBER(22)")
				.HasColumnName("MPD_WF_GQTY");
			entity.Property(e => e.MpdWfId)
				.HasMaxLength(50)
				.IsUnicode(false)
				.HasComment("WF ID")
				.HasColumnName("MPD_WF_ID");
			entity.Property(e => e.MpdWfNum)
				.HasMaxLength(10)
				.IsUnicode(false)
				.HasComment("WF NUMBER")
				.HasColumnName("MPD_WF_NUM");
			entity.Property(e => e.MpdYield)
				.HasComment("Yield")
				.HasColumnType("NUMBER(15,5)")
				.HasColumnName("MPD_YIELD");
			entity.Property(e => e.MpdZipFiledata)
				.HasComment("客戶MAP壓縮檔內容")
				.HasColumnType("BLOB")
				.HasColumnName("MPD_ZIP_FILEDATA");
			entity.Property(e => e.MpdZipFilename)
				.HasMaxLength(50)
				.IsUnicode(false)
				.HasComment("客戶MAP壓縮檔名")
				.HasColumnName("MPD_ZIP_FILENAME");
		});

		modelBuilder.Entity<EngBu2019InkBinmap>(entity =>
		{
			entity.HasKey(e => new { e.CNo, e.BmNo }).HasName("ENG_BU2019_INK_BINMAP_PK");

			entity.ToTable("ENG_BU2019_INK_BINMAP", tb => tb.HasComment("工程_BU2019_INKLESS_BIN對照_頭檔"));

			entity.Property(e => e.CNo)
				.HasMaxLength(10)
				.IsUnicode(false)
				.HasComment("公司別")
				.HasColumnName("C_NO");
			entity.Property(e => e.BmNo)
				.HasMaxLength(12)
				.IsUnicode(false)
				.HasComment("對照編號")
				.HasColumnName("BM_NO");
			entity.Property(e => e.BmCtime)
				.HasComment("建立時間")
				.HasColumnType("DATE")
				.HasColumnName("BM_CTIME");
			entity.Property(e => e.BmCuser)
				.HasMaxLength(30)
				.IsUnicode(false)
				.HasComment("建立人員")
				.HasColumnName("BM_CUSER");
			entity.Property(e => e.BmDesc)
				.HasMaxLength(50)
				.IsUnicode(false)
				.HasComment("對照說明")
				.HasColumnName("BM_DESC");
			entity.Property(e => e.BmIfEnable)
				.HasMaxLength(1)
				.IsUnicode(false)
				.HasComment("啟用")
				.HasColumnName("BM_IF_ENABLE");
			entity.Property(e => e.BmMtime)
				.HasComment("異動時間")
				.HasColumnType("DATE")
				.HasColumnName("BM_MTIME");
			entity.Property(e => e.BmMuser)
				.HasMaxLength(30)
				.IsUnicode(false)
				.HasComment("異動人員")
				.HasColumnName("BM_MUSER");
			entity.Property(e => e.BmRemark)
				.HasMaxLength(1024)
				.IsUnicode(false)
				.HasComment("備註")
				.HasColumnName("BM_REMARK");
		});

		modelBuilder.Entity<EngBu2019InkBinmapDt>(entity =>
		{
			entity.HasKey(e => new { e.CNo, e.BmNo, e.CuBinCode }).HasName("ENG_BU2019_INK_BINMAP_DT_PK");

			entity.ToTable("ENG_BU2019_INK_BINMAP_DT", tb => tb.HasComment("工程_BU2019_INKLESS_BIN對照_尾檔"));

			entity.Property(e => e.CNo)
				.HasMaxLength(10)
				.IsUnicode(false)
				.HasComment("公司別")
				.HasColumnName("C_NO");
			entity.Property(e => e.BmNo)
				.HasMaxLength(12)
				.IsUnicode(false)
				.HasComment("對照編號")
				.HasColumnName("BM_NO");
			entity.Property(e => e.CuBinCode)
				.HasMaxLength(10)
				.IsUnicode(false)
				.HasComment("CU BIN NO")
				.HasColumnName("CU_BIN_CODE");
			entity.Property(e => e.BinType)
				.HasMaxLength(20)
				.IsUnicode(false)
				.HasComment("BIN類別")
				.HasColumnName("BIN_TYPE");
			entity.Property(e => e.ThCode)
				.HasMaxLength(10)
				.IsUnicode(false)
				.HasComment("TH BIN NO")
				.HasColumnName("TH_CODE");
		});

		modelBuilder.Entity<Tbleqpequipmentbasis>(entity =>
		{
			entity.HasKey(e => e.Equipmentno).HasName("TBLEQPEQUIPMENTBASIS_PK");

			entity.ToTable("TBLEQPEQUIPMENTBASIS");

			entity.Property(e => e.Equipmentno)
				.HasMaxLength(50)
				.HasColumnName("EQUIPMENTNO");
			entity.Property(e => e.Assetno)
				.HasMaxLength(50)
				.HasColumnName("ASSETNO");
			entity.Property(e => e.Autoflag)
				.HasColumnType("NUMBER(1)")
				.HasColumnName("AUTOFLAG");
			entity.Property(e => e.Belongequipmentno)
				.HasMaxLength(50)
				.HasColumnName("BELONGEQUIPMENTNO");
			entity.Property(e => e.Capacity)
				.HasPrecision(8)
				.HasColumnName("CAPACITY");
			entity.Property(e => e.Chamberflag)
				.HasColumnType("NUMBER(1)")
				.HasColumnName("CHAMBERFLAG");
			entity.Property(e => e.Counteqpunitqty)
				.HasPrecision(6)
				.HasColumnName("COUNTEQPUNITQTY");
			entity.Property(e => e.Counter)
				.HasPrecision(5)
				.HasColumnName("COUNTER");
			entity.Property(e => e.Countopunitqty)
				.HasPrecision(6)
				.HasColumnName("COUNTOPUNITQTY");
			entity.Property(e => e.Createdate)
				.HasColumnType("DATE")
				.HasColumnName("CREATEDATE");
			entity.Property(e => e.Creator)
				.HasMaxLength(10)
				.HasColumnName("CREATOR");
			entity.Property(e => e.Description)
				.HasMaxLength(255)
				.HasColumnName("DESCRIPTION");
			entity.Property(e => e.Eacontroller)
				.HasMaxLength(200)
				.HasColumnName("EACONTROLLER");
			entity.Property(e => e.Engineergroupno)
				.HasMaxLength(20)
				.HasColumnName("ENGINEERGROUPNO");
			entity.Property(e => e.Eqprecipe)
				.HasColumnType("NUMBER(1)")
				.HasColumnName("EQPRECIPE");
			entity.Property(e => e.Equipmentclass)
				.HasMaxLength(50)
				.HasColumnName("EQUIPMENTCLASS");
			entity.Property(e => e.Equipmentclassno)
				.HasPrecision(3)
				.HasColumnName("EQUIPMENTCLASSNO");
			entity.Property(e => e.Equipmentfn)
				.HasMaxLength(50)
				.HasColumnName("EQUIPMENTFN");
			entity.Property(e => e.Equipmentip)
				.HasMaxLength(50)
				.HasColumnName("EQUIPMENTIP");
			entity.Property(e => e.Equipmentname)
				.HasMaxLength(255)
				.HasColumnName("EQUIPMENTNAME");
			entity.Property(e => e.Equipmenttype)
				.HasMaxLength(50)
				.HasColumnName("EQUIPMENTTYPE");
			entity.Property(e => e.Erpno)
				.HasMaxLength(50)
				.HasColumnName("ERPNO");
			entity.Property(e => e.Fixeqptime)
				.HasColumnType("NUMBER(6,2)")
				.HasColumnName("FIXEQPTIME");
			entity.Property(e => e.Issuestate)
				.HasColumnType("NUMBER(1)")
				.HasColumnName("ISSUESTATE");
			entity.Property(e => e.Loadport)
				.HasPrecision(2)
				.HasColumnName("LOADPORT");
			entity.Property(e => e.Maxtime)
				.HasColumnType("NUMBER(6,2)")
				.HasColumnName("MAXTIME");
			entity.Property(e => e.Modelno)
				.HasMaxLength(50)
				.HasColumnName("MODELNO");
			entity.Property(e => e.ProberTester)
				.HasMaxLength(50)
				.HasColumnName("PROBER_TESTER");
			entity.Property(e => e.Qclistno)
				.HasMaxLength(50)
				.HasColumnName("QCLISTNO");
			entity.Property(e => e.Queueserver)
				.HasMaxLength(50)
				.HasColumnName("QUEUESERVER");
			entity.Property(e => e.Requestmsgqueue)
				.HasMaxLength(100)
				.HasColumnName("REQUESTMSGQUEUE");
			entity.Property(e => e.Responsemsgqueue)
				.HasMaxLength(100)
				.HasColumnName("RESPONSEMSGQUEUE");
			entity.Property(e => e.Vareqptime)
				.HasColumnType("NUMBER(6,2)")
				.HasColumnName("VAREQPTIME");
			entity.Property(e => e.Vendorno)
				.HasMaxLength(50)
				.HasColumnName("VENDORNO");
		});

		modelBuilder.Entity<Tblprdproductrecipeidbasis>(entity =>
		{
			entity.HasKey(e => new { e.Id, e.Productno, e.Customerpartver, e.Opno, e.Modelno }).HasName("TBLPRDPRODUCTRECIPEIDBASIS_PK");

			entity.ToTable("TBLPRDPRODUCTRECIPEIDBASIS");

			entity.Property(e => e.Id)
				.HasMaxLength(50)
				.IsUnicode(false)
				.HasColumnName("ID");
			entity.Property(e => e.Productno)
				.HasMaxLength(30)
				.IsUnicode(false)
				.HasColumnName("PRODUCTNO");
			entity.Property(e => e.Customerpartver)
				.HasMaxLength(10)
				.IsUnicode(false)
				.HasColumnName("CUSTOMERPARTVER");
			entity.Property(e => e.Opno)
				.HasMaxLength(10)
				.IsUnicode(false)
				.HasColumnName("OPNO");
			entity.Property(e => e.Modelno)
				.HasMaxLength(20)
				.IsUnicode(false)
				.HasColumnName("MODELNO");
			entity.Property(e => e.Createdate)
				.HasColumnType("DATE")
				.HasColumnName("CREATEDATE");
			entity.Property(e => e.Creator)
				.HasMaxLength(20)
				.IsUnicode(false)
				.HasColumnName("CREATOR");
			entity.Property(e => e.Editdate)
				.HasColumnType("DATE")
				.HasColumnName("EDITDATE");
			entity.Property(e => e.Editor)
				.HasMaxLength(20)
				.IsUnicode(false)
				.HasColumnName("EDITOR");
			entity.Property(e => e.Inuse)
				.HasMaxLength(2)
				.IsUnicode(false)
				.HasColumnName("INUSE");
			entity.Property(e => e.IpcRecipe)
				.HasMaxLength(30)
				.IsUnicode(false)
				.HasColumnName("IPC_RECIPE");
			entity.Property(e => e.Motypeno)
				.HasColumnType("NUMBER(38)")
				.HasColumnName("MOTYPENO");
			entity.Property(e => e.Recipeid)
				.HasMaxLength(50)
				.IsUnicode(false)
				.HasColumnName("RECIPEID");
		});

		OnModelCreatingPartial(modelBuilder);
	}

	partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
