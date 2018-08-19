define(['jquery', 'Basics/basics_operation', 'order/params_search.service'], function ($, operation, search) {

    return {

        index: function () {
            $('#logDetailList_customerNo').combobox({
                url: '/ERP_Order/GetCustormer2',
                textField: 'Key',
                valueField: 'Id',
                filter: function (q, r) {
                    var opts = $(this).combobox('options');
                    if (r[opts.textField] == null) return -1;
                    return r[opts.textField].indexOf(q) >= 0;
                },
                onLoadSuccess: function () {
                    var data = $(this).combobox('getData');
                    $('#logDetailList_customerNo2').combobox('loadData', data);
                },
                onSelect: function (r) {
                    $(config.dom).datagrid('reload', { customerNo: r.Id });
                }

            });
            var config = {
                dom: '#logDetailList_datagrid',
                tool: '#logDetailList_toolbar',
                initUrl: '/ERP_Customer/GetLogListDetail',
                optionUrl: '',
                flag: '#customerLog_tb_save',
                columns: [[
                    { field: '订单日期', title: '订单日期', width: 100, align: 'center' },
                    { field: '客户名称', title: '客户名称', width: 120, align: 'center' },
                    { field: '手工单号', title: '手工单号', width: 100, align: 'center' },
                    { field: '订单说明', title: '订单说明', width: 120, align: 'center' },
                    { field: '发货说明', title: '发货说明', width: 120, align: 'center' },
                    { field: '结余说明', title: '结余说明', width: 120, align: 'center' },
                    { field: '办款说明', title: '办款说明', width: 120, align: 'center' },
                    { field: '制单人编码', title: '制单人', width: 100, align: 'center' },
                    { field: '制单日期', title: '制单日期', width: 100, align: 'center' },
                    { field: '确认人编码', title: '确认人', width: 80, align: 'center' },
                    { field: '确认日期', title: '确认日期', width: 100, align: 'center' },
                    { field: '确认说明', title: '确认说明', width: 100, align: 'center' }
                ]],
                optionConfig: {
                    dom: {
                        export: '#logDetailList_tb_excel',
                    },
                    datagrid: '#logDetailList_datagrid',
                    exportUrl: '/ERP_Customer/CustomerLogListExport',
                    editRow: undefined
                }
            };
            operation.datagrid(config);
            operation.ExportExcel(config.optionConfig);

            var searchConfig = {
                datagrid: '#logDetailList_datagrid',
                dialog: '#logDetailList_dialog',
                form: '#logDetailList_form',
                search: '#logDetailList_tb_search',
                search_go: '#logDetailList_go',
                search_cancel: '#logDetailList_cancel'
            };
            search.searchOption(searchConfig);



            $('#logDetailList_tb_print').click(function () {
                var page = $(config.dom).datagrid("options").pageNumber;
                var rows = $(config.dom).datagrid("options").pageSize;
                var queryParams = $(config.dom).datagrid('options').queryParams;
                //if (isEmptyObject(queryParams)) { return; }
                var strUrl = '';
                for (var item in queryParams) {
                    if (typeof (queryParams[item]) == 'object' || typeof (queryParams[item]) == 'function') { continue };
                    strUrl += '&' + item + '=' + queryParams[item];
                }
                var strUrl = '/ERP_Customer/PrintLogDetailPage?page=' + page + '&rows=' + rows + strUrl + '&d=' + Math.random();

                $('#logDetailListIframe').attr('src', strUrl);

            });

        }

    }
})