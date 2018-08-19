using ERPMS.IRepository.Basics;
using ERPMS.Repository.Sys;
using ERPMS.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ERPMS.Controllers.Sys
{
    /// <summary>
    /// 组织编码表
    /// </summary>
    public class OrganizeController : Controller
    {
        IOrganizeRepository oRepository = new OrganizeRepository();
        // GET: Organize
        public ActionResult Index()
        {
            return View();
        }
        //public ActionResult ShowTree()
        //{
        //    var query = oRepository.Query().Select(p => new Tree { Id = p.编码, Name = p.名称, ParentId = (string.IsNullOrEmpty(p.上级编码) ? "" : p.上级编码), Group = p.班组标识 == null ? 1 : p.班组标识, Workshop = p.车间属性 }).ToList();
        //    var list = Node.RawCollectionToTree(query, true);
        //    int total = oRepository.Query().Count();
        //    string json = JsonConvert.SerializeObject(list, Formatting.Indented,
        //    new JsonSerializerSettings
        //    {
        //        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
        //        NullValueHandling = NullValueHandling.Ignore
        //    });
        //    var str = "{\"total\":" + total + ",\"rows\":" + json + "}";
        //    return Content(str);
        //}

    }
}