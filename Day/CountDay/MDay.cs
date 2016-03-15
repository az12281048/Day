using System;

namespace Day
{
    /// <summary>
    /// 
    /// </summary>
    public class MDay
    {
        public string mdayName { get; set; }//名称
        public int mdayLenth { get; set; }//时间差

        public bool equals(MDay md)
        {
            return mdayName == md.mdayName && mdayLenth == md.mdayLenth;
        }
    }
}