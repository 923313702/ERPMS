define(['jquery', 'Basics/basics_operation'], function ($, operation) {
    return {
        index: function (){
            var config = {
                dom: '#ERP_organize',
                tool: '#ERP_organize_tb',
                initUrl: '/',
                optionUrl: '',
                flag: '',
                columns: [[
                    { field: 'ch', checkbox: true },
                    {
                        field: 'text',
                        title: '名称',
                        width: 150,
                        align: 'left',
                        editor: {
                            type: 'textbox',
                            options: { required: true }
                        }
                    },
                    {
                        field: 'Id',
                        title: '编号',
                        width: 100,
                        align: 'center',
                        editor: {
                            type: 'textbox',
                            options: {
                                required: true,
                                delay: 500,
                                validType: ['organizeNoAjax'/*, 'length[1,6]'*/]
                            }
                        }
                    },
                    {
                        field: 'Group',
                        title: '班组/部门',
                        width: 120,
                        align: 'center',
                        editor: {
                            type: 'checkbox',
                            options: { on: '1', off: '0' }
                        }
                    },
                    {
                        field: 'parentId',
                        title: '上级编码',
                        width: 100,
                        align: 'center',
                        editor: {
                            type: 'textbox',
                            options: { editable: false }
                        }
                    },
                    {
                        field: 'Workshop',
                        title: '车间属性',
                        width: 100,
                        align: 'center',
                        editor: { type: 'textbox' }
                    }
                ]],
                optionConfig: {
                    dom: {
                        add: '#organize_tb_add',
                        edit: '#organize_tb_edit',
                        remove: '#organize_tb_remove',
                        revort: '#organize_tb_revort',
                        export: '#organize_tb_excel',
                        save: '#organize_tb_save',
                    },
                    datagrid: '#ERP_printColor',
                    savebutton: '#organize_tb_save',
                    removeUrl: '',
                    exportUrl: '',
                    editRow: undefined
                }
            }
            operation.datagrid(config);
            operation.index(config.optionConfig);
        }

    }
})