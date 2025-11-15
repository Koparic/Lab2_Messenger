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
        private Form1 form;
        private string currentUserName;
        private messegeStruct messege;
        public struct messegeStruct{
            //public string ChatName;
            public string sender;
            public string recipient;
            public DateTime sendingTime;
            public string text;
        }

        public MessegeClass(string text, string recipient = "All")
        {
            messege.sender = currentUserName;
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
