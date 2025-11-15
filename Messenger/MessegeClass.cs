using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Messenger
{
    internal class MessegeClass
    {
        private messegeStruct messege;
        public struct messegeStruct{
            public string sender;
            public string recipient;
            public DateTime sendingTime;
            public string text;
        }

        public MessegeClass(string username, string text, string recipient = "All")
        {
            messege.sender = username;
            messege.recipient = recipient;
            messege.text = text;
            messege.sendingTime = DateTime.Now;
        }

        public string ConvertMessege()
        {
            return JsonConvert.SerializeObject(messege);
        }


    }
}
