define(['jquery', 'order/order.service', 'tool/toolbar.service', 'tool/initCombobox'], function ($,server,toolbar,initCombobox) {
    return {
        index: function () {
            /**
             * combobox
             */
            var comboboxConfig = [
                { dom: '#orderPrice_customer', url: '/ERP_Order/GetCustormer2' },
                { dom: '#orderPrice_staff', url: '/ERP_Order/GetStaff' },
                { dom: '#orderPrice_pCategory', url: '/ERP_Order/PCategory' }
            ];
            initCombobox.initCombobox(comboboxConfig);
            /**
            翻页配置
            */
            var runPageConfig = {
                before: '#orderPrice_pre',
                next: '#orderPrice_next',
                total: '#orderPrice_total',
                page: '#orderPrice_page',
                runPageUrl: '/ERP_OrderPrice/ShowOrderMaster',
                form: '#orderPrice_form',
                text: '#orderPrice_orderNo_text',
                no:'#orderPrice_orderNo',
                auditorPerson:1,
                list: [{ datagrid: '#orderPrice_datagrid', url: '/ERP_OrderPrice/ShowOrderJiJia', param_No: '#orderPrice_orderNo' }],
                menuArr: [{ menu: '#orderPrice_menu', items: ['#orderPrice_menu_edit', '#orderPrice_menu_save', '#orderPrice_menu_remove'] }],
                linkbuttonArr: ['#orderPrice_import', '#orderPrice_edit', '#orderPrice_remove', '#orderPrice_auditor']
            };
            toolbar.nextPage(runPageConfig);
            toolbar.beforePage(runPageConfig);
           /*
            审核配置
            */

            var auditorConfig = {
                auditor: '#orderPrice_auditor',
                unAuditor: '#orderPrice_unauditor',
                paramNo: '#orderPrice_orderNo',
                auditorNo: '#orderPrice_auditorNo',
                form: '#orderPrice_form',
                auditorUrl: '/ERP_OrderPrice/AuditorJijia',
                unAuditorUrl: '/ERP_OrderPrice/UnAuditorJijia',
                menuArr: [{ menu: '#orderPrice_menu', items: ['#orderPrice_menu_edit', '#orderPrice_menu_save', '#orderPrice_menu_remove'] }],
                linkbuttonArr: ['#orderPrice_import', '#orderPrice_edit', '#orderPrice_remove', '#orderPrice_auditor']
            };
            toolbar.auditor(auditorConfig);
            toolbar.unAuditor(auditorConfig);
            /**
             * datagrid 
             */
            var orderPriceConfig = {
                dom: '#orderPrice_datagrid',

                optionUrl: '/ERP_OrderPrice/UpdateJijia',
                flag: '#orderPrice_save',
                menu: '#orderPrice_menu',
                merge: ['统计编码'],
                calculation: {
                    材料: 'orderPrice_cailiao', 手工: 'orderPrice_shougong', 加工: 'orderPrice_jiagong', 覆膜: 'orderPrice_fumo', 印前: 'orderPrice_yinqian', 印刷: 'orderPrice_yinshua', 税费: 'orderPrice_shuifei', 返款: 'orderPrice_fankuan', 合计: 'orderPrice_count', 其他: 'orderPrice_qita'
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
                                onChange:function(n, o){
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
                        edit: '#orderPrice_menu_edit',
                        save: '#orderPrice_menu_save',
                        remove: '#orderPrice_menu_remove',
                        revort: '#orderPrice_menu_revort',
                    },
                    datagrid: '#orderPrice_datagrid',
                    removeUrl: '/ERP_OrderPrice/DeleteJijia',
                    savebutton: '#orderPrice_save',
                    paramNo: '#orderPrice_orderNo',
                    editRow: undefined
                }
            };
            server.datagrid(orderPriceConfig);
            server.datagridOption(orderPriceConfig.optionConfig);

          
            /*

            */
            $('#orderPrice_import').unbind('click').click(function () {
                var orderNo = $('#order_price_orderNo').val();
                if (orderNo == null || orderNo == '' || orderNo == undefined) { return; }
                server.ajaxOperation('/ERP_OrderPrice/ImportJijia', { orderNo: orderNo }, function (response) {
                    if (response.success == 0) {
                        server.showMsg(response.msg);
                    } else {
                        $.messager.alert('提示', response.msg, 'info');
                    }
                })
            });

            $('#orderPrice_offer').click(function () {
                console.log ('加载几次 orderPrice_offer')
                var orderNo = $('#orderPrice_orderNo').val();
                if (orderNo == null || orderNo == '') { return; };
                //toolbar.ajaxOption()
                var dialogConfig = { dom: '#orderPriceOffer_dialog', title: '报价单', width: 900, height: 565, href: '/ERP_OrderPriceOffer/Index?orderNo=' + orderNo };
                toolbar.initdialog(dialogConfig);
            });
            //$('#orderPrice_print').click(function () {
            //    printJS('orderPrice_datagrid','html')
            //})
            var optionConfig = {
                dom: {
                    edit: '#orderPrice_edit',
                    remove: '#orderPrice_remove',
                    refresh: '#orderPrice_refresh'
                },
                editUrl: '/ERP_OrderPrice/UpdateOrder',
                refreshUrl: '/ERP_OrderPrice/RefreshOrderMaster',
                removeUrl: '/ERP_OrderPrice/DeleteOrder',
                no: '#orderPrice_orderNo',
                text: '#orderPrice_orderNo_text',
                form: '#orderPrice_form',
                datagridList: [{ datagrid: '#orderPrice_datagrid', url: '/ERP_OrderPrice/ShowOrderJiJia', param_No: '#orderPrice_orderNo' }]
            };
            toolbar.edited(optionConfig);
            toolbar.removed(optionConfig);
            toolbar.refresh(optionConfig);
        }
    }

})