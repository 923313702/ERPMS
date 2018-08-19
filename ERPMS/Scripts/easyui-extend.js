/**
 * menu方法扩展
 * @param {Object} jq
 * @param {Object} itemEl
 */
$.extend($.fn.menu.methods, {
    /**
     * 激活选项（覆盖重写）
     * @param {Object} jq
     * @param {Object} itemEl
     */
    enableItem: function (jq, itemEl) {
        return jq.each(function () {
            var jqElements = $(itemEl);
            var state = $.data(this, 'menu');
            if (jqElements.length > 0) {
                jqElements.each(function () {
                    if ($(this).hasClass('menu-item-disabled')) {
                        for (var i = 0; i < state._eventsStore.length; i++) {
                            var itemData = state._eventsStore[i];
                            if (itemData.target == this) {
                                //恢复超链接
                                if (itemData.href) {
                                    $(this).attr("href", itemData.href);
                                }
                                //回复点击事件
                                if (itemData.onclicks) {
                                    for (var j = 0; j < itemData.onclicks.length; j++) {
                                        $(this).bind('click', itemData.onclicks[j]);
                                    }
                                }
                                //设置target为null，清空存储的事件处理程序
                                itemData.target = null;
                                itemData.onclicks = [];
                                $(this).removeClass('menu-item-disabled');
                            }
                        }
                    }
                });
            }
        });
    },
    /**
     * 禁用选项（覆盖重写）
     * @param {Object} jq
     * @param {Object} itemEl
     */
    disableItem: function (jq, itemEl) {
        return jq.each(function () {
            var jqElements = $(itemEl);
            var state = $.data(this, 'menu');
            if (jqElements.length > 0) {
                if (!state._eventsStore)
                    state._eventsStore = [];
                jqElements.each(function () {
                    if (!$(this).hasClass('menu-item-disabled')) {
                        var backStore = {};
                        backStore.target = this;
                        backStore.onclicks = [];
                        //处理超链接
                        var strHref = $(this).attr("href");
                        if (strHref) {
                            backStore.href = strHref;
                            $(this).attr("href", "javascript:void(0)");
                        }
                        //处理直接耦合绑定到onclick属性上的事件
                        var onclickStr = $(this).attr("onclick");
                        if (onclickStr && onclickStr != "") {
                            backStore.onclicks[backStore.onclicks.length] = new Function(onclickStr);
                            $(this).attr("onclick", "");
                        }
                        //处理使用jquery绑定的事件
                        var eventDatas = $(this).data("events") || $._data(this, 'events');
                        if (eventDatas["click"]) {
                            var eventData = eventDatas["click"];
                            for (var i = 0; i < eventData.length; i++) {
                                if (eventData[i].namespace != "menu") {
                                    backStore.onclicks[backStore.onclicks.length] = eventData[i]["handler"];
                                    $(this).unbind('click', eventData[i]["handler"]);
                                    i--;
                                }
                            }
                        }
                        //遍历_eventsStore数组，如果有target为null的元素，则利用起来
                        var isStored = false;
                        for (var j = 0; j < state._eventsStore.length; j++) {
                            var itemData = state._eventsStore[j];
                            if (itemData.target == null) {
                                isStored = true;
                                state._eventsStore[j] = backStore;
                            }
                        }
                        //没有现成的，则push进去
                        if (isStored == false) {
                            state._eventsStore[state._eventsStore.length] = backStore;
                        }
                        $(this).addClass('menu-item-disabled');
                    }
                });
            }
        });
    }
});
$.extend($.fn.datagrid.methods, {

    autoMergeCells: function (jq, fields) {

        return jq.each(function () {

            var target = $(this);

            if (!fields) {

                fields = target.datagrid("getColumnFields");

            }

            var rows = target.datagrid("getRows");

            var i = 0,

                j = 0,

                temp = {};

            for (i; i < rows.length; i++) {

                var row = rows[i];

                j = 0;

                for (j; j < fields.length; j++) {

                    var field = fields[j];

                    var tf = temp[field];

                    if (!tf) {

                        tf = temp[field] = {};

                        tf[row[field]] = [i];

                    } else {

                        var tfv = tf[row[field]];

                        if (tfv) {

                            tfv.push(i);

                        } else {

                            tfv = tf[row[field]] = [i];

                        }

                    }
                }

            }

            $.each(temp, function (field, colunm) {

                $.each(colunm, function () {

                    var group = this;



                    if (group.length > 1) {

                        var before,

                            after,

                            megerIndex = group[0];

                        for (var i = 0; i < group.length; i++) {

                            before = group[i];

                            after = group[i + 1];

                            if (after && (after - before) == 1) {

                                continue;

                            }

                            var rowspan = before - megerIndex + 1;

                            if (rowspan > 1) {

                                target.datagrid('mergeCells', {

                                    index: megerIndex,

                                    field: field,

                                    rowspan: rowspan

                                });

                            }

                            if (after && (after - before) != 1) {

                                megerIndex = after;

                            }

                        }

                    }

                });

            });

        });

    }

});
/**
 * linkbutton方法扩展
 * @param {Object} jq
 */
