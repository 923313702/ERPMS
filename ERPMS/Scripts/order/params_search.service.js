define(['jquery'], function ($) {
    return {
        searchOption: function (config) {
            var _this = this;
            _this.dialogOnClose(config);
            _this.search(config);
            _this.search_go(config);
            _this.search_cancel(config);
        },
        dialogOnClose: function (config) {
            $(config.dialog).dialog({
                onClose: function () {
                    $(config.form).form('clear');
                }
            })
        },
        search: function (config) {
            $(config.search).unbind('click').click(function () {
                $(config.dialog).dialog('open');
            });
        },
        search_go: function (config) {
            $(config.search_go).unbind('click').click(function () {
                var params = $(config.form).serializeArray();
                var values = {};
                for (var item in params) {
                    values[params[item].name] = params[item].value;
                }
                $(config.datagrid).datagrid('reload', { data: values });
                $(config.form).form('clear');
                $(config.dialog).dialog('close');
            })
        },
        search_cancel: function (config) {
            $(config.search_cancel).unbind('click').click(function () {
                $(config.form).form('clear');
                $(config.dialog).dialog('close');
            })
        }
    }
})