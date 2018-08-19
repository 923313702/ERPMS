define(['jquery'], function ($) {

	return {

		//前一页
        beforePage: function (config) {
            
            $(config.before).unbind('click').click(function () {
				var total = $(config.total).val();
				var page = $(config.page).val();
				if (total != -1 && page != 0) {
					if (total == 0) {
						$.messager.alert('提示', '没有任何数据', 'info');
						return;
					}
					if (parseInt(page) >= parseInt(total)) {
						$.messager.alert('提示', '讨厌,被你看光了', 'info');
						return;
					}
				}
				var flag = $(config.next).attr('flag');
				if (flag == 'next') {
					++page;
					$(config.next).attr('flag', '')
				}
				reloadForm(page, config);

				++page;
				$(config.page).val(page);
				$(this).attr('flag', 'pre');
			})
		},
		nextPage: function (config) {
            $(config.next).unbind('click').click(function () {
				var page = $(config.page).val();
				var total = $(config.total).val();
				if (total == 0) {
					$.messager.alert('提示', '没有任何数据', 'info');
					return;
				}
				if (page <= 0) {
					$(config.page).val(0)
					$.messager.alert('提示', '已是最后一个', 'info');
					return;
				}
				var flag = $(config.before).attr('flag');
				if (flag == 'pre') {
					--page;
					$(config.before).attr('flag', '');
					if (page <= 0) {
						$(config.page).val(1)
						$.messager.alert('提示', '已是最后一个', 'info');
						return;
					}
				}
				--page;
				reloadForm(page, config);
				$(config.page).val(page);
				$(this).attr('flag', 'next');
			});
		},
		//审核
		auditor: function (config) {
			$(config.auditor).click(function () {
                var paramNo = $(config.paramNo).val();
                if (paramNo == null || paramNo == '' || paramNo == undefined) { return; }
                var auditorNo = $(config.auditorNo).val();
				if (auditorNo != null && auditorNo != '') { return; }
                var data = $(config.form).serialize();
				ajaxOperation(config.auditorUrl, data, function (response) {
					if (response.success == 0) {
						showMsg(response.msg);
                        $(config.auditorNo).val(response.AuditorNo);
                        //审核时间(发货申请单)
                        if (config.auditorTime != undefined && config.auditorTime != '') {
                            var date = new Date();
                            var strDate = date.getMonth()+1 + "/" + date.getDate() + "/" + date.getFullYear();
                            $(config.auditorTime).datebox('setValue', strDate);
                        }
                        //审核人(发货申请单)
                        if (config.auditorPerson != undefined && config.auditorPerson != '') {
                            $(config.auditorPerson).combobox('setValue', response.AuditorNo);
                        }
						linkbuttonDisable(config.linkbuttonArr, true);
						menuItemDisable(config.menuArr, true);

					} else {
						$.messager.alert('提示', response.msg, 'info')
					}
				});
			})
		},
		//撤申
		unAuditor: function (config) {
            $(config.unAuditor).click(function () {
                var paramNo = $(config.paramNo).val();

                if (paramNo == null || paramNo == '' || paramNo == undefined) { return; }
				var auditorNo = $(config.auditorNo).val();
				if (auditorNo == null || auditorNo == '') { return; }
				var data = $(config.form).serialize();
				ajaxOperation(config.unAuditorUrl, data, function (response) {
					if (response.success == 0) {
						showMsg(response.msg);
                        $(config.auditorNo).val('');
                        //审核时间(发货申请单)
                        if (config.auditorTime != undefined && config.auditorTime != '') {
                            $(config.auditorTime).datebox('setValue', '');
                        }
                        //审核人(发货申请单)
                        if (config.auditorPerson != undefined && config.auditorPerson != '') {
                            $(config.auditorPerson).combobox('setValue', '');
                        }
						linkbuttonDisable(config.linkbuttonArr, false);
						menuItemDisable(config.menuArr, false);

					} else {
						$.messager.alert('提示', response.msg, 'info')
					}
				});
			})
        },
        added: function (config) {
            $(config.dom.add).unbind('click').click(function () {
                $(config.form).form('clear');
                $(config.text).html('');
                linkbuttonDisable(config.linkbuttonArr);
                menuItemDisable(config.menuArr);
                if (config.datagridList != null && config.datagridList.length > 0) {
                    var list = config.datagridList;
                    for (var i in list) {
                        $(list[i].datagrid).datagrid('loadData', { total: 0, rows: [] });
                    }
                }
            })
        },
        edited: function (config) {
            $(config.dom.edit).unbind('click').click(function () {
                var no = $(config.no).val();
                if (no == null || no == '') { return;}
                var data = $(config.form).serialize();
                if (!$(config.form).form('validate')) { return;}
                ajaxOperation(config.editUrl, data, function (response) {
                    if (response.success == 0) {
                        showMsg(response.msg);
                    } else {
                        $.messager.alert('提示', response.msg, 'info');
                    }
                })
            })
        },
        removed: function (config) {
            $(config.dom.remove).unbind('click').click(function () {
                var no=$(config.no).val();
                if (no == null || no == '') { return;}
                var data = $(config.form).serialize();
                ajaxOperation(config.removeUrl, data, function (response) {
                    if (response.success ==0) {
                        showMsg(response.msg);
                        $(config.text).text('');
                        $(config.form).form('clear');
                        var list = config.datagridList;
                        for (var i in list) {
                            $(list[i].datagrid).datagrid('loadData', { total: 0, rows: [] });
                        }
                    } else {
                        $.messager.alert('提示', response.msg, 'info');
                    }
                })
            })
        },
        saved: function (config) {
            $(config.dom.save).unbind('click').click(function () {
                var no = $(config.no).val();
                if (no != null && no != '') { return; }
                var flag = $(config.form).form('validate');
                if (!flag) { return;}
                var data = $(config.form).serialize();
                ajaxOperation(config.saveUrl, data, function (response) {
                    if (response.success == 0) {
                        showMsg(response.msg);
                        $(config.no).val(response.id);
                        $(config.text).html(response.id);
                    } else {
                        $.messager.alert('提示', response.msg, 'info');
                    }
                })
            });
           
        },
        refresh: function (config) {
            $(config.dom.refresh).unbind('click').click(function () {
                var no = $(config.no).val();
                console.log ('no'+no)
                if (no != null && no != '') {
                    ajaxOperation(config.refreshUrl, { paramNo: no }, function (response) {
                        if (response != null) {
                            $(config.form).form('load', response);
                            var list = config.datagridList;
                            if (list != undefined && list.length > 0) {
                                for (var i in list) {
                                    reloadDatagrid(list[i]);
                                }
                            }
                        }
                    })
                }
            })
        },
        controlMenuAndbutton: function (config) {
            console.log ('hello')
            menuItemDisable(config.menuArr, config.flag);
            linkbuttonDisable(config.linkbuttonArr, config.flag);
        },
        
                // diaload
        initdialog: function (config) {
                $(config.dom).dialog({
                    title: config.title,
                    model: true,
                    closed: true,
                    width: config.width,
                    height: config.height,
                    cache: false,
                    modal: true,
                    href: config.href
                });
          
         $(config.dom).dialog('open');
        },
        ajaxOption: function (url, data, callback) {
            $.ajax({
                url: url,
                type: 'post',
                data: data,
                success: function (response) {
                    callback(response);
                },
                error: function (XMLHttpRequest, testStatus, errorThrown) {
                    $.messager.alert('My Title', XMLHttpRequest.status, 'warning')
                }
            })
        },
        showMsg:function (msg) {
            $.messager.show({
                title: '提示',
                msg: msg,
                timeout: 3000,
                showType: 'slide'
            });
        }
	}
	//reload Datagrid
	function reloadDatagrid(option) {
		var data = new Object();
		data.paramNo = $(option.param_No).val();
		$(option.datagrid).datagrid({ url: option.url, queryParams: data });;
	}
	function reloadForm(page, config) {
		$.ajax({
			url: config.runPageUrl,
			type: 'post',
			data: { page: page },
            success: function (response) {
                console.log (response.rows)
                var jsondata = $.parseJSON(response);
                if (jsondata.rows != null) {
                    $(config.total).val(jsondata.total);
                    $(config.form).form('load', jsondata.rows);
					$(config.text).html($(config.no).val())
					var person = config.auditorPerson == 0 ? 0 : 1;
					var flag;
					if (person == 0) {
                        flag = (jsondata.rows.审核人编码 != null && jsondata.rows.审核人编码 != '') ? true : false;
					} else {
                        flag = (jsondata.rows.计价审核人编码 != null && jsondata.rows.计价审核人编码 != '') ? true : false;
                    }
					menuItemDisable(config.menuArr, flag)
					linkbuttonDisable(config.linkbuttonArr, flag);
					var datagridList = config.list;
					for (var i in datagridList) {
						reloadDatagrid(datagridList[i]);
					}
				}
			},
			error: function (XMLHttpRequest, testStatus, errorThrown) {
				$.messager.alert('My Title', XMLHttpRequest.status, 'warning')
			}
		})
	}
	//linkbutton enable(disable)
    function linkbuttonDisable(arr, flag) {
        if (arr != undefined && arr.length > 0) {
            var able = flag ? 'disable' : 'enable';
            for (var i in arr) {
                $(arr[i]).linkbutton(able);
            }
        }
	}
	//同上
    function menuItemDisable(menus, flag) {
        if (menus != undefined && menus.length > 0) {
            var able = flag ? 'disableItem' : 'enableItem';
            for (var i in menus) {
                var menu = menus[i].menu;
                var items = menus[i].items;
                for (var item in items) {
                    $(menu).menu(able, $(items[item]));
                }
            }
        }
	}
    function showMsg(msg) {
        $.messager.show({
            title: '提示',
            msg: msg,
            timeout: 3000,
            showType: 'slide'
        });
    }
	//ajax基本操作
	function ajaxOperation(url, data, callback) {
		$.ajax({
			url: url,
			type: 'post',
			data: data,
			success: function (response) {
				callback(response);
			},
			error: function (XMLHttpRequest, testStatus, errorThrown) {
				$.messager.alert('My Title', XMLHttpRequest.status, 'warning')
			}
		})
    }

})