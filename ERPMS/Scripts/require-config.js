require.config({
    urlArgs: 'r=' + (new Date()).getTime(),
    paths: {
        'jquery': 'jquery.min',

        'easyui': 'jquery.easyui.min',
        'easyui-extend':'easyui-extend',
        'easyui-lang': 'easyui-lang-zh_CN',
    },
    shim: {
        'easyui': ['jquery', 'css!../Content/easyui.css'],
        'easyui-extend':['easyui'],
        'easyui-lang': ['jquery', 'easyui']
    },
    map: {
        '*': {
            'css': 'require-css/css.min'
        }
    },
    waitSeconds: 30,
    charset: 'utf-8'
});


require(['jquery', 'easyui', 'easyui-lang', 'easyui-extend'], function ($) {
    var Config = requirejs.s.contexts._.config.config;
    $(function () {
        console.log(Config);
        require([Config.action], function (action) {
            action != undefined && action.index != undefined && action.index();
        });
    });
});
window.erpRequire = function (moduleArray) {
    moduleArray.unshift('jquery');
    require(moduleArray, function ($) {
        var actions = arguments;
        $(function () {
            for (var i = 1; i < actions.length; i++) {
                var action = actions[i];
                action != undefined && action.index != undefined && action.index();
            }
        });
    });
}