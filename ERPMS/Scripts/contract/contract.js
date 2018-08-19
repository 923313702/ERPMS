define(['jquery', 'order/datagridOption.server', 'tool/initCombobox', 'tool/toolbar.service','tool/convertToChinese','tool/encrypt'], function ($, service, initCom, toolbar,convertToChin,encrypt) {
    return {

        index: function () {

            var datagridConfig = {
                dom: '#contract-detail',
                optionUrl: '/ERP_Contract/SaveOrUpdatePrint',
                flag: '#contract_menu_save',
                columns: [[
                    { field: 'ck', checkbox: true },
                    { field: '行号', title: '序号', hidden: true },
                    { field: '合同号', title: '合同号', hidden: true },
                    {
                        field: '印品名称', title: '产品名称', align: 'center', width: 100, editor: {
                            type: 'textbox',
                            options: {
                                required: true
                            }
                        }
                    },
                    {
                        field: '成品尺寸', title: '规格', align: 'center', width: 100, editor: {
                            type: 'textbox',
                            options: {
                                //required: true
                            }
                        }
                    },
                    {
                        field: '计量单位', title: '计量单位', align: 'center', width: 100, editor: {
                            type: 'textbox',
                            options: {
                                // required: true
                            }
                        }
                    },
                    {
                        field: '成品数量', title: '数量', align: 'center', width: 50, editor: {
                            type: 'numberbox',
                            options: {
                                precision: 2,
                                required: true,
                                onChange: function (n, o) {
                                    var element = $(datagridConfig.dom).datagrid('getEditor', { index: datagridConfig.optionConfig.editRow, field: '单价' });
                                    var value = $(element.target).numberbox('getValue');
                                    element = $(datagridConfig.dom).datagrid('getEditor', { index: datagridConfig.optionConfig.editRow, field: '合同金额' });
                                    $(element.target).textbox('setValue', (n * value));
                                }
                            }
                        }
                    },
                    {
                        field: '单价', title: '单价', align: 'center', width: 50, editor: {
                            type: 'numberbox',
                            options: {
                                precision: 2,
                                required: true,
                                onChange: function (n, o) {
                                    var element = $(datagridConfig.dom).datagrid('getEditor', { index: datagridConfig.optionConfig.editRow, field: '成品数量' });
                                    var value = $(element.target).numberbox('getValue');
                                    element = $(datagridConfig.dom).datagrid('getEditor', { index: datagridConfig.optionConfig.editRow, field: '合同金额' });
                                    $(element.target).textbox('setValue', (n * value));
                                }
                            }
                        }
                    },
                    { field: '合同金额', title: '金额', align: 'center', width: 50, editor: { type: 'numberbox', options: { precision: 2 } } },
                    { field: '不含税金额', title: '不含税金额', align: 'center', width: 100, editor: { type: 'numberbox', options: { precision: 2 } } },
                    {
                        field: '交货日期', title: '交货日期', align: 'center', width: 100, editor: {
                            type: 'datebox',
                            options: {
                                required: true,

                            }
                        }
                    }
                ]],
                menu: '#contract_menu',
                optionConfig: {
                    dom: {
                        add: '#contract_menu_add',
                        edit: '#contract_menu_edit',
                        save: '#contract_menu_save',
                        remove: '#contract_menu_remove',
                        revort: '#contract_menu_revort',
                    },
                    datagrid: '#contract-detail',
                    removeUrl: '/ERP_Contract/DeletePrint',
                    savebutton: '#contract_menu_save',
                    paramNo: '#contract_contractNo',
                    editRow: undefined
                }
            };
            service.datagrid(datagridConfig);
            service.datagridOption(datagridConfig.optionConfig);
            $(datagridConfig.dom).datagrid({
                onAfterEdit: function (i, d, c) {
                    var flags = $(datagridConfig.flag).attr('flag');
                    var val = $(datagridConfig.optionConfig.paramNo).val();
                    if (flags == 'add') {
                        if (val == null || val == '') {
                            $.messager.alert('提示', '没获取到合同号', 'info');
                            $(datagridConfig.dom).datagrid('rejectChanges');
                            datagridConfig.optionConfig.editRow = undefined;
                            return;
                        }
                        d.合同号 = val;
                    }
                    $.ajax({
                        url: datagridConfig.optionUrl,
                        type: 'post',
                        data: { data: d, flag: flags },
                        success: function (response) {
                            if (response.success == 0) {
                                d.行号 = response.id;
                                datagridConfig.optionConfig.editRow = undefined;
                                service.showMsg(response.msg);
                                var result = reckon(datagridConfig.dom);
                                console.log(result)
                                $('#contract_money').numberbox('setValue', result.total);
                                $('#contract_money2').numberbox('setValue', result.total2);
                                var value = convertToChin.convertToChinese(result.total);
                                $('#contract_upper').textbox('setValue', value)


                            } else {
                                $.messager.alert('提示', response.msg, 'info');
                                $(datagridConfig.dom).datagrid('beginEdit', i);
                            }
                        },
                        error: function (XMLHttpRequest, testStatus, errorThrown) {
                            $.messager.alert('My Title', XMLHttpRequest.status, 'warning')
                        }
                    })
                },
                onLoadSuccess: function () {
                    var result = reckon(datagridConfig.dom);
                    $('#contract_money').numberbox('setValue', result.total);
                    $('#contract_money2').numberbox('setValue', result.total2);
                    var value = convertToChin.convertToChinese(result.total);
                    $('#contract_upper').textbox('setValue', value);
                    datagridConfig.optionConfig.editRow = undefined;
                }
            });
            var comboboxConfig = [
                { dom: '#contract_customer', url: '/ERP_Order/GetCustormer' },
                { dom: '#contract_saleman', url: '/ERP_Order/GetStaff' }];
            initCom.initCombobox(comboboxConfig);
            $('#contract_customer').combobox({
                onSelect: function (r) {
                    $('#contract_buyerName').html('需方(章):&nbsp;&nbsp;' + r.Key);
                    $('#contrct_buyer_phone').html(' 电话:&nbsp;&nbsp;' + r.Phone);
                    $('#contract_buyer_fax').html('传真:&nbsp;&nbsp;', +r.Fax);
                    var contractNo = $('#contract_contractNo').val();
                    if (contractNo == null || contractNo == '') {
                        var data = { 客户编码: r.Id };
                        insertContract('/ERP_Contract/AddContract', data, '#contract_contractNo', '#contract_contractText');
                    }
                  
                }
            });
   

            var runPageConfig = {
                before: '#contract_pre',
                next: '#contract_next',
                total: '#contract_total',
                page: '#contract_page',
                runPageUrl: '/ERP_Contract/RunPage',
                form: '#contract_form',
                text: '#contract_contractText',
                no: '#contract_contractNo',
                content: '#contract_content',
                buyer: '#contract_buyer',
                provider: '#contract_provider',
                remark: '#contract_process_remark',
                ask:'#contract_process_ask',
                auditorPerson: 0,
                list: [{ datagrid: '#contract-detail', url: '/ERP_Contract/ShowContractPrint', param_No: '#contract_contractNo' }],
                menuArr: [{ menu: '#contract_menu', items: ['#contract_menu_add', '#contract_menu_edit', '#contract_menu_save', '#contract_menu_remove'] }],
                linkbuttonArr: ['#contract_edit', '#contract_remove', '#contract_save', '#contract_auditor']
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
                reloadcontractForm(page, runPageConfig);

                ++page;
                $(runPageConfig.page).val(page);
                $(this).attr('flag', 'pre');
            })
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
                reloadcontractForm(page, runPageConfig);
                $(runPageConfig.page).val(page);
                $(this).attr('flag', 'next');
            });

            var optionConfig = {
                dom: {
                    add: '#contract_add',
                    edit: '#contract_edit',
                    remove: '#contract_remove',
                    save: '#contract_save',
                    print:'#contract_print'
                },
                editUrl: '/ERP_Contract/UpdateContract',
                saveUrl: '/ERP_Contract/UpdateContract',
                removeUrl: '/ERP_Contract/DeleteContract',
                printUrl:'/ERP_Contract/PrintPage',
                no: '#contract_contractNo',
                text: '#contract_contractText',
                form: '#contract_form',
                datagridList: [{ datagrid: '#contract-detail', url: '', param_No: '' }],
                menuArr: [{ menu: '#contract_menu', items: ['#contract_menu_add', '#contract_menu_edit', '#contract_menu_save', '#contract_menu_remove'] }],
                linkbuttonArr: ['#contract_edit', '#contract_remove', '#contract_save', '#contract_auditor']
            };
            $(optionConfig.dom.save).click(function () {
                var data = {};
                var process = {};
                data.客户编码 = $('#contract_customer').combobox('getValue');
                data.业务员编码 = $('#contract_saleman').combobox('getValue');
                data.合同号 = $('#contract_contractNo').val();
                data.金额 = $('#contract_money').numberbox('getValue');
                process.工艺要求 = $('#contract_process_ask').textbox('getValue');
                process.备注 = $('#contract_process_remark').textbox('getValue');
                data.T_OMS_合同_Detail_工艺 = process;
                var html = $('#contract_content').html();

                data.合同内容 = encrypt.escape(html);
                html = $('#contract_buyer').html();
                data.buyer = encrypt.escape(html);
                html = $('#contract_provider').html();
                data.provider = encrypt.escape(html);
                toolbar.ajaxOption(optionConfig.saveUrl, data, function (response) {
                    if (response.success = 0) {
                        toolbar.showMsg(response.msg);
                    } else {
                        $.messager.alert('提示', response.msg, 'info');
                    }
                })
            });
            $(optionConfig.dom.remove).click(function () {
                toolbar.ajaxOption(optionConfig.removeUrl, $(optionConfig.form).serialize(), function (response) {
                    if (response.success == 0) {
                        $(optionConfig.form).form('clear');
                        $(optionConfig.text).text('');
                        $('#contract-detail').datagrid('loadData', { total: 0, rows: [] });
                        $('#contract_process_ask').textbox('setValue', '');
                        $('#contract_process_remark').textbox('setValue', '');
                        var rawdata = rawData();
                        $('#contract_content').html(rawdata.content);
                        $('#contract_buyer').html(rawdata.buyer);
                        $('#contract_provider').html(rawdata.provider);
                    }
                })
            });
            $(optionConfig.dom.add).click(function () {
                $(optionConfig.form).form('clear');
                $(optionConfig.text).html('');
                optionConfig.flag = false;
                toolbar.controlMenuAndbutton(optionConfig)
                if (optionConfig.datagridList != null && optionConfig.datagridList.length > 0) {
                    var list = optionConfig.datagridList;
                    for (var i in list) {
                        $(list[i].datagrid).datagrid('loadData', { total: 0, rows: [] });
                    }
                }
                $('#contract_process_ask').textbox('setValue', '');
                $('#contract_process_remark').textbox('setValue', '');
                var rawdata = rawData();
                $('#contract_content').html(rawdata.content);
                $('#contract_buyer').html(rawdata.buyer);
                $('#contract_provider').html(rawdata.provider);
                var customer = $('#contract_customer').combobox('getText');
                //console.log(customer);
       
 
                
            });
            $(optionConfig.dom.print).click(function () {
 
                var contractNo = $('#contract_contractNo').val();
                if (contractNo == null || contractNo == '') { return;}
                var customer = $('#contract_customer').combobox('getText');
                var saleMan = $('#contract_saleman').combobox('getText')
                var strUrl= optionConfig.printUrl + '?contractNo=' + contractNo + '&saleMan=' + saleMan + '&customerName=' + customer + '&d=' + Math.random();
                $('#printcontractIframe').attr('src', strUrl);
            })

            var auditorConfig = {
                auditor: '#contract_auditor',
                unAuditor: '#contract_unauditor',
                paramNo: '#contract_contractNo',
                auditorNo: '#contract_auditorNo',
                form: '#contract_form',
                content: '#contract_content',
                buyer: '#contract_buyer',
                auditorUrl: '/ERP_Contract/Auditor',
                unAuditorUrl: '/ERP_Contract/UnAuditor',
                menuArr: [{ menu: '#contract_menu', items: ['#contract_menu_add', '#contract_menu_edit', '#contract_menu_save', '#contract_menu_remove'] }],
                linkbuttonArr: ['#contract_edit', '#contract_remove', '#contract_save', '#contract_auditor']
            };
            $(auditorConfig.auditor).click(function () {
                var paramNo = $(auditorConfig.paramNo).val();
                if (paramNo == null || paramNo == '' || paramNo == undefined) { return; }
                var auditorNo = $(auditorConfig.auditorNo).val();
                if (auditorNo != null && auditorNo != '') { return; }
                var data = $(auditorConfig.form).serialize();
                toolbar.ajaxOption(auditorConfig.auditorUrl, data, function (response) {
                    if (response.success == 0) {
                        toolbar.showMsg(response.msg);
                        $(auditorConfig.auditorNo).val(response.AuditorNo);
                        auditorConfig.flag = true;
                        toolbar.controlMenuAndbutton(auditorConfig);
                        $(auditorConfig.content).attr('contentEditable', false);
                        $(auditorConfig.buyer).attr('contentEditable', false);
                    } else {
                        $.messager.alert('提示', response.msg, 'info')
                    }
                })
            });
            $(auditorConfig.unAuditor).click(function () {
                var paramNo = $(auditorConfig.paramNo).val();
                if (paramNo == null || paramNo == '' || paramNo == undefined) { return; }
                var auditorNo = $(auditorConfig.auditorNo).val();
                if (auditorNo == null || auditorNo == '') { return; }
                var data = $(auditorConfig.form).serialize();
                toolbar.ajaxOption(auditorConfig.unAuditorUrl, data, function (response) {
                    if (response.success == 0) {
                        toolbar.showMsg(response.msg);
                        $(auditorConfig.auditorNo).val('');
                        auditorConfig.flag = false;
                        toolbar.controlMenuAndbutton(auditorConfig);
                        $(auditorConfig.content).attr('contentEditable', true);
                        $(auditorConfig.buyer).attr('contentEditable', true);

                    } else {
                        $.messager.alert('提示', response.msg, 'info')
                    }
                })
            })
        }
    }
  
    function insertContract(url,data,contractNo, text) {
        $.post(url, data, function (response) {
            if (response.success == 0) {
                $(contractNo).val(response.id);
                $(text).text(response.id);
            }
        })
    }
    function reckon(datagrid) {
        var rows = $(datagrid).datagrid('getRows');
        var total=0, total2 = 0;
        for (var i = 0; i < rows.length; i++) {
            total += parseFloat(rows[i]['合同金额']);
            total2 += parseFloat(rows[i]['不含税金额']);
        }
        return {total:total,total2:total2}
    }

    function reloadcontractForm(page, config) {
        $.ajax({
            url: config.runPageUrl,
            type: 'post',
            data: { page: page },
            success: function (response) {
                var jsondata = $.parseJSON(response);
                if (jsondata.rows != null) {
                    $(config.total).val(jsondata.total);
                    $(config.form).form('load', jsondata.rows);
                    $(config.text).html($(config.no).val())
                    var flag = (jsondata.rows.审核人编码 != null && jsondata.rows.审核人编码 != '') ? true : false;
                    if (flag) {
                        $(config.content).attr("contentEditable", false);
                    } else {
                        $(config.content).attr("contentEditable", true);
                    }
                    contractmenuItemDisable(config.menuArr, flag)
                    contractlinkbuttonDisable(config.linkbuttonArr, flag);
                    var datagridList = config.list;
                    for (var i in datagridList) {
                        reloadcontractDatagrid(datagridList[i]);
                    }
                    var rawdata = rawData();
                    var html = '';
                    if (jsondata.rows.合同内容 == null || jsondata.rows.合同内容 == ''||jsondata.rows.合同内容=='null') {
                        $(config.content).empty().html(rawdata.content);
                    } else {
                        html = encrypt.enescape(jsondata.rows.合同内容);
                        $(config.content).empty().html(html);
                    }
                    if (jsondata.rows.buyer == null || jsondata.rows.buyer == '' || jsondata.rows.buyer=='null') {
                        $(config.buyer).empty().html(rawdata.buyer);

                    } else {
                        html = encrypt.enescape(jsondata.rows.buyer);
                        $(config.buyer).empty().html(html);
                    }
                    if (jsondata.rows.provider == null || jsondata.rows.provider == '' || jsondata.provider == 'null') {
                        $(config.provider).empty().html(rawdata.provider);
                    } else {

                        html = encrypt.enescape(jsondata.rows.provider);
                        $(config.provider).empty().html(html);
                    }
                    (jsondata.rows.remark == null || jsondata.rows.remark == '') ? $(config.remark).textbox('setValue', '') : $(config.remark).textbox('setValue', jsondata.rows.remark);
                    (jsondata.rows.ask == null || jsondata.rows.ask == '') ? $(config.ask).textbox('setValue', '') : $(config.ask).textbox('setValue', jsondata.rows.ask)

                }
            },
            error: function (XMLHttpRequest, testStatus, errorThrown) {
                $.messager.alert('My Title', XMLHttpRequest.status, 'warning')
            }
        })
    }

    function contractlinkbuttonDisable(arr, flag) {
        if (arr != undefined && arr.length > 0) {
            var able = flag ? 'disable' : 'enable';
            for (var i in arr) {
                $(arr[i]).linkbutton(able);
            }
        }
    }
    //同上
    function contractmenuItemDisable(menus, flag) {
        if (menus != undefined && menus.length > 0) {
            var able = flag ? 'disableItem' : 'enableItem';
            for (var i in menus) {
                var menu = menus[i].menu;
                var items = menus[i].items;
                for (var item in items) {
                    $(menu).menu(able, $(items[item]));
                }
            }
        }
    }

    function reloadcontractDatagrid(option) {
        var data = new Object();
        data.paramNo = $(option.param_No).val();
        $(option.datagrid).datagrid({ url: option.url, queryParams: data });;
    }

    function transferred(html) {
        var element = $('<div>');
        var content = element.text(html).html();
        element = null;
        return content;

    }
    function rawData() {
        var rawdata = {};
        rawdata.content = ' 三.&nbsp;&nbsp;产品质量要求及技术标准按客户签样<br/>四.&nbsp;&nbsp; 付款方式 &nbsp;:&nbsp;&nbsp; <br/>' +
            '五.&nbsp;&nbsp; 实际交货数量允许与签约数在:&nbsp;&nbsp;&nbsp;&nbsp; % 内浮动, 货款按实际交货量计算 <br/>' +
            '六.&nbsp;&nbsp; 需方应在乙方交货三天内验收货物, 在收货之日起十天内无书面异议即视为产品合格 <br/>' +
            '七.&nbsp;&nbsp; 包装要求:&nbsp;&nbsp;&nbsp;&nbsp; <br/>八.&nbsp;&nbsp; 交货方式 &nbsp;:&nbsp;&nbsp;&nbsp;&nbsp; <br/>' +
            '&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 交货地点 &nbsp;:&nbsp;&nbsp;&nbsp;&nbsp;<br/>' +
            '九.&nbsp;&nbsp; 因需方原因影响交货日期的与供方无关, 单方因故需要修改、终止、解除协议，须双方盖章的书面协议 <br/>' +
            ' 十.&nbsp;&nbsp; 解决合同方的方式: 出现纠纷的由双方协商解决, 协商不成, 则由递交房源依法处理 <br/>' +
            '十一.&nbsp;&nbsp; 本合同一式二份, 供需上方各一份, 技术编撰及订单为合同附件, 合同双方在合同履行过程及相关联络中产生 <br/>' +
            '十二.&nbsp;&nbsp; 双方协商的其他条款 &nbsp;:&nbsp;&nbsp;&nbsp;&nbsp;';

        rawdata.buyer = '<p id="contract_buyerName">需方(章):&nbsp;&nbsp;</p>法定代表人(委托代理人):&nbsp;&nbsp;<br/>' +
            '地址:&nbsp;&nbsp; <br/> <p id="contrct_buyer_phone"> 电话:&nbsp;&nbsp;</p><p id="contract_buyer_fax">传真:&nbsp;&nbsp;</p>';

        rawdata.provider = '   供方(章):&nbsp;&nbsp;昆明市国强包装印刷有限公司<br/>法定代表人(委托代理人):&nbsp;&nbsp; <br/>' +
            '地址:&nbsp;&nbsp; <br/>开户行: 昆明市建行: 前卫西路支行 <br/> 账号:&nbsp;&nbsp; 4367423860999281723 <br/>' +
            '电话:&nbsp;&nbsp;&nbsp;&nbsp; 13354622464   传真:&nbsp;&nbsp; 0871 - 64576806';
        return rawdata;
    }

    function saveOrupdateProcess() {

        var data = {};
        data.合同号 = $('#contract_contractNo').val()
        data.工艺要求 = $('#contract_process_ask').textbox('getValue');
    }

})