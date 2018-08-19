define(['jquery', 'Basics/basics_operation'], function ($, operation) {
    return {
        index: function () {
            var config = {
                dom: '#ERP_customerLog',
                tool: '#ERP_customerLog_tb',
                initUrl: '/ERP_Customer/GetCustomerLog',
                optionUrl: '/ERP_Customer/SaveOrUpdateLog',
                flag: '#customerLog_tb_save',
                columns: [[

                    { field: 'ck', checkbox: true },
                    { field: 'ID', hidden: true },
                    { field: '制单日期', hidden: true },
                    {
                        field: '客户编码', title: '客户名称', width: 120, align: 'center',
                        formatter: function (v, r, i) {
                           
                            return r.客户名称
                        },
                        editor: {
                            type: 'combobox', options: {
                                textField: 'Key',
                                valueField: 'Id',
                                url: '/ERP_Order/GetCustormer2',
                                panelHeight: 120,
                                panelWidth:150,
                                required:true,
                                onSelect: function (r) {
                                    var index = config.optionConfig.editRow;
                                    var element = $(config.dom).datagrid('getEditor', { index: index, field: '手工单号' });
                                    $(element.target).combobox('reload', '/ERP_Order/GetOrderByCustomerNo?customerNo='+r.Id);
                                }
                            }
                        }
                    },
                    {
                        field: '手工单号', title: '订单号', width: 100, align: 'center', editor: {
                            type: 'combobox', options: {
                                textField: 'Key',
                                valueField: 'Id',
                                url: '',
                                panelHeight: 120,
                                panelWidth: 150,
                                required:true
                            }
                        }
                    },
                    { field: '订单说明', title: '订单说明', width: 120, align: 'center', editor: { type: 'textbox', options: {required:true}}},
                    { field: '发货说明', title: '发货说明', width: 120, align: 'center', editor: { type: 'textbox', options: { required: true }} },
                    { field: '结余说明', title: '结余说明', width: 120, align: 'center', editor: { type: 'textbox', options: { required: true }}  },
                    { field: '办款说明', title: '办款说明', width: 120, align: 'center', editor: { type: 'textbox', options: { required: true } } },
                    { field: '确认标知', title: '确认标识', width: 60, align: 'center' },
                    { field: '确认人编码', title: '确认人', width: 80,align: 'center' },
                    { field: '确认日期', title: '确认日期', width: 100, align: 'center' },
                    { field: '确认说明', title: '确认说明', width: 100, align: 'center' },
                    { field: '单据类型', hidden: true },
                    { field: '制单人编码', hidden: true }
                ]],
                optionConfig: {
                    dom: {
                        add: '#customerLog_tb_add',
                        edit: '#customerLog_tb_edit',
                        remove: '#customerLog_tb_remove',
                        revort: '#customerLog_tb_revort',
                        save: '#customerLog_tb_save',
                        export: '#customerLog_tb_excel'
                    },
                    datagrid: '#ERP_customerLog',
                    savebutton: '#customerLog_tb_save',
                    removeUrl: '/ERP_Customer/DeleteLog',
                    exportUrl: '/ERP_Customer/LogExportExcel',
                    editRow: undefined
                }
            };
            operation.datagrid(config);
            operation.index(config.optionConfig);
            $('#customerLog_tb_check').click(function (response) {
                var rows = $(config.dom).datagrid('getSelections');
                if (rows.length <= 0) { return; }
                operation.ajaxOperation('/ERP_Customer/ConfirmLog', { data: rows }, function (response) {
                    if (response.success == 0) {
                        $(config.dom).datagrid('reload');
                        var msg = response.num > 0 ? response.msg + '有' + response.num + '条数据待财务审核' : response.msg;
                        operation.showMsg(msg);
                    } else {
                        $.messager.alert('提示', response.msg, 'info');
                    }
                })

            });
            $('#customerLog_tb_print').click(function () {
                console.log ('hello')
                var page = $(config.dom).datagrid("options").pageNumber;
                console.log(page + '///');
                var rows = $(config.dom).datagrid("options").pageSize;
                console.log('rows' + rows);
                strUrl = '/ERP_Customer/LogPrintPage?page=' + page + '&rows=' + rows +'&d=' + Math.random();
               // location.href = strUrl;
                $('#customerlogList').attr('src', strUrl);

            });
        }
    }

})