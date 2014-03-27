

namespace wpfLockTest
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {


            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
                //System.Windows.Forms.Application.Run(new Form1());

                Window1 win = new Window1();
                win.Show();
                System.Windows.Threading.Dispatcher.Run();
            });
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
            while (true)
            {
                System.Threading.Thread.Sleep(1);
            }
            //app.Run(new Window1());
        }

        private void Button_Click_1(object sender, System.Windows.RoutedEventArgs e)
        {
            System.Threading.Thread t = new System.Threading.Thread(() =>
         {
             System.Windows.Forms.Application.Run(new Form1());
         });
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
            while (true)
            {
                System.Threading.Thread.Sleep(1);
            }
        }
    }
}
