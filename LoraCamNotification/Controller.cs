using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;

namespace LoraCamNotification
{
    class Controller
    {
        public static WebClient wClient = new WebClient();
        private string url;
        private string content;
        public int cam1, cam2, cam3, cam4;   // Trạng thái camera: 0-->blank 1-->onstream 2-->pending
        XmlDocument doc = new XmlDocument();

        public Controller(string url_in)
        {
            url = url_in;
            url += "/API?";
        }
        private string readFromLink()
        {
            System.Net.WebClient client = new System.Net.WebClient();
            byte[] data = client.DownloadData(url);
            string html = System.Text.Encoding.UTF8.GetString(data);

            XmlReader reader = XmlReader.Create(new StringReader(html));
            //Parse html to xml
            doc.LoadXml(html);
            
                return html;
        }

        public string GetContent()
        {
            content = readFromLink();
            return content;
        }

        public void ProcessContent()
        {
            GetContent();
            XmlNodeList elemList = doc.GetElementsByTagName("active");
            SetAllZero();
            int index = Convert.ToInt16(elemList[0].InnerText);
            switch (index)
            {
                case 1:
                    cam1 = 1;
                    break;
                case 2:
                    cam2 = 1;
                    break;
                case 3:
                    cam3 = 1;
                    break;
                case 4:
                    cam4 = 1;
                    break;
            }

            elemList = doc.GetElementsByTagName("preview");
            index = Convert.ToInt16(elemList[0].InnerText);
            switch (index)
            {
                case 1:
                    cam1 = 2;
                    break;
                case 2:
                    cam2 = 2;
                    break;
                case 3:
                    cam3 = 2;
                    break;
                case 4:
                    cam4 = 2;
                    break;
            }

           
        }

        public int GetCamStatus(int index)
        {
            switch (index)
            {
                case 1:
                    return cam1;
                case 2:
                    return cam2;                    
                case 3:
                    return cam3;                    
                case 4:
                    return cam4;                   
            }
            return 0;
        }

        private void SetAllZero()
        {
            //if ((cam1 != 1) && (cam1 != 2)) cam1 = 0;
            //if ((cam2 != 1) && (cam2 != 2)) cam2 = 0;
            //if ((cam3 != 1) && (cam3 != 2)) cam3 = 0;
            //if ((cam4 != 1) && (cam4 != 2)) cam4 = 0;
            cam1 = cam2 = cam3 = cam4 = 0;
        }
    }
}
