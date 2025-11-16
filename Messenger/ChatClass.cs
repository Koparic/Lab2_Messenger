using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Messenger
{
    internal class ChatClass
    {
        //Усложнение структуры оставим на будущее
        //private ChatStruct chat;
        //private struct ChatStruct
        //{
        //    public string Name;
        //    public DateTime CreationDate;
        //    public List<string> Members;
        //    public List<string> MessegeHistoryJSON;
        //}
        //public ChatClass()
        //{
        //}

        private List<MessegeClass> _messeges = new List<MessegeClass>();
        public List<MessegeClass> Messeges {  get { return _messeges; } }

        public void AddMessege(string messege)
        {
            MessegeClass newM = new MessegeClass(messege);
            if (newM != null && newM.Messege.text != null)
            {
                _messeges.Add(newM);
            }
        }

        public void SaveChat()
        {
            string filePath = "ChatHistory.json";
            using (StreamWriter writer = File.CreateText(filePath))
            {
                string json = JsonConvert.SerializeObject(_messeges, Newtonsoft.Json.Formatting.Indented);
                writer.Write(json);
            }
        }

        public void LoadChat()
        {
            string filePath = "ChatHistory.json";
            if (File.Exists(filePath))
            {
                using (StreamReader reader = File.OpenText(filePath))
                {
                    string json = reader.ReadToEnd();
                    _messeges.Clear();
                    try
                    {
                        _messeges = JsonConvert.DeserializeObject<List<MessegeClass>>(json);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Чат не смог загрузиться" + ex.Message);
                    }
                    foreach (var item in _messeges)
                    {
                        Console.WriteLine(item.ToString());
                    }
                }
            }
        }

        public ChatClass()
        {
        }
    }
}
