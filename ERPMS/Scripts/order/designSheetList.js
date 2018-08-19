define(['jquery', 'Basics/basics_operation', 'tool/initCombobox','order/params_search.service'], function ($, operation, initCom,search) {
    return {

        index: function () {
            var comboboxConfig = [
                { dom: '#designSheetList_saleMan', url: '/ERP_Order/GetStaff' },
                { dom: '#designSheetList_customerNo', url: '/ERP_Order/GetCustormer2' },
                { dom: '#designSheetList_orderNo', url: '/ERP_Order/GetOrder' }];
            initCom.initCombobox(comboboxConfig);
            var config = {
                dom: '#designSheetList_datagrid',
                initUrl: '/ERP_OrderDesign/ShowDesignSheetList',
                columns: [[
                    { field: '定稿日期', title: '定稿日期', width: 100, align: 'center' },
                    { field: '订单号', title: '流程单号', width: 100, align: 'center' },
                    { field: '客户名称', title: '客户名称', width: 100, align: 'center' },
                    { field: '订单名称', title: '印品名称', width: 100, align: 'center' },
                    { field: '制单日期', title: '制单日期', width: 100, align: 'center' },
                    { field: '制单人', title: '制单人', width: 100, align: 'center' },
                    { field: '业务员', title: '业务员', width: 100, align: 'center' },
                    { field: '审核人', title: '审核人', width: 100, align: 'center' }
                ]],
                optionConfig: {
                    dom: {
                        export: '#designSheetList_excel',
                    },
                    datagrid: '#designSheetList_datagrid',
                    exportUrl: '/ERP_OrderDesign/ExportExcel',
                    editRow: undefined
                }
            }
            operation.datagrid(config);
            operation.ExportExcel(config.optionConfig);
            var searchConfig = {
                datagrid: '#designSheetList_datagrid',
                dialog: '#designSheetList_dialog',
                form: '#designSheetList_form',
                search: '#designSheetList_search',
                search_go: '#designSheetList_go',
                search_cancel: '#designSheetList_cancel'
            };
            search.searchOption(searchConfig);

        }
    }
   
})
