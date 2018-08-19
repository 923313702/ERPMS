define(['jquery', 'Basics/basics_operation', 'tool/initCombobox','order/params_search.service'], function ($, operation, initCom,search) {
    return {
        index: function () {
            var datagridConfig = {
                dom: '#advanceList_datagrid',
                initUrl: '/ERP_OrderAdvance/ShowAdvanceList',
                columns: [[
                    { field: '单号', title: '单号', width: 100, align: 'center' },
                    { field: '订单号', title: '订单号', width: 100, align: 'center' },
                    { field: '订单名称', title: '订单号名称', width: 100, align: 'center' },
                    { field: '客户名称', title: '客户名称', width: 100, alidn: 'center' },
                    { field: '业务员', title: '业务员', width: 100, align: 'center' },
                    { field: '制单人', title: '制单人', width: 100, align: 'center' },
                    { field: '制单日期', title: '制单日期', width: 100, align: 'center' },
                    { field: '金额', title: '订单金额', width: 100, align: 'center' },
                    { field: '预收金额', title: '预收金额', width: 100, align: 'center' },
                    { field: '应收金额', title: '应收金额', width: 100, align: 'center' },
                    {field:'审核人',title:'审核人',width:100,align:'center'},
                    { field: '审核日期',title:'审核日期',width:100,align:'center'}
                ]],
                optionConfig: {
                    dom: {
                        export: '#advanceList_excel',
                    },
                    datagrid: '#advanceList_datagrid',
                    exportUrl: '/ERP_OrderAdvance/ExportExcel',
                    editRow: undefined
                }
            }
            operation.datagrid(datagridConfig);
            operation.ExportExcel(datagridConfig.optionConfig);
            var comboboxConfig = [
                { dom: '#advanceList_saleMan', url: '/ERP_Order/GetStaff' },
                { dom: '#advanceList_customerNo', url: '/ERP_Order/GetCustormer2' },
                { dom: '#advanceList_orderNo', url: '/ERP_Order/GetOrder' }];
            initCom.initCombobox(comboboxConfig);

            var searchConfig = {
                datagrid: '#advanceList_datagrid',
                dialog: '#advanceList_dialog',
                form: '#advanceList_form',
                search: '#advanceList_search',
                search_go: '#advanceList_go',
                search_cancel: '#advanceList_cancel'
            };
            search.searchOption(searchConfig);
        }
    }
})