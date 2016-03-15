using System;
using System.Linq;
using System.Net;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Navigation;
using System.Windows.Threading;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Threading;
using System.ComponentModel;
using Day.Resources;
using Day.CountDay;

namespace Day
{
    public partial class MainPage : PhoneApplicationPage
    {
        private BackgroundWorker backGroundWorker;
        private Popup popup;

        // 构造函数
        public MainPage()
        {
            InitializeComponent();

            //为了显示启动画面
            if ((bool)App.localSettings.Values["isIn"] == true)
            {
                App.localSettings.Values["isIn"] = false;
                popup = new Popup() { IsOpen = true, Child = new SplashScreen() };
                backGroundWorker = new BackgroundWorker();
                RunBackgroundWorker();
                this.SetValue(SystemTray.IsVisibleProperty, false);
                myMainPage.ApplicationBar.IsVisible = false;
            }

            //将主页面数据绑定到容器
            listBoxDaoshu.DataContext = App.myViewModel.CountDay;
            listBoxJishu.DataContext = App.myViewModel.MemorialDay;
            App.myViewModel.update();

            //实时显示当前时间
            textBlockDaoshu.Text = DateTime.Now.ToString("今天是 yyyy 年 MM 月 dd 日");
            textBlockJishu.Text = DateTime.Now.ToString("今天是 yyyy 年 MM 月 dd 日");
            DispatcherTimer newtimer = new DispatcherTimer();
            newtimer.Interval = TimeSpan.FromSeconds(1);
            newtimer.Tick += OnTimerTick;
            newtimer.Start();
        }

        private void RunBackgroundWorker()
        {
            backGroundWorker.DoWork += ((s, args) =>
            {
                Thread.Sleep(3000);
            });
            backGroundWorker.RunWorkerCompleted += ((s, args) =>
            {
                this.Dispatcher.BeginInvoke(() =>
                {
                    this.popup.IsOpen = false;
                    this.SetValue(SystemTray.IsVisibleProperty, true);
                    myMainPage.ApplicationBar.IsVisible = true;
                });
            });
            backGroundWorker.RunWorkerAsync();
        }

        void OnTimerTick(Object sender, EventArgs args)
        {
            textBlockDaoshu.Text = DateTime.Now.ToString("今天是 yyyy 年 MM 月 dd 日");
            textBlockJishu.Text = DateTime.Now.ToString("今天是 yyyy 年 MM 月 dd 日");
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (NavigationContext.QueryString != null &&
                (NavigationContext.QueryString.ContainsKey("homeFromAddDay") || NavigationContext.QueryString.ContainsKey("homeFromDtail")))
            {
                NavigationService.RemoveBackEntry();
            }
            base.OnNavigatedTo(e);
        }

        //添加按钮事件 state = 2
        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            App.localSettings.Values["state"] = 2;
            NavigationService.Navigate(
                new Uri("/CountDay/AddDay.xaml", UriKind.Relative));
        }

        private void PhoneApplicationPage_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
        }

        //小X倒数页面hold事件
        private void GestureListener_Hold(object sender, GestureEventArgs e)
        {
            var holdItem = (sender as Grid).DataContext;
            int i = -1;
            foreach (var item in listBoxDaoshu.Items)
            {
                i++;
                if (item == holdItem)
                {
                    break;
                }
            }
            App.localSettings.Values["holdPage"] = 1;
            App.localSettings.Values["PivotLoad"] = 0;
            App.localSettings.Values["holdIndex"] = i;
        }

        //小X纪念页面hold事件
        private void GestureListener_Hold_1(object sender, GestureEventArgs e)
        {
            var holdItem = (sender as Grid).DataContext;
            int i = -1;
            foreach (var item in listBoxJishu.Items)
            {
                i++;
                if (item == holdItem)
                {
                    break;
                }
            }
            App.localSettings.Values["holdPage"] = 2;
            App.localSettings.Values["PivotLoad"] = 1;
            App.localSettings.Values["holdIndex"] = i;
        }

        //固定到开始屏幕
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            //获取要固定到开始屏幕的对象
            MemoryDay item = App.myViewModel.getMemoryDay((int)App.localSettings.Values["holdPage"], (int)App.localSettings.Values["holdIndex"]);
            if (item == null) return;

            ShellTile TileToFind = ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().Contains(item.getName()));
            //判断该对象是否已经固定到桌面
            if (TileToFind == null)
            {
                StandardTileData newTitleData = new StandardTileData
                {
                    Title = item.getName(),
                    BackTitle = item.getName(),
                    BackContent = item.getDate().ToLongDateString().ToString() + Environment.NewLine +
                    (App.myViewModel.isCount(item) ? ("还有" + item.getCountLen() + "天" + (App.myViewModel.isMemorial(item) ?
                        (Environment.NewLine + "已经" + item.getMemoryLen() + "天") : "")) : (Environment.NewLine + "已经" + item.getMemoryLen() + "天")),
                    BackBackgroundImage = new Uri("/Image/backbackground.png", UriKind.Relative)
                };
                ShellTile.Create(new Uri("/CountDay/Detail.xaml?" + item.getName() + "=" + (int)App.localSettings.Values["holdPage"], UriKind.Relative), newTitleData);
            }
            else
            {
                MessageBox.Show("亲，您已经添加过此事项的磁贴了哦", "小X：", MessageBoxButton.OK);
            }
        }

        //查看 state = 3
        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            App.localSettings.Values["state"] = 3;
            NavigationService.Navigate(
                new Uri("/CountDay/Detail.xaml", UriKind.Relative));
        }

        //编辑 state = 1
        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            App.localSettings.Values["state"] = 1;
            NavigationService.Navigate(
                new Uri("/CountDay/AddDay.xaml", UriKind.Relative));
        }

        //删除
        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            var re = MessageBox.Show("删除？", "小X：", MessageBoxButton.OKCancel);
            if (re == MessageBoxResult.OK)
            {
                App.myViewModel.delete((int)App.localSettings.Values["holdPage"], (int)App.localSettings.Values["holdIndex"]);
                App.myViewModel.update();
            }
        }

        private void PivotItem_Loaded(object sender, RoutedEventArgs e)
        {
            myPivot.SelectedIndex = (int)App.localSettings.Values["PivotLoad"];
            App.localSettings.Values["PivotLoad"] = 0;
        }

        // 用于生成本地化 ApplicationBar 的示例代码
        //private void BuildLocalizedApplicationBar()
        //{
        //    // 将页面的 ApplicationBar 设置为 ApplicationBar 的新实例。
        //    ApplicationBar = new ApplicationBar();

        //    // 创建新按钮并将文本值设置为 AppResources 中的本地化字符串。
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // 使用 AppResources 中的本地化字符串创建新菜单项。
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}