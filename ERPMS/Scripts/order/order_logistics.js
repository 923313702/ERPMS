define(['jquery', 'order/datagridOption.server','tool/initCombobox','tool/toolbar.service'], function ($, server,initcombobox,toolbar) {

    return {
        index: function () {

            //var comboboxConfig = [
            //    { dom: '#designMaster_saleMan', url: '/ERP_Order/GetStaff' },
            //    { dom: '#designMaster_pCategory', url: '/ERP_Order/PCategory' },
            //    { dom: '#designMaster_customer', url: '/ERP_Order/GetCustormer' },
            //    { dom: '#designMaster_orderNos', url: '/ERP_Order/GetCustormer' },
            //    { dom: '#designMaster_openNumber', url: '/ERP_Order/GetOpenNumber' },
            //    { dom: '#designMaster_zhidan', url: '' },
            //    { dom: '#designMaster_auditorNo', url: '' }
            //];
            //initcombobox.initCombobox(comboboxConfig);
            //$('#designMaster_saleMan').combobox({
            //    onLoadSuccess: function () {
            //        var data = $(this).combobox('getData');
            //        $('#designMaster_zhidan').combobox('loadData', data);
            //        $('#designMaster_auditorNo').combobox('loadData', data);
            //    }
            //});
          
            var logisticsConfig = {
                dom: '#orderLogistics_datagrid',
                optionUrl: '/ERP_OrderLogistics/SaveOrUpdateLoginsticsDetail',
                flag: '#orderLogistics_menu_save',
                menu: '#orderLogistics_menu',
                columns: [[
                    { field: 'ck', checkbox: true },
                    { field: '行号', title: '行号', hidden: true },
                    {
                        field: '申请单号', title: '申请单号', hidden: true
                    },
                    {
                        field: '项目分类',
                        title: '项目',
                        align: 'center',
                        width: 100,
                        editor: {
                            type: 'combobox',
                            options: {
                                textField: 'Key',
                                valueField: 'Id',
                                panelHeight: 120,
                                data: [
                                    { 'Key': '送人', 'Id': '送人' },
                                    { 'Key': '送货', 'Id': '送货' },
                                    { 'Key': '拉货', 'Id': '拉货' },
                                    { 'Key': '接人', 'Id': '接人' },
                                    { 'Key': '其他', 'Id': '其他' }
                                ],
                                required: true,
                                //filter: function (q, r) {
                                //    var opts = $(this).combobox('options');
                                //    return r[opts.textField].indexOf(q) >= 0;
                                //},
                                //panelWidth: 150,
                                //panelHeight:120,
                                editable: false
                            }
                        }
                    },
                    {
                        field: '订单号',
                        title: '订单号',
                        align: 'center',
                        width: 150,
                        editor: {
                            type: 'combobox',
                            options: {
                                url: '/ERP_OrderLogistics/GetOrder',
                                textField: 'Id',
                                valueField: 'Id',
                                required: true,
                                panelWidth: 250,
                                panelHeight: 110,
                                filter: function (q, r) {
                                    var opts = $(this).combobox('options');
                                    return r[opts.textField].indexOf(q) >= 0;
                                },
                                formatter: function (row) {
                                    // Id = p.订单号, Key = p.订单名称, Cno = p.客户编码, Pno = p.业务员编码
                                    return '<spn>' + row.Id + '&ensp;&ensp;&ensp;&ensp;' + row.Key + '</span>'
                                },
                                onSelect: function (record) {
                                    var element = $(logisticsConfig.dom).datagrid('getEditor', { index: logisticsConfig.optionConfig.editRow, field: '订单名称' });
                                    $(element.target).textbox('setValue', record.Key);
                                    element = $(logisticsConfig.dom).datagrid('getEditor', { index: logisticsConfig.optionConfig.editRow, field: '客户编码' });
                                    $(element.target).combobox('setValue', record.Cno);
                                    element = $(logisticsConfig.dom).datagrid('getEditor', { index: logisticsConfig.optionConfig.editRow, field: '业务员编码' });
                                    $(element.target).combobox('setValue', record.Pno);
                                }
                            }
                        }
                    },
                    { field: '订单名称', title: '订单名称', align: 'center', width: 150, editor: { type: 'textbox', options: { readonly: true } } },

                    {
                        field: '客户编码',
                        title: '客户名称',
                        align: 'center',
                        width: 150,
                        formatter:function(value, row, index) {
                            return row.客户名称;
                        },
                        editor: {
                            type: 'combobox',
                            options: {
                                url: '/ERP_OrderLogistics/GetCustomer',
                                textField: 'Key',
                                valueField: 'Id',
                                readonly: true,
                            }
                        }
                    },
                    {
                        field: '业务员编码',
                        title: "业务员",
                        align: 'center',
                        width: 120,
                        formatter:function(value, row, index) {
                            return row.姓名
                        },

                        editor: {
                            type: 'combobox', options: {
                                url: '/ERP_Orderlogistics/GetSalesman',
                                textField: 'Key',
                                valueField: 'Id',
                                readonly: true,
                            }
                        }
                    },
                    {
                        field: '进仓费',
                        title: '进仓费',
                        align: 'center',
                        width: 100,
                        editor: {
                            type: 'numberbox',
                            options: {
                                required: true,
                                precision: 2,
                            }
                        }
                    },
                    {
                        field: '金额', title: '运费金额', align: 'center', width: 100,
                        editor: {
                            type: 'numberbox',
                            options: {
                                required: true,
                                precision: 2
                            }
                        }
                    }
                ]],
               // list: [{ datagrid: '#orderLogistics_datagrid', url: '/ERP_OrderLogistics/ShowLogisticsDetail', param_No: '#orderLogistics_logisticsNo' }],
                optionConfig: {
                    dom: {
                        add: '#orderLogistics_menu_add',
                        edit: '#orderLogistics_menu_edit',
                        save: '#orderLogistics_menu_save',
                        remove: '#orderLogistics_menu_remove',
                        revort: '#orderLogistics_menu_revort',
                    },
                    datagrid: '#orderLogistics_datagrid',
                    removeUrl: '/ERP_OrderLogistics/DeleteLoginsticsDetail',
                    savebutton: '#orderLogistics_menu_save',
                    paramNo: '#orderLogistics_logisticsNo',
                    editRow: undefined
                }
            };
            server.datagrid(logisticsConfig);
            server.datagridOption(logisticsConfig.optionConfig);
            $(logisticsConfig.dom).datagrid({
                onAfterEdit: function (i, d, c) {
                    var flags = $(logisticsConfig.flag).attr('flag');
                    var val = $(logisticsConfig.optionConfig.paramNo).val();
                    if (flags == 'add') {
                        if (val == null || val == '') {
                            $.messager.alert('提示', '没获取到申请单号', 'info');
                            $(logisticsConfig.dom).datagrid('rejectChanges');
                            logisticsConfig.optionConfig.editRow = undefined;
                            return;
                        }
                        d.申请单号 = val;
                    }
                    $.ajax({
                        url: logisticsConfig.optionUrl,
                        type: 'post',
                        data: { data: d, flag: flags },
                        success: function (response) {
                            if (response.success == 0) {
                                logisticsConfig.optionConfig.editRow = undefined;
                                server.showMsg(response.msg);
                                reloadDatagrid2(logisticsConfig.list);

                            } else {
                                $.messager.alert('提示', response.msg, 'info');
                                $(logisticsConfig.dom).datagrid('beginEdit', i);
                            }
                        },
                        error: function (XMLHttpRequest, testStatus, errorThrown) {
                            $.messager.alert('My Title', XMLHttpRequest.status, 'warning')
                        }
                    })
                },
                onLoadSuccess: function (data) {
                    logisticsConfig.optionConfig.editRow = undefined;
                    var freight = 0;
                    var jingcang = 0;
                    for (var i in data.rows) {
                        freight += isNaN(data.rows[i].金额) ? 0 : data.rows[i].金额;
                        jingcang += isNaN(data.rows[i].进仓费) ? 0 : data.rows[i].进仓费;
                    }
                    $('#orderLogistics_jincang').numberbox('setValue', jingcang);
                    $('#orderLogistics_freight').numberbox('setValue', freight);
                    $('#orderLogistics_Totalamount').numberbox('setValue', (jingcang + freight));
                }
            })


        
            var runPageConfig = {
                before: '#orderLogistics_pre',
                next: '#orderLogistics_next',
                total: '#orderLogistics_total',
                page: '#orderLogistics_page',
                runPageUrl: '/ERP_OrderLogistics/RunPage',
                form: '#orderLogistics_form',
                text: '#orderLogistics_logistics_text',
                no: '#orderLogistics_logisticsNo',
                auditorPerson: 0,
                list: [{ datagrid: '#orderLogistics_datagrid', url: '/ERP_OrderLogistics/ShowLogisticsDetail', param_No:'#orderLogistics_logisticsNo'}],
                menuArr: [{ menu: '#orderLogistics_menu', items: ['#orderLogistics_menu_add', '#orderLogistics_menu_edit', '#orderLogistics_menu_save','#orderLogistics_menu_remove'] }],
                linkbuttonArr: ['#orderLogistics_edit', '#orderLogistics_remove', '#orderLogistics_save', '#orderLogistics_auditor']
            };
            toolbar.nextPage(runPageConfig);
            toolbar.beforePage(runPageConfig);

            var auditorConfig = {
                auditor: '#orderLogistics_auditor',
                unAuditor: '#orderLogistics_unauditor',
                paramNo: '#orderLogistics_logisticsNo',
                auditorNo: '#orderLogistics_auditorPerson',
                form: '#orderLogistics_form',
                auditorUrl: '/ERP_OrderLogistics/Auditing',
                unAuditorUrl: '/ERP_OrderLogistics/Withdrawal',
                menuArr: [{ menu: '#orderLogistics_menu', items: ['#orderLogistics_menu_add', '#orderLogistics_menu_edit', '#orderLogistics_menu_save', '#orderLogistics_menu_remove'] }],
                linkbuttonArr: ['#orderLogistics_edit', '#orderLogistics_remove','#orderLogistics_save', '#orderLogistics_auditor']
            };
            toolbar.auditor(auditorConfig);
            toolbar.unAuditor(auditorConfig);
            var optionConfig = {
                dom: {
                    add: '#orderLogistics_add',
                    edit: '#orderLogistics_edit',
                    remove: '#orderLogistics_remove',
                    save: '#orderLogistics_save'
                },
                editUrl: '/ERP_OrderLogistics/Updatelogistics',
                saveUrl: '/ERP_OrderLogistics/Addlogistics',
                removeUrl: '/ERP_OrderLogistics/Deletlogistics',
                no: '#orderLogistics_logisticsNo',
                text:'#orderLogistics_logistics_text',
                form: '#orderLogistics_form',
                datagridList: [{ datagrid: '#orderLogistics_datagrid', url: '/ERP_OrderLogistics/ShowLogisticsDetail', param_No: '#orderLogistics_logisticsNo' }],
                menuArr: [{ menu: '#orderLogistics_menu', items: ['#orderLogistics_menu_add', '#orderLogistics_menu_edit', '#orderLogistics_menu_save', '#orderLogistics_menu_remove'] }],
                linkbuttonArr: ['#orderLogistics_edit', '#orderLogistics_remove','#orderLogistics_save', '#orderLogistics_auditor']
            };
            toolbar.added(optionConfig);
            toolbar.edited(optionConfig);
            toolbar.removed(optionConfig);
            toolbar.saved(optionConfig);
        }

    }
    function reloadDatagrid2(option) {
        for (var i in option) {
            var data = new Object();
            data.paramNo = $(option[i].param_No).val();
            $(option[i].datagrid).datagrid({ url: option[i].url, queryParams: data });
        }
       
    }
})