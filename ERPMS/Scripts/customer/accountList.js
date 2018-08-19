define(['jquery', 'tool/toolbar.service', 'Basics/basics_datagrid','tool/initCombobox'], function ($, toolbar, datagridOperation,initCom) {
    return {
        index: function () {
            $.extend($.fn.validatebox.defaults.rules, {
              
                dateCompare: {
                    validator: function (value, param) {
                        var rules = $.fn.validatebox.defaults.rules;
                        var p = $(param[0]).val();
                        console.log(p);
                        if (p >value) {
                            rules.dateCompare.message = '截止日期要大于开始日期';
                            return false;
                        }
                        return true;
                    },
                    message: ''
                },



            });
            var date = new Date();
            $('#accountList_date').datebox('setValue', myformatter(date));
            var comboboxConfig = [{ dom: '#accountList_customer', url: '/ERP_Order/GetCustormer' }];
            initCom.initCombobox(comboboxConfig);
            var datagridConfig = {
                dom: '#accountList_datagrid',
                initUrl: '',
                optionUrl: '/ERP_Customer/SaveOrUpdateAccountList',
                flag: '#accountList_menu_save',
                menu: '#accountList_menu',
                form: '#accountList_form',
                columns: [[
                    { field: 'ch', checkbox: true },
                    { field: '行号', hidden: true },
                    { field: '对账单号', hidden: true },
                    {
                        field: '订单号', title: '订单号', align: 'center', width: 100, editor: {
                            type: 'textbox',
                            options: {
                                required:true
                            }
                        }
                    },
                    {
                        field: '订单名称', title: '印品名称', align: 'center', width: 150, editor: {
                            type: 'textbox',
                            options: {
                                required: true
                            }
                        }
                    },
                    {
                        field: '制单日期', title: '下单日期', align: 'center', width: 100, editor: {
                            type: 'datetimebox',
                            options: {
                                required:true
                            }
                        }
                    },
                    {
                        field: '计量单位', title: '计量单位', align: 'center', width: 100, editor: {
                            type: 'textbox',
                            options:{}
                        }
                    },
                    {
                        field: '成品数量', title: '数量', align: 'center', width: 100, editor: {
                            type: 'numberbox',
                            options: {
                                precision: 0,
                                required:true
                            }
                        }
                    },
                    {
                        field: '金额', title: '金额', align: 'center', width: 100, editor: {
                            type: 'numberbox',
                            options: {
                                precision: 2,
                                required:true
                            }
                        }
                    },
                    {
                        field: '备注', title: '备注', align: 'center', width: 200, editor: {
                            type: 'textbox',
                            options: {

                            }
                        }
                    }
                ]],
                optionConfig: {
                    dom: {
                        add:'#accountList_menu_add',
                        edit: '#accountList_menu_edit',
                        remove: '#accountList_menu_remove',
                        revort: '#accountList_menu_revort',
                        save: '#accountList_menu_save',

                    },
                    paramNo:'#accountList_accountNo',
                    isReadonly: '订单号',
                  
                    datagridOptions: { url: '/ERP_Customer/GetAccountList', param:'#accountList_accountNo'},
                    datagrid: '#accountList_datagrid',
                    savebutton: '#accountList_menu_save',
                    removeUrl: '/ERP_Customer/DeleteAccountList',
                    editRow: undefined
                }
            };
            datagridOperation.datagrid(datagridConfig);
            datagridOperation.datagridOption(datagridConfig.optionConfig);
            $('#accountList_import').click(function () {
                var flag = $('#accountList_form').form('validate');
                if (!flag) { return; }

                toolbar.ajaxOption('/ERP_Customer/ImportAccount', $('#accountList_form').serialize() , function (response) {
                    if (response.success == 0) {
                        toolbar.showMsg(response.msg);
                        $(datagridConfig.dom).datagrid('options').url ='/ERP_Customer/GetAccountList'
                        $(datagridConfig.dom).datagrid('reload', { accountNo: response.id });
                    } else {
                        $.messager.alert('提示', response.msg, 'info');
                    }


                })

            });

            var runPageConfig = {
                before: '#accountList_pre',
                next: '#accountList_next',
                total: '#accountList_total',
                page: '#accountList_page',
                runPageUrl: '/ERP_Customer/RunPage',
                form: '#accountList_form',
                text: '#accountList_accountNo_text',
                no: '#accountList_accountNo',
                flag: '',
                datagrid:'#accountList_datagrid',
                menuArr: [{ menu: '#accountList_menu', items: ['#accountList_menu_add', '#accountList_menu_edit', '#accountList_menu_save', '#accountList_menu_remove'] }],
                linkbuttonArr: ['#accountList_import', '#accountList_auditor']
            };
            $(runPageConfig.before).click(function () {
                var total = $(runPageConfig.total).val();
                var page = $(runPageConfig.page).val();
                if (total != -1 && page != 0) {
                    if (total == 0) {
                        $.messager.alert('提示', '没有任何数据', 'info');
                        return;
                    }
                    if (parseInt(page) >= parseInt(total)) {
                        $.messager.alert('提示', '讨厌,被你看光了', 'info');
                        return;
                    }
                }
                var flag = $(runPageConfig.next).attr('flag');
                if (flag == 'next') {
                    ++page;
                    $(runPageConfig.next).attr('flag', '')
                }
                reloadForm(page, runPageConfig);

                ++page;
                $(runPageConfig.page).val(page);
                $(this).attr('flag', 'pre');
            });
            $(runPageConfig.next).click(function () {
                var page = $(runPageConfig.page).val();
                var total = $(runPageConfig.total).val();
                if (total == 0) {
                    $.messager.alert('提示', '没有任何数据', 'info');
                    return;
                }
                if (page <= 0) {
                    $(runPageConfig.page).val(0)
                    $.messager.alert('提示', '已是最后一个', 'info');
                    return;
                }
                var flag = $(runPageConfig.before).attr('flag');
                if (flag == 'pre') {
                    --page;
                    $(runPageConfig.before).attr('flag', '');
                    if (page <= 0) {
                        $(runPageConfig.page).val(1)
                        $.messager.alert('提示', '已是最后一个', 'info');
                        return;
                    }
                }
                --page;
                reloadForm(page, runPageConfig);
                $(runPageConfig.page).val(page);
                $(this).attr('flag', 'next');
            });
            $('#accountList_excel').click(function () {
                var params = $('#accountList_form').serializeArray();
                var page = $('#accountList_datagrid').datagrid("options").pageNumber;
                var rows = $('#accountList_datagrid').datagrid("options").pageSize;
                var strUrl = '';
                for (var item in params) {
                    strUrl += '&' + params[item].name + '=' + params[item].value;
                }
                strUrl = '/ERP_Customer/AccountListExportExcel?page=' + page + '&rows=' + rows + strUrl;
                $(this).attr('href', strUrl);
            });
            $('#accountList_print').click(function () {
                var accountNo = $('#accountList_accountNo').val();
                if (accountNo == '' && accountNo == null) { return; }
               /*location.href*/ var strUrl = '/ERP_Customer/AccountListPrintPage?accountNo=' + accountNo + '&d=' + Math.random();
                $('#accountListIframe').attr('src', strUrl);
            });
            var auditorConfig = {
                auditor: '#accountList_auditor',
                unAuditor: '#accountList_unauditor',
                paramNo: '#accountList_accountNo',
                auditorNo: '#accountList_auditorNo',
                auditorUrl: '/ERP_Customer/AuditorAccountList',
                unAuditorUrl: '/ERP_Customer/UnAuditorAccountList',
                menuArr: [{ menu: '#accountList_menu', items: ['#accountList_menu_add', '#accountList_menu_save', '#accountList_menu_remove'] }
                ],
                linkbuttonArr: ['#accountList_auditor', '#accountList_import']
            };
            $(auditorConfig.auditor).click(function () {
                var accountNo = $(auditorConfig.paramNo).val();
                if (accountNo == '' || accountNo == null) { return; }
                var auditorNo = $(auditorConfig.auditorNo).val();
                console.log('auditorNo' + auditorNo);
                if (auditorNo != '') { $.messager.alert('提示', '该对账单已审核', 'info'); return; }
                toolbar.ajaxOption(auditorConfig.auditorUrl, { accountNo: accountNo }, function (response) {
                    if (response.success == 0) {
                        $(auditorConfig.auditorNo).val(response.auditorNo);
                        auditorConfig.flag = true;
                        toolbar.controlMenuAndbutton(auditorConfig);
                        toolbar.showMsg(response.msg);
                    } else {
                        $.messager.alert('提示', response.msg, 'info');
                    }
                })


            });
            $(auditorConfig.unAuditor).click(function () {
                var accountNo = $(auditorConfig.paramNo).val();
                if (accountNo == '' || accountNo == null) { return; }
                var auditorNo = $(auditorConfig.auditorNo).val();
                if (accountNo == '') { return; }
                toolbar.ajaxOption(auditorConfig.unAuditorUrl, { accountNo: accountNo }, function (response) {
                    if (response.success == 0) {
                        $(auditorConfig.auditorNo).val('');
                        auditorConfig.flag = false;
                        toolbar.controlMenuAndbutton(auditorConfig);
                        toolbar.shwMsg(response.msg);
                    } else {
                        $.messager.alert('提示', response.msg, 'info');
                    }
                })


            })
        

        }

    }
    function myformatter(date) {
        var y = date.getFullYear();
        var m = date.getMonth() + 1;
        var d = date.getDate();
        return y + '-' + (m < 10 ? ('0' + m) : m) + '-' + (d < 10 ? ('0' + d) : d);
    }
    function reloadForm(page, config) {
        $.ajax({
            url: config.runPageUrl,
            type: 'post',
            data: { page: page },
            success: function (response) {
                console.log(response.rows)
                var jsondata = $.parseJSON(response);
                if (jsondata.rows != null) {
                    $(config.total).val(jsondata.total);
                    var formData = jsondata.rows[0];
                    $(config.form).form('load', formData);
                    $(config.text).html($(config.no).val())
                    var flag = (jsondata.rows[0].审核人编码 != null && jsondata.rows[0].审核人编码 != '') ? true : false;
                    $(config.datagrid).datagrid('loadData', jsondata);
                    config.flag = flag;
                    console.log (config.flag+"flag")
                    toolbar.controlMenuAndbutton(config);
                }
            },
            error: function (XMLHttpRequest, testStatus, errorThrown) {
                $.messager.alert('My Title', XMLHttpRequest.status, 'warning')
            }
        })
    }
})