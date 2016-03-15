using System;

namespace Day
{
    public class MemoryDay
    {
        public string name{get; set;}//名称
        public DateTime date{get; set;}//日期
        public string circle{get; set;}//周期

        public MemoryDay()
        {
            this.name = "";
            this.date = DateTime.Now;
            this.circle = "";
        }

        public MemoryDay(string sName, DateTime dt, string cir, int mo)
        {
            this.name = sName;
            this.date = dt;
            this.circle = cir;
        }

        public string getName()
        {
            return this.name;
        }

        public DateTime getDate()
        {
            return this.date;
        }

        public string getCircle()
        { 
            return this.circle;
        }

        public bool equals(MemoryDay a)
        {
            if (a.name != name || a.date != date || a.circle != circle)
                return false;
            return true;
        }

        public bool equals(string sName, DateTime sDate, string sCircle)
        {
            if (sName != name || sDate != date || sCircle != circle)
                return false;
            return true;
        }

        //根据周期获取倒数日差
        public int getCountLen()
        {
            DateTime dtNow = DateTime.Now;
            if(circle == "无")
            {
                return date.Subtract(dtNow).Days+1;
            }
            else if(circle == "每周")
            {
                int len = date.DayOfWeek - dtNow.DayOfWeek;
                return (len >= 0 ? len : len + 7);
            }
            else if(circle == "每月")
            {
                int monthLen;
                if (date.Day >= dtNow.Day)
                    return date.Day - dtNow.Day;
                int m = dtNow.Month;
                if (m == 2)
                {
                    monthLen = dtNow.Year % 4 == 0 ? 29 : 28;
                }
                else if (m == 4 || m == 6 || m == 9 || m == 11)
                {
                    monthLen = 30;
                }
                else
                    monthLen = 31;
                return (date.Day + monthLen - dtNow.Day);
            }
            else
            {
                DateTime tmp = new DateTime(dtNow.Year,date.Month,date.Day);
                int len = tmp.Subtract(dtNow).Days ;
                if(len < 0)
                {
                    tmp = new DateTime(dtNow.Year+1, date.Month, date.Day);
                    len = tmp.Subtract(dtNow).Days;
                }
                return len;
            }
        }

        //获取纪念日差
        public int getMemoryLen()
        {
            return DateTime.Now.Subtract(date).Days;
        }

        //返回本实例倒数Mday对象
        public MDay getCountMDay()
        {
            return new MDay() {mdayName = this.name, mdayLenth =  getCountLen()};
        }

        //返回本实例纪念MDay对象
        public MDay getMemoryMDay()
        {
            return new MDay() { mdayName = this.name, mdayLenth = getMemoryLen()};
        }
    }
}