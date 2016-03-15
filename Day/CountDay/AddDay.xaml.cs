using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Day
{
    public partial class AddDay : PhoneApplicationPage
    {
        public AddDay()
        {
            InitializeComponent();
            this.inputCircle.ItemsSource = new List<String>(){"无","每周","每月","每年" };
            if((int)App.localSettings.Values["state"] == 1)
            {
                MemoryDay item = App.myViewModel.getMemoryDay((int)App.localSettings.Values["holdPage"], (int)App.localSettings.Values["holdIndex"]);
                if(item != null)
                {
                    inputName.Text = item.getName();
                    inputDate.Value = item.getDate();
                    inputCircle.SelectedItem = item.getCircle();
                    inputMode_1.IsChecked = App.myViewModel.isCount((int)App.localSettings.Values["holdPage"], (int)App.localSettings.Values["holdIndex"]);
                    inputMode_2.IsChecked = App.myViewModel.isMemorial((int)App.localSettings.Values["holdPage"], (int)App.localSettings.Values["holdIndex"]);
                }
            }
            else
            {
                inputMode_1.IsChecked = true;
                inputMode_2.IsChecked = true;
            }
        }

        private void ApplicationBar_StateChanged(object sender, ApplicationBarStateChangedEventArgs e)
        {

        }

        //确定按钮 事件 编辑或者添加
        private void ApplicationBarIconButton_Click_yes(object sender, EventArgs e)
        {
            if(inputName.Text.Length  == 0)
            {
                MessageBox.Show("不可以没有名称的哦！","小X：",MessageBoxButton.OK);
                return;
            }
            if(inputMode_1.IsChecked == false && inputMode_2.IsChecked == false)
            {
                MessageBox.Show("猜猜您要显示到哪里呢","小X：", MessageBoxButton.OK);
                return;
            }
            else
            {
                if ((int)App.localSettings.Values["state"] == 1)//编辑
                {
                    App.myViewModel.delete((int)App.localSettings.Values["holdPage"], (int)App.localSettings.Values["holdIndex"]);
                }
                if(inputMode_1.IsChecked == true)
                {
                    App.myViewModel.addCountDay(inputName.Text, inputDate.Value.Value, inputCircle.SelectedItem.ToString());
                }
                if(inputMode_2.IsChecked == true)
                {
                    App.myViewModel.addMenorialDay(inputName.Text, inputDate.Value.Value, inputCircle.SelectedItem.ToString());
                }
            }
            App.myViewModel.update();
            NavigationService.RemoveBackEntry();
            if ((bool)App.localSettings.Values["addDayFromDetail"] == true)
            {
                App.localSettings.Values["addDayFromDetail"] = false;
                App.localSettings.Values["state"] = 3;
                NavigationService.Navigate(
                    new Uri("/CountDay/Detail.xaml?DetailFromAddDay=true", UriKind.Relative));
            }
            else
            {
                App.localSettings.Values["state"] = 0;
                NavigationService.Navigate(
                    new Uri("/MainPage.xaml?homeFromAddDay=true", UriKind.Relative));
            }
        }

        //取消按钮 事件
        private void ApplicationBarIconButton_Click_cancel(object sender, EventArgs e)
        {
            if ((bool)App.localSettings.Values["addDayFromDetail"] == true)
            {
                NavigationService.RemoveBackEntry();
                App.localSettings.Values["addDayFromDetail"] = false;
                App.localSettings.Values["state"] = 3;
                NavigationService.Navigate(
                    new Uri("/CountDay/Detail.xaml?DetailFromAddDay=true", UriKind.Relative));
            }
            else
            {
                NavigationService.Navigate(
                    new Uri("/MainPage.xaml?homeFromAddDay=true", UriKind.Relative));
            }
        }
    }
}