define(['jquery', 'easyui'], function ($, module) {
    return {
        index: function () {


   

            $(document).on('click', '.pf-logout', function () {
                layer.confirm('您确定要退出吗？', {
                    icon: 4,
                    title: '确定退出' //按钮
                }, function () {
                    location.href = 'login.html';
                });
            });
            //左侧菜单收起
            $(document).on('click', '.toggle-icon', function () {
                $(this).closest("#pf-bd").toggleClass("toggle");
                setTimeout(function () {
                    $(window).resize();
                }, 300)
            });

            //$(document).on('click', '.pf-modify-pwd', function () {
            //    $('#pf-page').find('iframe').eq(0).attr('src', 'backend/modify_pwd.html')
            //});

            //$(document).on('click', '.pf-notice-item', function () {
            //    $('#pf-page').find('iframe').eq(0).attr('src', 'backend/notice.html')
            //});
           
            $('#treeMenu').tree({
                url: '/Home/GetMenu',
                //data: data,
                onClick: function (node) {
                    console.log('//////////')
                    console.log(node);
                    // $(this).tree("expand", node.target);
                    //展开点击的文本的子节点
                    $(this).tree(node.state === 'closed' ? 'expand' : 'collapse', node.target);
                    node.state = node.state === 'closed' ? 'closed' : 'open';
                    // console.log(node.Attributes.url);
                    console.log(node.Attributes.url)
                    if (node.Attributes !== undefined && node.Attributes.url !== undefined && node.Attributes.url !== null) {
                        console.log(node.text, node.Attributes.url);
                        addTab(node.text, node.Attributes.url);
                        // alert(node.attributes.url);
                    }
                }
            });
            $('#contentTabs').tabs({
                tabHeight: 44,
                border: true,
                fit:true
            });
            $('#contentTabs').tabs('add', {
                title: "首页&nbsp;&nbsp;&nbsp;&nbsp;<span class='unread'>2</span> ",
                content: "hello word",
                closable: false
            });
            function addTab(title, url) {
                if ($('#contentTabs').tabs('exists', title)) {
                    $('#contentTabs').tabs('select', title);
                } else {
                    $('#contentTabs').tabs('add', {
                        title: title,
                        //content: content,
                        href: url,
                        closable: true
                    })
                    //var content = '<iframe scrolling="no" id="urlIframe" name="child" frameborder="0" src="' + url + '" style="width:99%;height:98%;border:none; padding:5px;"></iframe>';
                    var content ='<iframe class="page-iframe" src="'+url+'" frameborder="no"   border="no" height="100%" width="100%" scrolling="auto"></iframe>'
                   
                }
            }
        }




    }
});