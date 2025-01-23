var vm = new Vue({

	el: "#app",
	data() {
		return {
			Loading: true, //axios 資料回傳較 mounted 慢, 設定此參數避免看到不完整畫面
			//目前網址
			NowUrl: "Role",
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
			ParentData: [],
			ParentTblWidth: [160, 250, 250, 150, 100, 150, 100],
			ParentTblProperty: [],
			ParentTblLabel: [],
			Input3Options: [//角色類型選項
				{ value: "0", label: '部門預設' },
				{ value: "1", label: '可申請' },
				{ value: "9", label: '部門預設，不開放查詢' }
			],
			//新增角色
			FormAdd: {
				Input1: "",
				Input2: "",
				Input3: "",
			},
			FormRules: { //表單驗證規則,需與v-model綁定名稱相同
				Input1: [{ required: true, message: "請輸入角色代碼", trigger: "blur" }, //blur:輸入框失去焦點時觸發; change: 數據改變時觸發
				{ min: 13, max: 15, message: '處級(3)+部門/模組(3)+課級(3)+流水碼(4)共13碼', trigger: 'blur' }],
				Input2: [{ required: true, message: "請輸入名稱說明", trigger: "blur" }],
				Input3: [{ required: true, message: "請選擇類型", trigger: "change" }],
			},
			//編輯角色
			EditStatus: false,  //true: 編輯中, 將選中資料帶到左表以執行更新
			//選單基本資訊
			ChildData: [],
			ChildTblWidth: [150, 250, 100, 150, 120, 200, 120, 250, 300, 200, 200, 100, 200, 300, 200, 150, 100, 150, 300],
			ChildTblProperty: [],
			ChildTblLabel: [],

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
		ParentDataFilter: function () {
			return this.ParentData.filter(item => {
				return item.Roleno.toUpperCase().match(this.Search.toUpperCase()) || item.Rolename.toUpperCase().match(this.Search.toUpperCase());  //全部轉成大寫才比對
			})
		},
		DialogFilter: function () {
			//2025.1.15 新增: 依照checked項目排序
			var ChildDataCopy = this.ChildData;
			ChildDataCopy.forEach(role => {
				role.checked = this.ChildCheckboxArr.includes(role.Leveel03no) ? 1 : 0;
			});
			ChildDataCopy.sort((a, b) => b.checked - a.checked);
			return ChildDataCopy.filter(item => {
				return item.Level03.toUpperCase().match(this.SearchDialog.toUpperCase()) || item.Level03no.toUpperCase().match(this.SearchDialog.toUpperCase());  //全部轉成大寫才比對
			})
		},
	},

	methods: {
		//取得所有資料
		DataLoad() {
			var VmNew = this;  //定義外部變量,將this的值賦予它
			//取得 ARGO_CIM_CIM_USERROLEBASIS(角色基本資訊)
			axios.post('/api/UserManageSys_API/UserRoleBasisData', ''
			).then(function (res) {
				VmNew.ParentData = res.data;  //調用該變量
				//取得table 所有columns (key)
				var TblKey = [];
				for (key in VmNew.ParentData[0]) {
					TblKey.push(key);

				}
				//移除不需要的欄位
				var index = TblKey.findIndex(item => item === "Roletype"); //先找指定元素在原陣列index
				//將指定index的元素移出陣列
				TblKey.splice(index, 1);
				//寫入ParentTblProperty
				VmNew.ParentTblProperty = TblKey

				//寫入每列識別碼
				VmNew.ParentData.map(item => {
					VmNew.$set(item, 'DELBUTTON', false);  //for 刪除按鈕加載動畫, 於每一筆outputdata增加key:delButton, 才能賦予每一按鈕唯一識別
					VmNew.$set(item, 'KEY', item.Roleno);  //for 模板中刪除的唯一識別
					return item;
				});

				//取得columns(key) 對應的 DisplayName
				//先擴展ParentTblLabel陣列長度, 否則無法將 columns 覆寫為 DisplayName
				for (var i = 0; i < TblKey.length; i++) {
					VmNew.ParentTblLabel.push("");
				}
				var dataJson = {};
				dataJson['Columnname'] = VmNew.ParentTblProperty;
				dataJson['Typename'] = "ArgoCimCimUserrolebasis";
				axios.post('/api/Shared_API/GetDisplayName', dataJson
				).then(function (res) {
					VmNew.ParentTblLabel = res.data;  //調用該變量
				});
			});

			//取得 ARGO_CIM_CIM_SYSTEMMENULIST(選單基本資訊)
			axios.post('/api/UserManageSys_API/SysMenuData', ''
			).then(function (res) {
				VmNew.ChildData = res.data;  //調用該變量
				//取得table 所有columns (key)
				var TblKey = [];
				for (key in VmNew.ChildData[0]) {
					TblKey.push(key);
				}
				//移除不需要的欄位
				var index = TblKey.findIndex(item => item === "Id"); //先找指定元素在原陣列index
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
				dataJson['Typename'] = "ArgoCimCimSystemmenulist";
				axios.post('/api/Shared_API/GetDisplayName', dataJson
				).then(function (res) {
					VmNew.ChildTblLabel = res.data;  //調用該變量
				});
			});
		},

		HandleCurrentChange(Page) {
			this.CurrentPage = Page;
		},

		DialogHandleCurrentChange(Page) {
			this.DialogCurrentPage = Page;
		},

		//取得角色關聯選單
		GetUserRoleMenu(id) {
			var VmNew = this;  //定義外部變量,將this的值賦予它
			var ajaxJson = {
				Roleno: id,
				Level03no: ''
			};
			axios.post('/api/UserManageSys_API/UserRoleDetailMenu', ajaxJson
			).then(function (res) {
				VmNew.ChildCheckboxArr = res.data;  //調用該變量
			});
		},

		//新增角色基本資料
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
					formDataAjax.set('Roletype', this.FormAdd.Input3);
					setTimeout(() => {
						// , {headers: {'Content-Type': 'multipart/form-data'}} 檔案不一定上傳,axios會自動判斷有無檔案
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

		//角色綁定選單
		DataBind(id, arrname, tblname) {
			//判斷選中之項目是否存在於陣列 (ex.UserRoleArr/)
			if (arrname == 'ChildCheckboxArr') {
				var tempColumnName1 = 'Roleno';
				var tempValue1 = this.EditName;
				var tempColumnName2 = 'Level03no';
				var tempValue2 = id;
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

		//切換為角色編輯中
		DataEdit(row) {
			this.EditStatus = true;
			this.FormAdd.Input1 = row.Roleno;
			this.FormAdd.Input2 = row.Rolename;
			this.FormAdd.Input3 = row.Roletype;
		},

		//更新角色
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
						formDataAjax.set('Roleno', this.FormAdd.Input1);  //此input輸入框為禁用, 無法取值,故直接賦值
						formDataAjax.set('Roletype', this.FormAdd.Input3); //serializeArray只能取select的label,變更為取select的value
						setTimeout(() => {
							//傳送資料
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
			if (tblname == "UserRoleDel") {
				var confirmMessage = "是否確認刪除?";
				var confirmTitle = "即將刪除資料...";
				var sucessMessage = "資料刪除成功!";
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
				console.log(dataJson);
				setTimeout(() => {
					//傳送資料
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