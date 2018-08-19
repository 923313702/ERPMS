define(['jquery', 'Basics/basics_operation'], function ($, operation) {
    return {
        index: function () {
            $.extend($.fn.validatebox.defaults.rules, {
                colorNoAjax: {
                    validator: function (value) {
                        var a = true;
                        var flag = $('#printColor_tb_save').attr('flag');
                        if (flag == 'add') {
                            $.ajax({
                                type: "post",
                                async: false,
                                url: '/ERP_PrintColor/CheckPrintColorNo',
                                data: { 'colorNo': value },
                                success: function (data) {
                                    if (data.toLowerCase() == "true") {
                                        a = false;
                                    }
                                }
                            });
                        }
                        return a;
                    },
                    message: '印色编码已存在'
                },
            });
            var config = {
                dom: '#ERP_printColor',
                tool: '#ERP_printColor_tb',
                initUrl: '/ERP_PrintColor/GetPrintColor',
                optionUrl: '/ERP_PrintColor/SaveOrUpdate',
                flag: '#printColor_tb_save',
                columns: [[
                    { field: 'ck', checkbox: true },
                    {
                        field: '印色编码',
                        title: '印色编码',
                        width: 100,
                        align: 'center',
                        editor: {
                            type: 'numberbox',
                            options: {
                                required: true,
                                validType: 'colorNoAjax'
                            }
                        }
                    }, {
                        field: '印色名称',
                        title: '印色名称',
                        width: 100,
                        align: 'center',
                        editor: {
                            type: 'textbox',
                            options: { required: true }
                        }
                    }, {
                        field: '版数',
                        title: '版数',
                        width: 100,
                        align: 'center',
                        editor: { type: 'numberbox', options: { precision: 0 } }
                    }, {
                        field: '印色',
                        title: '印色',
                        width: 100,
                        align: 'center',
                        editor: { type: 'numberbox', options: { precision: 0 } }
                    }, {
                        field: '备注',
                        title: '备注',
                        width: 100,
                        align: 'center',
                        editor: { type: 'textbox' }
                    }
                ]],
                optionConfig: {
                    dom: {
                        add: '#printColor_tb_add',
                        edit: '#printColor_tb_edit',
                        remove: '#printColor_tb_remove',
                        save: '#printColor_tb_save',
                        revort: '#printColor_tb_revort',
                        export: '#printColor_tb_excel',
                    },
                    datagrid:'#ERP_printColor',
                    savebutton: '#printColor_tb_save',
                    removeUrl: '/ERP_PrintColor/DeletePrintColor',
                    exportUrl: '/ERP_PrintColor/ExportExcel',
                    editRow: undefined
                }
            } 
            operation.datagrid(config);
            operation.index(config.optionConfig);
        }
    }
})