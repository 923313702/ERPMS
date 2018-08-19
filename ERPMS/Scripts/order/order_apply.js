define(['jquery', 'order/datagridOption.server', 'tool/initCombobox', 'tool/toolbar.service'], function ($, server, initcombobox, toolbar) {

    return {
        index: function () {

            var comboboxConfig = [
                { dom: '#orderApply_zhidanren', url: '/ERP_Order/GetStaff' },
                { dom: '#orderApply_dept', url: '/ERP_OrderApplyPurchase/GetDepartment' },
                { dom: '#orderApply_ticket', url: '/ERP_OrderApplyPurchase/GetBill' },
                { dom: '#orderApply_applyPerson', url: '' },
                { dom: '#orderApply_leading', url: '' },
                { dom: '#orderApply_auditorPerson', url: '' }];
            initcombobox.initCombobox(comboboxConfig);
            $('#orderApply_zhidanren').combobox({
                onLoadSuccess: function () {
                    var data = $(this).combobox('getData');
                    $('#orderApply_applyPerson').combobox('loadData', data);
                    $('#orderApply_leading').combobox('loadData', data)
                    $('#orderApply_auditorPerson').combobox('loadData', data)
                }
            });
            var datagridConfig = {
                dom: '#orderApply_datagrid',
                optionUrl: '/ERP_OrderApplyPurchase/SaveOrUpdateDetail',
                flag: '#orderApply_menu_save',
                columns: [[
                    { field: 'ck', checkbox: true },
                    { field: '序号', title: '序号', hidden: true },
                    { field: '客户编码', title: '客户编码', hidden: true },
                    { field: '申请单号', title: '申请单号', hidden: true },
                    {
                        field: 'Flag', title: '物料类别', align: 'center', width: 100,
                        formatter: function (value, row, index) {
                            if (value == 1) {
                                return '纸张'
                            } else if (value == 2) {
                                return '材料'
                            } else {
                                return ''
                            }
                        },
                        editor: {
                            type: 'textbox',
                            options: {
                                readonly: true
                            }
                        }
                    },
                    {
                        field: '材料编码', title: '材料编码', align: 'center', width: 120,
                        editor: {
                            type: 'combobox',
                            options: {
                                url: '',
                                textField: 'Key',
                                valueField: 'Id',
                                panelWidth: 150,
                                panelHeight: 120,
                                onSelect: function (rec) {
                                    var element = $(datagridConfig.dom).datagrid('getEditor', { index: datagridConfig.optionConfig.editRow, field: '材料名称' });
                                    $(element.target).textbox('setValue', rec.Key);
                                    var element = $(datagridConfig.dom).datagrid('getEditor', { index: datagridConfig.optionConfig.editRow, field: '规格' });
                                    $(element.target).textbox('setValue', rec.Model);
                                    element = $(datagridConfig.dom).datagrid('getEditor', { index: datagridConfig.optionConfig.editRow, field: 'Flag' });
                                    var flag = $(element.target).textbox('getValue');
                                    if (flag == 1) {
                                        var customerNo = $('#pur_customerNo').val();
                                        if (customerNo != null && customerNo != '' && customerNo != undefined) {
                                            GetStock('/ERP_OrderApplyPurchase/GetTotalPaper', { paperNo: rec.Id, customerNo: customerNo }, datagridConfig.dom, datagridConfig.optionConfig.editRow, 'V_库存数量')
                                        }
                                    } else if (flag == 2) {
                                        GetStock('/ERP_OrderApplyPurchase/GetTotalMaterial', { materialNo: rec.Id }, datagridConfig.dom, datagridConfig.optionConfig.editRow, 'V_库存数量')
                                    }
                                },
                                required: true

                            }
                        }
                    },
                    {
                        field: '材料名称',
                        title: '材料名称',
                        align: 'center',
                        width: 120,
                        editor: {
                            type: 'textbox',
                            options: {
                                required: true,
                                readonly: true
                            }
                        }

                    },
                    {
                        field: '规格',
                        title: '规格型号',
                        align: 'center',
                        width: 80,
                        editor: {
                            type: 'textbox',
                        }
                    },
                    {
                        field: '订单号',
                        title: '订单号',
                        align: 'center',
                        width: 120,
                        editor: {
                            type: 'combobox',
                            options: {
                                url: '/ERP_OrderApplyPurchase/GetOrder',
                                textField: 'Key',
                                valueField: 'Id',
                                panelWidth: 150,
                                panelHeight: 120,
                                // required: true,
                                onSelect: function (rec) {
                                    var element = $(datagridConfig.dom).datagrid('getEditor', { index: datagridConfig.optionConfig.editRow, field: '材料编码' });
                                    var number = $(element.target).combobox('getValue');
                                    element = $(datagridConfig.dom).datagrid('getEditor', { index: datagridConfig.optionConfig.editRow, field: 'Flag' });
                                    var flag = $(element.target).textbox('getValue');
                                    $('#pur_customerNo').val(rec.cNo);
                                        if (flag == 1) {
                                            if (rec.cNo != null && rec.cNo != '') {
                                                GetStock('/ERP_OrderApplyPurchase/GetTotalPaper', { paperNo: number, customerNo: rec.cNo }, datagridConfig.dom, datagridConfig.optionConfig.editRow, 'V_库存数量')
                                            }
                                        } else if (flag == 2) {
                                            GetStock('/ERP_OrderApplyPurchase/GetTotalMaterial', { materialNo: number }, datagridConfig.dom, datagridConfig.optionConfig.editRow, 'V_库存数量')
                                        }
                                }

                            }
                        }
                    },
                    {
                        field: '计量单位',
                        title: '计量单位',
                        align: 'center',
                        width: 80,
                        editor: {
                            type: 'combobox',
                            options: {
                                url: '/ERP_OrderApplyPurchase/GetUnit',
                                textField: 'Key',
                                valueField: 'Id',
                                panelWidth: 150,
                                panelHeight: 120,
                            }
                        }
                    },

                    {
                        field: '数量',
                        title: '数量',
                        align: 'center',
                        width: 80,

                        editor: {
                            type: 'numberbox',
                            options: {
                                precision: 2,

                            }
                        }
                    },
                    {
                        field: 'V_库存数量',
                        title: "库存数量",
                        align: 'center',
                        width: 80,
                        editor: {
                            type: 'numberbox',
                            options: {
                                precision: 2,
                                readonly: true,
                            }
                        }
                    },
                    
                    {
                        field: '备注', title: '物料说明', align: 'center', width: 120,
                        editor: {
                            type: 'textbox',
                        }
                    }
                ]],
                menu: '#orderApply_menu',
                optionConfig: {
                    dom: {
                        add: '#orderApply_menu_add',
                        edit: '#orderApply_menu_edit',
                        save: '#orderApply_menu_save',
                        remove: '#orderApply_menu_remove',
                        revort: '#orderApply_menu_revort',
                    },
                    datagrid: '#orderApply_datagrid',
                    removeUrl: '/ERP_OrderApplyPurchase/DeleteSaleDetail',
                    savebutton: '#orderApply_menu_save',
                    paramNo: '#orderApply_applyNo',
                    //申请单有
                    type:'#orderApply_materialType',
                    editRow: undefined
                }
            };
            server.datagrid(datagridConfig);
            $(datagridConfig.dom).datagrid({
                onAfterEdit: function (i, d, c) {
                    var flags = $(datagridConfig.flag).attr('flag');
                    var val = $(datagridConfig.optionConfig.paramNo).val();
                    if (flags == 'add') {
                        if (val == null || val == '') {
                            $.messager.alert('提示', '没获取到申请单号', 'info');
                            $(datagridConfig.dom).datagrid('rejectChanges');
                            datagridConfig.optionConfig.editRow = undefined;
                            return;
                        }
                        d.申请单号 = val;
                    }
                    $.ajax({
                        url: datagridConfig.optionUrl,
                        type: 'post',
                        data: { data: d, flag: flags },
                        success: function (response) {
                            if (response.success == 0) {
                                datagridConfig.optionConfig.editRow = undefined;
                                $(datagridConfig.dom).datagrid('reload');
                                server.showMsg(response.msg);

                            } else {
                                $.messager.alert('提示', response.msg, 'info');
                                $(datagridConfig.dom).datagrid('beginEdit', i);
                            }
                        },
                        error: function (XMLHttpRequest, testStatus, errorThrown) {
                            $.messager.alert('My Title', XMLHttpRequest.status, 'warning')
                        }
                    })
                },
                onLoadSuccess: function () {
                    datagridConfig.optionConfig.editRow = undefined;
                    $(datagridConfig.dom).datagrid("autoMergeCells", ['Flag']);  
                }
            });

            var initPageConfig = {
                form: '#orderApply_form',
                text: '#orderApply_applyNotext',
                datagrid: '#orderApply_datagrid',
                initPageUrl: '/ERP_OrderApplyPurchase/Apply',
                menuArr: [{ menu: '#orderApply_menu', items: ['#orderApply_menu_add', '#orderApply_menu_edit', '#orderApply_menu_save', '#orderApply_menu_remove'] }],
                linkbuttonArr: ['#orderApply_edit', '#orderApply_remove','#orderApply_save', '#orderApply_auditor'],
                flag: true
            };
            InitPageData(initPageConfig);
            $(datagridConfig.optionConfig.dom.add).unbind('click').click(function () {

                var applyNo = $(datagridConfig.optionConfig.paramNo).val();
                if (applyNo == null || applyNo == '' || applyNo == undefined) {
                    $.messager.alert('提示', '没有获取到采购申请单', 'info');
                    return;
                }
                var text = $(datagridConfig.optionConfig.type).combobox('getText');
                if (text == '' || text == null || text == undefined) {
                    $.messager.alert('提示', '请选择要添加的物料类别', 'info');
                    return;
                }
                var value = $(datagridConfig.optionConfig.type).combobox('getValue');
                if (datagridConfig.optionConfig.editRow != undefined) {
                    $(datagridConfig.dom).datagrid('endEdit', datagridConfig.optionConfig.editRow)
                }
                if (datagridConfig.optionConfig.editRow == undefined) {
                    $(datagridConfig.dom).datagrid('insertRow', { index: 0, row: {} });
                    $(datagridConfig.dom).datagrid('beginEdit', 0);
                    datagridConfig.optionConfig.editRow  = 0;
                    var element = $(datagridConfig.dom).datagrid('getEditor', { index: datagridConfig.optionConfig.editRow, field: '材料编码' });
                    var element2 = $(datagridConfig.dom).datagrid('getEditor', { index: datagridConfig.optionConfig.editRow, field: 'Flag' });
                    if (value == 1) {
                        $(element.target).combobox('reload', '/ERP_OrderApplyPurchase/GetPaper');
                        $(element2.target).textbox('setValue', 1)
                    } else {
                        $(element.target).combobox('reload', '/ERP_OrderApplyPurchase/GetMaterial');
                        $(element2.target).textbox('setValue', 2);
                    }
                    $(datagridConfig.optionConfig.savebutton).attr('flag', 'add');
                }
            });
            //申请单dETAIL 修改
            $(datagridConfig.optionConfig.dom.edit).unbind('click').click(function () {
                //var Auditor = $('#pur_auditor').combobox('getValue');
                //if (Auditor != null && Auditor != '' && Auditor != undefined) {
                //    $.messager.alert('提示', '该单号已审核不允许任何操作', 'info')
                //    return;
                //}
                var rows = $(datagridConfig.dom).datagrid('getSelections');
                if (rows.length != 1) {
                    $.messager.alert('提示', '请选中一行', 'info');
                    return;
                }
                if (datagridConfig.optionConfig.editRow != undefined) {
                    $(datagridConfig.dom).datagrid('endEdit', datagridConfig.optionConfig.editRow);
                }
                var index = $(datagridConfig.dom).datagrid('getRowIndex', rows[0])
                if (datagridConfig.optionConfig.editRow == undefined) {
                    datagridConfig.optionConfig.editRow = index;
                    $(datagridConfig.dom).datagrid('beginEdit', index);
                    var flag = rows[0].Flag;
                    var element = $(datagridConfig.dom).datagrid('getEditor', { index: datagridConfig.optionConfig.editRow, field: '材料编码' })
                    if (flag == 1) {
                        $(element.target).combobox('reload', '/ERP_OrderApplyPurchase/GetPaper')
                    } else {
                        $(element.target).combobox('reload', '/ERP_OrderApplyPurchase/GetMaterial')
                    }
                    $(datagridConfig.optionConfig.savebutton).attr('flag', 'edit');
                }
            });
            //申请单Detail 删除
            $(datagridConfig.optionConfig.dom.remove).unbind('click').click(function () {
                //var Auditor = $('#pur_auditor').combobox('getValue');
                //if (Auditor != null && Auditor != '' && Auditor != undefined) {
                //    $.messager.alert('提示', '该单号已审核不允许任何操作', 'info')
                //    return;
                //}
                if (datagridConfig.optionConfig.editRow  != undefined) {
                    $.messager.alert('提示', '有未完成的编辑项', 'info');
                }
                var rows = $(datagridConfig.dom).datagrid('getSelections');
                if (rows.length < 1) {
                    $.messager.alert('提示', '请选中要删除的行', 'info');
                    return;
                }
                $.messager.confirm('Confirm', '确定要删除吗？', function (r) {
                    if (r) {
                        toolbar.ajaxOption('/ERP_OrderApplyPurchase/DeleteSaleDetail', { data: rows }, function (response) {
                            if (response.success == 0) {
                                $(datagridConfig.dom).datagrid('reload');
                                toolbar.showMsg(response.msg);
                            }
                        })
                    }
                });
            });
            //申请单Detail 撤销编辑
            $(datagridConfig.optionConfig.dom.revort).unbind('click').click(function () {
                if (datagridConfig.optionConfig.editRow != undefined) {
                    $(datagridConfig.dom).datagrid('rejectChanges');
                    datagridConfig.optionConfig.editRow = undefined;
                }
                
            });
            //申请单Detail 保存
            $(datagridConfig.optionConfig.dom.save).unbind('click').click(function () {
                if (datagridConfig.optionConfig.editRow != undefined) {
                    $(datagridConfig.dom).datagrid('endEdit', datagridConfig.optionConfig.editRow );
                }
            });



        var runPageConfig = {
                before: '#orderApply_pre',
                next: '#orderApply_next',
                total: '#orderApply_total',
                page: '#orderApply_page',
                runPageUrl: '/ERP_OrderApplyPurchase/RunPage',
                form: '#orderApply_form',
                text: '#orderApply_applyNotext',
                no: '#orderApply_applyNo',
                auditorPerson: 0,
                list: [{ datagrid: '#orderApply_datagrid', url: '/ERP_OrderApplyPurchase/GetPurchaseDetail', param_No: '#orderApply_applyNo' }],
                menuArr: [{ menu: '#orderApply_menu', items: ['#orderApply_menu_add', '#orderApply_menu_edit', '#orderApply_menu_save', '#orderApply_menu_remove'] }],
                linkbuttonArr: [ '#orderApply_edit','#orderApply_remove', '#orderApply_auditor']
            };
            toolbar.nextPage(runPageConfig);
            toolbar.beforePage(runPageConfig);

        var auditorConfig = {
                auditor: '#orderApply_auditor',
                unAuditor: '#orderApply_unauditor',
                paramNo: '#orderApply_applyNo',
                auditorNo: '#orderApply_auditorPerson',
                form: '#orderApply_form',
                auditorUrl: '/ERP_OrderApplyPurchase/Auditor',
                unAuditorUrl: '/ERP_OrderApplyPurchase/UnAuditor',
                menuArr: [{ menu: '#orderApply_menu', items: ['#orderApply_menu_add', '#orderApply_menu_edit', '#orderApply_menu_save', '#orderApply_menu_remove'] }],
                linkbuttonArr: [ '#orderApply_edit', '#orderApply_remove', '#orderApply_auditor']
            };
            toolbar.auditor(auditorConfig);
            toolbar.unAuditor(auditorConfig);

        var optionConfig = {
                dom: {
                    add: '#orderApply_add',
                    edit: '#orderApply_edit',
                    remove: '#orderApply_remove',
                    save: '#orderApply_save',
                    refresh:'#orderApply_refresh'
                },
                editUrl: '/ERP_OrderApplyPurchase/UpdateApply',
                saveUrl: '/ERP_OrderApplyPurchase/AddApply',
                removeUrl: '/ERP_OrderApplyPurchase/DeleteApply',
                refreshUrl:'/ERP_OrderApplyPurchase/Refresh',
                no: '#orderApply_applyNo',
                text: '#orderApply_applyNotext',
                form: '#orderApply_form',
                datagridList: [{ datagrid: '#orderApply_datagrid', url: '/ERP_OrderApplyPurchase/GetPurchaseDetail', param_No: '#orderApply_applyNo' }],
                menuArr: [{ menu: '#orderApply_menu', items: ['#orderApply_menu_add', '#orderApply_menu_edit', '#orderApply_menu_save', '#orderApply_menu_remove'] }],
                linkbuttonArr: ['#orderApply_edit', '#orderApply_remove', '#orderApply_auditor']
            };
            toolbar.added(optionConfig);
            toolbar.edited(optionConfig);
            toolbar.removed(optionConfig);
            toolbar.saved(optionConfig);
            toolbar.refresh(optionConfig);
           
        }

      

    }
    function GetStock(url, pars, dom, _index, _field) {
        $.ajax({
            url: url,
            type: 'post',
            data: pars,
            success: function (response) {
                var element = $(dom).datagrid('getEditor', { index: _index, field: _field });
                $(element.target).textbox('setValue', response);
            }

        })
    }

    function InitPageData(config) {
        $.post(config.initPageUrl, function (response) {
            if (response != null) {
                $(config.form).form('load', response);
                $(config.text).html(response.申请单号)
                $(config.datagrid).datagrid('options').url = "/ERP_OrderApplyPurchase/GetPurchaseDetail";
                $(config.datagrid).datagrid('reload', { paramNo: response.申请单号 });
                if (response.审核人编码 != null && response.审核人编码 != '') {
                    toolbar.controlMenuAndbutton(config);
                }
            }
        }, 'json')
    }
})