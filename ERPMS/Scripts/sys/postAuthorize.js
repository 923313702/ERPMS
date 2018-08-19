define(['jquery', 'Basics/basics_operation'], function ($, operation) {

    return {
        index: function () {
            var config = {
                dom: '#ERP_postAuthorize',
                tool: '#ERP_postAuthorize_tb',
                initUrl: '/ERP_PostAuthorize/GetPosts',
                optionUrl: '',
                flag: '',
                columns: [[{ field: 'ck', checkbox: true },
                    { field: '岗位编码', title: '编号', width: 100, },
                    { field: '岗位名称',title: '岗位名称',width: 100,align: 'center' }
                ]],
                optionConfig: {
                    editRow: undefined
                }
            };
            operation.datagrid(config);
            $('#postAuthorize_tree').tree({
                width: 200,
                url: '',
                queryParams: {},
                method: 'post',
                checkbox: 'true',
                onClick: function (node) {
                    //展开点击的文本的子节点
                    $(this).tree(node.state === 'closed' ? 'expand' : 'collapse', node.target);
                    node.state = node.state === 'closed' ? 'closed' : 'open';
                },
            });
            $('#postAuthorize_tb_search').click(function () {
                var value = $('#postAuthorize_name').textbox('getValue');
                if (value == null || value == '') { return };
                $('#ERP_postAuthorize').datagrid('load', { postName: value });
            })
            $('#postAuthorize_tb_add').click(function () {
                showTreeData('#ERP_postAuthorize', '#post_dialog', '添加权限', '#tbs #submit', '#postAuthorize_tree', '/ERP_PostAuthorize/ShowPostTree','add', true)
            });
            $('#postAuthorize_tb_remove').click(function () {
                showTreeData('#ERP_postAuthorize', '#post_dialog', '删除权限', '#tbs #submit', '#postAuthorize_tree', '/ERP_PostAuthorize/ShowPostTree','remove', false)
            });
            $('#tbs #submit').click(function () {
                var arr = [];
                var flag = $(this).attr('flag');
                var selectData = $("#ERP_postAuthorize").datagrid('getSelected');
                var postId = selectData.岗位编码;
                getTreeChecked('#postAuthorize_tree', flag, postId, '/ERP_PostAuthorize/AddPost', '/ERP_PostAuthorize/DeletePost')
            });
            $('#tbs #cancel').click(function () {
                var treeObject = $("#postAuthorize_tree")
                var rootNodes = treeObject.tree('getChecked');
                if (rootNodes.length <= 0) return;
                for (var i in rootNodes) {
                    treeObject.tree('uncheck', rootNodes[i].target);
                }
            })
        }
    }
    function showTreeData(datagrid,dialog,title ,submit, tree,treeUrl,optionflag,flag) {
        var data = $(datagrid).datagrid('getSelected');
        if (data == null) { alert("请选中要操作的岗位"); return; }
        $(dialog).dialog("setTitle", title);

        $(dialog).dialog('open');
        $(submit).attr('flag', optionflag);
        var post_tree = $(tree);
        post_tree.tree({
            url:treeUrl,
            queryParams: { postId: data.岗位编码, flag: flag },

        })
    }
    function getTreeChecked(tree, flag, postId,addUrl,removeUrl) {
        var arr=[]
        if (flag === 'add') {
            var nodes = $(tree).tree('getChecked', ['checked', 'indeterminate']);
            for (var i in nodes) {
                var data = new post(nodes[i].id, postId);
                console.log('nodes[i]' + nodes[i].id+'postID'+postId)
                arr.push(data);
            }
            if (arr.length > 0) {
                postAjax(tree,addUrl, arr)
            }
        } else {
            var nodes = $(tree).tree('getChecked');
            console.log(nodes);
            for (var i in nodes) {
                var data = new post(nodes[i].id, postId);
                arr.push(data);
            }
            if (arr.length > 0) {
                postAjax(tree,removeUrl, arr)
            }
        }

    }
    function post(postId, dId) {
        this.功能编码 = postId;
        this.岗位编码 = dId;
    }
    function postAjax(tree,url, data) {
        $.post(url, {data:data}, function (response) {
            if (response.success == 0) {
                $.messager.show({
                    title: '提示',
                    msg: response.msg,
                    timeout: 3000,
                    showType: 'slide'
                });
                $(tree).tree('reload')
            } else {
                $.messager.alert ('提示',response.msg,'info')
            }
        }, 'json')
    }

})