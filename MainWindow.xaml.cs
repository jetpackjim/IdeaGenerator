using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.IO;
using System.Xml.Serialization;
using Microsoft.Win32;


namespace IdeaGenerator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string currentFileName = "Ideas new.xml";

        public class IdeaList
        {
            public string listName { get; set; }
            public ObservableCollection<string> stringList = new ObservableCollection<string>();

            public IdeaList() { }

            public IdeaList(string name)
            {
                listName = name;
            }
        }

        public class MyData
        {
            public List<IdeaList> stringLists = new List<IdeaList>();
        }

        MyData mydata = new MyData();

       public class Idea
        {
            public string result { get; set; }
        }

        public MainWindow()
        {
            mydata.stringLists.Add(new IdeaList("List 1"));
            mydata.stringLists.Add(new IdeaList("List 2"));
            mydata.stringLists.Add(new IdeaList("List 3"));

            InitializeComponent();

           
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Bind default new data   
            ListNameTB01.DataContext = mydata.stringLists[0];
            ListBox01.ItemsSource = mydata.stringLists[0].stringList;
            ListNameTB02.DataContext = mydata.stringLists[1];
            ListBox02.ItemsSource = mydata.stringLists[1].stringList;
            ListNameTB03.DataContext = mydata.stringLists[2];
            ListBox03.ItemsSource = mydata.stringLists[2].stringList;

            Idea idea = new Idea();
            {
                idea.result = "None";
            }
            IdeaTextBox.DataContext = idea;
        }
        
        private void OpenFileSelected(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                //string filename = File.ReadAllText(openFileDialog.FileName);
                currentFileName = openFileDialog.FileName;

                mydata = DeserializeXMLToObject(mydata, openFileDialog.FileName);
                // Bind data   
                ListNameTB01.DataContext = mydata.stringLists[0];
                ListBox01.ItemsSource = mydata.stringLists[0].stringList;
                ListNameTB02.DataContext = mydata.stringLists[1];
                ListBox02.ItemsSource = mydata.stringLists[1].stringList;
                ListNameTB03.DataContext = mydata.stringLists[2];
                ListBox03.ItemsSource = mydata.stringLists[2].stringList;
            }
        }

        private void SaveFileSelected(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == true)
            {
                //string filename = File.ReadAllText(openFileDialog.FileName);
                currentFileName = saveFileDialog.FileName;

                SerializeObjectToXML<MyData>(mydata, currentFileName);
            }
        }

        private void NewFileSelected(object sender, RoutedEventArgs e)
        {
            mydata = new MyData();
            mydata.stringLists.Add(new IdeaList("List 1"));
            mydata.stringLists.Add(new IdeaList("List 2"));
            mydata.stringLists.Add(new IdeaList("List 3"));

            ListNameTB01.DataContext = mydata.stringLists[0];
            ListBox01.ItemsSource = mydata.stringLists[0].stringList;
            ListNameTB02.DataContext = mydata.stringLists[1];
            ListBox02.ItemsSource = mydata.stringLists[1].stringList;
            ListNameTB03.DataContext = mydata.stringLists[2];
            ListBox03.ItemsSource = mydata.stringLists[2].stringList;
        }

        private void OnResultButtenPressed(object sender, RoutedEventArgs e)
        {
            Idea idea = new Idea();
            {
                Random random = new Random();
                string character = "";
                if (mydata.stringLists[0].stringList.Count() > 0)
                {
                    character = mydata.stringLists[0].stringList[random.Next(mydata.stringLists[0].stringList.Count())];
                }
                string mechanic = "";
                if (mydata.stringLists[1].stringList.Count() > 0)
                {
                    mechanic = mydata.stringLists[1].stringList[random.Next(mydata.stringLists[1].stringList.Count())];
                }
                string theme = "";
                if (mydata.stringLists[2].stringList.Count() > 0)
                {
                    theme = mydata.stringLists[2].stringList[random.Next(mydata.stringLists[2].stringList.Count())];
                }
                
                idea.result = character + " " + mechanic + " " + theme;
            }
            IdeaTextBox.DataContext = idea;
        }

        public static void SerializeObjectToXML<T>(T item, string FilePath)
        {
            XmlSerializer xs = new XmlSerializer(typeof(T));
            using (StreamWriter wr = new StreamWriter(FilePath))
            {
                xs.Serialize(wr, item);
            }
        }

        public static T DeserializeXMLToObject<T>(T item, string filepath)
        {
            if (File.Exists(filepath))
            {
                // Create an instance of the XmlSerializer.
                XmlSerializer xs = new XmlSerializer(typeof(T));

                using (Stream reader = new FileStream(filepath, FileMode.Open))
                {
                    // Call the Deserialize method to restore the object's state.
                    item = (T)xs.Deserialize(reader);
                }
            }

            return item;
        }

        
        private void OnMouseDownTB01(object sender, MouseButtonEventArgs e)
        {
            textbox1.Text = "";
        }


        private void ListName01OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                mydata.stringLists[0].listName = ListNameTB01.Text;
            }
        }

        private void OnKeyDownHandler01(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                if (mydata.stringLists[0].stringList.Contains(textbox1.Text) == false)
                {
                    mydata.stringLists[0].stringList.Add(textbox1.Text);
                }
                textbox1.Text = "";
            }
        }

        private void OnMouseDownTB02(object sender, MouseButtonEventArgs e)
        {
            textbox2.Text = "";
        }

        private void ListName02OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                mydata.stringLists[1].listName = ListNameTB02.Text;
            }
        }


        private void OnKeyDownHandler02(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                if (mydata.stringLists[1].stringList.Contains(textbox2.Text) == false)
                {
                    mydata.stringLists[1].stringList.Add(textbox2.Text);
                }
                textbox2.Text = "";
            }
        }

        private void OnMouseDownTB03(object sender, MouseButtonEventArgs e)
        {
            textbox3.Text = "";
        }

        private void ListName03OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                mydata.stringLists[2].listName = ListNameTB03.Text;
            }
        }

        private void OnKeyDownHandler03(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                if (mydata.stringLists[2].stringList.Contains(textbox3.Text) == false)
                {
                    mydata.stringLists[2].stringList.Add(textbox3.Text);
                }
                textbox3.Text = "";
            }
        }

    }
}
