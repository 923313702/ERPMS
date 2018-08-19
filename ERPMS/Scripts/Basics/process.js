define(['jquery','Basics/basics_operation'],function($,operation){
    return {
        index: function () {

            var config = {
                dom: '#ERP_process',
                tool: '#ERP_process_tb',
                initUrl: '',
                optionUrl: '/ERP_Process/SaveorUpdateProcess',
                flag: '#process_tb_save',
                columns: [[
                    { field: 'ck', checkbox: true },
                    {
                        field: '项目编码',
                        title: '项目编码',
                        width: 100,
                        align: 'center',
                        editor: {
                            type: 'textbox',
                            options: {
                                required: true,
                                delay: 500,
                                validType: 'processNoAjax'
                            }
                        },
                    },
                    {
                        field: '项目名称',
                        title: '项目名称',
                        width: 100,
                        align: 'center',
                        editor: {
                            type: 'textbox',
                            options: { required: true }
                        },
                    },
                    {
                        field: '工艺类别',
                        title: '工艺类别',
                        width: 100,
                        align: 'center',
                        editor: {
                            type: 'combobox',
                            options: {
                                textField: 'Key',
                                valueField: 'Id',
                                url: '/ERP_Process/GetCategory',
                                panelHeight: 120,
                                //  data: data

                            }
                        }
                    },
                    {
                        field: '统计编码',
                        title: '统计名称',
                        width: 150,
                        align: 'center',
                        editor: {
                            type: 'combobox',
                            options: {
                                textField: 'name',
                                valueField: 'text',
                                panelHeight: 120,
                                editable: false,
                                data: [{
                                    id: 100,
                                    name: "材料",
                                    text: '材料'
                                }, {
                                    id: 105,
                                    name: "印前",
                                    text: "印前"
                                }, {
                                    id: 110,
                                    name: "CTP版",
                                    text: "CTP版"
                                }, {
                                    id: 115,
                                    name: "PS版",
                                    text: "PS版"
                                }, {
                                    id: 200,
                                    name: "印刷",
                                    text: "印刷"
                                }, {
                                    id: 201,
                                    name: "印后",
                                    text: "印后"
                                }, {
                                    id: 205,
                                    name: "手工",
                                    text: "手工"
                                }, {
                                    id: 210,
                                    name: "其他",
                                    text: "其他"
                                }, {
                                    id: 215,
                                    name: "返款",
                                    text: "返款"
                                }, {
                                    id: 300,
                                    name: "税款",
                                    text: "税款"
                                }, {
                                    id: 310,
                                    name: "外协",
                                    text: "外协"
                                }, {
                                    id: 600,
                                    name: "覆膜",
                                    text: "覆膜"
                                }, {
                                    id: 700,
                                    name: "淋膜",
                                    text: "淋膜"
                                }],
                                onSelect: function () {
                                    var newPtion = $(this).combobox('getText');
                                },
                                formatter: function (row) {
                                    return '<span >' + row.id + '</span>       <span>' + row.text + '</span>';
                                }

                            }
                        }
                    },
                    {
                        field: '计量单位',
                        title: '计量',
                        align: 'center',
                        width: 100,
                        editor: {
                            type: 'combobox',
                            options: {
                                delay: 400,
                                url: '/ERP_Process/Getmetering',
                                textField: 'Key',
                                valueField: 'Id',
                                panelHeight: 120,
                                required: true
                            }
                        }
                    },
                    {
                        field: '计数标识',
                        title: '计数',
                        align: 'center',
                        width: 100,
                        editor: {
                            type: 'combobox',
                            options: {
                                textField: 'Key',
                                valueField: 'Id',
                                panelHeight: 120,
                                data: [
                                    { "Key": "固定数", "Id": "固定数" },
                                    { "Key": "版数", "Id": "版数" },
                                    { "Key": "印刷数", "Id": "印刷数" },
                                    { "Key": "成品数", "Id": "成品数" }
                                ],
                                required: true
                            }
                        }

                    },
                    {
                        field: '报价标识',
                        title: '报价',
                        width: 100,
                        align: 'center',
                        formatter: function (v, r) {
                            return formatterOptions(v)
                        },
                        editor: {
                            type: 'checkbox',
                            options: { on: '1', off: '0' }
                        }
                    },
                    {
                        field: '工艺标识',
                        title: '工艺',
                        width: 100,
                        align: 'center',
                        formatter(v, r) {
                            return formatterOptions(v)
                        },
                        editor: {
                            type: 'checkbox',
                            options: { on: '1', off: '0' }
                        }
                    },
                    {
                        field: '换单金额',
                        title: '换单金额',
                        width: 100,
                        align: 'center',
                        editor: { type: 'numberbox', options: { precision: 2 } }
                    },
                    {
                        field: '内部单价',
                        title: '内部单价',
                        width: 100,
                        align: 'center',
                        editor: { type: 'numberbox', options: { precision: 2, required: true } }
                    },
                    {
                        field: '销售单价',
                        title: '销售单价',
                        width: 100,
                        align: 'center',
                        editor: { type: 'numberbox', options: { precision: 2, required: true } }
                    },
                    {
                        field: '部门编码',
                        title: '部门',
                        width: 100,
                        align: 'center',
                        //formatter: function (value, row, index) {
                        //    return row.部门名称;
                        //},
                        editor: {
                            type: 'combobox',
                            options: {
                                url: '/ERP_Process/ShowDept?str=' + Math.random(),
                                textField: 'Key',
                                valueField: 'Id',
                                panelHeight: 120,
                                onLoadSuccess: function () {
                                    var index = config.optionConfig.editRow;
                                    var element = $('#ERP_process').datagrid('getEditor', { index:index, field: '部门编码' });
                                    var value = $(element.target).combobox('getText');
                                    var data = $(element.target).combobox('getData');
                                    if (value != null && value != '') {
                                        var id = Search(data, value);
                                        $(element.target).combobox('setValue', id);
                                    }

                                }
  
                            }
                        }
                    },
                    {
                        field: '班组编码',
                        title: '班组',
                        width: 100,
                        align: 'center',
                        //formatter: function (value, row, index) {
                        // return row.班组名称
                        //},
                        editor: {
                            type: 'combobox',
                            options: {
                                url: '/ERP_Process/ShowTeam?str=' + Math.random(),
                                textField: 'Key',
                                valueField: 'Id',
                                panelHeight: 120,
                                onLoadSuccess: function () {
                                    var element = $('#ERP_process').datagrid('getEditor', { index: config.optionConfig.editRow, field: '班组编码' });
                                    var value = $(element.target).combobox('getText');
                                    var data = $(element.target).combobox('getData');
                                    if (value != null && value != '') {
                                        var id = Search(data, value);
                                        $(element.target).combobox('setValue', id);
                                    }

                                }
                            }
                        }
                    },
                    {
                        field: '小时产能',
                        title: '小时产能',
                        width: 100,
                        align: 'center',
                        editor: { type: 'numberbox' }
                    },
                    {
                        field: '机台编码',
                        title: '加工机台',
                        width: 100,
                        align: 'center',
                        //formatter: function (value, row, index) {
                        //    return row.设备名称
                        //},
                        editor: {
                            type: 'combobox',
                            options: {
                                delary: 1000,
                                url: '/ERP_Process/ShowEquipment',
                                textField: 'Key',
                                valueField: 'Id',
                                panelWidth: 150,
                                panelHeight:120,
                                onLoadSuccess: function () {
                                    var element = $('#ERP_process').datagrid('getEditor', { index: config.optionConfig.editRow, field: '机台编码' });
                                    var value = $(element.target).combobox('getText');
                                    var data = $(element.target).combobox('getData');
                                    if (value != null && value != '') {
                                        var id = Search(data, value);
                                        $(element.target).combobox('setValue', id);
                                    }

                                }
                            }
                        }
                    }
                ]],
                optionConfig: {
                    dom: {
                        add:'#process_tb_add',
                        edit: '#process_tb_edit',
                        remove: '#process_tb_remove',
                        revort: '#process_tb_revort',
                        save: '#process_tb_save',
                        export:'#process_tb_excel'
                    },
                    datagrid: '#ERP_process',
                    savebutton: '#process_tb_save',
                    removeUrl: '/ERP_Process/DeleteProcess',
                    exportUrl: '/ERP_Process/ExportExcel',
                    editRow: undefined
                }
            };
            operation.datagrid(config);
            operation.initCombobox('#process_category', '/ERP_Process/GetCategory');
            $.extend($.fn.validatebox.defaults.rules, {
                processNoAjax: {
                    validator: function (value) {
                        var a = true;
                        var flag = $('#process_tb_save').attr('flag');
                        if (flag == 'add') {
                            $.ajax({
                                type: "post",
                                async: false,
                                url: '/ERP_Process/CheckProcessNo',
                                data: { 'processNo': value },
                                success: function (data) {
                                    if (data.toLowerCase() == "true") {
                                        a = false;
                                    }
                                }
                            });
                        }
                        return a;
                    },
                    message: '工艺编码已存在'
                },
            })
            operation.index(config.optionConfig);
            $('#process_category').combobox({
                onSelect: function (r) {
                    $(config.dom).datagrid({ url: '/ERP_Process/GetProcess', queryParams: { pCategory: r.Id } });
                }
            });
            $('#process_tb_print').click(function () {
                var page = $(config.dom).datagrid("options").pageNumber;
                var rows = $(config.dom).datagrid("options").pageSize;
                var queryParams = $(config.dom).datagrid('options').queryParams;
                if (isEmptyObject(queryParams)) { return; }
                var strUrl = '';
                for (var item in queryParams) {
                    if (typeof (queryParams[item]) == 'object' || typeof (queryParams[item]) == 'function') { continue };
                    strUrl += '&' + item + '=' + queryParams[item];
                }
                strUrl = '/ERP_Process/PrintPage?page=' + page + '&rows=' + rows + strUrl + '&d=' + Math.random();
                 //location.href = strUrl;
               $('#printIframe').attr('src', strUrl);
             
            });
           
        }

    }
    //判断对象是否为空
    function isEmptyObject(e) {
        var t;
        for (t in e)
            return !1;
        return !0
    }
    function formatterOptions(v) {
        var html = '';
        if (v == 1) {
            html = '<span style="color:#00ff21">√</span>';
        } else {
            html = '<span style= "color:#ff0000">×</span>';
        }
        return html
    }
    function Search(arr, value) {
        for (var i in arr) {
            if (arr[i].Key == value) {
                return arr[i].Id;
            }
        }
    }
   

})