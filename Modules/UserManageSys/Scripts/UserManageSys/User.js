var vm = new Vue({

	el: "#app",
	data() {
		return {
			Loading: true, //axios 資料回傳較 mounted 慢, 設定此參數避免看到不完整畫面
			//目前網址
			NowUrl: "User",
			//檢索條件
			Search: "",
			SearchUserDept: "",
			SearchUserDeptOption: [],
			SearchComeDay: "",
			SearchLeaveDay: "",
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
			//使用者角色
			ParentData: [],
			ParentTblWidth: [150, 150, 350, 350, 150, 150, 150],
			ParentTblProperty: [],
			ParentTblLabel: [],
			//角色基本資訊
			ChildData: [],
			ChildTblWidth: [160, 250, 250, 150, 100, 150, 100],
			ChildTblProperty: [],
			ChildTblLabel: [],
		}
	},
	beforeMount: function () {
		var VmNew = this;  //定義外部變量,將this的值賦予它

		//取得 ARGO_CIM_CIM_USERBASIS(使用者資訊)
		axios.post('/api/UserManageSys_API/UserData', ''
		).then(function (res) {
			VmNew.ParentData = res.data;  //調用該變量

			//取得table 所有columns (key)
			var TblKey = [];
			for (key in VmNew.ParentData[0]) {
				TblKey.push(key);
			}
			//寫入ParentTblProperty
			VmNew.ParentTblProperty = TblKey
			//取得部門預設選項
			var SearchUserDeptOptionTemp = VmNew.ParentData.map(item => Object.values(item)[2]); // 陣列中有物件=>取得特性屬性值的陣列: 3 表示第3個屬性值[DNDESC部門]
			SearchUserDeptOptionTemp.push(''); //增加空白元素代表[ALL]選項
			VmNew.SearchUserDeptOption = SearchUserDeptOptionTemp.filter((item, index) => SearchUserDeptOptionTemp.indexOf(item) === index).sort(); //去重複

			//取得columns(key) 對應的 DisplayName
			//先擴展ParentTblLabel陣列長度, 否則無法將 columns 覆寫為 DisplayName
			for (var i = 0; i < TblKey.length; i++) {
				VmNew.ParentTblLabel.push("");
			}
			var dataJson = {};
			dataJson['Columnname'] = VmNew.ParentTblProperty;
			dataJson['Typename'] = "ArgoCimCimUserbasis";

			axios.post('/api/Shared_API/GetDisplayName', dataJson
			).then(function (res) {
				VmNew.ParentTblLabel = res.data;  //調用該變量
			});

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

			//寫入每列識別碼
			VmNew.ChildData.map(item => {
				VmNew.$set(item, 'DELBUTTON', false);  //for 刪除按鈕加載動畫, 於每一筆outputdata增加key:delButton, 才能賦予每一按鈕唯一識別
				VmNew.$set(item, 'KEY', item.Roleno);  //for 模板中刪除的唯一識別
				return item;
			});

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
		}).catch(error => {
			console.error('Error:', error);
			if (error.response) {
				console.error('response data:', error.response.data);
			}
		});
	},

	mounted() {
		setTimeout(() => {
			this.Loading = false;
		}, 100);
	},

	computed: {

		ParentDataFilter: function () {
			return this.ParentData.filter(item => {

				if (this.SearchLeaveDay !== "" && this.SearchLeaveDay !== null) {
					//若輸入離職日
					return (item.Leaveday >= this.SearchLeaveDay) && (item.Leaveday < '9999-01-01');
				}
				else if (this.SearchComeDay !== "" && this.SearchComeDay !== null) {
					//若輸入到職日
					return item.Comeday >= this.SearchComeDay;
				}
				else if (this.Search !== "") {
					//若輸入關鍵字則執行關鍵字查詢
					return item.Userno.match(this.Search) || item.Username.match(this.Search) ||
						item.Dndesc.match(this.Search) || item.Email.toUpperCase().match(this.Search.toUpperCase());  //EMAIL轉成大寫才比對
				} else {
					//否則依部門預先篩選
					return item.Dndesc.match(this.SearchUserDept);
				}
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

		//切換分頁
		HandleCurrentChange(Page) {
			this.CurrentPage = Page;
		},

		DialogHandleCurrentChange(Page) {
			this.DialogCurrentPage = Page;
		},

		sortByChecked(a, b) {
			var aChecked = this.ChildCheckboxArr.includes(a.Roleno);
			console.log(aChecked);
			var bChecked = this.ChildCheckboxArr.includes(b.Roleno);
			console.log(bChecked);
			//return aChecked === bChecked ? 0 : aChecked ? -1 : 1;
		},

		//取得指定工號角色
		GetUserRole(id) {
			var VmNew = this;  //定義外部變量,將this的值賦予它
			var ajaxJson = {
				Userno: id,
				Username: "",
				Email: "",
				Dndesc: "",
				Hirestatus: ""
			};
			axios.post('/api/UserManageSys_API/UserRoleData', ajaxJson
			).then(function (res) {
				VmNew.ChildCheckboxArr = res.data;  //調用該變量
				if (VmNew.ChildCheckboxArr == "") {
					VmNew.ChildCheckboxArr = [];
				} else {
					VmNew.ChildCheckboxArr = VmNew.ChildCheckboxArr.split(',');  //字串轉陣列
				}

			});

		},

		//綁定角色/選單
		DataBind(id, arrname, tblname) {

			//判斷選中之項目是否存在於陣列 (ex.ChildCheckboxArr/)
			if (arrname == 'ChildCheckboxArr') {
				var tempColumnName1 = 'Userno';
				var tempValue1 = this.EditName;
				var tempColumnName2 = 'Userrole';
				if (this.ChildCheckboxArr.includes(id)) { //改操作this實體數據,因分頁換頁會清除勾選項目
					//已存在,從實體Arr移除
					var index = this.ChildCheckboxArr.findIndex(item => item === id); //先找指定元素在原陣列index
					//將指定index的元素移出陣列
					this.ChildCheckboxArr.splice(index, 1);
				} else {
					//不存在,新增至實體Arr
					this.ChildCheckboxArr.push(id);
				}
				var tempStr = this.ChildCheckboxArr.join(",");
				var tempValue2 = tempStr;

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
	},
});