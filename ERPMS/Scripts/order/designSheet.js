define(['jquery', 'order/datagridOption.server', 'tool/initCombobox', 'tool/toolbar.service'], function ($, server, initcombobox, toolbar) {
    return {
        index: function () {
            var comboboxConfig = [
                { dom: '#designMaster2_saleMan', url: '/ERP_Order/GetStaff' },
                { dom: '#designMaster2_pCategory', url: '/ERP_Order/PCategory' },
                { dom: '#designMaster2_customer', url: '/ERP_Order/GetCustormer' },
                { dom: '#designMaster2_orderNos', url: '/ERP_OrderDesign/GetOrder' },
                { dom: '#designMaster2_openNumber', url: '/ERP_Order/GetOpenNumber' },
                { dom: '#designMaster2_zhidan', url: '' },
                { dom: '#designMaster2_auditorNo', url: '' }
            ];
            initcombobox.initCombobox(comboboxConfig);
            $('#designMaster2_saleMan').combobox({
                onLoadSuccess: function () {
                    var data = $(this).combobox('getData');
                    $('#designMaster2_zhidan').combobox('loadData', data);
                    $('#designMaster2_auditorNo').combobox('loadData', data);
                }
            });

            var designConfig = {
                dom: '#designMaster2_datagrid',
                optionUrl: '/ERP_OrderDesign/SaveOrUpdateDesignDetail',
                flag: '#designMaster2_menu_save',
                menu: '#designMaster2_menu',
                columns: [[
                    { field: 'ck', checkbox: true },
                    { field: '行号', title: '行号', hidden: true },
                    //订单号 是设计订单_Master 表中的订单号
                    { field: '订单号', title: '订单号', hidden: true },
                    { field: '项目编码', title: '工艺', hidden: true },

                    {
                        field: '数量', title: '数量', align: 'center', width: 120,
                        editor: {
                            type: 'numberbox',
                            options: {
                                precision: 0,
                                required: true,
                                onChange: function (n, o) {
                                    var element = $(designConfig.dom).datagrid('getEditor', { index: designConfig.optionConfig.editRow, field: '单价' });
                                    var value = $(element.target).numberbox('getValue');
                                    element = $(designConfig.dom).datagrid('getEditor', { index: designConfig.optionConfig.editRow, field: '金额' });
                                    $(element.target).textbox('setValue', (n * value));
                                }

                            }
                        }
                    },
                    {
                        field: '单价',
                        title: '单价',
                        align: 'center',
                        width: 120,
                        editor: {
                            type: 'numberbox',
                            options: {
                                required: true,
                                onChange: function (n, o) {
                                    var element = $(designConfig.dom).datagrid('getEditor', { index: designConfig.optionConfig.editRow, field: '数量' });
                                    var value = $(element.target).numberbox('getValue');
                                    element = $(designConfig.dom).datagrid('getEditor', { index: designConfig.optionConfig.editRow, field: '金额' });
                                    $(element.target).textbox('setValue', (n * value));
                                }
                            }
                        }

                    },
                    {
                        field: '金额',
                        title: '金额',
                        align: 'center',
                        width: 80,
                        editor: {
                            type: 'textbox',
                        }
                    },
                    {
                        field: '完工时间',
                        title: '完成时间',
                        align: 'center',
                        width: 80,
                        editor: {
                            type: 'datebox',

                        }
                    },

                    {
                        field: '完成数量',
                        title: '完工数量',
                        align: 'center',
                        width: 80,

                        editor: {
                            type: 'numberbox',
                            options: {
                                precision: 2,

                            }
                        }
                    }
                ]],
                // list: [{ datagrid: '#orderLogistics_datagrid', url: '/ERP_OrderLogistics/ShowLogisticsDetail', param_No: '#orderLogistics_logisticsNo' }],
                optionConfig: {
                    dom: {
                        add: '#designMaster2_menu_add',
                        edit: '#designMaster2_menu_edit',
                        save: '#designMaster2_menu_save',
                        remove: '#designMaster2_menu_remove',
                        revort: '#designMaster2_menu_revort',
                    },
                    datagrid: '#designMaster2_datagrid',
                    removeUrl: '/ERP_OrderDesign/DeleteDesignDetail',
                    savebutton: '#designMaster2_menu_save',
                    paramNo: '#designMaster2_orderNo',
                    editRow: undefined
                }
            };
            server.datagrid(designConfig);
            $(designConfig.dom).datagrid({
                onAfterEdit: function (i, d, c) {
                    var flags = $(designConfig.flag).attr('flag');
                    var val = $(designConfig.optionConfig.paramNo).val();
                    if (flags == 'add') {
                        if (val == null || val == '') {
                            $.messager.alert('提示', '没获取到申请单号', 'info');
                            $(designConfig.dom).datagrid('rejectChanges');
                            designConfig.optionConfig.editRow = undefined;
                            return;
                        }
                        d.订单号 = val;
                    }
                    $.ajax({
                        url: designConfig.optionUrl,
                        type: 'post',
                        data: { data: d, flag: flags },
                        success: function (response) {
                            if (response.success == 0) {
                                d.行号 = response.id;
                                designConfig.optionConfig.editRow = undefined;
                                server.showMsg(response.msg);

                            } else {
                                $.messager.alert('提示', response.msg, 'info');
                                $(designConfig.dom).datagrid('beginEdit', i);
                            }
                        },
                        error: function (XMLHttpRequest, testStatus, errorThrown) {
                            $.messager.alert('My Title', XMLHttpRequest.status, 'warning')
                        }
                    })
                },
                onLoadSuccess: function () {
                    designConfig.optionConfig.editRow = undefined;

                }
            });
            server.datagridOption(designConfig.optionConfig)

            var runPageConfig = {
                before: '#designMaster2_pre',
                next: '#designMaster2_next',
                total: '#designMaster2_total',
                page: '#designMaster2_page',
                runPageUrl: '/ERP_OrderDesign/RunPage',
                form: '#designMaster2_form',
                text: '#designMaster2_orderNo_text',
                no: '#designMaster2_orderNo',
                auditorPerson: 0,
                list: [{ datagrid: '#designMaster2_datagrid', url: '/ERP_OrderDesign/ShowDesignDetail', param_No: '#designMaster2_orderNo' }],
                menuArr: [{ menu: '#designMaster2_menu', items: ['#designMaster2_menu_add', '#designMaster2_menu_edit', '#designMaster2_menu_save', '#designMaster2_menu_remove'] }],
                linkbuttonArr: ['#designMaster2_edit', '#designMaster2_remove', '#designMaster2_save', '#designMaster2_auditor']
            };
            toolbar.nextPage(runPageConfig);
            toolbar.beforePage(runPageConfig);

            var auditorConfig = {
                auditor: '#designMaster2_auditor',
                unAuditor: '#designMaster2_unauditor',
                paramNo: '#designMaster2_orderNo',
                auditorNo: '#designMaster2_auditorNo',
                auditorTime: '#designMaster2_auditorTime',
                auditorPerson: '#designMaster2_auditorNo',
                form: '#designMaster2_form',
                auditorUrl: '/ERP_OrderDesign/AuditorDesign',
                unAuditorUrl: '/ERP_OrderDesign/UnAuditorDesign',
                menuArr: [{ menu: '#designMaster2_menu', items: ['#designMaster2_menu_add', '#designMaster2_menu_edit', '#designMaster2_menu_save', '#designMaster2_menu_remove'] }],
                linkbuttonArr: ['#designMaster2_edit', '#designMaster2_remove', '#designMaster2_save', '#designMaster2_auditor']
            };
            toolbar.auditor(auditorConfig);
            toolbar.unAuditor(auditorConfig);

            var optionConfig = {
                dom: {
                    add: '#designMaster2_add',
                    edit: '#designMaster2_edit',
                    remove: '#designMaster2_remove',
                    save: '#designMaster2_save'
                },
                editUrl: '/ERP_OrderDesign/UpdateDesignMaster',
                saveUrl: '/ERP_OrderDesign/AddDesignMaster',
                removeUrl: '/ERP_OrderDesign/DeleteDesignMaster',
                no: '#designMaster2_orderNo',
                text: '#designMaster2_orderNo_text',
                form: '#designMaster2_form',
                datagridList: [{ datagrid: '#designMaster2_datagrid', url: '', param_No: '' }],
                menuArr: [{ menu: '#designMaster2_menu', items: ['#designMaster2_menu_add', '#designMaster2_menu_edit', '#designMaster2_menu_save', '#designMaster2_menu_remove'] }],
                linkbuttonArr: ['#designMaster2_edit', '#designMaster2_remove', '#designMaster2_save', '#designMaster2_auditor']
            };
            toolbar.added(optionConfig);
            toolbar.edited(optionConfig);
            toolbar.removed(optionConfig);
            toolbar.saved(optionConfig);
        }
    }
})