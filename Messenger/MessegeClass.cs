using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Messenger
{
    public class MessegeClass
    {
        private messegeStruct _messege = new messegeStruct();
        public messegeStruct Messege {  get { return _messege; } set { _messege = value; } }
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
        public MessegeClass()
        {
        }
        public MessegeClass(string JSON)
        {
            try
            {
                _messege = (JsonConvert.DeserializeObject<MessegeClass.messegeStruct>(JSON));
            }
            catch (Exception ex)
            {
                _messege = new messegeStruct();
                Console.WriteLine("Полученная строка это не сообщение: " + ex.Message);
            }
        }

        public string ConvertMessege()
        {
            try
            {
                return JsonConvert.SerializeObject(_messege);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Сообщение не преобразовано в JSON: " + ex.Message);
            }
            return null;
        }

    }
}
