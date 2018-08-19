define(['jquery', 'Basics/basics_operation'], function ($, operation) {
    return {
        index: function () {
            var config = {
                dom: '#ERP_publish',
                tool: '#ERP_publish_tb',
                initUrl: '',
                optionUrl: '',
                flag: '#publish_tb_save',
                columns: [[
                    { field: 'ck', checkbox: true },
                    { field: '项目编码', title: '项目编码', width: 100, align: 'center', },
                    { field: '项目名称', title: '项目名称', width: 100, align: 'center' },
                    { field: '工艺类别', title: '工艺类别', width: 100, align: 'center' },
                    { field: '计量单位', title: '计量单位', width: 100, align: 'center' },
                    {field:'排序码',title:'排序码',width:100,align:'center'}
                ]],
                optionConfig: {
                    dom: {
                        add: '#publish_tb_add',
                        edit: '#publish_tb_edit',
                        remove: '#publish_tb_remove',
                        revort: '#publish_tb_revort',
                        save: '#publish_tb_save',
                        export: '#publish_tb_excel'
                    },
                    datagrid: '#ERP_publish',
                    savebutton: '#publish_tb_save',
                    removeUrl: '/',
                    exportUrl: '/',
                    editRow: undefined
                }
            };

            operation.datagrid(config);
        }

    }


})