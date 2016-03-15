using System;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Phone.Shell;
using System.Windows.Navigation;
using System.Linq;

namespace Day
{ 
    public class MDayViewModel : INotifyPropertyChanged
    {
        //定义倒数日和纪念日存储MDay元素的数组
        public List<MemoryDay> countDayList;
        public List<MemoryDay> memorialDayList;

        //定义所有倒数日的可见集合
        public ObservableCollection<MDay> countDay;
        //定义所有纪念日的可见集合
        public ObservableCollection<MDay> memorialDay;

        /// <summary>
        /// 将集合做为ViewModel层的属性
        /// </summary>
        public ObservableCollection<MDay> CountDay
        {
            get
            {
                if (countDay == null)
                    countDay = new ObservableCollection<MDay>();
                return countDay;
            }
            set 
            {
                if(countDay != value)
                {
                    countDay = value;
                    NotifyPropertyChanged("CountDay");
                }
            }
        }

        /// <summary>
        /// 将集合做为ViewModel层的属性
        /// </summary>
        public ObservableCollection<MDay> MemorialDay
        {
            get
            {
                if (memorialDay == null)
                    memorialDay = new ObservableCollection<MDay>();
                return memorialDay;
            }
            set
            {
                if (memorialDay != value)
                {
                    memorialDay = value;
                    NotifyPropertyChanged("MemorialDay");
                }
            }
        }

        /// <summary>
        /// 构造函数：从文件中加载数据
        /// </summary>
        public MDayViewModel()
        {
            LoadData();
        }

        /// <summary>
        /// 从文件中加载数据,不成功的话就初始化列表
        /// </summary>
        public void LoadData()
        {
            countDayList = DataIO.ReadFromFile("countday.dat", typeof(List<MemoryDay>)) as List<MemoryDay>;
            memorialDayList = DataIO.ReadFromFile("memorialday.dat", typeof(List<MemoryDay>)) as List<MemoryDay>;
            
            if (countDayList == null) countDayList = new List<MemoryDay>();
            if (memorialDayList == null) memorialDayList = new List<MemoryDay>();
        }

        /// <summary>
        /// 保存数据到文件
        /// </summary>
        public void StoreData()
        {
            DataIO.WriteToFile("countday.dat", typeof(List<MemoryDay>), this.countDayList);
            DataIO.WriteToFile("memorialday.dat", typeof(List<MemoryDay>), this.memorialDayList);
        }

        /// <summary>
        /// 获取列表中的一个对象
        /// </summary>
        /// <param name="pageIndex">指定列表:1-countDayList,2-memorialDayList</param>
        /// <param name="itemIndex">对象在指定列表中的下标</param>
        /// <returns></returns>
        public MemoryDay getMemoryDay(int pageIndex, int itemIndex)
        {
            if(pageIndex == 1 && countDayList.Count > itemIndex)
            {
                return countDayList[itemIndex];
            }
            else if(pageIndex == 2 && memorialDayList.Count > itemIndex)
            {
                return memorialDayList[itemIndex];
            }
            return null;
        }

