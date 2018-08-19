define(['jquery', 'tool/initCombobox', 'tool/toolbar.service','Basics/basics_datagrid'], function ($, initCom, toolbar, operation) {

    return {
        index: function () {
            var comboboxConfig = [
                { dom: '#orderMaterial_saleman', url: '/ERP_Order/GetStaff' },
                { dom: '#orderMaterial_pcategory', url: '/ERP_Order/PCategory' },
                { dom: '#orderMaterial_customer', url: '/ERP_Order/GetCustormer2' }
            ];
            initCom.initCombobox(comboboxConfig);

            var datagridConfig = {
                dom: '#orderMaterial_datagrid',
                tool: '',
                initUrl: '',
                optionUrl: '/ERP_OrderDetailMaterial/UpdateOrderMaterial',
                flag: '#process_tb_save',
                menu: '#orderMaterial_menu',
                columns: [[
                    { field: 'ck', checkbox: true },
                    { field: '行号', title: '行号', hidden: true },
                    { field: '订单号', title: '订单号', hidden: true },
                    {
                        field: '印品部件',
                        title: '印品部件',
                        align: 'center',
                        width: 100

                    },
                    {
                        field: '材料编码',
                        title: '材料名称',
                        align: 'center',
                        width: 150,
                        formatter: function (i, d) {
                            return '<span>' + d.材料名称 + '</span>'
                        },
                        editor: {
                            type: 'combobox',
                            options: {
                                // delay: 400,
                                url: '/ERP_Order/GetMaterial',
                                textField: 'Key',
                                valueField: 'Id',
                                required: true,
                                onSelect: function (rec) {
                                    var element = $(datagridConfig.dom).datagrid('getEditor', { index: datagridConfig.optionConfig.editRow, field: '规格型号' });
                                    $(element.target).textbox('setValue', rec.spec);
                                },
                            }

                        }
                    },
                    {
                        field: '规格型号',
                        title: '规格型号',
                        align: 'center',
                        width: 100,
                        editor: {
                            type: 'textbox',
                            options: {
                                readonly: true
                            }
                        }
                    },
                    {
                        field: '计量单位',
                        title: '计量单位',
                        align: 'center',
                        width: 90,
                        editor: {
                            type: 'combobox',
                            options: {
                                url: '/Order/GetMaterialUnit',
                                textField: 'Key',
                                valueField: 'Id',
                            }
                        }
                    },
                    {
                        field: '数量',
                        title: '数量',
                        align: 'center',
                        width: 70,
                        editor: {
                            type: 'numberbox',
                            options: {
                                precision: 2,
                                // required: true
                            }
                        }
                    }
                ]],
                optionConfig: {
                        dom: {
                            edit: '#orderMaterial_menu_edit',
                            remove: '#orderMaterial_menu_remove',
                            revort: '#orderMaterial_menu_revort',
                            save: '#orderMaterial_menu_save',
                        },
                        datagrid: '#orderMaterial_datagrid',
                        savebutton: '#orderMaterial_menu_save',
                        removeUrl: '/ERP_OrderDetailMaterial/DeleteOrderMaterial',
                        editRow: undefined
                    }
            };
            operation.datagrid(datagridConfig);
            operation.datagridOption(datagridConfig.optionConfig)

            var runPageConfig = {
                before: '#orderMaterial_pre',
                next: '#orderMaterial_next',
                total: '#orderMaterial_total',
                page: '#orderMaterial_page',
                runPageUrl: '/ERP_OrderDetailMaterial/RunPage',
                form: '#orderMaterial_form',
                text: '#orderMaterial_text',
                no: '#orderMaterial_orderNo',
                auditorPerson: 0,
                list: [{ datagrid: '#orderMaterial_datagrid', url: '/ERP_OrderDetailMaterial/GetMaterial', param_No: '#orderMaterial_orderNo' }],
                menuArr: [{ menu: '#orderMaterial_menu', items: ['#orderMaterial_menu_edit', '#orderMaterial_menu_save', '#orderMaterial_menu_remove'] }],
                linkbuttonArr: []
            };
            toolbar.beforePage(runPageConfig);
            toolbar.nextPage(runPageConfig);


            var initPageConfig = {
                form: '#orderMaterial_form',
                datagrid: '#orderMaterial_datagrid',
                initPageUrl: '/ERP_OrderDetailMaterial/InitPage',
                menuArr: [{ menu: '#orderMaterial_menu', items: ['#orderMaterial_menu_edit', '#orderMaterial_menu_save', '#orderMaterial_menu_remove'] }],
                linkbuttonArr: [],
                flag: true
            };
            InitPageData(initPageConfig)
        }
    }
    function InitPageData(config) {
        $.post(config.initPageUrl, function (response) {
            if (response != null) {
                $(config.form).form('load', response);
                $(config.datagrid).datagrid('options').url = "/ERP_OrderDetailMaterial/GetMaterial";
                $(config.datagrid).datagrid('reload', { paramNo: response.订单号 });
                if (response.审核人编码 != null && response.审核人编码 != '') {
                    toolbar.controlMenuAndbutton(config);
                }
            }
        }, 'json')
    }
})