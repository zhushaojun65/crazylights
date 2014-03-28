using System;
using System.Collections.Generic;

using System.Text;

namespace CSLightDebug
{
    public class Debug
    {
        public delegate void Action();
        public static void OpenDebugWin(Action onClose)
        {
            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
                System.Windows.Forms.Application.Run(new CSLightDebug.WhatAFuck());
                onClose();
            });
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
        }

    }
}
