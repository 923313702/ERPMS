define(['jquery', 'order/order.service', 'tool/initCombobox', 'tool/toolbar.service'], function ($, datagridservice, initCom, toolbar) {

    return {
        index: function () {
            var comboboxConfig = [
                { dom: '#quotation2_saleMan', url: '/ERP_Order/GetStaff' },
                { dom: '#quotation2_category', url: '/ERP_Order/PCategory' },
                { dom: '#quotation2_customer', url: '/ERP_Order/GetCustormer2' },
                { dom: '#quitation_unit', url: '/ERP_Order/GetUnit' },
                { dom: '#quotation2_procuction', url: '' },
                { dom: '#quotation2_person', url: '' },
                { dom: '#quotation2_auditorPerson', url: '' }
            ];
            initCom.initCombobox(comboboxConfig);

            var detailConfig = {
                dom: '#quotation2_detail',
                columns: [[
                    //{ field: 'ck', checkbox: true },
                    { field: '行号', title: '行号', hidden: true },
                    { field: '订单号', title: '订单号', hidden: true },
                    { field: '纸张客户编码', title: '客户编码', hidden: true },
                    { field: '印品部件',title: '部件',align: 'center', width: 100 },
                    { field: '纸张编码', title: '纸张名称', align: 'center', width: 150 },
                    {  field: '开料尺寸',title: '开料尺寸', align: 'center',width: 85 },
                    { field: '合计张',title: '合计数',align: 'center', width: 100}
                ]],
                optionConfig: {
                    editRow: undefined
                }
            };
            datagridservice.datagrid(detailConfig);
            var quotation2_priceConfig = {
                dom: '#quotation2_price',
                merge: ['统计编码'],
                columns: [[
                    //{ field: 'ck', checkbox: true },
                    { field: 'ID', title: 'ID', hidden: true },
                    { field: '订单号', title: '订单号', hidden: true },
                    { field: '订单Detail号', title: '订单Detail号', hidden: true },
                    { field: '订单工艺号', title: '订单工艺号', hidden: true },
                    { field: '订单材料号', title: '订单材料号', hidden: true },
                    {
                        field: '标准单价',
                        title: '标准单价', hidden: true,
                    },
                    {
                        field: '统计编码',
                        title: '类别',
                        align: 'center',
                        width: 100
                    },
                    {
                        field: '印品部件',
                        title: '印品部件',
                        align: 'center',
                        width: 100,
                    },
                    {
                        field: '项目编码',
                        title: '项目名称',
                        align: 'center',
                        width: 100,
                        formatter: function (v, r, i) {
                            return r.项目名称;
                        },
                    },
                    {
                        field: '计量单位',
                        title: '单位',
                        align: 'center',
                        width: 100,
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
                        }
                    },
                    {
                        field: '数量',
                        title: '数量',
                        align: 'center',
                        width: 100  ,              
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
                    editRow: undefined
                }
            };
            datagridservice.datagrid(quotation2_priceConfig);
            var runPageConfig = {
                before: '#quotation2_pre',
                next: '#quotation2_next',
                total: '#quotation2_total',
                page: '#quotation2_page',
                runPageUrl: '/ERP_Quotation/RunPage',
                form: '#quotation2_form',
                text: '#quotation2_orderNo_text',
                no: '#quotation2_orderNo',
                auditorPerson: 0,
                list: [],
                menuArr: [],
                linkbuttonArr: []
            }
            toolbar.beforePage(runPageConfig);
            toolbar.nextPage(runPageConfig);

            var initPageConfig = {
                form: '#quotation2_form',
                text:'quotation2_orderNo_text',
                initPageUrl:'/ERP_Quotation/InitPage',
                datagridList: [
                    { dom:'#quotation2_detail', url:'/ERP_Quotation/ShowDetail' },
                    { dom:'#quotation2_price', url:'/ERP_Quotation/ShowPrice'}
                ]
            };
            InitPageData(initPageConfig);
            $('#quotation2_print').click(function () {
                console.log ('zhixinglema')
                var paramNo = $('#quotation2_orderNo').val();
                /*location.href=*/ var strUrl= '/ERP_Quotation/PrintPage?paramNo=' + paramNo + "&d=" + Math.random();
                $('#quotation_printf').attr('src', strUrl);
               
            })
        }
    }
    function InitPageData(config) {
        $.post(config.initPageUrl, function (response) {
            if (response != null) {
                $(config.form).form('load', response);
                $(config.text).html(response.订单号);
                var list = config.datagridList;
                console.log (response.订单号)
                for (var i in list) {
                    $(list[i].dom).datagrid('options').url = list[i].url;
                    $(list[i].dom).datagrid('reload', { paramNo: response.订单号 });
                }

            }
        }, 'json')
    }


    function printPage() {
        //获取当前页的html代码  
        var bodyhtml = window.document.body.innerHTML;
        //设置打印开始区域、结束区域  
        var startFlag = "<!--startprint-->";
        var endFlag = "<!--endprint-->";
        // 要打印的部分  
        var printhtml = bodyhtml.substring(bodyhtml.indexOf(startFlag),
            bodyhtml.indexOf(endFlag));
        // 生成并打印ifrme  
        var f = document.getElementById('printf');
        f.contentDocument.write(printhtml);
        f.contentDocument.close();
        f.contentWindow.print();
    } 
})