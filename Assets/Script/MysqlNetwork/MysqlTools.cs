using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
/*
  DataSet类使用方法
  asp.NET中DataSet对象获取相应列值、行列数、列名、取出特定值这些操作的总结，具体代码如下：

  1.  DataSet.Table[0].Rows[ i ][ j ]
      其中i　代表第 i 行数, j 代表第 j 列数

  2.　DataSet.Table[0].Rows[ i ].ItemArray[ j ]
      其中i　代表第 i 行数, j 代表第 j 列数

  3.　DataSet.Tables[0].Columns.Count
      取得表的总列数

  4.　DataSet.Tables[0].Rows.Count
      取得表的总行数

  5.　DataSet.Tables[0].Columns[ i ].ToString()
      取得表的 i 列名(字段名)

  6.  ds.Tables.Count - 返回表的数量，一个 select 返回一个 table
*/

namespace Imdork.Mysql
{
    /// <summary>
    /// mysql工具类
    /// </summary>
    public class MysqlTools
    {

        /// <summary>
        /// 获取表中数据 字典中第一个键是字段名  第二个是 对应字段值 返回字典数组
        /// </summary>
        /// <param name="ds">数据表类</param>
        /// <returns></returns>
        public static Dictionary<string, object>[] TableData(DataSet ds)
        {
            //判断是否没有数据
            if (ds.Tables.Count == 0)
            {
                //返回空
                return null;
            }
            //声明集合
            List<Dictionary<string, object>> tableList = new List<Dictionary<string, object>>();
            //遍历DataSet表条数
            for (int i = 0; i < ds.Tables.Count; i++)
            {
                //遍历表该条字段数量
                for (int j = 0; j < ds.Tables[i].Rows.Count; j++)
                {
                    //根据该条数 并新建一块字典空间存储到该字典中
                    tableList.Add(new Dictionary<string, object>());
                    //存储该条数类对象
                    var temp = ds.Tables[i];
                    //获取该条所有字段值
                    var obj = temp.Rows[j].ItemArray;
                    //遍历所有字段数组
                    for (int k = 0; k < obj.Length; k++)
                    {
                        //获取该字段名称
                        string tableName = temp.Columns[k].ToString();
                        //根据字段名称 和对应字段值存储到字段中
                        tableList[j].Add(tableName, obj[k]);
                    }
                }
            }
            //返回字典数组
            return tableList.ToArray();
        }
        /// <summary>
        /// 获取单个数据
        /// </summary>
        /// <param name="ds">数据表类</param>
        /// <param name="Name">字段名称</param>
        /// <returns></returns>
        public static object GetValue(DataSet ds, string Name)
        {
            //判断是否没有数据
            if (ds.Tables.Count == 0)
            {
                //返回空
                return null;
            }

            //遍历DataSet表条数
            for (int i = 0; i < ds.Tables.Count; i++)
            {
                //遍历表该条字段数量
                for (int j = 0; j < ds.Tables[i].Rows.Count; j++)
                {
                    //存储该条数类对象
                    var temp = ds.Tables[i];
                    //获取该条所有字段值
                    var obj = temp.Rows[j].ItemArray;
                    //遍历所有字段数组
                    for (int k = 0; k < obj.Length; k++)
                    {
                        //获取该字段名称
                        string tableName = temp.Columns[k].ToString();
                        //判断该字段名称是否等于Name
                        if (tableName == Name)
                        {
                            //返回数据
                            return obj[k];
                        }
                    }
                }
            }
            //返回null
            return null;
        }
        /// <summary>
        /// 获取多个数据
        /// </summary>
        /// <returns></returns>
        public static object[] GetValues(DataSet ds, string Name)
        {
            //判断是否没有数据
            if (ds.Tables.Count == 0)
            {
                //返回空
                return null;
            }
            //声明字段集合
            List<object> list = new List<object>();
            //遍历DataSet表条数
            for (int i = 0; i < ds.Tables.Count; i++)
            {
                //遍历表该条字段数量
                for (int j = 0; j < ds.Tables[i].Rows.Count; j++)
                {
                    //存储该条数类对象
                    var temp = ds.Tables[i];
                    //获取该条所有字段值
                    var obj = temp.Rows[j].ItemArray;
                    //遍历所有字段数组
                    for (int k = 0; k < obj.Length; k++)
                    {
                        //获取该字段名称
                        string tableName = temp.Columns[k].ToString();
                        //判断该字段名称是否等于Name
                        if (tableName == Name)
                        {
                            //存储到集合中
                            list.Add(obj[k]);
                        }
                    }
                }
            }
            //返回数组
            return list.ToArray();
        }
    }
     
}

 