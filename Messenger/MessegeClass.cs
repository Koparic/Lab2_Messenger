using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace Messenger
{
    internal class MessegeClass
    {
        private messegeStruct _messege;
        public messegeStruct Messege {  get { return _messege; } }
        public struct messegeStruct{
            public string sender;
            public string recipient;
            public DateTime sendingTime;
            public string text;
        }
        public override string ToString()
        {
            return $"{_messege.sendingTime.ToShortDateString()} {_messege.sender}: {_messege.text}";
        }
        public MessegeClass(string un, string text, string recipient = "All")
        {
            _messege.sender = un;
            _messege.recipient = recipient;
            _messege.text = text;
            _messege.sendingTime = DateTime.Now;
        }

        public MessegeClass(string JSON)
        {
            try
            {
                _messege = (JsonConvert.DeserializeObject<MessegeClass.messegeStruct>(JSON));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Исключение при конвертации или добавлеии: " + ex.Message);
            }
        }

        public string ConvertMessege()
        {
            return JsonConvert.SerializeObject(_messege);
        }

    }
}
