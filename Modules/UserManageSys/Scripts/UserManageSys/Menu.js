var vm = new Vue({

	el: "#app",
	data() {
		return {
			Loading: true, //axios 資料回傳較 mounted 慢, 設定此參數避免看到不完整畫面
			//目前網址
			NowUrl: "Menu",
			AddBox: true,
			AddBoxWidth: 75,
			AddBoxIcon: "el-icon-arrow-left",
			AddButton: false,
			//檢索條件
			Search: "",
			SearchDialog: "",
			//分頁設定
			CurrentPage: 1,
			PageSize: 8,
			DialogCurrentPage: 1,
			DialogPageSize: 8,
			HideSinglePage: true,
			//懸浮視窗
			DialogTableVisible: false,
			EditName: "", //編輯中的項目
			DialogTitle: "",
			ChildCheckboxArr: [],
			//角色基本資訊
			ChildData: [],
			ChildTblWidth: [160, 250, 250, 150, 100, 150, 100],
			ChildTblProperty: [],
			ChildTblLabel: [],
			//編輯選單
			EditStatus: false,  //true: 編輯中, 將選中資料帶到左表以執行更新
			//選單基本資訊
			ParentData: [],
			ParentTblWidth: [150, 250, 100, 150, 120, 200, 120, 250, 300, 200, 200, 100, 200, 300, 200, 150, 100, 150, 300],
			ParentTblProperty: [],
			ParentTblLabel: [],
			//新增選單
			InputOptionsDefault: [],
			Input1Options: [],
			Input2Options: [],
			Input3Options: [],
			Input5Options: [],
			FormAdd: {
				Input1: "",  //歸屬BU
				Input2: "",  //歸屬部門
				Input3: "",  //系統代碼
				Input4: "",  //系統名稱
				Input5: "",  //模組代碼
				Input6: "",  //模組名稱
				Input7: "",  //功能代碼
				Input8: "",  //功能名稱
				Input9: "",  //ICON
				Input10: "",  //控制器
				Input11: "",  //ACTION
				Input12: 1,  //排序
				Input13: "",  //關鍵字
				Input14: "",  //是否啟用
				Input15: "",  //備註
				ImageUrl: "",  //圖片上傳
				ImageList: [],  //圖片上傳
			},
			FormRules: { //表單驗證規則,需與v-model綁定名稱相同
				Input1: [{ required: true, message: "請選擇歸屬BU", trigger: "change" },
				{ min: 3, max: 3, message: '限定長度=3', trigger: ['blur', 'change'] }],
				Input2: [{ required: true, message: "請選擇歸屬部門", trigger: "change" }],
				Input3: [{ required: true, message: "請選擇系統代碼", trigger: "change" },
				{ min: 3, max: 3, message: '限定長度=3', trigger: ['blur', 'change'] }],
				Input4: [{ required: true, message: "請輸入系統名稱", trigger: ['blur', 'change'] }],
				Input5: [{ required: true, message: "請選擇模組代碼", trigger: "change" },
				{ min: 3, max: 3, message: '限定長度=3', trigger: ['blur', 'change'] }],
				Input6: [{ required: true, message: "請輸入模組名稱", trigger: ['blur', 'change'] }],
				Input8: [{ required: true, message: "請輸入功能名稱", trigger: "blur" }],
				Input9: [{ required: true, message: "請輸入 ICON", trigger: "blur" }],
				Input10: [{ required: true, message: "請輸入 Controller", trigger: "blur" }],
				Input11: [{ required: true, message: "請輸入 Action", trigger: "blur" }],
				Input12: [{ required: true, message: "請輸入排序", trigger: "blur" },
				{ type: "number", min: 1, message: '排序需大於 1', trigger: ['blur', 'change'] }],
				Input14: [{ required: true, message: "請選擇是否啟用", trigger: "change" }],
			},
			//檔案上傳
			HideUploadBtn: false, //控制上傳組件是否隱藏
			FileList: [], //取得的檔案清單
			SelectedFile: null, //傳給formData物件
			ImgDialogUrl: '', //暫存的檔案路徑
			ImgDialogVisible: false, //圖片點開效果
		}
	},

	beforeMount: function () {
		this.DataLoad();
	},

	mounted() {
		setTimeout(() => {
			this.Loading = false;
		}, 100);
	},

	computed: {
		ParentFilter: function () {
			return this.ParentData.filter(item => {
				return item.Level03.toUpperCase().match(this.Search.toUpperCase()) || item.Level03no.toUpperCase().match(this.Search.toUpperCase());  //全部轉成大寫才比對
			})
		},

		DialogFilter: function () {
			//2025.1.15 新增: 依照checked項目排序
			var ChildDataCopy = this.ChildData;
			ChildDataCopy.forEach(role => {
				role.checked = this.ChildCheckboxArr.includes(role.Roleno) ? 1 : 0;
			});
			ChildDataCopy.sort((a, b) => b.checked - a.checked);
			return ChildDataCopy.filter(item => {
				return item.Roleno.toUpperCase().match(this.SearchDialog.toUpperCase()) || item.Rolename.toUpperCase().match(this.SearchDialog.toUpperCase());  //全部轉成大寫才比對
			})
		},
	},

	methods: {
		//取得所有資料
		DataLoad() {
			var VmNew = this;  //定義外部變量,將this的值賦予它

			//取得 ARGO_CIM_CIM_USERBASIS(使用者資訊)
			axios.post('/api/UserManageSys_API/UserData', ''
			).then(function (res) {
				VmNew.TempData = res.data;  //調用該變量
				var Input2OptionsArr = VmNew.TempData.map(item => Object.values(item)[2]); // 陣列中有物件=>取得特性屬性值的陣列: 2 表示第2個屬性值[DNDESC部門]
				Input2OptionsArr.push(''); //增加空白元素代表[ALL]選項
				VmNew.Input2Options = Input2OptionsArr.filter((item, index) => Input2OptionsArr.indexOf(item) === index).sort(); //去重複
			});

			//取得 ARGO_CIM_CIM_USERROLEBASIS(角色基本資訊)
			axios.post('/api/UserManageSys_API/UserRoleBasisData', ''
			).then(function (res) {
				VmNew.ChildData = res.data;  //調用該變量
				//取得table 所有columns (key)
				var TblKey = [];
				for (key in VmNew.ChildData[0]) {
					TblKey.push(key);
				}
				//移除不需要的欄位
				var index = TblKey.findIndex(item => item === "Roletype"); //先找指定元素在原陣列index
				//將指定index的元素移出陣列
				TblKey.splice(index, 1);
				//寫入ChildTblProperty
				VmNew.ChildTblProperty = TblKey

				//取得columns(key) 對應的 DisplayName
				//先擴展ChildTblLabel陣列長度, 否則無法將 columns 覆寫為 DisplayName
				for (var i = 0; i < TblKey.length; i++) {
					VmNew.ChildTblLabel.push("");
				}
				var dataJson = {};
				dataJson['Columnname'] = VmNew.ChildTblProperty;
				dataJson['Typename'] = "ArgoCimCimUserrolebasis";
				axios.post('/api/Shared_API/GetDisplayName', dataJson
				).then(function (res) {
					VmNew.ChildTblLabel = res.data;  //調用該變量
				});

			});

			//取得 ARGO_CIM_CIM_SYSTEMMENULIST(選單基本資訊)
			axios.post('/api/UserManageSys_API/SysMenuData', ''
			).then(function (res) {
				VmNew.ParentData = res.data;  //調用該變量
				//取得table 所有columns (key)
				var TblKey = [];
				for (key in VmNew.ParentData[0]) {
					TblKey.push(key);
				}
				//移除不需要的欄位
				var index = TblKey.findIndex(item => item === "Id"); //先找指定元素在原陣列index
				//將指定index的元素移出陣列
				TblKey.splice(index, 1);
				//寫入ParentTblProperty
				VmNew.ParentTblProperty = TblKey

				//寫入每列識別碼
				VmNew.ParentData.map(item => {
					VmNew.$set(item, 'DELBUTTON', false);  //for 刪除按鈕加載動畫, 於每一筆outputdata增加key:delButton, 才能賦予每一按鈕唯一識別
					VmNew.$set(item, 'KEY', item.Level03no);  //for 模板中刪除的唯一識別
					return item;
				});
				//取得歸屬BU選項預設值
				var Input1OptionsArr = VmNew.InputOptionsDefault.map(item => Object.values(item)[3]);
				Input1OptionsArr.push('BU0', 'BU1', 'BU2', 'BU3', 'BU4', 'BU5', 'CIM', 'CIT'); //預設選項
				VmNew.Input1Options = Input1OptionsArr.filter((item, index) => Input1OptionsArr.indexOf(item) === index).sort(); //去重複

				//取得columns(key) 對應的 DisplayName
				//先擴展ParentTblLabel陣列長度, 否則無法將 columns 覆寫為 DisplayName
				for (var i = 0; i < TblKey.length; i++) {
					VmNew.ParentTblLabel.push("");
				}
				var dataJson = {};
				dataJson['Columnname'] = VmNew.ParentTblProperty;
				dataJson['Typename'] = "ArgoCimCimSystemmenulist";
				axios.post('/api/Shared_API/GetDisplayName', dataJson
				).then(function (res) {
					VmNew.ParentTblLabel = res.data;  //調用該變量
				});
			});
		},

		HandleCurrentChange(Page) {
			this.CurrentPage = Page;
		},

		DialogHandleCurrentChange(Page) {
			this.DialogCurrentPage = Page;
		},

		//取得新增選單選項關聯
		GetOptions() {
			var VmNew = this;  //定義外部變量,將this的值賦予它
			axios.post('/api/UserManageSys_API/SysMenuData', ''
			).then(function (res) {
				VmNew.InputOptionsDefault = res.data;  //調用該變量
				//將新增選項轉成大寫
				VmNew.FormAdd.Input1 = VmNew.FormAdd.Input1.toUpperCase();
				VmNew.FormAdd.Input3 = VmNew.FormAdd.Input3.toUpperCase();
				VmNew.FormAdd.Input5 = VmNew.FormAdd.Input5.toUpperCase();
				//取歸屬BU+系統代碼+模組代碼選項關聯(資料庫與輸入框皆是大寫)
				VmNew.InputOptionsDefault = VmNew.InputOptionsDefault.filter(item => {
					return item.Belongbu.match(VmNew.FormAdd.Input1) && item.Level01no.match(VmNew.FormAdd.Input3) && item.Level02no.match(VmNew.FormAdd.Input5);
				});
				//取得歸屬BU選項
				var Input1OptionsArr = VmNew.InputOptionsDefault.map(item => Object.values(item)[3]);
				Input1OptionsArr.push('BU0', 'BU1', 'BU2', 'BU3', 'BU4', 'BU5', 'CIM', 'CIT'); //預設選項
				VmNew.Input1Options = Input1OptionsArr.filter((item, index) => Input1OptionsArr.indexOf(item) === index).sort(); //去重複
				//取得Level01選項
				var Level1NoArr1 = VmNew.InputOptionsDefault.map(item => Object.values(item)[5]);
				var Level1NoArr2 = VmNew.InputOptionsDefault.map(item => Object.values(item)[6]);
				var Level1NoArr = [];
				Level1NoArr1.forEach((item, i) => {
					Level1NoArr.push({
						code: item,
						name: Level1NoArr2[i]
					})
				});
				var map = new Map();
				VmNew.Input3Options = Level1NoArr.filter(item => !map.has(item.code) && map.set(item.code, 1)) // 對code屬性進行去重複
				//取得Level02選項
				var Level2NoArr1 = VmNew.InputOptionsDefault.map(item => Object.values(item)[7]);
				var Level2NoArr2 = VmNew.InputOptionsDefault.map(item => Object.values(item)[8]);
				var Level2NoArr = [];
				Level2NoArr1.forEach((item, i) => {
					Level2NoArr.push({
						code: item,
						name: Level2NoArr2[i]
					})
				});
				var map = new Map();
				VmNew.Input5Options = Level2NoArr.filter(item => !map.has(item.code) && map.set(item.code, 1)) // 對code屬性進行去重複
				//取得最新的Level03代碼
				var Level3NoArr = VmNew.InputOptionsDefault.map(item => Object.values(item)[1]).sort().reverse(); //降序
				if (Level3NoArr.length > 0) {
					var Level3Temp = Number(Level3NoArr[0].substr(-4)) + 1; //先轉數字再+1
					Level3Temp = String(Level3Temp).padStart(4, 0); //補4位數並轉回字串
				} else {
					var Level3Temp = '0001';
				}
				if (VmNew.FormAdd.Input1 != '' && VmNew.FormAdd.Input3 != '' && VmNew.FormAdd.Input5 != '') {
					VmNew.FormAdd.Input7 = VmNew.FormAdd.Input1 + VmNew.FormAdd.Input3 + VmNew.FormAdd.Input5 + Level3Temp;
				}
			});
		},

		//取得選單關聯角色
		GetUserMenuRole(id) {
			var VmNew = this;  //定義外部變量,將this的值賦予它
			var ajaxJson = {
				Roleno: '',
				Level03no: id
			};
			axios.post('/api/UserManageSys_API/UserRoleDetailRole', ajaxJson
			).then(function (res) {
				VmNew.ChildCheckboxArr = res.data;  //調用該變量
			});
		},

		//新增選單基本資料
		DataAdd(tblname) {
			//開啟按紐載入動畫
			this.AddButton = true;
			//element ui 表單驗證
			this.$refs[tblname].validate((valid) => {
				//驗證通過
				if (valid) {
					//API路徑
					var apiUrl = '/api/UserManageSys_API/' + tblname;
					//先取得表單內所有內容
					var formId = '#' + tblname;
					var formData = $(formId).serializeArray();
					//定義新的FormData
					const formDataAjax = new FormData();
					//將serializeArray()丟入new FormData()
					formData.map(item => {
						formDataAjax.append(item.name, item.value);
					});
					formDataAjax.append('Creator', userId);
					formDataAjax.set('Level03no', this.FormAdd.Input7);
					//丟入檔案(新增選單才需要)
					if (this.SelectedFile) {
						formDataAjax.append('file', this.SelectedFile); //'file'為固定(?) 改別的名稱會報錯 code 400
					}

					setTimeout(() => {
						axios.post(apiUrl, formDataAjax
						)
							.then(data => {
								this.$message({
									message: '資料新增成功!',
									type: 'success'
								});
								//資料新增成功,延遲reload(),避免沒看到提示成功訊息
								setTimeout(() => {
									//window.location.reload();
									this.DataLoad();
								}, 1000);

							})
							.catch(error => {
								if (error.response) {
									//伺服器回應了請求,但狀態碼非2開頭
									this.$message.error(error.response.data);
								} else if (error.request) {
									//請求已發出,但沒有收到回應
									console.log(error.request);
								} else {
									//其他錯誤
									console.log('error');
								}
							})
						//延遲1秒後關閉加載動畫
						this.AddButton = false;
					}, 1000);
					//驗證失敗
				} else {
					this.AddButton = false;
					return false;
				}
			});
		},

		//清除圖片縮圖
		HandleRemove(file, fileList) {
			this.ImgDialogUrl = '';
			this.FileList.splice(0, 1);
			this.SelectedFile = null;
			this.HideUploadBtn = this.ImgDialogUrl != '';
		},

		//圖片懸浮效果
		HandlePictureCardPreview(file) {
			this.ImgDialogUrl = file.url;
			this.ImgDialogVisible = true;
		},

		GetUrl(file, fileList) {
			this.ImgDialogUrl = file.url;
			this.FileList = fileList;
			this.SelectedFile = file.raw;
			this.HideUploadBtn = this.ImgDialogUrl != '';
			//檔案驗證
			const IsPNG = file.raw.type === 'image/png';
			const IsLt2M = file.size / 1024 / 1024 < 2;
			if (!IsPNG) {
				this.$message.error('限定上傳類型為PNG !');
				this.ImgDialogUrl = '';
				this.FileList.splice(0, 1);
				this.SelectedFile = null;
				this.HideUploadBtn = this.ImgDialogUrl != '';
			}
			if (!IsLt2M) {
				this.$message.error('檔案不可超過2MB !');
				this.ImgDialogUrl = '';
				this.FileList.splice(0, 1);
				this.SelectedFile = null;
				this.HideUploadBtn = this.ImgDialogUrl != '';
			}
			return IsPNG && IsLt2M;
		},

		//選單綁定角色
		DataBind(id, arrname, tblname) {
			//判斷選中之項目是否存在於陣列 (ex.UserRoleArr/)
			if (arrname == 'ChildCheckboxArr') {
				var tempColumnName1 = 'Roleno';
				var tempValue1 = id;
				var tempColumnName2 = 'Level03no';
				var tempValue2 = this.EditName;
				if (this.ChildCheckboxArr.includes(id)) { //改操作this實體數據,因分頁換頁會清除勾選項目
					//已存在,從實體Arr移除
					var index = this.ChildCheckboxArr.findIndex(item => item === id); //先找指定元素在原陣列index
					//將指定index的元素移出陣列
					this.ChildCheckboxArr.splice(index, 1);
				} else {
					//不存在,新增至實體Arr
					this.ChildCheckboxArr.push(id);
				}
			} else {
				var tempArr = [];
				var tempColumnName1 = '';
				var tempValue1 = '';
				var tempColumnName2 = '';
			}

			//API路徑
			var apiUrl = '/api/UserManageSys_API/' + tblname;
			//改為json格式
			var dataJson = {};
			dataJson[tempColumnName1] = tempValue1;
			dataJson[tempColumnName2] = tempValue2;
			dataJson['Creator'] = userId;
			dataJson['Updater'] = userId;
			//傳送資料
			axios.post(apiUrl, dataJson
			).then(data => {
				this.$message({
					message: '異動成功!',
					type: 'success'
				});
			}).catch(function (error) {
				this.$message.error('異動失敗，請重新作業!');
				console.log(error);
			})
		},

		//切換為選單編輯中
		DataMenuEdit(row) {
			this.EditStatus = true;
			this.FormAdd.Input1 = row.Belongbu;
			this.FormAdd.Input2 = row.Belongdept;
			this.FormAdd.Input3 = row.Level01no;
			this.FormAdd.Input4 = row.Level01;
			this.FormAdd.Input5 = row.Level02no;
			this.FormAdd.Input6 = row.Level02;
			this.FormAdd.Input7 = row.Level03no;
			this.FormAdd.Input8 = row.Level03;
			this.FormAdd.Input9 = row.Icon;
			this.FormAdd.Input10 = row.Controller;
			this.FormAdd.Input11 = row.Action;
			this.FormAdd.Input12 = row.Sequence;
			this.FormAdd.Input13 = row.Keyword;
			this.FormAdd.Input14 = row.Enabled;
			this.FormAdd.Input15 = row.Remark;
			//圖片處理
			if (row.Imgname != null) {
				this.FormAdd.ImageUrl = "/UserManageSys/images/UserManageSys/" + row.Imgname;
				this.FormAdd.ImageList = [];  //先清除,避免連擊情境下會帶入多張圖片..
				this.FormAdd.ImageList.push(this.FormAdd.ImageUrl);
			} else {
				this.FormAdd.ImageUrl = "";
				this.FormAdd.ImageList = [];
			}

		},

		//更新選單
		DataUpdate(tblname, route) {
			var confirmMessage = "是否確認更新?";
			var confirmTitle = "即將異動資料...";
			var sucessMessage = "資料更新成功!";

			this.$confirm(confirmMessage, confirmTitle, {
				confirmButtonText: '確定',
				cancelButtonText: '取消',
				type: 'warning'
			}).then(() => {
				//開啟按紐載入動畫
				this.AddButton = true;
				//element ui 表單驗證
				this.$refs[tblname].validate((valid) => {
					//驗證通過
					if (valid) {
						//API路徑
						var apiUrl = '/api/UserManageSys_API/' + route;
						//先取得表單內所有內容
						var formId = '#' + tblname;
						var formData = $(formId).serializeArray();
						//定義新的FormData
						const formDataAjax = new FormData();
						//將serializeArray()丟入new FormData()
						formData.map(item => {
							formDataAjax.append(item.name, item.value);
						});
						formDataAjax.append('Updater', userId);
						formDataAjax.set('Belongbu', this.FormAdd.Input1);  //此input輸入框為禁用, 無法取值,故直接賦值
						formDataAjax.set('Level01no', this.FormAdd.Input3);
						formDataAjax.set('Level01', this.FormAdd.Input4);
						formDataAjax.set('Level02no', this.FormAdd.Input5);
						formDataAjax.set('Level02', this.FormAdd.Input6);
						formDataAjax.set('Level03no', this.FormAdd.Input7);
						if (this.SelectedFile) {
							formDataAjax.append('file', this.SelectedFile); //'file'為固定(?) 改別的名稱會報錯 code 400
						}

						setTimeout(() => {
							axios.post(apiUrl, formDataAjax
							)
								.then(data => {
									this.$message({
										message: sucessMessage,
										type: 'success'
									});
									//資料新增成功,延遲reload(),避免沒看到提示成功訊息
									setTimeout(() => {
										//window.location.reload();
										this.DataLoad();
									}, 1000);
								})
								.catch(error => {
									if (error.response) {
										//伺服器回應了請求,但狀態碼非2開頭
										this.$message.error(error.response.data);
									} else if (error.request) {
										//請求已發出,但沒有收到回應
										console.log(error.request);
									} else {
										//其他錯誤
										console.log('error');
									}
								})
							//延遲1秒後關閉加載動畫
							this.AddButton = false;
						}, 1000);
						//驗證失敗
					} else {
						this.AddButton = false;
						return false;
					}
				});
			}).catch(() => {
				this.$message({
					type: 'info',
					message: '已取消'
				});
			});
		},

		DataDel(tblname, delrow, column) {
			if (tblname == "SysMenuEnabled") {
				var confirmMessage = "是否確認變更?";
				var confirmTitle = "即將變更狀態...";
				var sucessMessage = "狀態變更成功!";
			} else {
				var confirmMessage = "";
				var confirmTitle = "";
				var sucessMessage = "";
			}

			this.$confirm(confirmMessage, confirmTitle, {
				confirmButtonText: '確定',
				cancelButtonText: '取消',
				type: 'warning'
			}).then(() => {
				//變更為加載中
				delrow.DELBUTTON = true;
				var ApiUrl = "/api/UserManageSys_API/" + tblname;
				//改json格式
				var dataJson = {
					id: delrow[column],
					updator: userId
				};
				setTimeout(() => {
					axios.post(ApiUrl, dataJson
					).then(data => {
						this.$message({
							message: sucessMessage,
							type: 'success'
						});
						//延遲reload,避免沒看到提示成功訊息
						setTimeout(() => {
							//window.location.reload();
							this.DataLoad();
						}, 1000);
					}).catch(function (error) {
						this.$message.error('操作失敗，請重新作業!');
						console.log(error);
					});
				}, 1000);  //延遲1秒
			}).catch(() => {
				this.$message({
					type: 'info',
					message: '已取消'
				});
			});
		},
	},
});