$.extend($.fn.linkbutton.methods, {
    /**
     * 激活选项（覆盖重写）
     * @param {Object} jq
     */
    enable: function (jq) {
        return jq.each(function () {
            var state = $.data(this, 'linkbutton');
            if ($(this).hasClass('l-btn-disabled')) {
                var itemData = state._eventsStore;
                //恢复超链接
                if (itemData.href) {
                    $(this).attr("href", itemData.href);
                }
                //回复点击事件
                if (itemData.onclicks) {
                    for (var j = 0; j < itemData.onclicks.length; j++) {
                        $(this).bind('click', itemData.onclicks[j]);
                    }
                }
                //设置target为null，清空存储的事件处理程序
                itemData.target = null;
                itemData.onclicks = [];
                $(this).removeClass('l-btn-disabled');
            }
        });
    },
    /**
     * 禁用选项（覆盖重写）
     * @param {Object} jq
     */
    disable: function (jq) {
        return jq.each(function () {
            var state = $.data(this, 'linkbutton');
            if (!state._eventsStore)
                state._eventsStore = {};
            if (!$(this).hasClass('l-btn-disabled')) {
                var eventsStore = {};
                eventsStore.target = this;
                eventsStore.onclicks = [];
                //处理超链接
                var strHref = $(this).attr("href");
                if (strHref) {
                    eventsStore.href = strHref;
                    $(this).attr("href", "javascript:void(0)");
                }
                //处理直接耦合绑定到onclick属性上的事件
                var onclickStr = $(this).attr("onclick");
                if (onclickStr && onclickStr != "") {
                    eventsStore.onclicks[eventsStore.onclicks.length] = new Function(onclickStr);
                    $(this).attr("onclick", "");
                }
                //处理使用jquery绑定的事件
                var eventDatas = $(this).data("events") || $._data(this, 'events');
                if (eventDatas["click"]) {
                    var eventData = eventDatas["click"];
                    for (var i = 0; i < eventData.length; i++) {
                        if (eventData[i].namespace != "menu") {
                            eventsStore.onclicks[eventsStore.onclicks.length] = eventData[i]["handler"];
                            $(this).unbind('click', eventData[i]["handler"]);
                            i--;
                        }
                    }
                }
                state._eventsStore = eventsStore;
                $(this).addClass('l-btn-disabled');
            }
        });
    }
});


