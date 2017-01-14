using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaBox
{
    class PlayItemData : System.ComponentModel.INotifyPropertyChanged
    {
        string m_path;
        public PlayItemData(String path)
        {
            m_path = path;
        }

        public string Name
        {
            get
            {
                return m_path.Substring(m_path.LastIndexOf("\\") +1);
            }
            
        }
        public string Uri
        {
            get
            {
                if(m_path.StartsWith("\\\\127.0.0.1\\"))
                {
                    return m_path;
                }
                string local = m_path.Remove(0, 3);
                string new_file = local.Insert(0, "\\\\127.0.0.1\\" + m_path.Substring(0, 1) + "$\\");
                return new_file;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
    class PlayItemDataList : INotifyPropertyChanged
    {
        List<PlayItemData> m_list = new List<PlayItemData>();
       
        public List<String> ListItems

        {
            get
            {
                List<String> listItems = new List<string>();
                foreach(var item in m_list)
                {
                    listItems.Add(item.Name);
                }
                return listItems;
                
            }
        }

        public List<PlayItemData> Items
        {
            get
            {
                return m_list;
            }            
        }

        public PlayItemData GetItem(string name)
        {
            return m_list.Find(x => x.Name == name);
        }

        public int Count
        {
            get
            {
                return m_list.Count;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void AddItem(PlayItemData item)
        {
            m_list.Add(item);
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs("ListItems"));
        }

        public void AddItems(ICollection<String> uris)
        {
            foreach(var uri in uris)
            {
                PlayItemData item = new PlayItemData(uri);
                m_list.Add(item);
            }

            if(uris.Count != 0 && PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs("ListItems"));
            }
        }

        public void Clear()
        {
            m_list.Clear();
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs("ListItems"));
        }

    }
}
