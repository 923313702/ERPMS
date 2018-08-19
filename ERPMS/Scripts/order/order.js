define(['jquery', 'order/order.service','tool/toolbar.service','tool/initCombobox'], function ($, orderService,toolbar,initCombobox) {
    return {
        index: function () {
            $.extend($.fn.validatebox.defaults.rules, {
                testSize: {
                    validator: function (value) {
                        return /^\d+(\.*\d{0,2})([*]\d+(\.*\d{0,2})){1}$/i.test(value)
                    },
                    message: '格式不正确,正确格式如:1234*1234'
                },
                evenNumber: {
                    validator: function (value) {
                        return (value % 2 == 0);
                    },
                    message: '输入数字必须为偶数'
                },
               
            });
            /**
             oorder Master
             */
            var orderComboboxConfig = [
                { dom: '#order_customer', url: '/ERP_Order/GetCustormer' },
                { dom: '#order_sale', url: '/ERP_Order/GetStaff' },
                { dom: '#order_printCategory', url: '/ERP_Order/PCategory' },
                { dom: '#order_unit2', url: '/ERP_Order/GetUnit' }
            ];
            initCombobox.initCombobox(orderComboboxConfig);
            $('#order_customer').combobox({
                onSelect: function (record) {
                    var phone = record.Phone;
                    $('#order_c_phone').textbox('setValue', phone);
                }
            });  
            var optionConfig = {
                dom: {
                    add: '#order_add',
                    edit: '#order_edit',
                    remove: '#order_remove',
                    save: '#order_save',
                    refresh:'#order_refresh'
                },
                editUrl: '/ERP_Order/EditOrder',
                saveUrl: '/ERP_Order/AddOrder',
                removeUrl: '/ERP_Order/DeleteOrder',
                refreshUrl:'/ERP_Order/Refresh',
                no: '#order_orderNo',
                text: '#order_orderNo_text',
                form: '#order_form',
                datagridList: [
                    { datagrid: '#order_paper', url: '/ERP_Order/GetOrderDetail', param_No: '#order_orderNo' },
                    { datagrid: '#order_process', url: '/ERP_Order/GetOrderProcess', param_No: '#order_orderNo' },
                    { datagrid: '#order_material', url: '/ERP_Order/GetOrderMaterial', param_No: '#order_orderNo' }],
            };
            toolbar.added(optionConfig);
            toolbar.edited(optionConfig);
            toolbar.saved(optionConfig);
            toolbar.removed(optionConfig);
            toolbar.refresh(optionConfig);

            var auditorConfig = {
                menuArr: [{ menu: '#order_material_menu', items: ['#order_material_edit', '#order_material_save', '#order_material_remove'] },
                { menu: '#order_paper_menu', items: ['#order_importProcess', '#order_importMaterial', '#order_paper_add', '#order_paper_edit', '#order_paper_save', '#order_paper_remove'] },
                { menu: '#order_process_menu', items: ['#order_process_edit', '#order_process_save', '#order_process_remove'] },
                ],
                linkbuttonArr: ['#order_edit', '#order_remove', '#order_save', '#order_auditor'],
                flag: true
            };
            $('#order_auditor').click(function () {
                var orderNo = $('#order_orderNo').val();
                if (orderNo == null || orderNo == '') { return; };
                var auditorNo = $('#order_auditorNo').val();
                if (auditorNo != null && auditorNo != '') { return; }
                var data = $('#order_form').serialize();
                toolbar.ajaxOption('/ERP_Order/AuditorOrder', data, function (response) {
                    if (response.success == 0) {
                        if (response.plist != null && response.plist.length > 0) {
                            var list = response.plist;
                            var str = '';
                            $('#order_purchase_content').empty();
                            for (var i in list) {
                                str += '<span>' + list[i] + '</span>';
                            }
                            $('#order_purchase_content').append(str);
                            $('#order_purchase_dialog').dialog('open');
                          
                        }
                        auditorConfig.flag = true;
                        toolbar.controlMenuAndbutton(auditorConfig);
                        $('#order_auditorNo').val(response.auditorNo);
                        toolbar.showMsg(response.msg);
                    } else {
                        $.messager.alert('提示', response.msg, 'info');
                    }
                })
            });
            $('#order_unauditor').click(function () {
                var orderNo = $('#order_orderNo').val();
                if (orderNo == '' || orderNo == null || orderNo == undefined) { return;}
                var auditorNo = $('#order_auditorNo').val();
                if (auditorNo == null || auditorNo == '') { return; }
                var data = $('#order_form').serialize();
                toolbar.ajaxOption('/ERP_Order/UnAuditorOrder',data, function (response) {
                    if (response.success == 0) {
                        auditorConfig.flag = false;

                        toolbar.controlMenuAndbutton(auditorConfig);
                        toolbar.showMsg(response.msg);
                        $('#order_auditorNo').val('');
                    } else {
                        $.messager.alert('提示', response.msg, 'info');
                    }

                })

            })



            $('#order_logistics').click(function () {
                var dialogConfig = { dom: '#order_logistics_dialog', title: '物流申请单', width: 800, height: 555, href: '/ERP_OrderLogistics/Index' };
                toolbar.initdialog(dialogConfig);
            });
            $('#order_invoice').click(function () {
                var orderNo = $('#order_orderNo').val();
                //// if (orderNo == null || orderNo == '') { return; };
                var dialogConfig = { dom: '#order_invoice_dialog', title: '发货申请单', width: 800, height: 420, href: '/ERP_OrderInvoice/Index?orderNo=' + orderNo };
                toolbar.initdialog(dialogConfig);
            });
            $('#order_cailiao').click(function () {
                var orderNo = $('#order_orderNo').val();
                // if (orderNo == null || orderNo == '') { return; };
                var dialogConfig = { dom: '#order_material_dialog', title: '订单用料', width: 800, height: 420, href: '/ERP_OrderDetailMaterial/Index?orderNo=' + orderNo };
                toolbar.initdialog(dialogConfig);
            });
            $('#order_apply').click(function () {

                var orderNo = $('#order_orderNo').val();
                var orderName = $('#order_orderName').textbox('getValue');
               // if (orderNo == null || orderNo == '') { return; };
                var dialogConfig = { dom: '#order_apply_dialog', title: '材料申购单', width: 900, height: 420, href: '/ERP_OrderApplyPurchase/Index?orderNo=' + orderNo+'&orderName='+orderName };
                toolbar.initdialog(dialogConfig);
            });
            $('#order_valuation').click(function () {
                var orderNo = $('#order_orderNo').val();
                if (orderNo == null || orderNo == '') { return; };
                var dialogConfig = { dom: '#order_valuation_dialog', title: '业务计价单', width:910, height: 560, href: '/ERP_OrderPrice/Index2?orderNo=' + orderNo };
                toolbar.initdialog(dialogConfig);
            });
            $('#order_detail').click(function () {
                var orderNo = $('#order_orderNo').val();
                if (orderNo == null || orderNo == '') { return; };
                var dialogConfig = { dom: '#order_account_dialog', title: '业务承接单合板明细', width: 800, height: 420, href: '/ERP_OrderAccount/Index?orderNo=' + orderNo };
                toolbar.initdialog(dialogConfig);
            });
            $('#order_design').click(function () {
                var orderNo = $('#order_orderNo').val();
                var dialogConfig = { dom: '#order_design_dialog', title: '设计生产流程单', width: 900, height: 530, href: '/ERP_OrderDesign/Index?orderNo=' + orderNo };
                if (orderNo == null || orderNo == '') { return; };
                toolbar.ajaxOption('/ERP_OrderDesign/isOrder', { orderNo: orderNo }, function (response) {
                    if (response.success == -1) {
                        $.messager.confirm('提示', '是否申成设计流程单?', function (t) {
                            if (t) {
                                toolbar.ajaxOption('/ERP_OrderDesign/InitPageData', { orderNo: orderNo }, function (response) {
                                    if (response.success == 0) {
                                        toolbar.initdialog(dialogConfig);
                                    } else {
                                        $.messager.alert('提示', response.msg, 'info');
                                    }
                                })
                            }
                        })
                    } else {
                        toolbar.initdialog(dialogConfig);
                    }
                })
            
                
            });

         /**
          * 翻页配置
          */
            var runPageConfig = {
                before: '#order_pre',
                next: '#order_next',
                total: '#order_total',
                page: '#order_page',
                runPageUrl: '/ERP_Order/RunPage',
                form: '#order_form',
                text: '#order_orderNo_text',
                no:'#order_orderNo',
                auditorPerson: 0,
                list: [
                    { datagrid: '#order_paper', url: '/ERP_Order/GetOrderDetail', param_No: '#order_orderNo' },
                    { datagrid: '#order_process', url: '/ERP_Order/GetOrderProcess', param_No: '#order_orderNo' },
                    { datagrid: '#order_material', url: '/ERP_Order/GetOrderMaterial', param_No: '#order_orderNo' }
                ],
                menuArr: [{ menu: '#order_material_menu', items: ['#order_material_edit', '#order_material_save', '#order_material_remove'] },
                    { menu: '#order_paper_menu', items: ['#order_importProcess', '#order_importMaterial', '#order_paper_add', '#order_paper_edit', '#order_paper_save', '#order_paper_remove'] },
                    { menu: '#order_process_menu', items: ['#order_process_edit', '#order_process_save', '#order_process_remove'] },
                ],
                linkbuttonArr: ['#order_edit', '#order_remove','#order_save', '#order_auditor']
            }
            toolbar.beforePage(runPageConfig);
            toolbar.nextPage(runPageConfig)

         /**
           * 订单detail
           */
            var orderConfig = {
                dom: '#order_paper',
                optionUrl: '/ERP_Order/SaveOrUpdateOrderDetail',
                flag: '#order_paper_save',
                menu: '#order_paper_menu',
                columns: [[
                    { field: 'ck', checkbox: true },
                    { field: '行号', title: '行号', hidden: true },
                    { field: '订单号', title: '订单号', hidden: true },
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
                        field: '纸张客户编码',
                        title: '来源',
                        align: 'center',
                        width: 100,
                        editor: {
                            type: 'combobox',
                            options: {
                                url: '/ERP_Order/GetFactory?orderNo=' + $('#order_orderNo').val(),
                                method: 'post',
                                textField: 'Alias',
                                valueField: 'FactoryNo',
                                panelWidth: 230,
                                panelHeight: 100,
                                required: true,
                                formatter: function (row) {
                                    return '<span>' + row.FactoryNo + '&nbsp; &nbsp;' + row.FactoryName + '&nbsp; &nbsp;' + row.Alias + '</span>'
                                }
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
                        width: 70,
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
                                    var value1 = $('#orderNumber').numberbox('getValue');
                                    var kaiShu = getDataGridColumnValue('#order_paper', orderConfig.optionConfig.editRow, ['上机开数']);
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
                                    var element = $('#order_paper').datagrid('getEditor', { index: orderConfig.optionConfig.editRow, field: '正用数量' });
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
                        width: 70,
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
                                    var value1 = $('#orderNumber').numberbox('getValue');
                                    //var number = getDataGridColumnValue('#order_paper', orderConfig.optionConfig.editRow, ['印刷加放', '后道加放']);
                                    var pinShu = getDataGridColumnValue('#order_paper', orderConfig.optionConfig.editRow, ['拼数']);
                                    console.log('pinShu' + pinShu)
                                    console.log ('kaishu'+n)

                                    if (n != '') {
                                        console.log('n' + n);
                                        value = value1 % n == 0 ? value1 / n : parseInt(value1 / n) + 1;
                                        if (pinShu != 0) {
                                        
                                            value = value % pinShu == 0 ? value / pinShu : parseInt(value / pinShu) + 1;
                                        }
                                    } else {
                                        if (pinShu != 0) {
                                          
                                            value = value1 % pinShu == 0 ? value1 / pinShu : parseInt(value1 / pinShu) + 1;
                                        }
                                    }
                                    
                                 
                                   // number2= number2 % rec.Id == 0 ? number2 / rec.Id : parseInt(number2 / rec.Id) + 1;
                                    var element = $('#order_paper').datagrid('getEditor', { index: orderConfig.optionConfig.editRow, field: '正用数量' });
                                    $(element.target).numberbox('setValue', value)
                                    //element = $('#order_paper').datagrid('getEditor', { index: orderConfig.optionConfig.editRow, field: '分切数量' });
                                    //$(element.target).numberbox('setValue', value + number)
                                },
                                required: true

                            }

                        }
                    },
                    {
                        field: '机切尺寸',
                        title: '机切尺寸',
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
                                    var number = getDataGridColumnValue('#order_paper', orderConfig.optionConfig.editRow, ['印刷加放', '后道加放']);

                                    number += parseFloat(n);
                                  
                                    element = $('#order_paper').datagrid('getEditor', { index: orderConfig.optionConfig.editRow, field: '分切数量' });
                                   
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
                                    var number = getDataGridColumnValue('#order_paper', orderConfig.optionConfig.editRow, ['正用数量', '后道加放']);
                                    //var element = $('#order_paper').datagrid('getEditor', { index: orderConfig.optionConfig.editRow, field: '上机开数' });
                                    //var open = $(element.target).combobox('getValue');
                                    //number += parseFloat(n);
                                    //if (open == '' || open == null || open == undefined) {
                                    //}
                                    var element = $('#order_paper').datagrid('getEditor', { index: orderConfig.optionConfig.editRow, field: '分切数量' });
                                    //number = number % open == 0 ? number / open : parseInt(number / open) + 1;
                                    $(element.target).numberbox('setValue', number+parseFloat(n))
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
                                    var number = getDataGridColumnValue('#order_paper', orderConfig.optionConfig.editRow, ['正用数量', '印刷加放'])
                                    //var element = $('#order_paper').datagrid('getEditor', { index: orderConfig.optionConfig.editRow, field: '上机开数' });
                                    //var open = $(element.target).combobox('getValue');
                                    //number += parseFloat(n);
                                    //if (open == '' || open == null || open == undefined) {
                                    //}
                                   var  element = $('#order_paper').datagrid('getEditor', { index: orderConfig.optionConfig.editRow, field: '分切数量' });
                                    //number = number % open == 0 ? number / open : parseInt(number / open) + 1;
                                   $(element.target).numberbox('setValue', number + parseFloat(n))
                                }
                            }
                        }
                    },
                    {
                        field: '分切数量',
                        title: '大纸数',
                        align: 'center',
                        width: 80,
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
                    dom:{
                        add: '#order_paper_add',
                        edit: '#order_paper_edit',
                        save: '#order_paper_save',
                        remove: '#order_paper_remove',
                        revort:'#order_paper_revort',
                    },
                    datagrid: '#order_paper',
                    datagridList: [
                        { dom: '#order_process', url: '/ERP_Order/GetOrderProcess' },
                        { dom: '#order_material', url: '/ERP_Order/GetOrderMaterial' }
                    ],
                    removeUrl: '/ERP_Order/DeleteOrderDetail',
                    savebutton: '#order_paper_save',
                    paramNo:'#order_orderNo',
                    editRow: undefined
                }
            };
            orderService.datagrid(orderConfig);
           
            orderService.datagridOption(orderConfig.optionConfig);
            //单独的不配置了(可以在封装)
            $('#order_importProcess').click(function () {
                //todo:审核没写
                var rows = $('#order_paper').datagrid('getSelections');
                if (rows.length != 1) {
                    $.messager.alert('提示', '请选中一行导入工艺', 'info');
                    return;
                }
                var index = $('#order_paper').datagrid('getRowIndex', rows[0]);
                if (index == orderConfig.optionConfig.editRow) {
                    $.messager.alert('提示', '该行处于编辑中', 'info');
                    return;
                }
                $('#import_menu ul').empty();
                $('#import_content').empty();
                orderService.ajaxOperation('/ERP_Order/ProcessCategory', null, function (response) {
                    var context = '';
                    for (var i in response) {
                        var child = response[i];
                        var title = child[0].工艺类别;
                        $('#import_menu ul').append('<li id="pMenu_' + i + '" flag="' + i + '"><a href="#" onclick="clickMe(this)">' + title + '</a></li>');
                        for (var j in child) {
                            context += '<span><input type="checkbox" value="' + child[j].项目编码 + '" flag="' + child[j].计数标识 + '"/>' + child[j].项目名称 + '</span>'
                        }
                        $('#import_content').append('<div id="pContext_' + i + '" hidden="hidden">' + context + '</div>');
                        context = '';
                    }
                    $('#pContext_0').show();
                })
                $('#import_dialog').dialog('open').dialog('setTitle', '导入工艺');
                $('#import_go').attr('flag', 'process');
            });
            $('#order_importMaterial').click(function () {
                //todo:审核没写
                var rows = $('#order_paper').datagrid('getSelections');
                if (rows.length != 1) {
                    $.messager.alert('提示', '请选中一行导入材料', 'info');
                    return;
                }
                var index = $('#order_paper').datagrid('getRowIndex', rows[0]);
                if (index == orderConfig.optionConfig.editRow) {
                    $.messager.alert('提示', '该行处于编辑中', 'info');
                    return;
                }
                $('#import_menu ul').empty();
                $('#import_content').empty();
                orderService.ajaxOperation('/ERP_Order/ShowMaterial', null, function (response) {
                    var context = '';
                    for (var i in response) {
                        var child = response[i];
                        var group = child[0].材料类别名称;
                        $('#import_menu ul').append('<li id="mMenu_"' + i + ' flag="' + i + '"><a href="#" onclick="clickMe(this)">' + group + '</a></li>');
                        for (var j in child) {
                            context += '<span><input type="checkbox" value="' + child[j].材料编码 + '" />' + child[j].材料名称 + '</span>';
                        };
                        $('#import_content').append('<div id="mContext_' + i + '" hidden="hidden">' + context + '</div>');
                        context = '';
                    }
                    // $('#pMenu_0').children('a').addClass('addclass');
                    $('#mContext_0').show();
                })
                $('#import_dialog').dialog('open').dialog('setTitle', '导入材料');
                $('#import_go').attr('flag', 'material');
            });
            $('#import_go').unbind('click').click(function () {
                var flag = $(this).attr('flag');
                var $checked = $('#import_content :checkbox:checked');
                if ($checked.length <= 0) {
                    $.messager.alert('提示', '请选中要导入的数据', 'info');
                    return;
                }
                var row = $('#order_paper').datagrid('getSelected');

                var arr = [];
                if (flag == 'process') {
                    $checked.each(function (i, o) {
                        var obj = {};
                        obj.订单号 = row.订单号;
                        obj.订单detail号 = row.行号;
                        obj.印品部件 = row.印品部件;
                        obj.项目编码 = $(o).val();
                        var flag = $(o).attr('flag');
                        obj.数量 = ComputeAmount(flag);
                        arr.push(obj);
                    })
                    orderService.ajaxOperation('/ERP_Order/ImportProcess', { data: arr }, function (response) {
                        if (response.success == 0) {
                            $checked.each(function (i, o) {
                                $(o).prop('checked', false);
                            })
                            orderService.showMsg(response.msg);
                            $('#order_process').datagrid('options').url = '/ERP_Order/GetOrderProcess';
                            $('#order_process').datagrid('reload', { paramNo: row.订单号 });
                        } else {
                            $.messager.alert('提示', response.msg, 'info');
                        }
                    })
                } else if (flag == 'material') {
                    $checked.each(function (i, o) {
                        var obj = {};
                        obj.订单号 = row.订单号;
                        obj.订单detail号 = row.行号;
                        obj.印品部件 = row.印品部件;
                        obj.材料编码 = $(o).val();
                        arr.push(obj);
                    });
                    orderService.ajaxOperation('/ERP_Order/ImportMaterial', { data: arr }, function (response) {
                        if (response.success == 0) {
                            $checked.each(function (i, o) {
                                $(o).prop('checked', false);
                            })
                            orderService.showMsg(response.msg);
                            $('#order_material').datagrid('options').url = '/ERP_Order/GetOrderMaterial';
                            $('#order_material').datagrid('reload', { paramNo: row.订单号 });
                        } else {
                            $.messager.alert('提示', response.msg, 'info');
                        }
                    })
                }

            });
            $('#import_cancel').click(function () {
                console.log ('import_cancel')
                var $checked = $('#import_content :checkbox:checked');
                $checked.each(function (i, o) {
                    $(o).prop('checked', false);
                })
            })
            /**
             * 订单detail 工艺
             */
            var order_processConfig = {
                dom: '#order_process',
                optionUrl: '/ERP_Order/SaveOrUpdateOrderProcess',
                flag: '#order_process_save',
                menu: '#order_process_menu',
                columns: [[
                    { field: 'ck', checkbox: true },
                    { field: '行号', title: '行号', hidden: true },
                    {field:'订单detail号',title:'订单detail号',hidden:true},
                    { field: '订单号', title: '订单号', hidden: true },
                    {
                        field: '印品部件',
                        title: '部件',
                        align: 'center',
                        width: 100,
                        //editor: {
                        //    type: 'combobox',
                        //    options: {
                        //        url: '/Order/GetParts',
                        //        textField: 'Key',
                        //        valueField: 'Id',
                        //        required: true
                        //    }
                        //}
                    },
                    {
                        field: '项目编码',
                        title: '工艺',
                        align: 'center',
                        width: 100,
                        formatter: function (i, d) {
                            return '<span>' + d.项目名称 + '</span>';
                        },
                        editor: {
                            type: 'combobox',
                            options: {
                                url: '/ERP_Order/GetProcess',
                                textField: 'Key',
                                valueField: 'Id',
                                required: true,
                                editable: false,
                                filter: function (q, r) {
                                    var opts = $(this).combobox('options');
                                    return r[opts.textField].indexOf(q) >= 0;
                                },
                                //onLoadSuccess: function () {

                                //    var element = $('#order_project').datagrid('getEditor', { index: processEditRow, field: '项目编码' });
                                //    var value = $(element.target).combobox('getText');
                                //    var data = $(element.target).combobox('getData');
                                //    if (value != null && value != '') {
                                //        var id = Search(data, value);
                                //        $(element.target).combobox('setValue', id);
                                //    }

                                //},
                                //onSelect: function (record) {
                                //    var text = $(this).combobox('getText');
                                //    var row = $('#order_project').datagrid('getSelected');
                                //    if (row != null) {
                                //        row.项目名称 = text;
                                //    }
                                //}
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
                                precision: 2,
                                required: true
                            }
                        }
                    },
                ]],
                optionConfig: {
                    dom: {
                        add: '#order_process_add',
                        edit: '#order_process_edit',
                        save: '#order_process_save',
                        remove: '#order_process_remove',
                        revort: '#order_process_revort',
                    },
                    datagrid: '#order_process',
                    removeUrl: '/ERP_Order/DeleteOrderProcess',
                    savebutton: '#order_process_save',
                    paramNo: '#order_orderNo',
                    editRow: undefined
                }
            }
            orderService.datagrid(order_processConfig);
           
            orderService.datagridOption(order_processConfig.optionConfig);
            /**
             *订单detail 材料
             */
            var order_materialConfig = {
                dom: '#order_material',
                optionUrl: '/ERP_Order/SaveOrUpdateOrderMaterial',
                flag: '#order_material_save',
                columns: [[
                    { field: 'ck', checkbox: true },
                    { field: '行号', title: '行号', hidden: true },
                    {field:'订单detail号',title:'订单detail号',hidden:true},
                    { field: '订单号', title: '订单号', hidden: true },
                    {
                        field: '材料来源',
                        title: '材料来源',
                        align: 'center',
                        width: 100,
                        editor: {
                            type: 'combobox',
                            options: {
                                //url: '',
                                textField: 'Key',
                                valueField: 'Id',
                                panelHeight: 120,
                                data: [{ 'Key': '本厂', "Id": '本厂' }, { 'Key': '外厂', 'Id': '外厂' }],
                                required: true
                            }
                        }
                    },
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
                                panelHeight:120,
                                filter: function (q, r) {
                                    var opts = $(this).combobox('options');
                                    return r[opts.textField].indexOf(q) >= 0;
                                },
                                //onSelect: function (record) {
                                //    var text = $(this).combobox('getText');
                                //    var row = $('#order_material').datagrid('getSelected');
                                //    if (row != null) {
                                //        row.材料名称 = text;
                                //    }
                                //}
                            }
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
                                url: '/ERP_Order/GetUnit',
                                textField: 'Key',
                                valueField: 'Id',
                                //panelHeight: 120,
                                //panelWidth: 100,
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
                    },
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
                menu: '#order_material_menu',
                optionConfig: {
                    dom: {
                        add: '#order_material_add',
                        edit: '#order_material_edit',
                        save: '#order_material_save',
                        remove: '#order_material_remove',
                        revort: '#order_material_revort',
                    },
                    datagrid: '#order_material',
                    removeUrl: '/ERP_Order/DeleteOrderMaterial',
                    savebutton: '#order_material_save',
                    paramNo: '#order_orderNo',
                    editRow: undefined
                }
            }
            orderService.datagrid(order_materialConfig);
            orderService.datagridOption(order_materialConfig.optionConfig);

            clickMe = function (dom) {
                var i = $(dom).parent('li').attr('flag');
                $(dom).parent('li').addClass('addClass').siblings().removeClass('addClass');
                var flag = $('#import_go').attr('flag');
                if (flag == 'process') {
                    $('#pContext_' + i).show().siblings().hide()
                } else if (flag == 'material') {
                    $('#mContext_' + i).show().siblings().hide()
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
    function reloadDatagrid(dom,url, data) {
        $(dom).datagrid('options').url = url;
        $(dom).datagrid('reload', {orderNo:data})
    }
});