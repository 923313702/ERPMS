define(['jquery', 'Basics/basics_operation'], function ($, operation) {

    return {
        index: function () {
            var config = {
                dom: '#ERP_customer',
                tool: '#ERP_customer_tb',
                initUrl: '',
                optionUrl: '',
                flag: '#customer_tb_save',
                columns: [[
                    { field: 'ck', checkbox: true },
                    { field: '客户编码', title: '客户编码', width: 100, align: 'center', },
                    { field: '客户名称', title: '客户名称', width: 100, align: 'center' },
                    {
                        field: '使用状态', title: '使用状态', width: 100, align: 'center', formatter: function (v, r, i) {
                            var result =
                                console.log(v);
                            if (v == 1) {
                                result = '<span style="color:#1da02b">√</span>';
                            } else {
                                result = '<span style="color:#ff0000">×</span>';
                            }
                            return result;
                        }
                    },
                    { field: '客户级别', title: '客户级别', width: 100, align: 'center' },
                    { field: '客户类别', title: '客户类别', width: 100, align: 'center' },
                    {field:'建档日期',title:'建档日期',width:100,align:'center'}
                ]],
                optionConfig: {
                    dom: {
                        remove: '#customer_tb_remove',
                        export: '#customer_tb_excel'
                    },
                    datagrid: '#ERP_customer',
                    savebutton: '#customer_tb_save',
                    removeUrl: '/ERP_Customer/DeleteCustomer',
                    exportUrl: '/ERP_Customer/ExportExcel',
                    editRow: undefined
                }

            }

            operation.datagrid(config);
            operation.index(config.optionConfig);
            $('#customer_saleMan').combobox({
                url: '/ERP_Order/GetStaff',
                textField: 'Key',
                valueField: 'Id',
                filter: function (q, r) {
                    var opts = $(this).combobox('options');
                    return r[opts.textField].indexOf(q) >= 0;
                },
                onSelect: function (r) {
                    $(config.dom).datagrid({ url: '/ERP_Customer/GetCustomer', queryParams: { saleMan: r.Id } });
                },
                onLoadSuccess: function () {
                    var data = $(this).combobox('getData');
                    console.log(data);
                    $('#customer_saleMan2').combobox('loadData', data);
                }
            });
            $('#customer_tb_add').click(function () {
                $('#customer_go').attr('flag', 'add');
                $('#customer_dialog').dialog('open');
            });
            $('#customer_tb_edit').click(function () {
                var rows = $('#ERP_customer').datagrid('getSelections');
                if (rows.length != 1) {
                    $.messager.alert('提示', '请选中一行进行修改', 'info');
                    return 
                }
               $('#customer_dialog').dialog('setTitle', '修改客户')
                $('#customer_go').attr('flag', 'edit');
                $('#customer_form').form('load', rows[0]);

                $('#customer_dialog').dialog('open');
            })
            $('#customer_go').click(function () {
                var flag = $(this).attr('flag');
               
                var data = $('#customer_form').serialize();
                var url = flag == 'add' ? '/ERP_Customer/SaveCustomer' : flag == 'edit' ? '/ERP_Customer/UpdateCustomer' : '';
                operation.ajaxOperation(url, data, function (response) {
                    if (response.success == 0) {
                        console.log ('好像没执行')
                        operation.showMsg(response.msg);
                        $('#customer_form').form('clear');
                        $('#customer_dialog').dialog('close');
                    } else {
                        $.messager.alert('提示', response.msg, 'info');
                    }
                })
            });
            $('#customer_cancel').click(function () {
                $('#customer_form').form('clear');
                $('#customer_dialog').dialog('close');
            });
            $('#customer_tb_print').click(function () {
                var page = $(config.dom).datagrid("options").pageNumber;
                var rows = $(config.dom).datagrid("options").pageSize;
                var queryParams = $(config.dom).datagrid('options').queryParams;
                if (isEmptyObject(queryParams)) { return; }
                var strUrl = '';
                for (var item in queryParams) {
                    if (typeof (queryParams[item]) == 'object' || typeof (queryParams[item]) == 'function') { continue };
                    strUrl += '&' + item + '=' + queryParams[item];
                }
                strUrl = '/ERP_Customer/CustomerPrintPage?page=' + page + '&rows=' + rows + strUrl + '&d=' + Math.random();
                //location.href = strUrl;
                $('#customer_printIframe').attr('src', strUrl);
            })
            //$('#customer_tb_remove').click(function () {
            //    var rows = $('#ERP_customer').datagrid('getSelections');
            //    if (rows.length <= 0) {
            //        $.messager.alert('提示', '请选中进行删除', 'info');
            //        return
            //    }
            //    operation.ajaxOperation('/ERP_Customer/DeleteCustomer', { data: rows }, function (response) {
            //        if (response.success == 0) {
            //            operation.showMsg(response.msg);
            //            $(config.dom).datagrid('reload');
            //        } else {
            //            $.messager.alert('提示', response.msg, 'info');
            //        }
            //    })
            //});
            //$('#customer_tb_excel').click()
        }

    }

    //判断对象是否为空
    function isEmptyObject(e) {
        var t;
        for (t in e)
            return !1;
        return !0
    }
})