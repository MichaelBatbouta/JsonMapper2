using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadJson();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int i = 0;
            string text = System.IO.File.ReadAllText("test.json");
            JsonTextReader reader = new JsonTextReader(new StringReader(text));
            JsonColumn test = new JsonColumn { Property = "", Value = "" };

            JsonTreeWrapper masterTree = new JsonTreeWrapper();
            String jProp = "", jVal = "";
            List<String> jValList = new List<String>();
            bool arrayFlag = false;

            //Parse JSON
            while (reader.Read())
            {
                //If a property type save value 
                if (reader.TokenType.ToString() == "PropertyName")
                {
                    //test.Property = reader.Value.ToString();
                    jProp = reader.Value.ToString();
                }
                //if a string value, set it and add to master tree
                else if (reader.TokenType.ToString() == "String" || reader.TokenType.ToString() == "Integer")
                {
                    if (!arrayFlag)
                    {
                        //test.Value = reader.Value.ToString();
                        jVal = reader.Value.ToString();
                        masterTree.addObject(new JsonTreeObject(jProp, jVal));
                        jProp = "";
                        jVal = "";
                    }
                    else
                    {
                        jValList.Add(reader.Value.ToString());
                    }
                }
                else if (reader.TokenType.ToString() == "StartArray")
                {
                    arrayFlag = true;
                }
                else if (reader.TokenType.ToString() == "EndArray")
                {
                    masterTree.addObject(new JsonTreeObject(jProp, jValList));
                    arrayFlag = false;
                    jValList.Clear();
                }


            }

            masterTree.printTree();


        }

        public int get_height(string jsonFILE)
        {
            using (StreamReader r = new StreamReader(jsonFILE))
            {
                string json = r.ReadToEnd();
                JObject items = (JObject)JsonConvert.DeserializeObject(json);
                return items.Count;
            }
        }


        public void LoadJson()
        {
            using (StreamReader r = new StreamReader("test.json"))
            {
                string json = r.ReadToEnd();
                JObject items = (JObject)JsonConvert.DeserializeObject(json);

            }
        }

        public class JsonTreeWrapper
        {
            //root list of tree
            public List<JsonTreeObject> data = new List<JsonTreeObject>();


            public void addObject(JsonTreeObject newObject)
            {
                data.Add(newObject);
            }


            public void printTree()
            {
                foreach (JsonTreeObject obj in data)
                {
                    obj.printTreeObject();
                }
            }



        }

        public class JsonTreeObject
        {
            public String property;
            public List<String> values = new List<string>();
            public bool isArray;

            public JsonTreeObject(String p, String v)
            {
                property = p;
                values.Add(v);
                isArray = false;
            }

            public JsonTreeObject(String p, List<String> v)
            {
                property = p;
                values = new List<String>(v);
                isArray = true;
            }

            public void printTreeObject()
            {
                if (!isArray)
                {
                    Console.WriteLine("Token {0}, Value {1}", property, values[0]);
                }
                else
                {
                    Console.WriteLine("Token {0}", property);
                    foreach (String obj in values)
                    {
                        Console.WriteLine("Array Value: {0}", obj);
                    }
                }

            }

        }

        public class MyItem
        {
            public int Id { get; set; }

            public string Name { get; set; }
        }

        public class JsonColumn
        {
            public string Property { get; set; }

            public string Value { get; set; }
        }


        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
          
        }
    }
}
