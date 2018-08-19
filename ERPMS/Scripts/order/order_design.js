define(['jquery', 'order/datagridOption.server', 'tool/initCombobox', 'tool/toolbar.service'], function ($, server, initcombobox, toolbar) {

    return {
        index: function () {
            var comboboxConfig = [
                { dom: '#designMaster_saleMan', url: '/ERP_Order/GetStaff' },
                { dom: '#designMaster_pCategory', url: '/ERP_Order/PCategory' },
                { dom: '#designMaster_customer', url: '/ERP_Order/GetCustormer2' },
                { dom: '#designMaster_orderNos', url: '/ERP_OrderDesign/GetOrder' },
                { dom: '#designMaster_openNumber', url: '/ERP_Order/GetOpenNumber' },
                { dom: '#designMaster_zhidan', url: '' },
                { dom: '#designMaster_auditorNo', url: '' }
            ];
            initcombobox.initCombobox(comboboxConfig);
            $('#designMaster_saleMan').combobox({
                onLoadSuccess: function () {
                    var data = $(this).combobox('getData');
                    $('#designMaster_zhidan').combobox('loadData', data);
                    $('#designMaster_auditorNo').combobox('loadData', data);
                }
            });

            var designConfig = {
                dom: '#designMaster_datagrid',
                optionUrl: '/ERP_OrderDesign/SaveOrUpdateDesignDetail',
                flag: '#designMaster_menu_save',
                menu: '#designMaster_menu',
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
                        add: '#designMaster_menu_add',
                        edit: '#designMaster_menu_edit',
                        save: '#designMaster_menu_save',
                        remove: '#designMaster_menu_remove',
                        revort: '#designMaster_menu_revort',
                    },
                    datagrid: '#designMaster_datagrid',
                    removeUrl: '/ERP_OrderDesign/DeleteDesignDetail',
                    savebutton: '#designMaster_menu_save',
                    paramNo: '#designMaster_orderNo',
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
            server.datagridOption(designConfig.optionConfig);

            var initPageConfig = {
                form: '#designMaster_form',
                text: '#designMaster_orderNo_text',
                datagrid: '#designMaster_datagrid',
                initPageUrl: '/ERP_OrderDesign/GetDesignMaster',
                menuArr: [{ menu: '#designMaster_menu', items: ['#designMaster_menu_add', '#designMaster_menu_edit', '#designMaster_menu_save', '#designMaster_menu_remove'] }],
                linkbuttonArr: ['#designMaster_edit', '#designMaster_remove','#designMaster_save', '#designMaster_auditor'],
                flag:true
            };
            InitPageData(initPageConfig);

            var runPageConfig = {
                before: '#designMaster_pre',
                next: '#designMaster_next',
                total: '#designMaster_total',
                page: '#designMaster_page',
                runPageUrl: '/ERP_OrderDesign/RunPage',
                form: '#designMaster_form',
                text: '#designMaster_orderNo_text',
                no: '#designMaster_orderNo',
                auditorPerson: 0,
                list: [{ datagrid: '#designMaster_datagrid', url: '/ERP_OrderDesign/ShowDesignDetail', param_No: '#designMaster_orderNo' }],
                menuArr: [{ menu: '#designMaster_menu', items: ['#designMaster_menu_add', '#designMaster_menu_edit', '#designMaster_menu_save', '#designMaster_menu_remove'] }],
                linkbuttonArr: ['#designMaster_edit', '#designMaster_remove','#designMaster_save', '#designMaster_auditor']
            };
            toolbar.nextPage(runPageConfig);
            toolbar.beforePage(runPageConfig);

            var auditorConfig = {
                auditor: '#designMaster_auditor',
                unAuditor: '#designMaster_unauditor',
                paramNo: '#designMaster_orderNo',
                auditorNo: '#designMaster_auditorNo',
                auditorTime: '#designMaster_auditorTime',
                auditorPerson: '#designMaster_auditorNo',
                form: '#designMaster_form',
                auditorUrl: '/ERP_OrderDesign/AuditorDesign',
                unAuditorUrl: '/ERP_OrderDesign/UnAuditorDesign',
                menuArr: [{ menu: '#designMaster_menu', items: ['#designMaster_menu_add', '#designMaster_menu_edit', '#designMaster_menu_save', '#designMaster_menu_remove'] }],
                linkbuttonArr: ['#designMaster_edit', '#designMaster_remove', '#designMaster_save','#designMaster_auditor']
            };
            toolbar.auditor(auditorConfig);
            toolbar.unAuditor(auditorConfig);

            var optionConfig = {
                dom: {
                    add: '#designMaster_add',
                    edit: '#designMaster_edit',
                    remove: '#designMaster_remove',
                    save: '#designMaster_save'
                },
                editUrl: '/ERP_OrderDesign/UpdateDesignMaster',
                saveUrl: '/ERP_OrderDesign/AddDesignMaster',
                removeUrl: '/ERP_OrderDesign/DeleteDesignMaster',
                no: '#designMaster_orderNo',
                text: '#designMaster_orderNo_text',
                form: '#designMaster_form',
                datagridList: [{ datagrid: '#designMaster_datagrid', url: '', param_No: '' }],
                menuArr: [{ menu: '#designMaster_menu', items: ['#designMaster_menu_add', '#designMaster_menu_edit', '#designMaster_menu_save', '#designMaster_menu_remove'] }],
                linkbuttonArr: ['#designMaster_edit', '#designMaster_remove','#designMaster_save', '#designMaster_auditor']
            };
            toolbar.added(optionConfig);
            toolbar.edited(optionConfig);
            toolbar.removed(optionConfig);
            toolbar.saved(optionConfig);

        }

    }
    function InitPageData(config) {
        $.post(config.initPageUrl, function (response) {
            if (response != null) {
                $(config.form).form('load', response);
                $(config.text).html(response.订单号)
                $(config.datagrid).datagrid('options').url = "/ERP_OrderDesign/ShowDesignDetail";
                $(config.datagrid).datagrid('reload', { paramNo: response.订单号 });
                if (response.审核人编码 != null && response.审核人编码 != '') {
                    toolbar.controlMenuAndbutton(config);
                }
            }
        }, 'json')
    }
})