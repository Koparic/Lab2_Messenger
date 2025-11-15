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

        private List<MessegeClass.messegeStruct> messeges;

        public void AddMessege(string messege)
        {
            try
            {
                messeges.Add(JsonConvert.DeserializeObject<MessegeClass.messegeStruct>(messege));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Исключение при конвертации или добавлеии: " + ex.Message);
            }
        }

        public void SaveChat()
        {
            string filePath = "ChatHistory.json";
            using (StreamWriter writer = File.CreateText(filePath))
            {
                string json = JsonConvert.SerializeObject(messeges, Newtonsoft.Json.Formatting.Indented);
                writer.Write(json);
            }
        }

        public List<MessegeClass.messegeStruct> LoadChat()
        {
            string filePath = "ChatHistory.json";
            if (File.Exists(filePath))
            {
                using (StreamReader reader = File.OpenText(filePath))
                {
                    string json = reader.ReadToEnd();
                    messeges.Clear();
                    foreach (MessegeClass.messegeStruct m in JsonConvert.DeserializeObject<List<MessegeClass.messegeStruct>>(json))
                        messeges.Add(m);
                }
            }
            return messeges;
        }
    }
}
