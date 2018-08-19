define(['jquery','tool/initCombobox', 'tool/toolbar.service'], function ($, initcom, toolbar) {

    return {
        index: function () {
            $.extend($.fn.validatebox.defaults.rules, {
                PhoneOrMobile: {//非空电话号码 匹配 移动与固定电话号码
                    validator: function (value, param) {
                        return /^(0[0-9]{2,3}\-)?([2-9][0-9]{6,7})+(\-[0-9]{1,4})?$|(^(13[0-9]|15[0|3|6|7|8|9]|18[8|9])\d{8}$)/.test(value);
                    },
                    message: '格式不正确,请输入正确的电话格式。'
                },
                rangeTo: {

                    validator: function (value, param) {
                        p = parseFloat($(param[0]).val());
                        var a = (p >= value) ? true : false;
                        return value > 0 && value <= parseFloat($(param[0]).val());

                    },
                    message: '发货数量在1-订单数量之间'
                }
            });
            var comboboxConfig = [/*{ dom: '#orderinvoice_applyNo', url: '/ERP_OrderInvoice/GetSalesman' },*/
            { dom: '#orderinvoice_orderNo', url: '/ERP_OrderInvoice/GetOrder' },
            { dom: '#orderinvoice_customer', url: '/ERP_OrderInvoice/GetCustomer' },
            { dom: '#orderinvoice_aud', url: '' },
            { dom: '#orderinvoice_person', url: '' }
            ];
            initcom.initCombobox(comboboxConfig);
            $('#orderinvoice_applyNo').combobox({
                url: '/ERP_OrderInvoice/GetSalesman',
                textField: 'Key',
                valueField: 'Id',
                filter: function (q, r) {
                    var opts = $(this).combobox('options');
                    if (r[opts.textField] == null) return -1;
                    return r[opts.textField].indexOf(q) >= 0;
                },
                onLoadSuccess: function () {
                    var data = $(this).combobox('getData');
                    $('#orderinvoice_aud').combobox('loadData', data);
                    $('#orderinvoice_person').combobox('loadData', data)
                }
            });
            $('#orderinvoice_orderNo').combobox({
                onSelect: function (record) {
                    $('#orderinvoice_orderNumber').numberbox('setValue', record.Quantity);
                    $('#orderinvoice_customer').combobox('setValue', record.CustomerNo);
                }
            });
            $('#orderInvoice_payment').click(function () {
                var applyNo = $('#orderInvoiceApplyNo').val();
                if (applyNo == '' || applyNo == null) { return; }
                if ($('#orderinvoice_panymentflag').is(':checked')) { return;}
                var data = $('#orderinvoice_form').serialize();
                toolbar.ajaxOption('/ERP_OrderInvoice/Settlement', data, function (response) {
                    if (response.success == 0) {
                        toolbar.showMsg(response.msg);
                        $('#orderinvoice_panyment').attr('checked', true);
                        var date = new Date();
                        var strDate = date.getMonth() + "/" + date.getDate() + "/" + date.getFullYear();
                        $('#orderinvoice_date').datebox('setValue', strDate);
                    } else {
                        $.messager.alert('提示', response.msg, 'info');
                    }
                })
            });
            var optionConfig = {
                dom: {
                    add: '#orderInvoice_add',
                    edit: '#orderInvoice_edit',
                    remove: '#orderInvoice_remove',
                    save: '#orderInvoice_save'
                },
                editUrl: '/ERP_OrderInvoice/UpdateInvoice',
                saveUrl: '/ERP_OrderInvoice/AddInvoice',
                removeUrl: '/ERP_OrderInvoice/DeleteInvoice',
                no: '#orderInvoiceApplyNo',
                text: '#orderInvoice_text',
                form: '#orderinvoice_form',
                datagridList: [],
                linkbuttonArr: ['#orderInvoice_edit', '#orderInvoice_remove','#orderInvoice_save', '#orderInvoice_auditor']
            };
            toolbar.added(optionConfig);
            toolbar.edited(optionConfig);
            toolbar.removed(optionConfig);
            toolbar.saved(optionConfig);

            var runPageConfig = {
                before: '#orderInvoice_pre',
                next: '#orderInvoice_next',
                total: '#orderInvoice_total',
                page: '#orderInvoice_page',
                runPageUrl: '/ERP_OrderInvoice/RunPage',
                form: '#orderinvoice_form',
                text: '#orderInvoice_text',
                no: '#orderInvoiceApplyNo',
                auditorPerson: 0,
                list: [],
                menuArr: [],
                linkbuttonArr: ['#orderInvoice_edit','#orderInvoice_save', '#orderInvoice_remove','#orderInvoice_auditor']
            };
            toolbar.nextPage(runPageConfig);
            toolbar.beforePage(runPageConfig);


            var auditorConfig = {
                auditor: '#orderInvoice_auditor',
                unAuditor: '#orderInvoice_unauditor',
                paramNo: '#orderInvoiceApplyNo',
                auditorNo: '#orderInvoiceAuditorNo',
                auditorTime: '#orderinvoice_auditorTime',
                auditorPerson:'#orderinvoice_aud',
                form: '#orderinvoice_form',
                auditorUrl: '/ERP_OrderInvoice/Auditor',
                unAuditorUrl: '/ERP_OrderInvoice/UnAuditor',
                menuArr: [],
                linkbuttonArr: ['#orderInvoice_edit','#orderInvoice_save', '#orderInvoice_remove', '#orderInvoice_auditor']
            };
            toolbar.auditor(auditorConfig);
            toolbar.unAuditor(auditorConfig);

            var initPageConfig = {
                form: '#orderinvoice_form',
                text: '#orderInvoice_text',
                initPageUrl: '/ERP_OrderInvoice/InitPage',
                menuArr: [],
                linkbuttonArr: ['#orderInvoice_edit', '#orderInvoice_save', '#orderInvoice_remove', '#orderInvoice_auditor'],
                flag: true
            };
            InitPageData(initPageConfig)
        }
    }
    function InitPageData(config) {
        $.post(config.initPageUrl, function (response) {
            if (response != null) {
                $(config.form).form('load', response);
                $(config.text).html(response.申请单号)
                if (response.审核人编码 != null && response.审核人编码 != '') {
                    toolbar.controlMenuAndbutton(config);
                }
            }
        }, 'json')
    }

})