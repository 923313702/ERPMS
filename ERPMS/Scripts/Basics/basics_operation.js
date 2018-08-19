define(['jquery'], function ($) {
    //flag ,savebutton相同可以去掉一个 dom,datagrid 也重复了
    return {
        index: function (config) {
            var _this = this;
           // var dom = config.dom, datagrid = config.datagrid, editRow = config.editRow, removeUrl = config.ajaxUrl,exportUrl=config.exportUrl;
            _this.addRow(config);
            _this.editRow(config);
            _this.removeRow(config);
            _this.saveRow(config);
            _this.revortRow(config);
            _this.ExportExcel(config);
        },
        //初始化datagrid
        datagrid: function (config) {
            console.log(config.dom + config.initUrl + "datgrid");
            $(config.dom).datagrid({
                url: config.initUrl,
                method: 'post',
                fitColumns: true,
                striped: true,//条纹
                pagination: true,//分页
                queryParams: config.queryParams,
                toolbar: config.tool,
                pageList: [5, 10, 15, 20],
                fit: true,
                columns: config.columns,
                onAfterEdit: function (i, d, c) {
                    var flags = $(config.flag).attr('flag');
                    console.log(flags);
                    console.log('////');
                    $.ajax({
                        url: config.optionUrl,
                        type: 'post',
                        data: { data: d, flag: flags },
                        success: function (response) {
                            console.log(response);
                            if (response.success == 0) {
                                config.optionConfig.editRow = undefined;
                                showMsg(response.msg);
                            } else {
                                $.messager.alert('提示', response.msg, 'info');
                                $(config.dom).datagrid('beginEdit', i);
                            }
                        },
                        error: function (XMLHttpRequest, testStatus, errorThrown) {
                            $.messager.alert('My Title', XMLHttpRequest.status, 'warning')
                        }
                    })
                },
                onLoadSuccess: function ()
                {
                    config.optionConfig.editRow = undefined;
                    console.log ('loadSuccess')
                }
            });
        },
        //ajax基本操作
        ajaxOperation: function (url, data, callback) {
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
        //初始化combobox
        initCombobox: function (dom,url) {
            $(dom).combobox({
                url: url,
                textField: 'Key',
                valueField: 'Id',
                filter: function (q, r) {
                    var opts = $(this).combobox('options');
                    return r[opts.textField].indexOf(q) >= 0;
                }
            })
        },
       //添加行
        addRow: function (config) {
            $(config.dom.add).click(function () {
                if (config.editRow != undefined) {
                    $(config.datagrid).datagrid('endEdit', config.editRow)
                } else {
                    $(config.savebutton).attr('flag', 'add');
                    $(config.datagrid).datagrid('insertRow', { index: 0, row: {} });
                    $(config.datagrid).datagrid('beginEdit', 0);
                    config.editRow = 0;
                }
                console.log(config.editRow)
            });
        },
        //修改行
        editRow: function (config) {
            $(config.dom.edit).click(function (){
                if (config.editRow != undefined) {
                    $.messager.alert('提示', '有未完成的编辑项,请保存', 'info');
                    return;
                }
                var rows = $(config.datagrid).datagrid('getSelections');
                if (rows.length != 1) {
                    $.messager.alert('提示', '请选中要修改的数据', 'info');
                    return;
                }
                $(config.savebutton).attr('flag', 'edit');
                var index = $(config.datagrid).datagrid('getRowIndex', rows[0]);
                config.editRow = index;
                $(config.datagrid).datagrid('beginEdit', index);
            });
        },
        //删除行
        removeRow: function (config) {
            $(config.dom.remove).click(function () {
                console.log ('zhixingma ')
                if (config.editRow != undefined) {
                    $.messager.alert('提示', '有未完成的编辑项', 'info');
                    return;
                }
                var rows = $(config.datagrid).datagrid('getSelections');
                if (rows.length <= 0) {
                    $.messager.alert('提示', '请选中要删除的数据', 'info');
                    return;
                }
                var data = { data: rows };
               // ajaxOperation(ajaxUrl, data, function (response) {
                $.ajax({
                    url: config.removeUrl,
                        type: 'post',
                        data: data,
                        success: function (response) {
                            if (response.success == 0) {
                                $(config.datagrid).datagrid('reload');
                                showMsg(response.msg);
                            } else {
                                $.messager.alert('提示', response.msg, 'info');
                            }
                        },
                        error: function (XMLHttpRequest, testStatus, errorThrown) {
                            $.messager.alert('My Title', XMLHttpRequest.status, 'warning')
                        }
                    })  
                })
          //  });
        },
        //保存行
        saveRow: function (config) {
            $(config.dom.save).click(function () {
                if (config.editRow != undefined) {
                    $(config.datagrid).datagrid('endEdit', config.editRow);
                }
            })
        },
        //撤销行
        revortRow: function (config) {
            $(config.dom.revort).click(function () {
                if (config.editRow != undefined) {
                    var flag = $(config.savebutton).attr('flag');
                    if (flag == 'add') {
                        $(config.datagrid).datagrid('deleteRow', config.editRow);
                    } else {
                        $(config.datagrid).datagrid('refreshRow', config.editRow);
                    }
                    config.editRow = undefined;
                }
            });
        },
        refresh: function (config) {
            $(config.dom).datagrid('reload');
        },
        //导出Excel
        ExportExcel: function (config) {
            console.log('start' + config.dom.export)
            $(config.dom.export).click(function () {
                console.log('start'+config.dom.export)
                var strUrl = '';
                var page = $(config.datagrid).datagrid("options").pageNumber;
                var rows = $(config.datagrid).datagrid("options").pageSize;
                var queryParams = $(config.datagrid).datagrid('options').queryParams;
                if (queryParams != null && queryParams != undefined) {
                    for (var item in queryParams) {
                        if (typeof (queryParams[item]) == 'object' || typeof (queryParams[item]) == 'function') { continue };
                        strUrl += '&' + item + '=' + queryParams[item];
                    }
                    strUrl = config.exportUrl +'?page='+page + '&rows='+ rows + strUrl
                } else {
                    strUrl = config.exportUrl
                }
                //var strUrl = '{0}?page=' + page + '&rows=' + rows;
                //strUrl = strUrl.format(url);
                console.log ('strUrl'+strUrl)
                $(this).attr('href', strUrl);
            })
        },
         //提示
        showMsg: function (msg) {
            $.messager.show({
                title: '提示',
                msg: msg,
                timeout: 3000,
                showType: 'slide'
            });
        }
    }
    //字符串格式化
    String.prototype.format = function () {
        var args = arguments;
        return this.replace(/\{(\d+)\}/g,
            function (m, i) {
                return args[i];
            });
    }
    //判断对象是否为空
    function isEmptyObject(e) {
        var t;
        for (t in e)
            return !1;
        return !0
    }
    function showMsg(msg) {
        $.messager.show({
            title: '提示',
            msg: msg,
            timeout: 3000,
            showType: 'slide'
        });
    }
})