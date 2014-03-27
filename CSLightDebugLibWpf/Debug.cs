using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSLightDebug
{
    public class DebugWPF
    {
        public static void OpenDebugWin(Action onClose)
        {
            System.Threading.Thread t =new System.Threading.Thread(()=>
                {
                    System.Windows.Application app = new System.Windows.Application();
                    CSLightDebugLibWpf.DebugWindow win = new CSLightDebugLibWpf.DebugWindow();
                    app.Run(win);
                    onClose();
                });
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
        }

    }
}
