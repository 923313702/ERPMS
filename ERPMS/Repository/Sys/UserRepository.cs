﻿using ERPMS.IRepository.Sys;
using ERPMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERPMS.Repository.Sys
{
    public class UserRepository : BaseRepository<SYS_用户信息表>, IUserRepository
    {
    }
}