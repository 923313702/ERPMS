define(['jquery', 'Basics/basics_operation', 'tool/initCombobox', 'order/params_search.service'], function ($, operation, initcombobox, search) {

    return {
        index: function () {
            var comboboxConfig = [
                { dom: '#contractList_saleMan', url: '/ERP_Order/GetStaff' },
                { dom: '#contractList_customerNo', url: '/ERP_Order/GetCustormer' },
                { dom: '#contractList_contractNo', url: '/ERP_Contract/GetContractNo' },
            ];
            initcombobox.initCombobox(comboboxConfig);
            var datagridConfig = {
                dom: '#contractList_datagrid',
                initUrl: '/ERP_Contract/ShowContractList',
                columns: [[
                    { field: '合同号', title: '合同号', width: 100, align: 'center' },
                    { field: '制单日期', title: '签订日期', width: 100, align: 'center' },
                    { field: '客户名称', title: '客户名称', width: 100, align: 'center' },
                    { field: '客户法定人', title: '客户法定人', width: 100, align: 'center' },
                    { field: '交货日期', title: '交货日期', width: 100, align: 'center' },
                    { field: '评审单号', title: '评审单号', width: 100, align: 'center' },
                    { field: '金额', title: '合同金额', width: 100, align: 'center' },
                    { field: '审核人', title: '审核人', width: 100, align: 'center' },
                    { field: '审核日期', title: '审核日期', width: 100, align: 'center' }
                ]],
                optionConfig: {
                    dom: {
                        export: '/',
                    },
                    datagrid: '#contractList_datagrid',
                    exportUrl: '/',
                    editRow: undefined
                }
            }
            operation.datagrid(datagridConfig);

            var searchConfig = {
                datagrid: '#contractList_datagrid',
                dialog: '#contractList_dialog',
                form: '#contractList_form',
                search: '#contractList_search',
                search_go: '#contractList_go',
                search_cancel: '#contractList_cancel'
            };
            search.searchOption(searchConfig);
            //var date = new Date();
            //today = date.getFullYear() + "/" + date.getMonth() + 1 + "/" + date.getDate();
            //$("#contractList_zhidanend").datebox("setValue", today);
            $('#contractList_print').click(function () {
                var page = $(datagridConfig.dom).datagrid("options").pageNumber;
                var rows = $(datagridConfig.dom).datagrid("options").pageSize;
                var queryParams = $(datagridConfig.dom).datagrid('options').queryParams;
                //if (isEmptyObject(queryParams)) { return; }
                var strUrl = '';
                for (var item in queryParams) {
                    if (typeof (queryParams[item]) == 'object' || typeof (queryParams[item]) == 'function') { continue };
                    strUrl += '&' + item + '=' + queryParams[item];
                }
                var strUrl = '/ERP_Contract/ContractListPrintPage?page=' + page + '&rows=' + rows + strUrl + '&d=' + Math.random();

                $('#contractListIframe').attr('src', strUrl);

            });
        }
    }
})