        /// <summary>
        /// 指定对象是否显示到小X倒数
        /// </summary>
        /// <param name="pageIndex">引发次判断的页面：1小X倒数；2小X纪念</param>
        /// <param name="itemIndex">对象在列表中的下标</param>
        /// <returns></returns>
        public bool isCount(int pageIndex, int itemIndex)
        {
            if (pageIndex == 1) return true;
            if(pageIndex == 2 && itemIndex < memorialDayList.Count)
            {
                foreach(MemoryDay item in countDayList)
                {
                    if (item.equals(memorialDayList[itemIndex]))
                        return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 指定对象是否显示到小X倒数
        /// </summary>
        /// <param name="m">要查询的对象</param>
        /// <returns></returns>
        public bool isCount(MemoryDay m)
        {
            
            foreach (MemoryDay item in countDayList)
            {
                if (item.equals(m))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 指定对象是否显示到小X纪念
        /// </summary>
        /// <param name="pageIndex">引发次判断的页面：1小X倒数；2小X纪念</param>
        /// <param name="itemIndex">记录在容器中的序号</param>
        /// <returns></returns>
        public bool isMemorial(int pageIndex, int itemIndex)
        {
            if(pageIndex == 2) return true;
            if(pageIndex == 1 && itemIndex < countDayList.Count)
            {
                foreach(MemoryDay item in memorialDayList)
                {
                    if (item.equals(countDayList[itemIndex]))
                        return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 指定对象是否显示到小X纪念
        /// </summary>
        /// <param name="m">要查询的对象</param>
        /// <returns></returns>
        public bool isMemorial(MemoryDay m)
        {

            foreach (MemoryDay item in memorialDayList)
            {
                if (item.equals(m))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 更新小X倒数绑定的可见集合CountDay
        /// </summary>
        private void updateCountDay()
        {
            ShellTile TileToFind;
            if (countDay == null) countDay = new ObservableCollection<MDay>();
            CountDay.Clear();
            if (countDayList == null || countDayList.Count == 0) return;
            foreach (MemoryDay m in countDayList)
            {
                //更新磁贴
                TileToFind = ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().Contains(m.getName()));
                if (TileToFind != null)
                {
                    StandardTileData NewData = new StandardTileData
                    {
                        Title = m.getName(),
                        BackTitle = m.getName(),
                        BackContent = m.getDate().ToLongDateString().ToString() + Environment.NewLine +
                        (App.myViewModel.isCount(m) ? ("还有" + m.getCountLen() + "天" + (App.myViewModel.isMemorial(m) ?
                            (Environment.NewLine + "已经" + m.getMemoryLen() + "天") : "")) : (Environment.NewLine + "已经" + m.getMemoryLen() + "天")),
                        BackBackgroundImage = new Uri("/Image/backbackground.png", UriKind.Relative)
                    };
                    TileToFind.Update(NewData);
                }
                CountDay.Add(m.getCountMDay());
            }
        }

        /// <summary>
        /// 更新小X纪念绑定的可见集合MemorialDay
        /// </summary>
        private void updateMemorialDay()
        {
            ShellTile TileToFind;
            if (memorialDay == null) memorialDay = new ObservableCollection<MDay>();
            MemorialDay.Clear();
            if (memorialDayList == null || memorialDayList.Count == 0) return;
            foreach (MemoryDay m in memorialDayList)
            {
                //更新磁贴
                TileToFind = ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().Contains(m.getName()));
                if (TileToFind != null)
                {
                    StandardTileData NewData = new StandardTileData
                    {
                        Title = m.getName(),
                        BackTitle = m.getName(),
                        BackContent = m.getDate().ToLongDateString().ToString() + Environment.NewLine +
                        (App.myViewModel.isCount(m) ? ("还有" + m.getCountLen() + "天" + (App.myViewModel.isMemorial(m) ?
                            (Environment.NewLine + "已经" + m.getMemoryLen() + "天") : "")) : (Environment.NewLine + "已经" + m.getMemoryLen() + "天")),
                        BackBackgroundImage = new Uri("/Image/backbackground.png", UriKind.Relative)
                    };
                    TileToFind.Update(NewData);
                }
                MemorialDay.Add(m.getMemoryMDay());
            }
        }

        /// <summary>
        /// 更新所有绑定数据：将列表数据同步到可见集合
        /// </summary>
        public void update()
        {
            updateCountDay();
            updateMemorialDay();
        }


        /// <summary>
        /// 添加一项到小X倒数
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="date">日期</param>
        /// <param name="circle">周期</param>
        public void addCountDay(string name, DateTime date, string circle)
        {
            if (countDayList == null)
            {
                countDayList = new List<MemoryDay>();
            }
            countDayList.Add(new MemoryDay(name, date, circle,1));
            countDayList.Sort(delegate(MemoryDay a, MemoryDay b)
            {
                return a.getCountLen().CompareTo(b.getCountLen());
            });
        }

        /// <summary>
        /// 添加一项到小X纪念
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="date">日期</param>
        /// <param name="circle">周期</param>
        public void addMenorialDay(string name, DateTime date, string circle)
        {
            if(memorialDayList == null)
            {
                memorialDayList = new List<MemoryDay>();
            }
            memorialDayList.Add(new MemoryDay(name, date, circle,2));
            memorialDayList.Sort(delegate(MemoryDay a, MemoryDay b) {
                return a.getMemoryLen().CompareTo(b.getMemoryLen());
            });
        }

        /// <summary>
        /// 删除指定事项，并删除对应磁贴
        /// </summary>
        /// <param name="pageIndex">引发操作的页面:1-小X倒数，2-小X纪念</param>
        /// <param name="itemIndex">事项在列表中的下标</param>
        public void delete(int pageIndex, int itemIndex)
        {
            ShellTile TileToFind;
            if (pageIndex == 1 && itemIndex < countDayList.Count)
            {
                foreach(MemoryDay item in memorialDayList)
                {
                    if(item.equals(countDayList[itemIndex]))
                    {
                        //如果该对象有磁贴，一并删除
                        TileToFind = ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().Contains(item.getName()));
                        if (TileToFind != null)
                        {
                            TileToFind.Delete();
                        }
                        memorialDayList.Remove(item);
                        break;
                    }
                }
                TileToFind = ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().Contains(countDayList[itemIndex].getName()));
                if (TileToFind != null)
                {
                    TileToFind.Delete();
                }
                countDayList.RemoveAt(itemIndex);
            }
            else if (pageIndex == 2 && itemIndex < memorialDayList.Count)
            {
                foreach(MemoryDay item in countDayList)
                {
                    if(item.equals(memorialDayList[itemIndex]))
                    {
                        //如果该对象有磁贴，一并删除
                        TileToFind = ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().Contains(item.getName()));
                        if (TileToFind != null)
                        {
                            TileToFind.Delete();
                        }
                        countDayList.Remove(item);
                        break;
                    }
                }
                TileToFind = ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().Contains(memorialDayList[itemIndex].getName()));
                if (TileToFind != null)
                {
                    TileToFind.Delete();
                }
                memorialDayList.RemoveAt(itemIndex);
            }
        }

        //定义属性改变事件
        public event PropertyChangedEventHandler PropertyChanged;

        //实现属性改变事件
        public void NotifyPropertyChanged(string propertyName)
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}