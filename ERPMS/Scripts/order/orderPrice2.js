define(['jquery','order/order.service','tool/toolbar.service','tool/initCombobox'], function ($,server,toolbar,initCombobox) {
    return {
        index: function () {
            /**
             * combobox
             */
            var comboboxConfig = [
                { dom: '#orderPrice2_customer', url: '/ERP_Order/GetCustormer2' },
                { dom: '#orderPrice2_staff', url: '/ERP_Order/GetStaff' },
                { dom: '#orderPrice2_pCategory', url: '/ERP_Order/PCategory' }
            ];
            initCombobox.initCombobox(comboboxConfig);
            /**
            翻页配置
            */
            var runPageConfig = {
                before: '#orderPrice2_pre',
                next: '#orderPrice2_next',
                total: '#orderPrice2_total',
                page: '#orderPrice2_page',
                runPageUrl: '/ERP_OrderPrice/ShowOrderMaster',
                form: '#orderPrice2_form',
                text: '#orderPrice2_orderNo_text',
                no:'#orderPrice2_orderNo',
                auditorPerson:1,
                list: [{ datagrid: '#orderPrice2_datagrid', url: '/ERP_OrderPrice/ShowOrderJiJia', param_No: '#orderPrice2_orderNo' }],
                menuArr: [{ menu: '#orderPrice2_menu', items: ['#orderPrice2_menu_edit', '#orderPrice2_menu_save', '#orderPrice2_menu_remove'] }],
                linkbuttonArr: ['#orderPrice2_import', '#orderPrice2_edit', '#orderPrice2_remove', '#orderPrice2_auditor']
            };
            toolbar.nextPage(runPageConfig);
            toolbar.beforePage(runPageConfig);
           /*
            审核配置
            */

            var auditorConfig = {
                auditor: '#orderPrice2_auditor',
                unAuditor: '#orderPrice2_unauditor',
                paramNo: '#orderPrice2_orderNo',
                auditorNo: '#orderPrice2_auditorNo',
                form: '#orderPrice2_form',
                auditorUrl: '/ERP_OrderPrice/AuditorJijia',
                unAuditorUrl: '/ERP_OrderPrice/UnAuditorJijia',
                menuArr: [{ menu: '#orderPrice2_menu', items: ['#orderPrice2_menu_edit', '#orderPrice2_menu_save', '#orderPrice2_menu_remove'] }],
                linkbuttonArr: ['#orderPrice2_import', '#orderPrice2_edit', '#orderPrice2_remove', '#orderPrice2_auditor']
            };
            toolbar.auditor(auditorConfig);
            toolbar.unAuditor(auditorConfig);
            /**
             * datagrid 
             */
            var orderPriceConfig = {
                dom: '#orderPrice2_datagrid',

                optionUrl: '/ERP_OrderPrice/UpdateJijia',
                flag: '#orderPrice2_save',
                menu: '#orderPrice2_menu',
                merge: ['统计编码'],
                calculation: {
                    材料: 'orderPrice2_cailiao', 手工: 'orderPrice2_shougong', 加工: 'orderPrice2_jiagong', 覆膜: 'orderPrice2_fumo', 印前: 'orderPrice2_yinqian', 印刷: 'orderPrice2_yinshua', 税费: 'orderPrice2_shuifei', 返款: 'orderPrice2_fankuan', 合计: 'orderPrice2_count', 其他: 'orderPrice2_qita'
                },
                columns: [[
                    { field: 'ck', checkbox: true },
                    { field: 'ID', title: 'ID', hidden: true },
                    { field: '订单号', title: '订单号', hidden: true },
                    { field: '订单Detail号', title: '订单Detail号', hidden: true },
                    { field: '订单工艺号', title: '订单工艺号', hidden: true },
                    { field: '订单材料号', title: '订单材料号', hidden: true },
                    {
                        field: '标准单价',
                        title: '标准单价', hidden: true,
                        editor:
                        {
                            type: 'numberbox',
                            options: { precision: 2 }
                        }
                    },
                    {
                        field: '统计编码',
                        title: '类别',
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
                        field: '印品部件',
                        title: '印品部件',
                        align: 'center',
                        width: 100,
                        editor: {
                            type: "combobox",
                            options: {
                                url: '/ERP_Order/GetParts',
                                textField: 'Key',
                                valueField: 'Id',
                                panelHeight: 150,
                                required: true,
                                filter: function (q, r) {
                                    var opts = $(this).combobox('options');
                                    return r[opts.textField].indexOf(q) >= 0;
                                },
                            }
                        }
                    },
                    {
                        field: '项目编码',
                        title: '项目名称',
                        align: 'center',
                        width: 100,
                        formatter: function (v, r, i) {
                            return r.项目名称;
                        },
                        editor: {
                            type: 'combobox',
                            options: {
                                url: '/ERP_Order/GetProcess',
                                textField: 'Key',
                                valueField: 'Id',
                                panelWidth: 150,
                                panelHeight: 150,
                                required: true,
                                onSelect: function (record) {
                                    var element = $(orderPriceConfig.dom).datagrid('getEditor', { index: orderPriceConfig.optionConfig.editRow, field: '统计编码' });
                                    $(element.target).textbox('setValue', record.Flag);
                                },
                                filter: function (q, r) {
                                    var opts = $(this).combobox('options');
                                    return r[opts.textField].indexOf(q) >= 0;
                                },
                            }

                        }
                    },
                    {
                        field: '计量单位',
                        title: '单位',
                        align: 'center',
                        width: 100,
                        editor: {
                            type: 'combobox',
                            options: {
                                url: '/ERP_Order/GetUnit',
                                textField: 'Key',
                                valueField: 'Id',
                                panelHeight: 150,
                            }
                        }
                    },
                    {
                        field: '单价',
                        title: '单价',
                        align: 'center',
                        width: 100,
                        formatter: function (v, r, i) {
                            if (r.单价 < r.标准单价) {
                                return '<span style="color:red;">' + (v == null ? 0 : v) + '</sapn>'
                            } else {
                                return v
                            }
                        },
                        editor: {
                            type: 'numberbox',
                            options: {
                                required: true,
                                precision: 2,
                                onChange: function (n, o) {
                                    var element = $(orderPriceConfig.dom).datagrid('getEditor', { index: orderPriceConfig.optionConfig.editRow, field: '数量' });
                                    var unit = $(element.target).numberbox('getValue');
                                    var total = n * unit;
                                    element = $(orderPriceConfig.dom).datagrid('getEditor', { index: orderPriceConfig.optionConfig.editRow, field: '金额' });
                                    $(element.target).numberbox('setValue', total);
                                    element = $(orderPriceConfig.dom).datagrid('getEditor', { index: orderPriceConfig.optionConfig.editRow, field: '标准单价' });
                                    var biaozhun = $(element.target).numberbox('getValue');
                                    //if (n < biaozhun) {
                                    //    console.log(n + '单价:')
                                    //    console.log('old' + o)
                                    //    console.log($(this));
                                    //    $(this).attr('style', 'color:red')
                                    //    //var editors = $('#order_jijia').datagrid('getEditors', jijiaEditRow);
                                    //    //for (var i in editors) {
                                    //    //    console.log('小于标准单价的加上css')
                                    //    //    var editor = editors[i]
                                    //    //    $(editor.target).css('color','red');
                                    //    //}
                                    //} else {
                                    //    console.log (n)
                                    //    console.log ('else excute ')
                                    //    $(this).attr('style', 'color:#000000')
                                    //}

                                }
                            }
                        }
                    },
                    {
                        field: '数量',
                        title: '数量',
                        align: 'center',
                        width: 100,
                        editor: {
                            type: 'numberbox',
                            options: {
                                required: true,
                                precision: 2,
                                onChange:function(n, o) {
                                    var element = $(orderPriceConfig.dom).datagrid('getEditor', { index: orderPriceConfig.optionConfig.editRow, field: '单价' });
                                    var price = $(element.target).numberbox('getValue');
                                    var total = n * price;
                                    element = $(orderPriceConfig.dom).datagrid('getEditor', { index: orderPriceConfig.optionConfig.editRow, field: '金额' });
                                    $(element.target).numberbox('setValue', total);
                                }

                            }
                        },
                        formatter: function (v, r, i) {
                            if (r.单价 < r.标准单价) {
                                return '<span style="color:red;">' + (v == null ? '' : v) + '</sapn>'
                            } else {
                                return v;
                            }
                        }
                    },
                    {
                        field: '系数',
                        title: '系数',
                        align: 'center',
                        width: 100,
                        editor: {
                            type: 'numberbox',
                            options: {
                                precision: 0
                            }
                        },
                        formatter: function (v, r, i) {
                            if (r.单价 < r.标准单价) {
                                return '<span style="color:red;">' + (v == null ? '' : v) + '</sapn>'
                            } else {
                                return v;
                            }
                        }
                    },
                    {
                        field: '金额',
                        title: '金额',
                        align: 'center',
                        width: 100,
                        editor: {
                            type: 'numberbox',
                            options: {
                                precision: 2
                            }
                        },
                        formatter: function (v, r, i) {
                            if (r.单价 < r.标准单价) {
                                return '<span style="color:red;">' + (v == null ? '' : v) + '</sapn>'
                            } else {
                                return v;
                            }
                        }
                    },
                ]],
              
                optionConfig: {
                    dom: {
                        // add: '#order_paper_add',
                        edit: '#orderPrice2_menu_edit',
                        save: '#orderPrice2_menu_save',
                        remove: '#orderPrice2_menu_remove',
                        revort: '#orderPrice2_menu_revort',
                    },
                    datagrid: '#orderPrice2_datagrid',
                    removeUrl: '/ERP_OrderPrice/DeleteJijia',
                    savebutton: '#orderPrice2_save',
                    paramNo: '#orderPrice2_orderNo',
                    editRow: undefined
                }
            };
            server.datagrid(orderPriceConfig);
            server.datagridOption(orderPriceConfig.optionConfig);

            var initPageConfig = {
                form: '#orderPrice2_form',
                text: '#orderPrice2_orderNo_text',
                datagrid: '#orderPrice2_datagrid',
                initPageUrl: '/ERP_OrderPrice/InitPage',
                menuArr: [{ menu: '#orderPrice2_menu', items: ['#orderPrice2_menu_edit', '#orderPrice2_menu_save', '#orderPrice2_menu_remove'] }],
                linkbuttonArr: ['#orderPrice2_import', '#orderPrice2_edit', '#orderPrice2_remove', '#orderPrice2_auditor'],
                flag: true
            };
            InitPageData(initPageConfig)

            var optionConfig = {
                dom: {
                    edit: '#orderPrice2_edit',
                    remove: '#orderPrice2_remove',
                    refresh: '#orderPrice2_refresh'
                },
                editUrl: '/ERP_OrderPrice/UpdateOrder',
                refreshUrl: '/ERP_OrderPrice/RefreshOrderMaster',
                removeUrl: '/ERP_OrderPrice/DeleteOrder',
                no: '#orderPrice2_orderNo',
                text: '#orderPrice2_orderNo_text',
                form: '#orderPrice2_form',
                datagridList: [{ datagrid: '#orderPrice2_datagrid', url: '/ERP_OrderPrice/ShowOrderJiJia', param_No: '#orderPrice2_orderNo' }]
            };
            toolbar.edited(optionConfig);
            toolbar.removed(optionConfig);
            toolbar.refresh(optionConfig);
            $('#orderPrice2_import').click(function () {
                var orderNo = $('#orderPrice2_orderNo').val();
                if (orderNo == null || orderNo == '' || orderNo == undefined) { return; }
                toolbar.ajaxOption('/ERP_OrderPrice/ImportJijia', { orderNo: orderNo }, function (response) {
                    if (response.success == 0) {
                        toolbar.showMsg(response.msg);
                    } else {
                        $.messager.alert('提示', response.msg, 'info');
                    }
                })
            });

            $('#orderPrice2_offer').click(function () {
                var orderNo = $('#orderPrice2_orderNo').val();
                if (orderNo == null || orderNo == '') { return; };
                //toolbar.ajaxOption()
                var dialogConfig = { dom: '#orderPriceOffer2_dialog', title: '报价单', width: 900, height: 565, href: '/ERP_OrderPriceOffer/Index?orderNo=' + orderNo };
                toolbar.initdialog(dialogConfig);
            })
        }
    }
    function InitPageData(config) {
        $.post(config.initPageUrl, function (response) {
            if (response != null) {
                $(config.form).form('load', response);
                $(config.text).html(response.订单号)
                $(config.datagrid).datagrid('options').url = "/ERP_OrderPrice/ShowOrderJiJia";
                $(config.datagrid).datagrid('reload', { paramNo: response.订单号 });
                if (response.计价审核人编码 != null && response.计价审核人编码 != '') {
                    toolbar.controlMenuAndbutton(config);
                }
            }
        }, 'json')
    }
})