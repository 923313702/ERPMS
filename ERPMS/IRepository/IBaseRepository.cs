using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace ERPMS.IRepository
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {

        IQueryable<TEntity> Query();

        IEnumerable<TEntity> SqlQuery(string sql, params Object[] parameters);
        /// <summary>
        /// 单表查询
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        IQueryable<TEntity> QueryWhere(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 多表查询
        /// </summary>
        /// <param name="predicate">lamda</param>
        /// <param name="tableNames">表明</param>
        /// <returns></returns>

        IQueryable<TEntity> QueryJoin(Expression<Func<TEntity, bool>> predicate, string[] tableNames);
        /// <summary>
        /// 动态多条件查询
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        IQueryable<TEntity> AsExpandable<TKey>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TKey>> keySelector, bool IsQueryOrderBy);

        /// <summary>
        /// 升序查询还是降序查询
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="predicate"></param>
        /// <param name="keySelector"></param>
        /// <param name="IsQueryOrderBy"></param>
        /// <returns></returns>

        IQueryable<TEntity> QueryOrderBy<TKey>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TKey>> keySelector, bool IsQueryOrderBy);

        /// <summary>
        /// 升序分页查询还是降序分页
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="pageIndex">第几页</param>
        /// <param name="pagesize">一页多少条</param>
        /// <param name="rowcount">返回共多少条</param>
        /// <param name="predicate">查询条件</param>
        /// <param name="keySelector">排序字段</param>
        /// <param name="IsQueryOrderBy">true为升序 false为降序</param>
        /// <returns></returns>
        IQueryable<TEntity> QueryByPage<TKey>(int pageIndex, int pagesize, out int rowcount, Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TKey>> keySelector, bool IsQueryOrderBy);

        #region 编辑
        /// <summary>
        /// 通过传入的model加需要修改的字段来更改数据
        /// </summary>
        /// <param name="model"></param>
        /// <param name="propertys"></param>
        void Edit(TEntity model, string[] propertys);

        /// <summary>
        /// 直接查询之后再修改
        /// </summary>
        /// <param name="model"></param>
        void Edit(TEntity model);
        #endregion

        #region 删除
        void Delete(TEntity model, bool isadded);

     
        #endregion

        #region 新增
        void Add(TEntity model);
        void Add(IEnumerable<TEntity> models);
        #endregion

        #region 统一提交
        int SaverChanges();
        #endregion

        #region 调用存储过程返回一个指定的TResult
        List<TResult> RunProc<TResult>(string sql, params object[] pamrs);
        int ExecuteProc(string sql, params SqlParameter[] pars);


        #endregion
    }
   
}