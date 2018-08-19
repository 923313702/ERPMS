define(['jquery', 'Basics/basics_operation', 'tool/initCombobox', 'order/params_search.service', 'tool/toolbar.service'], function ($, operation, initcombobox, search,toolbar) {

    return {
        index: function () {

            var comboboxConfig = [
                { dom: '#quotationList_saleMan', url: '/ERP_Order/GetStaff' },
                { dom: '#quotationList_customerNo', url: '/ERP_Order/GetCustormer' },
                { dom: '#quotationList_quotationNo', url: '/ERP_Quotation/GetQuotation' },
            ];
            initcombobox.initCombobox(comboboxConfig);

            var datagridConfig = {
                dom: '#quotationList_datagrid',
                initUrl: '/ERP_Quotation/GetQuotationList',
                columns: [[
                    { field: 'ck', checkbox:true},
                    { field: '订单号', title: '报价单号', width: 100, align: 'center' },
                    { field: '业务单号', title: '承接单号', width: 100, align: 'center', editor: { type: 'textbox', options: { readonly: true }} },
                    { field: '客户名称', title: '客户名称', width: 130, align: 'center' },
                    { field: '订单名称', title: '印品名称', width: 150, align: 'center' },
                    { field: '制单日期', title: '报价日期', width: 100, align: 'center' },
                    { field: '业务员', title: '报价人', width: 80, align: 'center' },
                    { field: '审核人', title: '审核人', width: 80, align: 'center' },
                    { field: '金额', title: '金额', width: 80, align: 'center' }

                ]],
                optionConfig: {
                    dom: {
                        export: '#quotationList_excel',
                    },
                    datagrid: '#quotationList_datagrid',
                    exportUrl: '/ERP_Quotation/ExportExcel',
                    editRow: undefined
                }
            };
            operation.datagrid(datagridConfig);
            operation.ExportExcel(datagridConfig.optionConfig);
            var searchConfig = {
                datagrid: '#quotationList_datagrid',
                dialog: '#quotationList_dialog',
                form: '#quotationList_form',
                search: '#quotationList_search',
                search_go: '#quotationList_go',
                search_cancel: '#quotationList_cancel'
            };
            search.searchOption(searchConfig);

            var optionConfig = {
                dom: {
                    refresh: '#order_refresh'
                },
                refreshUrl: '/ERP_Order/Refresh',
                no: '#quotation_orderNo',
                text: '#order_orderNo_text',
                form: '#order_form',
                datagridList: [
                    { datagrid: '#order_paper', url: '/ERP_Order/GetOrderDetail', param_No: '#order_orderNo' },
                    { datagrid: '#order_process', url: '/ERP_Order/GetOrderProcess', param_No: '#order_orderNo' },
                    { datagrid: '#order_material', url: '/ERP_Order/GetOrderMaterial', param_No: '#order_orderNo' }]
            };
           
            $('#quotationList_generate').click(function () {
                var rows = $(datagridConfig.dom).datagrid('getSelections');
                if (rows.length != 1) {
                    $.messager.alert('提示', '请选中要生成的报价单', 'info');
                    return;
                }
                var orderNo = rows[0].订单号;
                $.post('/ERP_Quotation/AcceptanceList', { orderNo: orderNo }, function (response) {
                    if (response.success == 0) {
                        addTab('业务承接单', '/ERP_Order/Index');
                        loadPage(optionConfig, response.id);
                        var index = $(datagridConfig.dom).datagrid('getRowIndex', rows[0]);
                        $(datagridConfig.dom).datagrid('beginEdit', index);
                        var element = $(datagridConfig.dom).datagrid('getEditor', { index: index, field: '业务单号' });
                        $(element.target).textbox('setValue', response.id);
                        $(datagridConfig.dom).datagrid('endEdit', index);
                        $.messager.alert('提示', response.msg, 'info');
                    } else {
                        $.messager.alert('提示', response.msg, 'info');
                    }
                })

            });
            $('#quotationList_print').click(function () {
                var page = $(datagridConfig.dom).datagrid("options").pageNumber;
                var rows = $(datagridConfig.dom).datagrid("options").pageSize;
                var queryParams = $(datagridConfig.dom).datagrid('options').queryParams;
                //if (isEmptyObject(queryParams)) { return; }
                var strUrl = '';
                for (var item in queryParams) {
                    if (typeof (queryParams[item]) == 'object' || typeof (queryParams[item]) == 'function') { continue };
                    strUrl += '&' + item + '=' + queryParams[item];
                }
               var strUrl = '/ERP_Quotation/QuotationListPrintPage?page=' + page + '&rows=' + rows + strUrl + '&d=' + Math.random();

               $('#quotationListIframe').attr('src', strUrl);

            });
        }

    }
    function  loadPage(config ,paramNo){
        $.post(config.refreshUrl, { paramNo: paramNo }, function (resp) {
            if (resp != null) {
                $(config.form).form('load', resp);
                $(config.text).text(paramNo)
                var list = config.datagridList;
                if (list != undefined && list.length > 0) {
                    for (var i in list) {
                        var data = new Object();
                        data.paramNo = paramNo;
                        $(list[i].datagrid).datagrid({ url: list[i].url, queryParams: data });;
                    }
                }
            }
        })
    }
    function addTab(title, url) {
        if ($('#contentTabs').tabs('exists', title)) {
            $('#contentTabs').tabs('select', title);
        } else {
            $('#contentTabs').tabs('add', {
                title: title,
                //content: content,
                href: url,
                closable: true
            })
            //var content = '<iframe scrolling="no" id="urlIframe" name="child" frameborder="0" src="' + url + '" style="width:99%;height:98%;border:none; padding:5px;"></iframe>';
            var content = '<iframe class="page-iframe" src="' + url + '" frameborder="no"   border="no" height="100%" width="100%" scrolling="auto"></iframe>'

        }
    }

})