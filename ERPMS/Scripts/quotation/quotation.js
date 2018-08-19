define(['jquery', 'order/order.service', 'tool/initCombobox', 'tool/toolbar.service'], function ($, datagridservice, initCom, toolbar) {

    return {
        index: function () {
            var comboboxConfig = [
                { dom: '#quotation_saleMan', url: '/ERP_Order/GetStaff' },
                { dom: '#quotation_category', url: '/ERP_Order/PCategory' },
                { dom: '#quotation_customer', url: '/ERP_Order/GetCustormer2' },
                { dom: '#quitation_unit', url: '/ERP_Quotation/GetUnit' },
                { dom: '#quotation_procuction', url: '' },
                { dom: '#quotation_person', url: '' },
                { dom: '#quotation_auditorPerson', url: '' }
            ];
            initCom.initCombobox(comboboxConfig);
            $('#quotation_saleMan').combobox({
                onLoadSuccess: function () {
                    var data = $(this).combobox('getData');
                    $('#quotation_person').combobox('loadData', data);
                    $('#quotation_auditorPerson').combobox('loadData', data);
                }
            });
            $('#quotation_customer').combobox({
                onSelect: function (record) {
                    var phone = record.Phone;
                    $('#quotation_contact').textbox('setValue', phone);
                }
            });
            var optionConfig = {
                dom: {
                    add: '#quotation_add',
                    edit: '#quotation_edit',
                    remove: '#quotation_remove',
                    save: '#quotation_save',
                    refresh: '#quotation_refresh'
                },
                editUrl: '/ERP_Quotation/EditQuotation',
                saveUrl: '/ERP_Quotation/AddQuotation',
                removeUrl: '/ERP_Quotation/RemoveQuotation',
                refreshUrl: '/',
                no: '#quotation_orderNo',
                text: '#quotation_orderNo_text',
                form: '#quotation_form',
                datagridList: [
                    { datagrid: '#quotation_detail', url: '/ERP_Quotation/ShowDetail', param_No: '#quotation_orderNo' },
                    { datagrid: '#quotation_material', url: '/ERP_Quotation/ShowMaterial', param_No: '#quotation_orderNo' },
                    { datagrid: '#quotation_price', url: '/ERP_Quotation/ShowPrice', param_No: '#quotation_orderNo' }],
            };
            toolbar.added(optionConfig);
            toolbar.edited(optionConfig);
            toolbar.saved(optionConfig);
            toolbar.removed(optionConfig);
            toolbar.refresh(optionConfig);


            var detailConfig = {
                dom: '#quotation_detail',
                optionUrl: '/ERP_Quotation/SaveOrUpdateQuotationDetail',
                flag: '#quotation_detail_save',
                menu: '#quotation_detail_menu',
                columns: [[
                    { field: 'ck', checkbox: true },
                    { field: '行号', title: '行号', hidden: true },
                    { field: '订单号', title: '订单号', hidden: true },
                    {field:'纸张客户编码',title:'客户编码',hidden:true},
                    {
                        field: '印品部件',
                        title: '部件',
                        align: 'center',
                        width: 100,
                        editor: {
                            type: 'combobox',
                            options: {
                                url: '/ERP_Order/GetParts',
                                textField: 'Key',
                                valueField: 'Id',
                                required: true
                            }
                        }
                    },
                    
                    {
                        field: '纸张编码', title: '纸张名称', align: 'center', width: 150,
                        editor: {
                            type: 'combobox',
                            options: {
                                url: '/ERP_Order/GetPaper',
                                textField: 'Key',
                                valueField: 'Id',
                                panelWidth: 150,
                                panelHeight: 100,
                                required: true
                            },

                        }
                    },

                    {
                        field: '正面色数',
                        title: '正面',
                        align: 'center',
                        width: 80,
                        editor: {
                            type: 'numberbox',
                            options: {
                                precision: 0,
                            }
                        }
                    },
                    {
                        field: '背面色数',
                        title: '背面',
                        align: 'center',
                        width: 70,
                        editor: {
                            type: 'numberbox',
                            options: {
                                precision: 0,
                            }
                        }
                    },
                    {
                        field: '版数',
                        title: '版数',
                        align: 'center',
                        width: 70,
                        editor: {
                            type: 'numberbox',
                            options: {
                                precision: 0,
                            }
                        }
                    },
                    {
                        field: '上版方式',
                        title: '印刷方式',
                        align: 'center',
                        width: 80,
                        editor: {
                            type: 'combobox',
                            options: {
                                textField: 'name',
                                valueField: 'text',
                                editable: false,
                                panelWidth: 100,
                                panelHeight: 100,
                                data: [{
                                    "name": "单面",
                                    "text": "单面"
                                }, {
                                    "name": "正反",
                                    "text": "正反"
                                }, {
                                    "name": "自翻",
                                    "text": "自翻"
                                }, {
                                    "name": "大翻",
                                    "text": "大翻"
                                }, {

                                    "name": "不印",
                                    "text": "不印"
                                }]
                            }
                        }
                    },
                    {
                        field: '拼数',
                        title: '拼数',
                        align: 'center',
                        width: 70,
                        editor: {
                            type: 'numberbox',
                            options: {
                                precision: 0,
                                required: true,
                                onChange: function (n, o) {
                                    var value = 0;
                                    var value1 = $('#quotation_number').numberbox('getValue');
                                    var kaiShu = getDataGridColumnValue(detailConfig.dom, detailConfig.optionConfig.editRow, ['上机开数']);
                                    if (n != '') {
                                        console.log('n' + n);
                                        value = value1 % n == 0 ? value1 / n : parseInt(value1 / n) + 1;
                                        if (kaiShu != 0) {
                                            value = value % kaiShu == 0 ? value / kaiShu : parseInt(value / kaiShu) + 1;
                                        }
                                    } else {
                                        if (kaiShu != 0) {
                                            value = value1 % kaiShu == 0 ? value1 / kaiShu : parseInt(value1 / kaiShu) + 1;
                                        }
                                    }
                                    var element = $(detailConfig.dom).datagrid('getEditor', { index: detailConfig.optionConfig.editRow, field: '正用数量' });
                                    $(element.target).numberbox('setValue', value)
                                }
                            }
                        }
                    },
                    {
                        field: '贴数',
                        title: '模数',
                        align: 'center',
                        width: 70,
                        editor: {
                            type: 'numberbox',
                            options: {
                                precision: 0,
                            }
                        }
                    },
                    {
                        field: '上机开数',
                        title: '开数',
                        align: 'center',
                        width: 100,
                        editor: {
                            type: 'combobox',
                            options: {
                                url: '/ERP_Order/GetOpenNumber',
                                textField: 'Key',
                                valueField: 'Id',
                                panelWidth: 100,
                                panelHeight: 100,
                                onChange: function (n, o) {
                                    var value = 0;
                                    var value1 = $('#quotation_number').numberbox('getValue');
                                    var pinShu = getDataGridColumnValue(detailConfig.dom, detailConfig.optionConfig.editRow, ['拼数']);
                                    if (n != '') {
                                        value = value1 % n == 0 ? value1 / n : parseInt(value1 / n) + 1;
                                        if (pinShu != 0) {

                                            value = value % pinShu == 0 ? value / pinShu : parseInt(value / pinShu) + 1;
                                        }
                                    } else {
                                        if (pinShu != 0) {

                                            value = value1 % pinShu == 0 ? value1 / pinShu : parseInt(value1 / pinShu) + 1;
                                        }
                                    }
                                    var element = $(detailConfig.dom).datagrid('getEditor', { index: detailConfig.optionConfig.editRow, field: '正用数量' });
                                    $(element.target).numberbox('setValue', value);
                                },
                                required: true

                            }

                        }
                    },
                    {
                        field: '开料尺寸',
                        title: '开料尺寸',
                        align: 'center',
                        width: 85,
                        editor: {
                            type: 'textbox',
                            options: {
                                required: true,
                                delay: 300,
                                validType: 'chengfa'
                            }
                        }
                    },
                    {
                        field: '正用数量', title: '正用数', align: 'center', width: 80,
                        editor:
                        {
                            type: 'numberbox',
                            options: {
                                precision: 0,
                                onChange: function (n, o) {
                                    var number = getDataGridColumnValue(detailConfig.dom, detailConfig.optionConfig.editRow, ['印刷加放', '后道加放']);

                                    number += parseFloat(n);

                                    element = $(detailConfig.dom).datagrid('getEditor', { index: detailConfig.optionConfig.editRow, field: '合计张' });

                                    $(element.target).numberbox('setValue', number)
                                },
                             
                                required: true,
                                readonly: true
                            }
                        }
                    },
                    {
                        field: '印刷加放',
                        title: '印品加数',
                        align: 'center',
                        width: 85,
                        editor:
                        {
                            type: 'numberbox',
                            options: {
                                precision: 2,
                                required: true,
                                onChange: function (n, o) {
                                    var number = getDataGridColumnValue(detailConfig.dom, detailConfig.optionConfig.editRow, ['正用数量', '后道加放']);
                                    var element = $(detailConfig.dom).datagrid('getEditor', { index: detailConfig.optionConfig.editRow, field: '合计张' });
                                    $(element.target).numberbox('setValue', number + parseFloat(n))
                                }
                            }
                        }
                    },
                    {
                        field: '后道加放',
                        title: '后道加数',
                        algin: 'center',
                        width: 80,
                        editor: {
                            type: 'numberbox',
                            options: {
                                precision: 2,
                                required: true,
                                onChange: function (n, o) {
                                    var number = getDataGridColumnValue(detailConfig.dom, detailConfig.optionConfig.editRow,['正用数量', '印刷加放'])
                                    var element = $(detailConfig.dom).datagrid('getEditor', { index: detailConfig.optionConfig.editRow, field: '合计张' });
                                    $(element.target).numberbox('setValue', number + parseFloat(n))
                                }
                            }
                        }
                    },
                    {
                        field: '合计张',
                        title: '合计数',
                        align: 'center',
                        width: 100,
                        editor: {
                            type: 'numberbox',
                            options: {
                                precision: 0,
                                readonly: true
                            }
                        }
                    }
                ]],

                optionConfig: {
                    dom: {
                        add: '#quotation_detail_add',
                        edit: '#quotation_detail_edit',
                        save: '#quotation_detail_save',
                        remove: '#quotation_detail_remove',
                        revort: '#quotation_detail_revort',
                    },
                    datagrid: '#quotation_detail',
                    datagridList: [
                        { dom: '#quotation_price', url: '/ERP_Quotation/ShowPrice' },
                        { dom: '#quotation_material', url: '/ERP_Quotation/ShowDetail' }
                    ],
                    removeUrl: '/ERP_Quotation/DeleteQuotationDetail',
                    savebutton: '#quotation_detail_save',
                    paramNo: '#quotation_orderNo',
                    editRow: undefined
                }
            };
            datagridservice.datagrid(detailConfig);
            datagridservice.datagridOption(detailConfig.optionConfig);

            $('#quotation_importProcess').click(function () {
                //todo:审核没写
                var rows = $(detailConfig.dom).datagrid('getSelections');
                if (rows.length != 1) {
                    $.messager.alert('提示', '请选中一行导入', 'info');
                    return;
                }
                var index = $(detailConfig.dom).datagrid('getRowIndex', rows[0]);
                if (index == detailConfig.optionConfig.editRow) {
                    $.messager.alert('提示', '该行处于编辑中', 'info');
                    return;
                }
                $('#quotation_import_menu ul').empty();
                $('#quotation_import_content').empty();
                datagridservice.ajaxOperation('/ERP_Order/ProcessCategory', null, function (response) {
                    var context = '';
                    for (var i in response) {
                        var child = response[i];
                        var title = child[0].工艺类别;
                        $('#quotation_import_menu ul').append('<li id="qpMenu_' + i + '" flag="' + i + '"><a href="#" onclick="clickMe(this)">' + title + '</a></li>');
                        for (var j in child) {
                            context += '<span><input type="checkbox" value="' + child[j].项目编码 + '" flag="' + child[j].计数标识 + '"/>' + child[j].项目名称 + '</span>'
                        }
                        $('#quotation_import_content').append('<div id="qpContext_' + i + '" hidden="hidden">' + context + '</div>');
                        context = '';
                    }
                    $('#qpContext_0').show();
                })
                $('#quotation_import_dialog').dialog('open').dialog('setTitle', '导入工艺');
                $('#quotation_import_go').attr('flag', 'process');
            });
            $('#quotation_importMaterial').click(function () {
                //todo:审核没写
                var rows = $(detailConfig.dom).datagrid('getSelections');
                if (rows.length != 1) {
                    $.messager.alert('提示', '请选中一行导入材料', 'info');
                    return;
                }
                var index = $(detailConfig.dom).datagrid('getRowIndex', rows[0]);
                if (index == detailConfig.optionConfig.editRow) {
                    $.messager.alert('提示', '该行处于编辑中', 'info');
                    return;
                }
                $('#quotation_import_menu ul').empty();
                $('#quotation_import_content').empty();
                datagridservice.ajaxOperation('/ERP_Order/ShowMaterial', null, function (response) {
                    var context = '';
                    for (var i in response) {
                        var child = response[i];
                        var group = child[0].材料类别名称;

                        $('#quotation_import_menu ul').append('<li id="qmMenu_"' + i + ' flag="' + i + '"><a href="#" onclick="clickMe(this)">' + group + '</a></li>');
                        for (var j in child) {
                            context += '<span><input type="checkbox" value="' + child[j].材料编码 + '" category="' + child[j].材料类别编码+'" unit="'+child[j].计量单位+'"/>' + child[j].材料名称 + '</span>';
                        };
                        $('#quotation_import_content').append('<div id="qmContext_' + i + '" hidden="hidden">' + context + '</div>');
                        context = '';
                    }
                    // $('#pMenu_0').children('a').addClass('addclass');
                    $('#qmContext_0').show();
                })
                $('#quotation_import_dialog').dialog('open').dialog('setTitle', '导入材料');
                $('#quotation_import_go').attr('flag', 'material');
            });
            $('#quotation_import_go').unbind('click').click(function () {
                var flag = $(this).attr('flag');
                var $checked = $('#quotation_import_content :checkbox:checked');
                if ($checked.length <= 0) {
                    $.messager.alert('提示', '请选中要导入的数据', 'info');
                    return;
                }
                var row = $(detailConfig.dom).datagrid('getSelected');
                console.log ('报价detail号'+row.行号)
                var arr = [];
                if (flag == 'process') {
                    $checked.each(function (i, o) {
                        var obj = {};
                        obj.订单号 = row.订单号;
                        obj.报价detail号 = row.行号;
                        obj.印品部件 = row.印品部件;
                        obj.项目编码 = $(o).val();
                      //  obj.计量单位 = $(o).attr('unit');
                       // obj.工艺类别 = $(o).attr('processType');
                        var flag = $(o).attr('flag');
                        obj.数量 = ComputeAmount(flag);
                        arr.push(obj);
                    })
                    datagridservice.ajaxOperation('/ERP_Quotation/ImportProcess', { data: arr }, function (response) {
                        if (response.success == 0) {
                            $checked.each(function (i, o) {
                                $(o).prop('checked', false);
                            })
                            datagridservice.showMsg(response.msg);
                            $(quotation_priceConfig.dom).datagrid('options').url = '/ERP_Quotation/ShowPrice';
                            $(quotation_priceConfig.dom).datagrid('reload', { paramNo: row.订单号 });
                        } else {
                            $.messager.alert('提示', response.msg, 'info');
                        }
                    })
                } else if (flag == 'material') {
                    $checked.each(function (i, o) {
                        var obj = {};
                        obj.订单号 = row.订单号;
                        obj.报价detail号 = row.行号;
                        obj.印品部件 = row.印品部件;
                        obj.材料编码 = $(o).val();
                        obj.材料类别编码 = $(o).attr('category');
                        obj.计量单位 = $(o).attr('unit');
                        arr.push(obj);
                    });
                    datagridservice.ajaxOperation('/ERP_Quotation/ImportMaterial', { data: arr }, function (response) {
                        if (response.success == 0) {
                            $checked.each(function (i, o) {
                                $(o).prop('checked', false);
                            })
                            datagridservice.showMsg(response.msg);
                            $(quotation_materialConfig.dom).datagrid('options').url = '/ERP_Quotation/ShowMaterial';
                            $(quotation_materialConfig.dom).datagrid('reload', { paramNo: row.订单号 });
                            $(quotation_priceConfig.dom).datagrid('options').url = '/ERP_Quotation/ShowPrice';
                            $(quotation_priceConfig.dom).datagrid('reload', { paramNo: row.订单号 });
                        } else {
                            $.messager.alert('提示', response.msg, 'info');
                        }
                    })
                }

            });
            $('#quotation_import_cancel').click(function () {
                console.log('import_cancel')
                var $checked = $('#quotation_import_content :checkbox:checked');
                $checked.each(function (i, o) {
                    $(o).prop('checked', false);
                })
            });
            $('#quotation_import').click(function () {
                datagridservice.ajaxOperation('/ERP_Quotation/ImportJijia', { paramNo: $('#quotation_orderNo').val() }, function (response) {
                    if (response.success == 0) {
                        $(quotation_priceConfig.dom).datagrid('reload');
                    }
                })
            });
            $('#quotation_makingbill').click(function () {

                var orderNo = $('#quotation_orderNo').val();
                // if (orderNo == null || orderNo == '') { return; };
                var dialogConfig = { dom: '#quotation_Making_bill', title: '报价单', width: 900, height: 600, href: '/ERP_Quotation/MackingBill?paramNo=' + orderNo };
                toolbar.initdialog(dialogConfig);

            })

            var quotation_materialConfig = {
                dom: '#quotation_material',
                optionUrl: '/ERP_Quotation/SaveOrUpdateQuotationMaterial',
                flag: '#quotation_material_save',
                columns: [[
                    { field: 'ck', checkbox: true },
                    { field: '行号', title: '行号', hidden: true },
                    { field: '报价detail号', title: '报价detail号', hidden: true },
                    { field: '订单号', title: '订单号', hidden: true },
                    {
                        field: '材料编码',
                        title: '材料名称',
                        align: 'center',
                        width: 150,
                        formatter: function (i, d) {
                            return '<span>' + d.材料名称 + '</span>'
                        },
                        editor: {
                            type: 'combobox',
                            options: {
                                url: '/ERP_Order/GetMaterial',
                                textField: 'Key',
                                valueField: 'Id',
                                required: true,
                                // editable: false,
                                panelWidth: 150,
                                panelHeight: 120,
                                filter: function (q, r) {
                                    var opts = $(this).combobox('options');
                                    return r[opts.textField].indexOf(q) >= 0;
                                },
                                onSelect: function (record) {
                                    var element = $(quotation_materialConfig.dom).datagrid('getEditor', { index: quotation_materialConfig.optionConfig.editRow, field: '规格型号' });
                                    $(element.target).textbox('setValue', record.spec);
                                }
                            }
                        }
                    },
                    {
                        field: '规格型号',
                        title: '规格型号',
                        align: 'center',
                        width: 100,
                        editor: {
                            type: 'textbox',
                            readonly:true
                        }
                    },
                    {
                        field: '计量单位',
                        title: '计量单位',
                        align: 'center',
                        width: 90,
                        editor: {
                            type: 'combobox',
                            options: {
                                url: '/ERP_Quotation/GetUnit',
                                textField: 'Key',
                                valueField: 'Id',
                                panelHeight: 120,
                                panelWidth: 100,
                                required: true
                            }
                        }
                    },
                    {
                        field: '数量',
                        title: '数量',
                        align: 'center',
                        width: 70,
                        editor: {
                            type: 'numberbox',
                            options: {
                                precision: 2,
                                required: true
                            }
                        }
                    }
                    ,
                    {
                        field: '单价',
                        title: '单价',
                        align: 'center',
                        width: 70,
                        editor: {
                            type: 'numberbox',
                            options: {
                                precision: 2,
                                required: true
                            }
                        }
                    }
                ]],
                menu: '#quotation_material_menu',
                optionConfig: {
                    dom: {
                        add: '#quotation_material_add',
                        edit: '#quotation_material_edit',
                        save: '#quotation_material_save',
                        remove: '#quotation_material_remove',
                        revort: '#quotation_material_revort',
                    },
                    datagrid: '#quotation_material',
                    datagridList: [
                        { dom: '#quotation_price', url: '/ERP_Quotation/ShowPrice' }
                    ],
                    removeUrl: '/ERP_Quotation/DeleteQuotationMaterial',
                    savebutton: '#quotation_material_save',
                    paramNo: '#quotation_orderNo',
                    editRow: undefined
                }
            }
            datagridservice.datagrid(quotation_materialConfig);
            datagridservice.datagridOption(quotation_materialConfig.optionConfig);

            var quotation_priceConfig = {
                dom: '#quotation_price',
                optionUrl: '/ERP_Quotation/SaveOrUpdateQuotationPrice',
                flag: '#quotation_price_save',
                menu: '#quotation_price_menu',
                merge: ['统计编码'],
                refresh:true,
                columns: [[
                    { field: 'ck', checkbox: true },
                    { field: 'ID', title: 'ID', hidden: true },
                    { field: '订单号', title: '订单号', hidden: true },
                    { field: '订单Detail号', title: '订单Detail号', hidden: true },
                    { field: '订单工艺号', title: '订单工艺号', hidden: true },
                    { field: '订单材料号', title: '订单材料号', hidden: true },
                    {
                        field: '标准单价',
                        title: '标准单价', hidden: true,
                        editor:
                        {
                            type: 'numberbox',
                            options: { precision: 2 }
                        }
                    },
                    {
                        field: '统计编码',
                        title: '类别',
                        align: 'center',
                        width: 100,
                        editor: {
                            type: 'textbox',
                            options: {
                                readonly: true
                            }
                        }
                    },
                    {
                        field: '印品部件',
                        title: '印品部件',
                        align: 'center',
                        width: 100,
                        editor: {
                            type: "combobox",
                            options: {
                                url: '/ERP_Order/GetParts',
                                textField: 'Key',
                                valueField: 'Id',
                                panelHeight: 150,
                                required: true,
                                filter: function (q, r) {
                                    var opts = $(this).combobox('options');
                                    return r[opts.textField].indexOf(q) >= 0;
                                },
                            }
                        }
                    },
                    {
                        field: '项目编码',
                        title: '项目名称',
                        align: 'center',
                        width: 100,
                        formatter: function (v, r, i) {
                            return r.项目名称;
                        },
                        editor: {
                            type: 'combobox',
                            options: {
                                url: '/ERP_Order/GetProcess',
                                textField: 'Key',
                                valueField: 'Id',
                                panelWidth: 150,
                                panelHeight: 150,
                                required: true,
                                onSelect: function (record) {
                                    var element = $(quotation_priceConfig.dom).datagrid('getEditor', { index: quotation_priceConfig.optionConfig.editRow, field: '统计编码' });
                                    $(element.target).textbox('setValue', record.Flag);
                                },
                                filter: function (q, r) {
                                    var opts = $(this).combobox('options');
                                    return r[opts.textField].indexOf(q) >= 0;
                                },
                            }

                        }
                    },
                    {
                        field: '计量单位',
                        title: '单位',
                        align: 'center',
                        width: 100,
                        editor: {
                            type: 'combobox',
                            options: {
                                url: '/ERP_Order/GetUnit',
                                textField: 'Key',
                                valueField: 'Id',
                                panelHeight: 150,
                            }
                        }
                    },
                    {
                        field: '单价',
                        title: '单价',
                        align: 'center',
                        width: 100,
                        formatter: function (v, r, i) {
                            if (r.单价 < r.标准单价) {
                                return '<span style="color:red;">' + (v == null ? 0 : v) + '</sapn>'
                            } else {
                                return v
                            }
                        },
                        editor: {
                            type: 'numberbox',
                            options: {
                                required: true,
                                precision: 2,
                                onChange: function (n, o) {
                                    var element = $(quotation_priceConfig.dom).datagrid('getEditor', { index: quotation_priceConfig.optionConfig.editRow, field: '数量' });
                                    var unit = $(element.target).numberbox('getValue');
                                    var total = n * unit;
                                    element = $(quotation_priceConfig.dom).datagrid('getEditor', { index: quotation_priceConfig.optionConfig.editRow, field: '金额' });
                                    $(element.target).numberbox('setValue', total);
                                    element = $(quotation_priceConfig.dom).datagrid('getEditor', { index: quotation_priceConfig.optionConfig.editRow, field: '标准单价' });
                                    var biaozhun = $(element.target).numberbox('getValue');
                                    //if (n < biaozhun) {
                                    //    console.log(n + '单价:')
                                    //    console.log('old' + o)
                                    //    console.log($(this));
                                    //    $(this).attr('style', 'color:red')
                                    //    //var editors = $('#order_jijia').datagrid('getEditors', jijiaEditRow);
                                    //    //for (var i in editors) {
                                    //    //    console.log('小于标准单价的加上css')
                                    //    //    var editor = editors[i]
                                    //    //    $(editor.target).css('color','red');
                                    //    //}
                                    //} else {
                                    //    console.log (n)
                                    //    console.log ('else excute ')
                                    //    $(this).attr('style', 'color:#000000')
                                    //}

                                }
                            }
                        }
                    },
                    {
                        field: '数量',
                        title: '数量',
                        align: 'center',
                        width: 100,
                        editor: {
                            type: 'numberbox',
                            options: {
                                required: true,
                                precision: 2,
                                onChange: function (n, o) {
                                    var element = $(quotation_priceConfig.dom).datagrid('getEditor', { index: quotation_priceConfig.optionConfig.editRow, field: '单价' });
                                    var price = $(element.target).numberbox('getValue');
                                    var total = n * price;
                                    element = $(quotation_priceConfig.dom).datagrid('getEditor', { index: quotation_priceConfig.optionConfig.editRow, field: '金额' });
                                    $(element.target).numberbox('setValue', total);
                                }

                            }
                        },
                        formatter: function (v, r, i) {
                            if (r.单价 < r.标准单价) {
                                return '<span style="color:red;">' + (v == null ? '' : v) + '</sapn>'
                            } else {
                                return v;
                            }
                        }
                    },
                    {
                        field: '系数',
                        title: '系数',
                        align: 'center',
                        width: 100,
                        editor: {
                            type: 'numberbox',
                            options: {
                                precision: 0
                            }
                        },
                        formatter: function (v, r, i) {
                            if (r.单价 < r.标准单价) {
                                return '<span style="color:red;">' + (v == null ? '' : v) + '</sapn>'
                            } else {
                                return v;
                            }
                        }
                    },
                    {
                        field: '金额',
                        title: '金额',
                        align: 'center',
                        width: 100,
                        editor: {
                            type: 'numberbox',
                            options: {
                                precision: 2
                            }
                        },
                        formatter: function (v, r, i) {
                            if (r.单价 < r.标准单价) {
                                return '<span style="color:red;">' + (v == null ? '' : v) + '</sapn>'
                            } else {
                                return v;
                            }
                        }
                    },
                ]],
                optionConfig: {
                    dom: {
                        add: '#quotation_price_add',
                        edit: '#quotation_price_edit',
                        save: '#quotation_price_save',
                        remove: '#quotation_price_remove',
                        revort: '#quotaion_price_revort',
                    },
                    datagrid: '#quotation_price',
                    removeUrl: '/ERP_Quotation/DeleteQuotationPrice',
                    savebutton: '#quotation_price_save',
                    paramNo: '#quotation_orderNo',
                    editRow: undefined
                }
            };
            datagridservice.datagrid(quotation_priceConfig);
            datagridservice.datagridOption(quotation_priceConfig.optionConfig);

            var runPageConfig = {
                before: '#quotation_pre',
                next: '#quotation_orderNo_next',
                total: '#quotation_total',
                page: '#quotation_page',
                runPageUrl:'/ERP_Quotation/RunPage',
                form: '#quotation_form',
                text: '#quotation_orderNo_text',
                no: '#quotation_orderNo',
                auditorPerson: 0,
                list: [
                    { datagrid: '#quotation_detail', url: '/ERP_Quotation/ShowDetail', param_No: '#quotation_orderNo' },
                    { datagrid: '#quotation_price', url: '/ERP_Quotation/ShowPrice', param_No: '#quotation_orderNo' },
                    { datagrid: '#quotation_material', url: '/ERP_Quotation/ShowMaterial', param_No: '#quotation_orderNo' }
                ],
                menuArr: [{ menu: '#quotation_material_menu', items: ['#quotation_material_edit', '#quotation_material_save', '#quotation_material_remove'] },
                    { menu: '#quotation_detail_menu', items: ['#quotation_importProcess', '#quotation_importMaterial', '#quotation_detail_add', '#quotation_detail_edit', '#quotation_detail_save', '#quotation_detail_remove'] },
                    { menu: '#quotation_price_menu', items: ['#quotation_price_edit', '#quotation_price_save', '#quotation_price_remove'] },
                ],
                linkbuttonArr: ['#quotation_edit', '#quotation_remove', '#quotation_save', '#quotation_auditor','#quotation_import']
            }
            toolbar.beforePage(runPageConfig);
            toolbar.nextPage(runPageConfig);

            var auditorConfig = {
                auditor: '#quotation_auditor',
                unAuditor: '#quotatin_unauditor',
                paramNo: '#quotation_orderNo',
                auditorNo: '#quotation_auditorPerson',
                auditorTime: '#quotation_auditorTime',
                auditorPerson:'#quotation_auditorPerson',
                form: '#quotation_form',
                auditorUrl: '/ERP_Quotation/Auditing',
                unAuditorUrl: '/ERP_Quotation/UnAuditing',
                menuArr: [{ menu: '#quotation_material_menu', items: ['#quotation_material_edit', '#quotation_material_save', '#quotation_material_remove'] },
                { menu: '#quotation_detail_menu', items: ['#quotation_importProcess', '#quotation_importMaterial', '#quotation_detail_add', '#quotation_detail_edit', '#quotation_detail_save', '#quotation_detail_remove'] },
                { menu: '#quotation_price_menu', items: ['#quotation_price_edit', '#quotation_price_save', '#quotation_price_remove'] }
                ],
                linkbuttonArr: ['#quotation_edit', '#quotation_remove', '#quotation_save', '#quotation_auditor', '#quotation_import']
            };
            toolbar.auditor(auditorConfig);
            toolbar.unAuditor(auditorConfig);
            clickMe = function (dom) {
                var i = $(dom).parent('li').attr('flag');
                $(dom).parent('li').addClass('addClass').siblings().removeClass('addClass');
                var flag = $('#quotation_import_go').attr('flag');
                if (flag == 'process') {
                    $('#qpContext_' + i).show().siblings().hide()
                } else if (flag == 'material') {
                    $('#qmContext_' + i).show().siblings().hide()
                }
            }
        }
    }
    /// 计算datagrid numberbox
    function getDataGridColumnValue(dom, index, arr) {
        var number = 0;
        if (arr.length > 0) {
            for (var i in arr) {
                var element = $(dom).datagrid('getEditor', { index: index, field: arr[i] });
                var value = $(element.target).numberbox('getValue');
                if (value == '' || value == null || value == undefined) {
                    continue;
                } else {
                    number += parseFloat(value)
                }
            }
        }
        return number;
    }

    //计算页面加工项目数量(固定的)
    function ComputeAmount(amount) {
        var result = ''
        switch (amount) {
            case '固定数':
                result = 1;
                break;
            case '版数':
                var row = $('#order_paper').datagrid('getSelected');
                result = (row.版数 == '' || row.版数 == null || row.版数 == undefined) ? 0 : parseFloat(row.版数)
                break;
            case '印刷数':
                var row = $('#order_paper').datagrid('getSelected');
                var zhen = (row.正用数量 == '' || row.正用数量 == null || row.正用数量 == undefined) ? 0 : parseFloat(row.正用数量);
                var hou = (row.后道加放 == '' || row.后道加放 == null || row.后道加放 == undefined) ? 0 : parseFloat(row.后道加放);
                result = zhen + hou;
                break;
            case '成品数':
                var ret = $('#orderNumber').numberbox('getValue');
                result = (ret == '' || ret == null || ret == undefined) ? 0 : parseFloat(ret);
                break;
            default:
                //本想写-1给与警告
                result = 0;
                break;
        }
        return result;
    }
})