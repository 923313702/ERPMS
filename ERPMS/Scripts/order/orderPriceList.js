define(['jquery', 'Basics/basics_operation','tool/initCombobox','order/params_search.service'], function ($, operation,initCom,search) {

    return {
        index: function () {
            var datagridConfig = {
                dom: '#orderPriceList_datagrid',
                initUrl: '/ERP_Order/showOrderList',
                columns: [[
                    { field: '交货日期', title: '交货日期', width: 100, align: 'center' },
                    { field: '订单号', title: '订单号', width: 100, align: 'center' },
                    { field: '生产单号', title: '生产单号', width: 100, align: 'center' },
                    { field: '客户名称', title: '客户名称', width: 100, align: 'center' },
                    { field: '订单名称', title: '订单名称', width: 100, align: 'center' },
                    { field: '制单日期', title: '制单日期', width: 100, align: 'center' },
                    { field: '制单人', title: '制单人', width: 100, align: 'center' },
                    { field: '业务员', title: '业务员', width: 100, align: 'center' },
                    { field: '审核人', title: '审核人', width: 100, align: 'center' },
                    { field: '金额', title: '金额', width: 100, align: 'center' },
                ]],
                optionConfig: {
                    dom: {
                        export: '#orderPriceList_excel',
                    },
                    datagrid: '#orderPriceList_datagrid',
                    exportUrl: '/ERP_Order/ExportExcel',
                    editRow: undefined
                }
            }

            operation.datagrid(datagridConfig);
            operation.ExportExcel(datagridConfig.optionConfig);
            var comboboxConfig = [
                { dom: '#orderPriceList_saleMan', url: '/ERP_Order/GetStaff' },
                { dom: '#orderPriceList_printCategory', url: '/ERP_Order/PCategory' },
                { dom: '#orderPriceList_customerNo', url: '/ERP_Order/GetCustormer2' },
                { dom: '#orderPriceList_orderNo', url: '/ERP_Order/GetOrder' }];
            initCom.initCombobox(comboboxConfig);

            var searchConfig = {
                datagrid: '#orderPriceList_datagrid',
                dialog: '#orderPriceList_dialog',
                form: '#orderPriceList_form',
                search: '#orderPriceList_search',
                search_go: '#orderPriceList_go',
                search_cancel: '#orderPriceList_cancel'
            };
            search.searchOption(searchConfig);
        }
    }
})