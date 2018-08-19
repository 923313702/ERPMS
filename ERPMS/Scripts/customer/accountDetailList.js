define(['jquery', 'Basics/basics_operation', 'tool/initCombobox', 'order/params_search.service'], function ($, operation, initCom, search) {
    return {
        index: function () {
            var comboboxConfig = [
                { dom: '#accountDetailList_customerNo', url: '/ERP_Order/GetCustormer' },
                { dom: '#accountDetailList_accountNo', url: '/ERP_Customer/GetAccountNoList' },
            ];
            initCom.initCombobox(comboboxConfig);

            var datagridConfig = {
                dom: '#accountDetailList_datagrid',
                initUrl: '/ERP_Customer/GetAccountDetailList',
                columns: [[
                    { field: '对账单号', title: '对账单号', width: 100, align: 'center' },
                    { field: '对账日期', title: '对账日期', width: 100, align: 'center' },
                    { field: '客户名称', title: '客户名称', width: 130, align: 'center' },
                    { field: '订单号', title: '订单号', width: 100, align: 'center' },
                    { field: '制单日期', title: '制单日期', width: 100, align: 'center' },
                    { field: '订单名称', title: '订单名称', width: 150, align: 'center' },
                    { field: '计量单位', title: '计量单位', width: 80, align: 'center' },
                    { field: '成品数量', title: '数量', width: 80, align: 'center' },
                    { field: '金额', title: '金额', width: 80, align: 'center' },
                    { field: '备注', title: '备注', width: 150, align: 'center' },
                    { field: '制单人', title: '制单人', width: 80, align: 'center' },
                    { field: '审核人', title: '审核人', width: 80, align: 'center' }
                ]],
                optionConfig: {
                    dom: {
                        export: '#accountDetailList_excel',
                    },
                    datagrid: '#accountDetailList_datagrid',
                    exportUrl: '/ERP_Customer/ExportAccountDetailListExcel',
                    editRow: undefined
                }
            };
            operation.datagrid(datagridConfig);
            operation.ExportExcel(datagridConfig.optionConfig);
            var searchConfig = {
                datagrid: '#accountDetailList_datagrid',
                dialog: '#accountDetailList_dialog',
                form: '#accountDetailList_form',
                search: '#accountDetailList_search',
                search_go: '#accountDetailList_go',
                search_cancel: '#accountDetailList_cancel'
            };
            search.searchOption(searchConfig);

            $('#accountDetailList_print').click(function () {
                var page = $(datagridConfig.dom).datagrid("options").pageNumber;
                var rows = $(datagridConfig.dom).datagrid("options").pageSize;
                var queryParams = $(datagridConfig.dom).datagrid('options').queryParams;
                //if (isEmptyObject(queryParams)) { return; }
                var strUrl = '';
                for (var item in queryParams) {
                    if (typeof (queryParams[item]) == 'object' || typeof (queryParams[item]) == 'function') { continue };
                    strUrl += '&' + item + '=' + queryParams[item];
                }
                var strUrl = '/ERP_Customer/AccountListDetailPrintPage?page=' + page + '&rows=' + rows + strUrl + '&d=' + Math.random();

                $('#accountDetailListIframe').attr('src', strUrl);

            });
        }
    }
})