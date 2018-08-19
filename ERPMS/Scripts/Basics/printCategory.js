define(['jquery', 'Basics/basics_operation'], function ($, operation) {

    return {
        index: function () {
            var config = {
                dom: '#ERP_printCategory',
                tool: '#ERP_printCategory_tb',
                initUrl: '/ERP_PrintCategory/GetPrintCategoryList',
                optionUrl: '/ERP_PrintCategory/SaveorUpdatePrintCategory',
                flag: '#printCategory_tb_save',
                columns: [[
                    { field: 'ck', checkbox: true },
                    {
                        field: '印品类别编码',
                        title: '印品类别编码',
                        width: 100,
                        align: 'center',
                        editor: {
                            type: 'textbox',  
                        },
                    },
                    {
                        field: '印品类别名称',
                        title: '印品类别名称',
                        width: 100,
                        align: 'center',
                        editor: {
                            type: 'textbox',
                            options: { required: true }
                        },
                    },
                    {
                        field: '工价',
                        title: '接单工价',
                        width: 100,
                        align: 'center',
                        editor: {
                            type: 'numberbox',
                            options: { precision: 2 }
                        }
                    },
                    {
                        field: '备注',
                        title: '备注',
                        width: 150,
                        align: 'center',
                        editor: {
                            type: 'textbox',
                        }
                    }    
                ]],
                optionConfig: {
                    dom: {
                        add: '#printCategory_tb_add',
                        edit: '#printCategory_tb_edit',
                        remove: '#printCategory_tb_remove',
                        revort: '#printCategory_tb_revort',
                        save: '#printCategory_tb_save',
                        export: '#printCategory_tb_excel'
                    },
                    datagrid: '#ERP_printCategory',
                    savebutton: '#printCategory_tb_save',
                    removeUrl: '/ERP_PrintCategory/DeletePrintCategory',
                    exportUrl: '/Erp_PrintCategory/ExportExcel',
                    editRow: undefined
                }
            };
            operation.datagrid(config);
            operation.index(config.optionConfig);
            $('#printCategory_tb_print').click(function () {
                var page = $(config.dom).datagrid("options").pageNumber;
                var rows = $(config.dom).datagrid("options").pageSize;
                var strUrl= '/ERP_PrintCategory/PrintPage?page=' + page + '&rows=' + rows + '&d=' + Math.random();
                $('#printCategoryIframe').attr('src', strUrl);

            });
        }
    }
})