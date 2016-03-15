using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Day.CountDay
{
    public partial class Detail : PhoneApplicationPage
    {
        public Detail()
        {
            InitializeComponent();
            App.localSettings.Values["isIn"] = false;
            if((int)App.localSettings.Values["state"] == 3)
            {
                MemoryDay item = App.myViewModel.getMemoryDay((int)App.localSettings.Values["holdPage"], (int)App.localSettings.Values["holdIndex"]);
                load(item, (int)App.localSettings.Values["holdPage"]);
            }
        }

        /// <summary>
        /// 加载页面数据
        /// </summary>
        /// <param name="m">存储页面数据的对象</param>
        /// <param name="mod">显示模式:1-小X还有，1-已经</param>
        public void load(MemoryDay m, int mod)
        {
            this.DetailName.Text = m.getName();
            if (mod == 1)
            {
                this.DetailModeUp.Text = "还有";
                this.DetailNum.Text = m.getCountLen().ToString(); ;
            }
            else
            {
                this.DetailModeUp.Text = "已经";
                this.DetailNum.Text = m.getMemoryLen().ToString();
            }
            this.DetailDate.Text = m.getDate().ToLongDateString().ToString();
            this.DetailCircle.Text = m.getCircle();
            string mode = "";
            if (App.myViewModel.isCount(m))
                mode = "小X倒数 " + mode;
            if (App.myViewModel.isMemorial(m))
                mode = "小X纪念 " + mode;
            this.DetailMode.Text = mode;
        }

        //编辑
        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            App.localSettings.Values["state"] = 1;
            App.localSettings.Values["addDayFromDetail"] = true;
            NavigationService.Navigate(
                new Uri("/CountDay/AddDay.xaml", UriKind.Relative));
        }

        //删除
        private void ApplicationBarIconButton_Click_1(object sender, EventArgs e)
        {
            App.myViewModel.delete((int)App.localSettings.Values["holdPage"], (int)App.localSettings.Values["holdIndex"]);
            NavigationService.Navigate(
                new Uri("/MainPage.xaml?homeFromDtail=true", UriKind.Relative));
        }

        //添加到桌面
        private void ApplicationBarIconButton_Click_2(object sender, EventArgs e)
        {
            //获取要固定到开始屏幕的对象
            MemoryDay item = App.myViewModel.getMemoryDay((int)App.localSettings.Values["holdPage"], (int)App.localSettings.Values["holdIndex"]);
            if (item == null) return;

            ShellTile TileToFind = ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().Contains(item.getName()));
            //判断该对象是否已经固定到桌面
            if (TileToFind == null)
            {
                StandardTileData newTileData = new StandardTileData
                {
                    Title = item.getName(),
                    BackTitle = item.getName(),
                    BackContent = item.getDate().ToLongDateString().ToString() + Environment.NewLine +
                    (App.myViewModel.isCount(item) ? ("还有" + item.getCountLen() + "天" + (App.myViewModel.isMemorial(item) ?
                        (Environment.NewLine + "已经" + item.getMemoryLen() + "天") : "")) : (Environment.NewLine + "已经" + item.getMemoryLen() + "天")),
                    BackBackgroundImage = new Uri("/Image/backbackground.png", UriKind.Relative)
                };
                ShellTile.Create(new Uri("/CountDay/Detail.xaml?" + item.getName() + "=" + (int)App.localSettings.Values["holdPage"], UriKind.Relative), newTileData);
            }
            else
            {
                MessageBox.Show("亲，您已经添加过这个事项磁贴了哦","小X：",MessageBoxButton.OK);
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //处理从添加页面返回来的页面记录
            if (e.NavigationMode == System.Windows.Navigation.NavigationMode.New && NavigationContext.QueryString.ContainsKey("DetailFromAddDay"))
            {
               // MemoryDay item = App.myViewModel.getMemoryDay((int)App.localSettings.Values["holdPage"], (int)App.localSettings.Values["holdIndex"]);
               // load(item, (int)App.localSettings.Values["holdPage"]);
                NavigationService.RemoveBackEntry();
            }
            base.OnNavigatedTo(e);
            
            //处理从磁贴访问的情况
            string name = null;
            if(this.NavigationContext.QueryString != null)
            {
                if(App.myViewModel.countDayList != null)
                {
                    int i = -1;
                    foreach(MemoryDay m in App.myViewModel.countDayList)
                    {
                        i++;
                        this.NavigationContext.QueryString.TryGetValue(m.getName(), out name);
                        if(name != null)
                        {
                            if (name != "1") break; ;
                            App.localSettings.Values["holdPage"] = 1;
                            App.localSettings.Values["holdIndex"] = i;
                            load(m, 1);
                            return;
                        }
                    }
                    i = -1;
                    foreach (MemoryDay m in App.myViewModel.memorialDayList)
                    {
                        i++;
                        this.NavigationContext.QueryString.TryGetValue(m.getName(), out name);
                        if (name != null)
                        {
                            if (name != "2") break;
                            App.localSettings.Values["holdPage"] = 2;
                            App.localSettings.Values["PivotLoad"] = 1;
                            App.localSettings.Values["holdIndex"] = i;
                            load(m, 2);
                            return;
                        }
                    }
                }
            }
        }

        //回到首页
        private void ApplicationBarIconButton_Click_3(object sender, EventArgs e)
        {
            NavigationService.Navigate(
                new Uri("/MainPage.xaml?homeFromDtail=true", UriKind.Relative));
        }
    }
}