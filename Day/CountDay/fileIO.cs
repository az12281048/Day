using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Runtime.Serialization;
using System.Diagnostics;

namespace Day
{
    public class DataIO
    {
        //获取应用独立存储文件
        public static IsolatedStorageFile iso = IsolatedStorageFile.GetUserStoreForApplication();

        /// <summary>
        /// 创建一个独立存储文件
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns>独立存储文件流</returns>
        public static IsolatedStorageFileStream CreateFile(string path)
        {
            try
            {
                //如果文件存在需要将文件删除
                if(iso.FileExists(path))
                {
                    iso.DeleteFile(path);
                }
                return iso.CreateFile(path);
            }
            catch(Exception e)
            {
                Debug.WriteLine("DataIO: 创建文件错误 "+e+path);
                return null;
            }

        }

        /// <summary>
        /// 将可序列化对象写入到独立存储文件
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="type">可序列化对象的类型</param>
        /// <param name="obj">可序列化对象</param>
        /// <returns></returns>
        public static bool WriteToFile(string path, Type type, object obj)
        {
            try
            {
                FileStream stream = CreateFile(path);
                new DataContractSerializer(type).WriteObject(stream, obj);
                stream.Close();
                Debug.WriteLine("DataIO: 写入到文件成功"+path);
                return true;
            }
            catch(Exception e)
            {
                Debug.WriteLine("DataIO: 写入文件错误 "+e+path);
                return false;
            }
        }

        /// <summary>
        /// 打开一个文件
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="mode">打开文件模型</param>
        /// <returns>独立存储文件流</returns>
        public static IsolatedStorageFileStream OpenFile(string path, FileMode mode)
        {
            if(iso.FileExists(path))
            {
                return iso.OpenFile(path, mode);
            }
            else
            {
                Debug.WriteLine("DataIO: 文件不存在"+path);
                return null;
            }
        }

        /// <summary>
        /// 从独立存储文件中读取对象
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="type">可序列化对象类型</param>
        /// <returns>序列化对象</returns>
        public static object ReadFromFile(string path, Type type)
        {
            try
            {
                FileStream stream = OpenFile(path, FileMode.Open);
                object obj2 = new DataContractSerializer(type).ReadObject(stream);
                stream.Close();
                Debug.WriteLine("DataIO: 读取文件成功" + path);
                return obj2;
            }
            catch(Exception e)
            {
                Debug.WriteLine("DataIO: 读取文件错误 "+e+path);
                return null;
            }
        }
    }
}