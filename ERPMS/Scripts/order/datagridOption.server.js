define(['jquery'], function ($) {
    return {
        //datagrid 增删改查
        datagridOption: function (config) {
            var _this = this;
            _this.addRow(config);
            _this.editRow(config);
            _this.removeRow(config);
            _this.saveRow(config);
            _this.revortRow(config);

        },
        //初始化datagrid
        datagrid: function (config) {
            $(config.dom).datagrid({
                url: config.initUrl,
                method: 'post',
                fitColumns: true,
                striped: true,//条纹
                queryParams: config.queryParams,
                fit: true,
                columns: config.columns,
                onRowContextMenu: function (e, index, row) {
                    $(this).datagrid("selectRow", index); //根据索引选中该行
                    e.preventDefault();
                    $(config.menu).menu('show', {
                        left: e.pageX,
                        top: e.pageY
                    });
                }
            })
        },
        //添加行
        addRow: function (config) {

            $(config.dom.add).off("click");
            $(config.dom.add).click(function () {
                var value = $(config.paramNo).val();
                if (value == null || value == '') { return;}
                if (config.editRow != undefined) {
                    $(config.datagrid).datagrid('endEdit', config.editRow)
                } else {
                    $(config.savebutton).attr('flag', 'add');
                    $(config.datagrid).datagrid('insertRow', { index: 0, row: {} });
                    $(config.datagrid).datagrid('beginEdit', 0);
                    config.editRow = 0;

                }
            });
        },
        //修改行
        editRow: function (config) {
            $(config.dom.edit).off("click");
            $(config.dom.edit).click(function () {
                //var value = $(config.paramNo).val();
                //if (value == null || value == '') { return; }
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
            $(config.dom.remove).off("click");
            $(config.dom.remove).click(function () {
                //var value = $(config.paramNo).val();
                //if (value == null || value == '') { return; }
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
            $(config.dom.save).off("click");
            $(config.dom.save).click(function () {
                if (config.editRow != undefined) {
                    $(config.datagrid).datagrid('endEdit', config.editRow);
                }
            })
        },
        //撤销行
        revortRow: function (config) {
            $(config.dom.revort).off("click");
            $(config.dom.revort).click(function () {
                if (config.editRow != undefined) {
                    var flag = $(config.savebutton).attr('flag');
                    if (flag == 'add') {
                        $(config.datagrid).datagrid('deleteRow', config.editRow);
                    } else {
                        $(config.datagrid).datagrid('rejectChanges');
                    }
                    config.editRow = undefined;
                }
            });
        },

        //提示
        showMsg: function (msg) {
            $.messager.show({
                title: '提示',
                msg: msg,
                timeout: 3000,
                showType: 'slide'
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

    }
    function showMsg(msg) {
        $.messager.show({
            title: '提示',
            msg: msg,
            timeout: 3000,
            showType: 'slide'
        });
    }
});