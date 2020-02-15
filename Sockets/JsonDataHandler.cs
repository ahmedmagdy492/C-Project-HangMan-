using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Sockets
{
    public static class JsonDataHandler<Type>
    {
        public static Type DesObject<Type>(byte[] data)
        {
            string strData = Encoding.Default.GetString(data);
            Type obj = JsonConvert.DeserializeObject<Type>(strData);
            return obj;
        }

        public static string SerObj<Type>(Type obj)
        {
            string data = JsonConvert.SerializeObject(obj);
            return data;
        }
    }
}
