using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Imdork.Mysql;

public class Test : MonoBehaviour
{

    void Start()
    {
        //创建数据库类                 IP地址       端口    用户名   密码     数据库项目名称
        var mySqlTools = new SqlHelper("127.0.0.1", "3306", "root", "666666", "user");
        //打开数据库
        mySqlTools.Open();

        //创建表方法              表名       字段名称                              字段类型
        //mySqlTools.CreateTable("userdata", new[] { "UID", "User","Password" }, new[] { "tinytext", "tinytext", "tinytext" });

        //查询方法
        FindMysql(mySqlTools, "userdata", new[] { "UID", "User", "Password" });

        //  插入方法                表名         字段名                             插入数据
        //mySqlTools.InsertInto("userdata", new[] { "UID", "User", "Password" },new[] {"52022","ddxj1","123456" });

        //  更新方法                      表名         更新字段名    判断符号         更新数据          查询条件字段        条件成立字段
        //mySqlTools.UpdateIntoSpecific("userdata", new[] { "User" }, new[] { "=" }, new[] { "ddxj1" }, new[] { "Password" }, new[] { "456789" });

        //  删除方法         表名          删除字段        判断条件     条件成立数据         
        //mySqlTools.Delete("userdata", new[] { "User" }, new[] { "=" }, new[] { "ddxj1" });

        // 从SqlHelper查询出来数据库 都会返回Dataset  DataSet类               字段名
        //       返回对象object     获取数据方法    
        // var GetValues = MysqlTools.GetValue(mySqlTools.Select("userdata"), "User");
        //print(GetValues);

        //查询方法                         表名        查询字段名         判断字段名       判断符号        条件成立数据
        // var ds = mySqlTools.SelectWhere("userdata", new[] { "UID" }, new[] { "User" }, new[] { "=" }, new[] { "ddxj1" });
        //查询方法                         表名         判断字段名       判断符号        条件成立数据
        // var ds = mySqlTools.SelectWhere("userdata", new[] { "User" }, new[] { "=" }, new[] { "ddxj1" });

        //SelectWhere方法会返回Dataset类对象， 声明ds变量接收如上图
        //MysqlTools 工具类使用GetValue方法负责接收DataSet对象 给字段名称返回对应数据
        //调用MysqlTools 工具类                 Dataset类对象  查询字段
        //object values = MysqlTools.GetValue(ds, "UID");
        //print(values); //最后打印15924

       // mySqlTools.DeleteContents("userdata"); 删除表中全部数据


        //关闭数据库
        mySqlTools.Close();

        
    }

    /// <summary>
    /// 查询表中数据   记得先调用Open()方法  用完此方法后直接Close()
    /// </summary>
    /// <param name="mySqlTools">Mysql框架类</param>
    /// <param name="tableName">表名</param>
    /// <param name="items">字段名称</param>
    void FindMysql(SqlHelper mySqlTools,string tableName,string[] items)
    {
        var ds = mySqlTools.Select(tableName, items);
        var pairs = MysqlTools.TableData(ds);
        DebugMysql(pairs);      
    }
    /// <summary>
    /// 打印查询数据库
    /// </summary>
    /// <param name="pairs"></param>
    private void DebugMysql(Dictionary<string,object>[] pairs)
    {
        for (int i = 0; i < pairs.Length; i++)
        {
            foreach (var table in pairs[i])
            {
                string tableList = string.Format("第{0}行，表字段名对应数据是 {1}", i + 1, table);
                print(tableList);
            }
        }  
    }
       
}
 