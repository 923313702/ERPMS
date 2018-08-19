using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERPMS.Models.DingTalk
{
    public class User
    {

        public User ()
        {
        
            this.extattr = new Dictionary<string, string>();
        }
        public string userid { get; set; }
        public string  name { get; set; }
        public List<int> department { get; set; }

     
        public string orderInDepts { get; set; }
        public string position { get; set; }
        public string mobile { get; set; }
        public string tel { get; set; }
        public string  workPlace { get; set; }
        public string enail { get; set; }
        public string orgEmail { get; set; }
        public string jobnumber { get; set; }
        public bool isHide { get; set; }
        public bool isSenior { get; set; }
        public  Dictionary<string,string>  extattr{ get; set; }
    }

   
}