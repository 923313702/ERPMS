define(['jquery', 'Basics/basics_operation'], function ($, operation) {

    return {
        index: function () {
            var config = {
                dom: '#ERP_JiaFangLu',
                tool: '#ERP_JiaFangLu_tb',
                initUrl: '/ERP_JiaFangLu/GetJiaFangLuList',
                optionUrl: '/ERP_JiaFangLu/SaveorUpdateJiaFangLu',
                flag: '#JiaFangLu_tb_save',
                columns: [
                    
                  
                   
                    [{ field: 'ck', checkbox: true, rowspan: 2 },
                        { field: '序号', hidden: true, rowspan: 2, },
                        {
                        field: '最低印数',
                        title: '最低印数',
                        rowspan: 2,
                        width: 60,
                        align: 'center',
                        editor: {
                            type: 'numberbox',
                            options: { precision: 0 }
                        }
                    },
                        {
                            field: '最高印数',
                            title: '最高印数',
                            rowspan: 2,
                            width: 60,
                            align: 'center',
                            editor: {
                                type: 'numberbox',
                                options: { precision: 0 }
                            }
                        }, { title: '业务加放', colspan: 4 }, { title: '生产加放', colspan: 4 },
                        { field: '备注', title: '备注', align: 'center', width: 200, editor: {type:'textbox'},rowspan:2}

                    ],
               
                    [ {
                        field: '印刷业务放数',
                        title: '印刷放数',
                        rowspan: 1,
                        width: 60,
                        align: 'center',
                        editor: {
                            type: 'numberbox',
                            options: { precision: 0 }
                        }
                    },
                    {
                        field: '装订业务放数',
                        title: '后道放数',
                        rowspan: 1,
                        width: 60,
                        align: 'center',
                        editor: {
                            type: 'numberbox',
                            options: { precision: 0 }
                        }
                    },
                    {
                        field: '印刷业务加放率',
                        title: '印刷放率',
                        rowspan: 1,
                        width: 60,
                        align: 'center',
                        editor: {
                            type: 'numberbox',
                            options: { precision: 3 }
                        }
                    },
                    {
                        field: '装订业务加放率',
                        title: '每后道放率',
                        rowspan: 1,
                        width: 60,
                        align: 'center',
                        editor: {
                            type: 'numberbox',
                            options: { precision: 3 }
                        }
                    },
                    {
                        field: '印刷生产放数',
                        title: '印刷放数',
                        rowspan: 1,
                        width: 60,
                        align: 'center',
                        editor: {
                            type: 'numberbox',
                            options: { precision: 0 }
                        }
                    },
                    {
                        field: '装订生产放数',
                        title: '后道放数',
                        rowspan: 1,
                        width: 60,
                        align: 'center',
                        editor: {
                            type: 'numberbox',
                            options: { precision: 0 }
                        }
                    },
                    {
                        field: '印刷生产加放率',
                        title: '印刷放率',
                        rowspan: 1,
                        width: 60,
                        align: 'center',
                        editor: {
                            type: 'numberbox',
                            options: { precision: 3 }
                        }
                    },
                    {
                        field: '装订生产加放率',
                        title: '每后道放率',
                        rowspan: 1,
                        width: 60,
                        align: 'center',
                        editor: {
                            type: 'numberbox',
                            options: { precision: 3 }
                        }
                    }
                    ]
                ],
                optionConfig: {
                    dom: {
                        add: '#JiaFangLu_tb_add',
                        edit: '#JiaFangLu_tb_edit',
                        remove: '#JiaFangLu_tb_remove',
                        revort: '#JiaFangLu_tb_revort',
                        save: '#JiaFangLu_tb_save',
                        export: '#JiaFangLu_tb_excel'
                    },
                    datagrid: '#ERP_JiaFangLu',
                    savebutton: '#JiaFangLu_tb_save',
                    removeUrl: '/ERP_JiaFangLu/DeleteJiaFangLu',
                    exportUrl: '/Erp_JiaFangLu/ExportExcel',
                    editRow: undefined
                }
            };
            operation.datagrid(config);
            operation.index(config.optionConfig);
            $('#JiaFangLu_tb_print').click(function () {
                var page = $(config.dom).datagrid("options").pageNumber;
                var rows = $(config.dom).datagrid("options").pageSize;
                var strUrl= '/ERP_JiaFangLu/PrintPage?page=' + page + '&rows=' + rows + '&d=' + Math.random();
               $('#JiaFangLuIframe').attr('src', strUrl);

            });
        }
    }
})