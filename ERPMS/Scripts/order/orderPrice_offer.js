define(['jquery', 'order/datagridOption.server','tool/initCombobox', 'tool/toolbar.service'], function ($, service, initcombobox,toolbar) {
    return {
        index: function () {

            var comboboxConfig = [
                { dom: '#orderPriceOffer_customer', url: '/ERP_Order/GetCustormer2' },
                { dom: '#orderPriceOffer_saleMan', url: '/ERP_Order/GetStaff' },
                { dom: '#orderPriceOffer_unit', url: '/ERP_Order/GetUnit' },
                { dom: '#orderPriceOffer_auditorNo', url: '/ERP_Order/GetStaff' }
            ];
            initcombobox.initCombobox(comboboxConfig);
            var offerDetailConfig = {
                dom: '#offerDetail_datagrid',
                optionUrl: 'ERP_OrderPriceOffer/SaveOrUpdateOfferDetail',
                columns: [[
                    { field: 'ck', checkbox: true },
                    { field: '行号', title: '行号', hidden: true },
                    { field: '报价单号', title: '报价单号', hidden: true },
                    {
                        field: '印品部件', title: '部件', align: 'center', width: 100, editor: {
                            type: 'combobox',
                            options: {
                                url: '/ERP_Order/GetParts',
                                textField: 'Key',
                                valueField: 'Id',
                                required: true
                            }
                        }
                    },
                    {
                        field: '纸张客户编码', title: '来源', align: 'center', width: 100, formatter: function (v, r, i) {
                            return r.客户名称;
                        }, editor: {
                            type: 'combobox',
                            options: {
                                url: '/ERP_Order/GetCustormer',
                                textField: 'Key',
                                valueField: 'Id',
                                required: true
                            }
                        }
                    },
                    {
                        field: '纸张编码', title: '纸张名称', align: 'center', width: 100, editor: {
                            type: 'combobox', options: {
                                //validType: ['length[1,15]'],
                                url: '/ERP_Order/GetPaper',
                                textField: 'Key',
                                valueField: 'Id',
                                required: true
                            }
                        }
                    }
                ]],
                flag: '#offerDetail_menu_save',
                menu: '#offerDetial_menu',
                optionConfig: {
                    dom: {
                        add: '#offerDetail_menu_add',
                        edit: '#offerDetail_menu_edit',
                        save: '#offerDetail_menu_save',
                        remove: '#offerDetail_menu_remove',
                        revort: '#offerDetail_menu_revort',
                    },
                    datagrid: '#offerDetail_datagrid',
                    removeUrl: 'ERP_OrderPriceOffer/DeleteDetail',
                    savebutton: '#offerDetail_menu_save',
                    paramNo: '#orderPriceOffer_offerNo',
                    editRow: undefined
                }
            }
            service.datagrid(offerDetailConfig);
            service.datagridOption(offerDetailConfig.optionConfig);
            $(offerDetailConfig.dom).datagrid({
                onAfterEdit: function (i, d, c) {
                    var flags = $(offerDetailConfig.flag).attr('flag');
                    var val = $(offerDetailConfig.optionConfig.paramNo).val();
                    if (flags == 'add') {
                        if (val == null || val == '') {
                            $.messager.alert('提示', '没获取到申请单号', 'info');
                            $(offerDetailConfig.dom).datagrid('rejectChanges');
                            datagridConfig.optionConfig.editRow = undefined;
                            return;
                        }
                        d.报价单号 = val;
                    }
                    $.ajax({
                        url: offerDetailConfig.optionUrl,
                        type: 'post',
                        data: { data: d, flag: flags },
                        success: function (response) {
                            if (response.success == 0) {
                                offerDetailConfig.optionConfig.editRow = undefined;
                                $(offerDetailConfig.dom).datagrid('reload');
                                service.showMsg(response.msg);

                            } else {
                                $.messager.alert('提示', response.msg, 'info');
                                $(offerDetailConfig.dom).datagrid('beginEdit', i);
                            }
                        },
                        error: function (XMLHttpRequest, testStatus, errorThrown) {
                            $.messager.alert('My Title', XMLHttpRequest.status, 'warning')
                        }
                    })
                },
                onLoadSuccess: function () {
                    offerDetailConfig.optionConfig.editRow = undefined;
                }
            });

            var offerPriceConfig = {
                dom: '#offerPrice_datagrid',
                optionUrl: 'ERP_OrderPriceOffer/SaveOrUpdateOfferPrice',
                flag: '#offerPrice_menu_save',
                columns: [[
                    { field: 'ck', checkbox: true },
                    { field: 'ID', title: 'ID', hidden: true },
                    { field: '报价单号', title: '报价单号', hidden: true },
                    {
                        field: 'flag', title: '合并单元', align: 'center', width: 100, formatter: function (v, r, i) {
                            return r.印品部件;
                        },
                    },
                    {
                        field: '印品部件', title: '印品部件', align: 'center', width: 100, editor: {
                            type: 'combobox',
                            options: {
                                url: '/ERP_Order/GetParts',
                                textField: 'Key',
                                valueField: 'Id',
                                required: true
                            }
                        }
                    },
                    { field: '项目编码', title: '项目名称', align: 'center', width: 100, editor: { type: 'textbox', options: { required: true } } },
                    {
                        field: '计量单位', title: '单位', align: 'center', width: 100, editor: {
                            type: 'combobox',
                            options: {
                                url: '/ERP_Order/GetUnit',
                                textField: 'Key',
                                valueField: 'Id',
                            }
                        }
                    },
                    {
                        field: '单价', title: '单价', align: 'center', width: 100,
                        editor: {
                            type: 'numberbox',
                            options: {
                                precision: 2,
                                required: true,
                                onChange: function (e, o) {
                                    var element = $(offerPriceConfig.dom).datagrid('getEditor', { index: offerPriceConfig.optionConfig.editRow, field: '数量' });
                                    var number = $(element.target).numberbox('getValue');
                                    var total = e * number;
                                    element = $(offerPriceConfig.dom).datagrid('getEditor', { index: offerPriceConfig.optionConfig.editRow, field: '金额' });
                                    $(element.target).numberbox('setValue', total);
                                }
                            }

                        }
                    },
                    {
                        field: '数量', title: '数量', align: 'center', width: 100, editor: {
                            type: 'numberbox',
                            options: {
                                precision: 0,
                                required: true,
                                onChange: function (e, o) {
                                    var element = $(offerPriceConfig.dom).datagrid('getEditor', { index: offerPriceConfig.optionConfig.editRow, field: '单价' });
                                    var price = $(element.target).numberbox('getValue');
                                    var total = e * price;
                                    element = $(offerPriceConfig.dom).datagrid('getEditor', { index: offerPriceConfig.optionConfig.editRow, field: '备注' });
                                    $(element.target).textbox('setValue', price+'*'+e);
                                    element = $(offerPriceConfig.dom).datagrid('getEditor', { index: offerPriceConfig.optionConfig.editRow, field: '金额' });
                                    $(element.target).numberbox('setValue', total);
                                }
                            }
                        }
                    },
                    {
                        field: '备注', title: '公式', align: 'center', width: 100, editor: {
                            type: 'textbox',
                            options: {
                                required: true
                            }
                        }
                    },
                    {
                        field: '金额', title: '金额', align: 'center', width: 100, editor: {
                            type: 'numberbox',
                            options: {
                                readonly: true
                            }
                        }
                    }

                ]],
                menu: '#offerPrice_menu',
                optionConfig: {
                    dom: {
                        add: '#offerPrice_menu_add',
                        edit: '#offerPrice_menu_edit',
                        save: '#offerPrice_menu_save',
                        remove: '#offerPrice_menu_remove',
                        revort: '#offerPrice_menu_revort',
                    },
                    datagrid: '#offerPrice_datagrid',
                    removeUrl: 'ERP_OrderPriceOffer/DeleteOfferPrice',
                    savebutton: '#offerPrice_menu_save',
                    paramNo: '#orderPriceOffer_offerNo',
                    editRow: undefined
                }
            }
            service.datagrid(offerPriceConfig);
            service.datagridOption(offerPriceConfig.optionConfig);

            $(offerPriceConfig.dom).datagrid({
                onAfterEdit: function (i, d, c) {
                    var flags = $(offerPriceConfig.flag).attr('flag');
                    var val = $(offerPriceConfig.optionConfig.paramNo).val();
                    if (flags == 'add') {
                        if (val == null || val == '') {
                            $.messager.alert('提示', '没获取到申请单号', 'info');
                            $(offerPriceConfig.dom).datagrid('rejectChanges');
                            offerPriceConfig.optionConfig.editRow = undefined;
                            return;
                        }
                        d.报价单号 = val;
                    }
                    $.ajax({
                        url: offerPriceConfig.optionUrl,
                        type: 'post',
                        data: { data: d, flag: flags },
                        success: function (response) {
                            if (response.success == 0) {
                                offerPriceConfig.optionConfig.editRow = undefined;
                                $(offerPriceConfig.dom).datagrid('reload');
                                service.showMsg(response.msg);

                            } else {
                                $.messager.alert('提示', response.msg, 'info');
                                $(offerPriceConfig.dom).datagrid('beginEdit', i);
                            }
                        },
                        error: function (XMLHttpRequest, testStatus, errorThrown) {
                            $.messager.alert('My Title', XMLHttpRequest.status, 'warning')
                        }
                    })
                },
                onLoadSuccess: function () {
                    offerPriceConfig.optionConfig.editRow = undefined;
                    $(offerPriceConfig.dom).datagrid("autoMergeCells", ['flag']);
                }
            });

            var initPageConfig = {
                form: '#orderPriceOffer_form',
                text: '#orderPriceOffer_offerNo_text',
                datagrid: [{ dom: '#offerDetail_datagrid', url: '/ERP_OrderPriceOffer/GetOfferDetail' }, { dom: '#offerPrice_datagrid', url: '/ERP_OrderPriceOffer/GetOfferPrice' }],
                initPageUrl: '/ERP_OrderPriceOffer/InitPage',
                menuArr: [{ menu: '#offerDetial_menu', items: ['#offerDetail_menu_add', '#offerDetail_menu_edit', '#offerDetail_menu_save', '#offerDetail_menu_remove'] },
                { menu: '#offerPrice_menu', items: ['#offerPrice_menu_add', '#offerPrice_menu_edit', '#offerPrice_menu_save', '#offerPrice_menu_remove'] }],
                linkbuttonArr: ['#orderPriceOffer_edit', '#orderPriceOffer_remove', '#orderPriceOffer_auditor'],
                flag: true
            };
            InitPageData(initPageConfig);

            var optionConfig = {
                dom: {
                    edit: '#orderPriceOffer_edit',
                    remove: '#orderPriceOffer_remove',
                },
                editUrl: '/ERP_OrderPriceOffer/Update',
                removeUrl: '/ERP_OrderPriceOffer/Delete',
                no: '#orderPriceOffer_offerNo',
                text: '#orderPriceOffer_offerNo_text',
                form: '#orderPriceOffer_form',
                datagridList: [{ datagrid: '#offerDetail_datagrid' }, { datagrid: '#offerPrice_datagrid'}]
            };
            toolbar.edited(optionConfig);
            toolbar.removed(optionConfig);

            var auditorConfig = {
                auditor: '#orderPriceOffer_auditor',
                unAuditor: '#orderPriceOffer_unauditor',
                paramNo: '#orderPriceOffer_offerNo',
                auditorNo: '#orderPriceOffer_auditorNo',
                form: '#orderPriceOffer_form',
                auditorUrl: '/ERP_OrderPriceOffer/Auditor',
                unAuditorUrl: '/ERP_OrderPriceOffer/UnAuditor',
                menuArr: [{ menu: '#offerDetial_menu', items: ['#offerDetail_menu_add', '#offerDetail_menu_edit', '#offerDetail_menu_save', '#offerDetail_menu_remove'] },
                          { menu: '#offerPrice_menu', items: ['#offerPrice_menu_add', '#offerPrice_menu_edit', '#offerPrice_menu_save', '#offerPrice_menu_remove'] }],
                linkbuttonArr: ['#orderPriceOffer_edit', '#orderPriceOffer_remove', '#orderPriceOffer_auditor'],
            };
            toolbar.auditor(auditorConfig);
            toolbar.unAuditor(auditorConfig);
        }
    }
    function InitPageData(config) {
        $.post(config.initPageUrl, function (response) {
            if (response != null) {
                $(config.form).form('load', response);
                $(config.text).html(response.订单号)
                var datagrid = config.datagrid;
                for (var i in datagrid) {
                    $(datagrid[i].dom).datagrid('options').url = datagrid[i].url;
                    $(datagrid[i].dom).datagrid('reload', { paramNo: response.报价单号 });
                }
              
                if (response.审核人编码 != null && response.审核人编码 != '') {
                    toolbar.controlMenuAndbutton(config);
                }
            }
        }, 'json')
    }
})