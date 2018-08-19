define(['jquery'], function ($) {

    return {
        //初始化combobox
        initCombobox: function (config) {
            for (var i in config) {
                $(config[i].dom).combobox({
                    url: config[i].url,
                    textField: 'Key',
                    valueField: 'Id',
                    filter: function (q, r) {
                        var opts = $(this).combobox('options');
                        if (r[opts.textField] == null) return -1;
                        return r[opts.textField].indexOf(q) >= 0;
                    }
                   
                })
            }

        }
    }
})