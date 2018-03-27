using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace LoraCamNotification
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /***************************
        *       Variables
        ***************************/
        string yellow_btn_url = Directory.GetCurrentDirectory().ToString();
        string green_btn_url = Directory.GetCurrentDirectory().ToString();
        string red_btn_url = Directory.GetCurrentDirectory().ToString();
        ImageSource yellow_btn, green_btn, red_btn;
        /***************************
        *       Constant
        ***************************/
        const int
            blank = 0,
            active = 1,
            preview = 2;
        const int
            interval = 1000;
        /***************************
        *       Timer
        ***************************/
        DispatcherTimer Process_Timer = new DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();
            Application.Current.ShutdownMode = ShutdownMode.OnLastWindowClose;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            yellow_btn_url =  GetResourceFolder(yellow_btn_url);
            yellow_btn_url += @"\Sources\yellow_btn.png";
            green_btn_url = GetResourceFolder(green_btn_url);
            green_btn_url += @"\Sources\green_btn.png";
            red_btn_url = GetResourceFolder(red_btn_url);
            red_btn_url += @"\Sources\red_btn.png";

            yellow_btn = GetImgageSource(yellow_btn_url);
            green_btn = GetImgageSource(green_btn_url);
            red_btn = GetImgageSource(red_btn_url);

            Cam1_noti.Source = red_btn;
            Cam1_noti_Text.Text = "Nothing";

            Cam2_noti.Source = red_btn;
            Cam2_noti_Text.Text = "Nothing";

            Cam3_noti.Source = red_btn;
            Cam3_noti_Text.Text = "Nothing";

            Cam4_noti.Source = red_btn;
            Cam4_noti_Text.Text = "Nothing";

            GlobalVar.Serial_Data = new Serial_Data();

            Serial_Add_Element();    
        }

        private void SetCamNoti(int index)
        {
            switch (index)
            {
                case 1:
                    switch (GlobalVar.Control.cam1)
                    {
                        case blank:
                            Cam1_noti.Source = red_btn;
                            Cam1_noti_Text.Text = "Nothing";
                            break;
                        case active:
                            Cam1_noti.Source = green_btn;
                            Cam1_noti_Text.Text = "Active";
                            break;
                        case preview:
                            Cam1_noti.Source = yellow_btn;
                            Cam1_noti_Text.Text = "Preview";
                            break;
                    }
                    break;
                case 2:
                    switch (GlobalVar.Control.cam2)
                    {
                        case blank:
                            Cam2_noti.Source = red_btn;
                            Cam2_noti_Text.Text = "Nothing";
                            break;
                        case active:
                            Cam2_noti.Source = green_btn;
                            Cam2_noti_Text.Text = "Active";
                            break;
                        case preview:
                            Cam2_noti.Source = yellow_btn;
                            Cam2_noti_Text.Text = "Preview";
                            break;
                    }
                    break;
                case 3:
                    switch (GlobalVar.Control.cam3)
                    {
                        case blank:
                            Cam3_noti.Source = red_btn;
                            Cam3_noti_Text.Text = "Nothing";
                            break;
                        case active:
                            Cam3_noti.Source = green_btn;
                            Cam3_noti_Text.Text = "Active";
                            break;
                        case preview:
                            Cam3_noti.Source = yellow_btn;
                            Cam3_noti_Text.Text = "Preview";
                            break;
                    }
                    break;
                case 4:
                    switch (GlobalVar.Control.cam4)
                    {
                        case blank:
                            Cam4_noti.Source = red_btn;
                            Cam4_noti_Text.Text = "Nothing";
                            break;
                        case active:
                            Cam4_noti.Source = green_btn;
                            Cam4_noti_Text.Text = "Active";
                            break;
                        case preview:
                            Cam4_noti.Source = yellow_btn;
                            Cam4_noti_Text.Text = "Preview";
                            break;
                    }
                    break;
            }
        }

        private void Address_Connect_Btn_Click(object sender, RoutedEventArgs e)
        {
            if ((Address.Text != "") && (Address.Text != " "))
            {
                GlobalVar.Control = new Controller(Address.Text);
                GlobalVar.Control.GetContent();

                Process_Timer.Tick += Process_Tick;
                Process_Timer.Interval = TimeSpan.FromMilliseconds(interval);
                Process_Timer.Start();
            }
        }

        private void Process_Tick(object sender, EventArgs e)
        {
            GlobalVar.Control.ProcessContent();
            GlobalVar.Control.cam1 = GlobalVar.Control.GetCamStatus(1);
            GlobalVar.Control.cam2 = GlobalVar.Control.GetCamStatus(2);
            GlobalVar.Control.cam3 = GlobalVar.Control.GetCamStatus(3);
            GlobalVar.Control.cam4 = GlobalVar.Control.GetCamStatus(4);

            for (int i = 1; i <= 4; i++)
            {
                SetCamNoti(i);
            }
        }

        private void Serial_Connect_Btn_Click(object sender, RoutedEventArgs e)
        {
            string comName, Baud, Databits, Parity, Stopbit;
            Boolean OK;

            try
            {
                comName = combo_box_COM.SelectedItem.ToString();
                Baud = combo_box_BaudRate.SelectedItem.ToString();
                Databits = combo_box_Databits.SelectedItem.ToString();
                Parity = combo_box_Parity.SelectedItem.ToString();
                Stopbit = combo_box_Stopbit.SelectedItem.ToString();

                OK = GlobalVar.Serial_Data.connect(comName, Baud, Databits, Parity, Stopbit);
                if (OK)
                {
                    ProgressBar_Connection_Status.Value = 100;

                }
                else
                {
                    ProgressBar_Connection_Status.Value = 0;
                }
            }
            catch (Exception ex)
            {

            }
        }

        private string GetResourceFolder(string directory)
        {
            directory = Directory.GetParent(directory).ToString();
            directory = Directory.GetParent(directory).ToString();
            return directory;
        }

        private void Serial_Refresh_Btn_Click(object sender, RoutedEventArgs e)
        {
            Serial_Add_Element();
        }

        private ImageSource GetImgageSource(string url)
        {
            ImageSource ImgSource = new BitmapImage(new Uri(url));
            return ImgSource;
        }

        private void Serial_Add_Element()
        {
            List<string> temp = new List<string>();

            // Get All PortName
            temp = GlobalVar.Serial_Data.Get_PORT();
            combo_box_COM.ItemsSource = temp;
            //combo_box_COM.SelectedIndex = 0;

            //Get BaudRate
            temp = GlobalVar.Serial_Data.Get_Baud();
            combo_box_BaudRate.ItemsSource = temp;
            combo_box_BaudRate.SelectedIndex = 2;

            //Get Databits
            temp = GlobalVar.Serial_Data.Get_Databits();
            combo_box_Databits.ItemsSource = temp;
            combo_box_Databits.SelectedIndex = 1;

            //Get Parity
            temp = GlobalVar.Serial_Data.Get_Parity();
            combo_box_Parity.ItemsSource = temp;
            combo_box_Parity.SelectedIndex = 0;

            //Get Stopbit
            temp = GlobalVar.Serial_Data.Get_Stopbit();
            combo_box_Stopbit.ItemsSource = temp;
            combo_box_Stopbit.SelectedIndex = 1;
        }
    }
}
