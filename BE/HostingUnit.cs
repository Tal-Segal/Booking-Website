using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BE
{
    public class HostingUnit
    {
        #region Properties
        public int HostingUnitKey { get; set; }
        public Host Owner { get; set; }
        public string HostingUnitName { get; set; }

        [XmlIgnore]
        public bool [,] Diary { get; set; }
        [XmlArray("Diary")]
        public bool[] DiaryTo
        {
            get { return Diary.Flatten(); }
            set { Diary = value.Expand(13); } //13 is the number of rows in the matrix
        }

        public int price { get;set; }
        public area Area { get; set; }
        public type Type { get; set; }
        public int Adults { get; set; }
        public int Children { get; set; }
        public bool Pool { get; set; }
        public bool Jacuzzi { get; set; }
        public bool Garden { get; set; }
        public bool Beach { get; set; }
        public bool ChildrenAttractions { get; set; }
        #endregion

        #region ToString Function
        public override string ToString()
        {
            return "Hosting Unit Key: " + HostingUnitKey.ToString() + "\n"
                + "Hosting Unit Name: " + HostingUnitName.ToString() + "\n"
                + "Price: " + price.ToString() + "\n"
                + "Area: " + Area.ToString() + "\n"
                + "Type: " + Type.ToString() + "\n"
                + "Adults: " + Adults.ToString() + "\n"
                + "Children: " + Children.ToString() + "\n"
                + "Pool: " + Pool.ToString() + "\n"
                + "Jacuzzi: " + Jacuzzi.ToString() + "\n"
                + "Garden: " + Garden.ToString() + "\n"
                + "Beach: " + Beach.ToString() + "\n"
                + "Attractions: " + ChildrenAttractions.ToString() + "\n";
        }
        #endregion
    }
    public static class Tools
    {
        public static T[] Flatten<T>(this T[,] arr)
        {
            int rows = arr.GetLength(0);
            int columns = arr.GetLength(1);
            T[] arrFlattened = new T[rows * columns];
            for (int j = 0; j < rows; j++)
            {
                for (int i = 0; i < columns; i++)
                {
                    var test = arr[j,i];
                    arrFlattened[j * rows + i] = arr[j, i];
                }
            }
            return arrFlattened;
        }
        public static T[,] Expand<T>(this T[] arr, int rows)
        {
            int length = arr.GetLength(0);
            int columns = length / rows;
            T[,] arrExpanded = new T[rows, columns];
            for (int j = 0; j < rows; j++)
            {
                for (int i = 0; i < columns; i++)
                {
                    arrExpanded[j, i] = arr[j * rows + i];
                }
            }
            return arrExpanded;
        }
    }
}
