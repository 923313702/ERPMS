define(['jquery', 'order/datagridOption.server', 'tool/initCombobox'], function ($, server, initcombobox) {
    return {
        index: function () {
            var comboboxConfig = [
                { dom: '#orderAccount_zhidan', url: '/ERP_Order/GetStaff' },
                { dom: '#orderAccount_auditorNo', url: '/ERP_Order/GetStaff' }
            ];
            initcombobox.initCombobox(comboboxConfig)
            var datagridConfig = {
                dom: '#orderAccount_datagrid',
                optionUrl: '/ERP_OrderAccount/SaveOrUpdateAccessory',
                flag: '#orderAccount_menu_save',
                columns: [[
                    { field: 'ck', checkbox: true },
                    { field: '行号', title: '行号', hidden: true },
                    { field: '订单号', title: '订单号', hidden: true },
                    {
                        field: '版号', title: '合版版号', align: 'center', width: 100,
                        editor: {
                            type: 'textbox',
                            options: {
                                required: true
                            }
                        }
                    },
                    {
                        field: '名称', title: '合版明细', align: 'center', width: 120,
                        editor: {
                            type: 'textbox',
                            options: {
                                required: true,
                            }
                        }
                    },
                    {
                        field: '拼数', title: '拼数', align: 'center', width: 100,
                        editor: {
                            type: 'numberbox',
                            options: {
                                precision: 2,
                            }
                        }
                    },

                    {
                        field: '成品数', title: '成品数', align: 'center', width: 100,
                        editor: {
                            type: 'numberbox',
                            options: {
                                precision: 2,
                                onChange: function (n, o) {
                                    var element = $(datagridConfig.dom).datagrid('getEditor', { index: datagridConfig.optionConfig.editRow, field: '单价' });
                                    var price = $(element.target).numberbox('getValue');
                                    element = $(datagridConfig.dom).datagrid('getEditor', { index: datagridConfig.optionConfig.editRow, field: '金额' });
                                    $(element.target).numberbox('setValue', price * n);

                                }
                            }
                        }
                    },
                    {
                        field: '用料尺寸', title: '成品尺寸', align: 'center', width: 100,
                        editor: {
                            type: 'textbox',
                            options: {
                                required: true,

                            }
                        }
                    },
                    {
                        field: '单价', title: '单价', align: 'center', width: 100,
                        editor: {
                            type: 'numberbox',
                            options: {
                                precision: 2,
                                onChange: function (n, o) {
                                    var element = $(datagridConfig.dom).datagrid('getEditor', { index: datagridConfig.optionConfig.editRow, field: '成品数' });
                                    var unit = $(element.target).numberbox('getValue');
                                    element = $(datagridConfig.dom).datagrid('getEditor', { index: datagridConfig.optionConfig.editRow, field: '金额' });
                                    $(element.target).numberbox('setValue', unit * n);

                                }
                            }
                        }
                    },
                    {
                        field: '金额', title: '金额', align: 'center', width: 100,
                        editor: {
                            type: 'numberbox',
                            options: {
                                precision: 2
                            }
                        }
                    }
                ]],
                menu: '#orderAccount_menu',
                optionConfig: {
                    dom: {
                        add: '#orderAccount_menu_add',
                        edit: '#orderAccount_menu_edit',
                        save: '#orderAccount_menu_save',
                        remove: '#orderAccount_menu_remove',
                        revort: '#orderAccount_menu_revort',
                    },
                    datagrid: '#orderAccount_datagrid',
                    removeUrl: 'ERP_OrderAccount/DeleteAccessory',
                    savebutton: '#orderAccount_menu_save',
                    paramNo: '#orderAccount_orderNo',
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
                            $.messager.alert('提示', '没获取到订单号', 'info');
                            $(datagridConfig.dom).datagrid('rejectChanges');
                            datagridConfig.optionConfig.editRow = undefined;
                            return;
                        }
                        d.订单号 = val;
                    }
                    $.ajax({
                        url: datagridConfig.optionUrl,
                        type: 'post',
                        data: { data: d, flag: flags },
                        success: function (response) {
                            if (response.success == 0) {
                                datagridConfig.optionConfig.editRow = undefined;
                                server.showMsg(response.msg);

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
                onLoadSuccess: function () {
                    datagridConfig.optionConfig.editRow = undefined;
                }
            });
            server.datagridOption(datagridConfig.optionConfig);
            InitPageData('/ERP_OrderAccount/ShowOrder', '#orderAccount_form', datagridConfig.dom)
        }
    }
    function InitPageData(url, form, datagrid) {

        $.post(url, function (response) {
            if (response != null) {
                $(form).form('load', response);
                $(datagrid).datagrid('options').url = "/ERP_OrderAccount/GetAccessoryData";
                $(datagrid).datagrid('reload', { paramNo: response.订单号 });
            }
        }, 'json')
    }
})