$.extend($.fn.datagrid.methods, {

    /**
     * 增加空行 用于没有数据或者数据不足以填满整个列表
     * @param jq datagrid对象
     */
    fillRows: function (jq) {
        var datacounts = 10; //填充总数
        var grid = $(jq);
        var pageopt = grid.datagrid('getPager').data("pagination").options;
        var pageSize = pageopt.pageSize;
        var rows = grid.datagrid("getRows");
        var length = rows.length;

        if (pageSize >= datacounts && length < datacounts) {
            var options = grid.datagrid("options");
            var gpanel = grid.datagrid("getPanel");
            var gbody1 = gpanel.find(".datagrid-view1 .datagrid-body");
            var tbody1 = gbody1.find("table>tbody");
            var gbody2 = gpanel.find(".datagrid-view2 .datagrid-body");
            var tbody2 = gbody2.find("table>tbody");

            var column1s = grid.datagrid("getColumnFields", true);
            var column2s = grid.datagrid("getColumnFields");
            var td1s = "";
            var td2s = "";


            if (tbody1.length > 0) {
                td1s += options.rownumbers ? "<td class='datagrid-td-rownumber'><div class=\"datagrid-cell-rownumber\"></div></td>" : "";

                if (column1s.length > 0) {
                    $.each(column1s, function (i, field) {
                        var opt = grid.datagrid("getColumnOption", field);
                        if (opt != null && !opt.hidden) {
                            if (opt.checkbox) {
                                td1s += "<td field=\"" + field + "\"><div class=\"datagrid-cell-check\"></div></td>";
                            } else {
                                td1s += "<td field=\"" + field + "\"><div class=\"datagrid-cell datagrid-cell-c1-" + field + "\"></div></td>";
                            }
                        }
                    });
                }
                var events = $._data(gbody1.children().get(0), 'events');
                if (events != null) {
                    //重新绑定click、dblclick事件
                    var clickfunc = events.click[0].handler;
                    events.click[0].handler = function (e) {
                        var tt = $(e.target);
                        var tr = tt.closest("tr.datagrid-row");
                        if (tr.hasClass("datagrid-blank-row")) {
                            return;
                        }
                        clickfunc(e);
                    };
                    var dblclickfunc = events.dblclick[0].handler;
                    events.dblclick[0].handler = function (e) {
                        var tt = $(e.target);
                        var tr = tt.closest("tr.datagrid-row");
                        if (tr.hasClass("datagrid-blank-row")) {
                            return;
                        }
                        dblclickfunc(e);
                    };
                }
            }
            if (tbody2.length > 0 && column2s.length > 0) {
                $.each(column2s, function (i, field) {
                    var opt = grid.datagrid("getColumnOption", field);
                    if (opt != null && !opt.hidden) {
                        if (opt.checkbox) {
                            td2s += "<td field=\"" + field + "\"><div class=\"datagrid-cell-check\"></div></td>";
                        } else {
                            td2s += "<td field=\"" + field + "\"><div class=\"datagrid-cell datagrid-cell-c1-" + field + "\"></div></td>";
                        }
                    }
                });

                var events = $._data(gbody2.get(0), 'events');
                if (events != null) {
                    //重新绑定click、dblclick事件
                    var clickfunc = events.click[0].handler;
                    events.click[0].handler = function (e) {
                        var tt = $(e.target);
                        var tr = tt.closest("tr.datagrid-row");
                        if (tr.hasClass("datagrid-blank-row")) {
                            return;
                        }
                        clickfunc(e);
                    };
                    var dblclickfunc = events.dblclick[0].handler;
                    events.dblclick[0].handler = function (e) {
                        var tt = $(e.target);
                        var tr = tt.closest("tr.datagrid-row");
                        if (tr.hasClass("datagrid-blank-row")) {
                            return;
                        }
                        dblclickfunc(e);
                    };
                }
            }
            for (var i = length; i < datacounts; i++) {
                if (td1s != "") {
                    var tr = '<tr class="datagrid-blank-row datagrid-row'
                        + (options.striped && i % 2 != 0 ? ' datagrid-row-alt' : '') + '">' + td1s + '</tr>';
                    $(tr).appendTo(tbody1);
                }
                if (td2s != "") {
                    var tr = '<tr class="datagrid-blank-row datagrid-row'
                        + (options.striped && i % 2 != 0 ? ' datagrid-row-alt' : '') + '">' + td2s + '</tr>';
                    $(tr).appendTo(tbody2);
                }
            }
        }
    }
});