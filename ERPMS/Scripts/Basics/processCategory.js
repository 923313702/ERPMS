define(['jquery', 'Basics/basics_operation'], function ($, operation) {
    return {
        index: function () {
            var config = {
                dom: '#ERP_processCategory',
                tool: '#ERP_processCategory_tb',
                initUrl: '',
                optionUrl: '/ERP_ProcessCategory/SaveOrUpdate',
                flag: '#processCategory_tb_save',
                columns: [[
                    { field: 'ck', checkbox: true },
                    {
                        field: 'ID', title: '编号', hidden:true
                    },
                    {
                        field: '工艺类别',
                        title: '工艺类别',
                        width: 100,
                        align: 'center',
                        editor: {
                            type: 'textbox',
                            options: {
                                required: true,
                            }
                        }
                    },
                    {
                        field: '系数编码',
                        title: '系数编码',
                        width: 100,
                        align: 'center',
                        editor: {
                            type: 'textbox',
                            options: {
                                required: true,
                            }
                        }
                    },
                    {
                        field: '难度系数',
                        title: '难度系数',
                        width: 100, align:
                        'center',
                        editor: {
                            type: 'numberbox',
                            options: {
                                precision: 2,
                                required: true
                            },

                        }
                    },
                    {
                        field: '难度说明',
                        title: '难度说明',
                        width: 150,
                        align: 'center',
                        editor: {
                            type: 'textbox',
                            options: {
                                required: true,
                            }
                        }
                    }
                ]],
                optionConfig: {
                    dom: {
                        add: '#processCategory_tb_add',
                        edit: '#processCategory_tb_edit',
                        remove: '#processCategory_tb_remove',
                        revort: '#processCategory_tb_revort',
                        save: '#processCategory_tb_save',
                        export:'#processCategory_tb_excel'
                    },
                    datagrid:'#ERP_processCategory',
                    savebutton: '#processCategory_tb_save',
                    removeUrl: '/ERP_ProcessCategory/DeleteCategory',
                    exportUrl: '/ERP_ProcessCategory/ExportExcel',
                    editRow: undefined
                }
            };
            operation.datagrid(config);
            operation.initCombobox('#processCategory_c','/ERP_ProcessCategory/GetCategory')
            operation.index(config.optionConfig);
            $('#processCategory_c').combobox({
                onSelect: function (r) {
                    
                    $(config.dom).datagrid({ url:'/ERP_ProcessCategory/GetProcessCategory', queryParams: { pcategory: r.Id } });
                }
            })
        }
    }
})