﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSX.Contract.DTOs;

public class RegisterDTO
{
    public string Email { get; set; } = string.Empty; 
    public string Password { get; set; } = string.Empty;
}
