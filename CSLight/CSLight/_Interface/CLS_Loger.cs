﻿using System;
using System.Collections.Generic;
using System.Text;
namespace CSLight
{
   
    public interface ICLS_Logger
    {
        void Log(string str);
        void Log_Warn(string str);
        void Log_Error(string str);
    }

}