define(['jquery', 'tool/initCombobox', 'tool/toolbar.service'], function ($, initCom, toolbar) {
    return {
        index: function () {
            var comboboxConfig = [{ dom: '#orderAdvance_saleMan', url: '/ERP_OrderInvoice/GetSalesman' },
                { dom: '#orderAdvance_orderNo', url: '/ERP_OrderInvoice/GetOrder' },
            { dom: '#orderAdvance_customerNo', url: '/ERP_OrderInvoice/GetCustomer' },
            { dom: '#orderAdvance_unit', url: '/ERP_Order/GetUnit' },
            { dom: '#orderAdvance_auditorNo', url: '' },
            { dom: '#orderAdvance_zhidan', url: '' }];
            initCom.initCombobox(comboboxConfig);
            $('#orderAdvance_saleMan').combobox({
                onLoadSuccess: function () {
                    var data = $(this).combobox('getData');
                    $('#orderAdvance_auditorNo').combobox('loadData', data);
                    $('#orderAdvance_zhidan').combobox('loadData', data)
                }
            });

            var optionConfig = {
                dom: {
                    add: '#orderAdvance_add',
                    edit: '#orderAdvance_edit',
                    remove: '#orderAdvance_remove',
                    save: '#orderAdvance_save'
                },
                editUrl: '/ERP_OrderAdvance/EditAdvance',
                saveUrl: '/ERP_OrderAdvance/AddAdvance',
                removeUrl: '/ERP_OrderAdvance/DeleteAdvance',
                no: '#orderAdvance_number',
                text: '#orderAdvance_number_text',
                form: '#orderAdvance_form',
                datagridList: [],
                linkbuttonArr: ['#orderAdvance_edit', '#orderAdvance_remove', '#orderAdvance_save', '#orderAdvance_auditor']
            };
            toolbar.added(optionConfig);
            toolbar.edited(optionConfig);
            toolbar.removed(optionConfig);
            toolbar.saved(optionConfig);

            var runPageConfig = {
                before: '#orderAdvance_pre',
                next: '#orderAdvance_next',
                total: '#orderAdvance_total',
                page: '#orderAdvance_page',
                runPageUrl: '/ERP_OrderAdvance/RunPage',
                form: '#orderAdvance_form',
                text: '#orderAdvance_number_text',
                no: '#orderAdvance_number',
                auditorPerson: 0,
                list: [],
                menuArr: [],
                linkbuttonArr: ['#orderAdvance_edit', '#orderAdvance_remove', '#orderAdvance_save', '#orderAdvance_auditor']
            };
            toolbar.nextPage(runPageConfig);
            toolbar.beforePage(runPageConfig);

            var auditorConfig = {
                auditor: '#orderAdvance_auditor',
                unAuditor: '#orderAdvance_unauditor',
                paramNo: '#orderAdvance_number',
                auditorNo: '#orderAdvance_auditorNo',
                auditorTime: '#orderAdvance_auditorTime',
                auditorPerson: '#orderAdvance_auditorNo',
                form: '#orderAdvance_form',
                auditorUrl: '/ERP_OrderAdvance/Auditoring',
                unAuditorUrl: '/ERP_OrderAdvance/UnAuditor',
                menuArr: [],
                linkbuttonArr: ['#orderAdvance_edit', '#orderAdvance_remove', '#orderAdvance_save', '#orderAdvance_auditor']
            };
            toolbar.auditor(auditorConfig);
            toolbar.unAuditor(auditorConfig);
        }
    }
})