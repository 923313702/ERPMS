define(['jquery','Basics/basics_operation'], function($,operation) {
    return {
        index: function () {
            var config = {
                dom: '#ERP_openNumber',
                tool: '#ERP_openNumber_tb',
                initUrl: '/ERP_OpenNumber/GetOpenNumber',
                optionUrl: '/ERP_OpenNumber/SaveOrUpdate',
                flag: '#openNumber_tb_save',
                columns: [[
                    { field: 'ck', checkbox: true },
                    { field: '开数编码', title: '编码', hidden: true },
                    { field: '开数名称', title: '名称', align: 'center', width: 100, editor: { type: 'textbox', options: { required: true } } },
                    {
                        field: '所属开式', title: '所属开式', align: 'center', width: 100, editor: { type: 'numberbox', options: { precision: 0, required: true } }
                    },
                    { field: '开料尺寸', title: '开料尺寸', align: 'center', width: 100, editor: { type: 'textbox' } },
                    { field: "备注说明", title: '备注', align: 'center', width: 100, editor: { type: 'textbox' } }
                ]],
                optionConfig: {
                    dom: {
                        add: "#openNumber_tb_add",
                        edit: "#openNumber_tb_edit",
                        save: "#openNumber_tb_save",
                        remove: "#openNumber_tb_remove",
                        revort: "#openNumber_tb_revort",
                        export: "#openNumber_tb_excel"
                    },
                    datagrid: '#ERP_openNumber',
                    removeUrl: '/ERP_OpenNumber/DeletOpenNumber',
                    savebutton: '#openNumber_tb_save',
                    exportUrl: '/ERP_OpenNumber/ExportExcel',
                    editRow: undefined
                }
            };
            operation.datagrid(config);
            operation.index(config.optionConfig);
        }
    }
})