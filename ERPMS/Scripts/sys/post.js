define(['jquery', 'Basics/basics_operation'], function ($, operation) {

    return {

        index: function () {
            var config = {
                dom: '#ERP_post',
                tool: '#ERP_post_tb',
                initUrl: '/ERP_Post/GetPosts',
                optionUrl: '/ERP_Post/SaveOrUpdatePost',
                flag: '#post_tb_save',
                columns: [[
                    { field: 'ck', checkbox: true },
                    {
                        field: '岗位编码',
                        title: '岗位编码',
                        width: 100,
                        align: 'center',
                        editor: {
                            type: 'validatebox',
                            options: {
                                required: true,
                                delay: 500,
                                validType: ['postNoAjax', 'length[1,6]']
                            }
                        }
                    }, {
                        field: '岗位名称',
                        title: '岗位名称',
                        width: 100,
                        align: 'center',
                        editor: { type: 'validatebox', options: { required: true } },
                    }
                ]],
                optionConfig: {
                    dom: {
                        add: '#post_tb_add',
                        edit: '#post_tb_edit',
                        remove: '#post_tb_remove',
                        save: '#post_tb_save',
                        revort: '#post_tb_revort',

                    },
                    datagrid: '#ERP_post',
                    savebutton: '#post_tb_save',
                    removeUrl: '/ERP_Post/DeletePosts',
                    editRow: undefined
                },
            }
            operation.datagrid(config);
            operation.index(config.optionConfig);

        }

    }

})