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
            //todo:导入材料,导入工艺
        },
        //初始化datagrid
        datagrid: function (config) {
            $(config.dom).datagrid({
                url: config.initUrl,
                method: 'post',
                fitColumns: true,
                striped: true,//条纹
               // pagination: true,//分页
                queryParams: config.queryParams,
               // toolbar: config.tool,
               // pageList: [5, 10, 15, 20],
                fit: true,
                //border: false,
                columns: config.columns,
                onRowContextMenu: function (e, index, row) {
                    $(this).datagrid("selectRow", index); //根据索引选中该行
                    e.preventDefault();
                    $(config.menu).menu('show', {
                        left: e.pageX,
                        top: e.pageY
                    });
                },
                    onAfterEdit: function (i, d, c) {
                        var flags = $(config.flag).attr('flag');
                        var val = $(config.optionConfig.paramNo).val();
                        if (val == null || val == '') {
                            $.messager.alert('提示', '没获取到订单号', 'info');
                            $(config.datagrid).datagrid('rejectChanges');
                            config.optionConfig.editRow = undefined;
                            return;
                        }
                        if (flags == 'add') { d.订单号 = val; }
                        console.log (d)
                            ajaxOperation(config.optionUrl, { data: d, flag: flags }, function (response) {
                            if (response.success == 0) {
                                d.行号 = response.id;
                                config.optionConfig.editRow = undefined;
                                if (config.refresh) { $(config.dom).datagrid('reload')}
                                showMsg(response.msg);

                            } else {
                                $(config.dom).datagrid('beginEdit', i);
                                $.messager.alert('提示', response.msg, 'info');
                              
                            }
                        })
                    },
                    onLoadSuccess: function (data) {
                        config.optionConfig.editRow = undefined;

                        if (config.merge != null && config.merge.length > 0) {
                            $(config.dom).datagrid("autoMergeCells", config.merge);
                        }
                        // orderPrice 独有计算
                        if (config.calculation != null && config.calculation != undefined) {
                            reckon(data.rows, config.calculation)
                        }
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
                var value = $(config.paramNo).val();
                if (value == null || value == '') { return; }
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
                var value = $(config.paramNo).val();
                if (value == null || value == '') { return; }
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
                            var list = config.datagridList;
                            if (list != undefined && list.length > 0) {
                                console.log ('datagridyoushujuma ')
                                for (var i = 0; i < list.length; i++) {
                                    reloadDatagrid(list[i].dom, list[i].url, value);
                                }
                            }
                            showMsg(response.msg);
                        } else {
                            $.messager.alert('提示', response.msg, 'info');
                              $(config.datagrid).datagrid('reload');
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
    //计算(固定的)
    function reckon(arr, data) {
        var caiLiao = shouGong = yinHou = yinQian = yinShua = shuiFei = fanKuan = qiTa = fuMo = total = 0
        for (var i = 0; i < arr.length; i++) {
            var tem = arr[i].统计编码
            switch (tem) {
                case '材料':
                    if (!isNaN(arr[i].金额)) {
                        caiLiao += arr[i].金额
                    }
                    break;
                case '手工':
                    if (!isNaN(arr[i].金额)) {
                        shouGong += arr[i].金额
                    }
                    break;
                case '印后':
                    if (!isNaN(arr[i].金额)) {
                        yinHou += arr[i].金额;
                    }
                    break;
                case '覆膜':
                    if (!isNaN(arr[i].金额)) {
                        fuMo += arr[i].金额;
                    }
                    break;
                case '印前':
                    if (!isNaN(arr[i].金额)) {
                        yinQian += arr[i].金额;
                    }
                    break;
                case '印刷':
                    if (!isNaN(arr[i].金额)) {
                        yinShua += arr[i].金额;
                    }
                    break;

                case '税款':
                    if (!isNaN(arr[i].金额)) {
                        shuiFei += arr[i].金额;
                    }
                    break;
                case '返款':
                    if (!isNaN(arr[i].金额)) {
                        fanKuan += arr[i].金额;
                    }
                    break;
                default:
                    if (!isNaN(arr[i].金额)) {
                        qiTa += arr[i].金额;
                    }
                    break;
            }
            if (!isNaN(arr[i].金额)) {
                total += arr[i].金额;
            }
        }
        for (var i in data) {
            switch (i) {
                case '材料':
                    $('#' + data[i]).textbox('setValue', '￥' + caiLiao.toFixed(2));
                    $('#_' + data[i]).val(caiLiao.toFixed(2));
                    break;
                case '手工':

                    $('#' + data[i]).textbox('setValue', '￥' + shouGong.toFixed(2));
                    $('#_' + data[i]).val(shouGong.toFixed(2));
                    break;
                case '加工':
                    $('#' + data[i]).textbox('setValue', '￥' + yinHou.toFixed(2));
                    $('#_' + data[i]).val(yinHou.toFixed(2));
                    break;
                case '覆膜':
                    $('#' + data[i]).textbox('setValue', '￥' + fuMo.toFixed(2));
                    $('#_' + data[i]).val(fuMo.toFixed(2));
                    break;
                case '印前':
                    $('#' + data[i]).textbox('setValue', '￥' + yinQian.toFixed(2));
                    $('#_' + data[i]).val(yinQian.toFixed(2));
                    break;
                case '印刷':
                    $('#' + data[i]).textbox('setValue', '￥' + yinShua.toFixed(2));
                    $('#_' + data[i]).val(yinShua.toFixed(2));
                    break;
                case '税费':
                    console.log(shuiFei.toFixed(2))
                    console.log (data[i])
                    $('#' + data[i]).textbox('setValue', '￥' + shuiFei.toFixed(2));
                    $('#_' + data[i]).val(shuiFei.toFixed(2));
                    break;
                case '返款':
                    $('#' + data[i]).textbox('setValue', '￥' + fanKuan.toFixed(2));
                    $('#_' + data[i]).val(fanKuan.toFixed(2));
                    break;
                case '合计':
                    $('#' + data[i]).textbox('setValue', '￥' + total.toFixed(2));
                    $('#_' + data[i]).val(total.toFixed(2));
                    break;
                case '其他':
                    $('#' + data[i]).textbox('setValue', '￥' + qiTa.toFixed(2));
                    $('#_' + data[i]).val(qiTa.toFixed(2));
                    break;
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
    function  ajaxOperation(url, data, callback) {
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

    function reloadDatagrid(dom, url, data) {
        $(dom).datagrid('options').url = url;
        $(dom).datagrid('reload', { paramNo: data })
    }


  

});