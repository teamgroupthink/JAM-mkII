﻿using System.Collections.Generic;
using JAM_mkII.Models;
using Microsoft.AspNetCore.Identity;

public class UserViewModel
{
    public IEnumerable<User> Users { get; set; }
    public IEnumerable<IdentityRole> Roles { get; set; }
}