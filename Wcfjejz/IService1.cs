using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Data;
using System.Data.OleDb;

namespace Wcfjejz
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的接口名“IService1”。
    [ServiceContract]
    public interface IService1
    {
        [OperationContract]
        void WriteError(string message);

        [OperationContract]
        DataSet Exchange(DateTime date1, DateTime date2);

        [OperationContract]
        Boolean CheckInformation(string Code, string Password);

        [OperationContract]
        String GetPower(string Code);
        // TODO: 在此添加您的服务操作
    }


    
}
