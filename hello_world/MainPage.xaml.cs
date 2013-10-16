using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using hello_world.Resources;

namespace hello_world
{

    public partial class MainPage : PhoneApplicationPage
    {
        // Конструктор
        //уникальный идентификатор приложения вконтакте https://vk.com/editapp?act=create, которое создается по этой ссылке
        private string appId = "3934298";
        public MainPage()
        {
            InitializeComponent();
            Loaded += MainPage_Loaded;
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            webView.Navigated += webView_Navigated;
            webView.Navigate(
                new Uri(
                    string.Format(
                        "https://api.vk.com/oauth/authorize?client_id={0}&scope=wall&redirect_uri={1}&display=touch&response_type=token",
                        appId, "http://oauth.vk.com/blank.html")));
        }

        void webView_Navigated(object sender, NavigationEventArgs e)
        {
            GetAccess(e.Uri.AbsoluteUri);
        }
        /// <summary>
        /// метод для парсинга эксесс токена и айди юзера
        /// </summary>
        /// <param name="u"></param>
        public void GetAccess(string u)
        {

            char[] ch = { '=', '&' };
            string[] massRespon = u.Split(ch);

            if (massRespon.Length == 6)
            {
                App.AccessToken = massRespon[1];
                App.IdUser = massRespon[5];
                webView.Visibility = Visibility.Collapsed;

            }

        }
        /// <summary>
        /// при нажатии происходит отправка сообщения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UIElement_OnTap(object sender, GestureEventArgs e)
        {
            var message = box.Text;
            WebClient wc = new WebClient();
            string uri = string.Format("https://api.vk.com/method/wall.post.xml?owner_id={0}&message={1}&access_token={2}", App.IdUser, message, App.AccessToken);
            wc.DownloadStringAsync(new Uri(uri));
        }
    }
}