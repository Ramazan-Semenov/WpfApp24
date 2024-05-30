using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using System.Xml.Linq;

namespace WpfApp24
{

    class SetWidthConvert : IMultiValueConverter, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnChanged(PropertyChangedEventArgs args) => this.PropertyChanged?.Invoke(this, args);

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {

            var w1 = (double)values[0];

            if (double.TryParse(values[1]?.ToString(), out var w))
            {
                return w1 / w;
            }
            var w2 = (double)values[0];
           return w1 / 1;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        //public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        //{
        //    throw new NotImplementedException();
        //}

        //public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        //{
        //    throw new NotImplementedException();
        //}
    }
    public class DataNewColumn : DataGridTemplateColumn
    {
        public static readonly DependencyProperty ItemSourceProperty = DependencyProperty.Register("ItemSource", typeof(object), typeof(DataNewColumn));
        public object ItemSource
        {
            get
            {
                return (object)GetValue(ItemSourceProperty);

            }
            set
            {
                SetValue(ItemSourceProperty, value);
            }
        }

        protected override FrameworkElement GenerateElement(DataGridCell cell, object dataItem)
        {
            cell.BorderThickness = new Thickness(0);
            var d = cell.Column as DataGridTemplateColumn;
            var p = d.CellTemplate;
            if (ItemSource != null)
            {
                var ds = p.LoadContent() as ItemsControl;
                ds.SetValue(ItemsControl.ItemsSourceProperty, (ItemSource as List<DDL>)[0].TestType);
                return ds;
            }
            return null;

        }
        public DataTemplate MakeDataTemplate(string myName)
        {
            //A XAML is dynamically generated in memory, describing a DataTemplate
            XNamespace ns = "http://schemas.microsoft.com/winfx/2006/xaml/presentation";

            var attribute1 = new XAttribute("HorizontalAlignment", "Center");
            var attribute2 = new XAttribute("VerticalAlignment", "Center");

            XElement uGrid = new XElement(ns + "UniformGrid", new XAttribute("Columns", "3"), new XAttribute("Background", "Transparent"),
                new XElement(ns + "Border", new XAttribute("BorderBrush", "Black"), new XAttribute("BorderThickness", "0 0 1 0"),
                          new XElement(ns + "TextBlock", new XAttribute("Text", "{Binding Path=Number of records" + myName + "}"), attribute1, attribute2)),

                new XElement(ns + "Border", new XAttribute("BorderBrush", "Black"), new XAttribute("BorderThickness", "0 0 1 0"),
                    new XElement(ns + "TextBlock", new XAttribute("Text", "{Binding Path=row count" + myName + "}"), attribute1, attribute2)),

                new XElement(ns + "TextBlock", new XAttribute("Text", "{Binding Path=Data Volume" + myName + "}"), attribute1, attribute2)
                );
            XElement xDataTemplate = new XElement(ns + "DataTemplate", new XAttribute("xmlns", "http://schemas.microsoft.com/winfx/2006/xaml/presentation"));
            xDataTemplate.Add(uGrid);
            XmlReader xr = xDataTemplate.CreateReader();
            DataTemplate dataTemplate = System.Windows.Markup.XamlReader.Load(xr) as DataTemplate;
            return dataTemplate;

            #region  Refer to DataTemplate
            //            <DataTemplate x:Key="cbTemplate" >
            //    <UniformGrid Columns="3"  Background="Transparent"  >
            //        <Border BorderBrush="Black" BorderThickness="0 0 1 0">
            //        <TextBlock Name="tb1" Text="{Binding Path=Shop[1]}" HorizontalAlignment="Center"  VerticalAlignment="Center"/>
            //        </Border>
            //    <Border BorderBrush="Black" BorderThickness="0 0 1 0">
            // <TextBlock Name="tb2" Text="{Binding row count A}" HorizontalAlignment="Center" VerticalAlignment="Center"/>
            //    </Border>
            // <TextBlock Name="tb3" Text="{Binding data volume A}" HorizontalAlignment="Center" VerticalAlignment="Center"/>
            //    </UniformGrid>
            //</DataTemplate>
            #endregion

        }

    }
    public class DataList
    {
        public string SchoolName { get; set; }
        public List<DDL> List { get; set; }
    }
    public class DDL
    {
        public string RequestName { get; set; }
        public List<int> RequestNumList { get; set; }
        public List<string> TestType { get; set; }
    }

    /// <summary>
    /// mainwindow.xaml interaction logic
    /// </summary>
    public partial class MainWindow : Window
    {
        List<DataList> GetDataLists = new List<DataList>();
        public MainWindow()
        {
            InitializeComponent();

            Init();
            Create();
        }
        private void Create()
        {
            this.DG.ItemsSource = GetDataLists;
            for (var i = 0; i < GetDataLists.Count; i++)
                Add(i);
        }

        private void Add(int i)
        {
            var clm = this.DG.Columns;
            var b = new DataNewColumn();
            b.ItemSource = GetDataLists[i].List;
            b.CellTemplate = this.FindResource("CDT") as DataTemplate;
            b.HeaderStyle = this.FindResource("s1") as Style;
            clm.Add(b);
        }

        private void Init()
        {
            Random random = new Random();
            for (var O = 0; O < 5; O++)
            {
                List<string> testtpyelist = new List<string>();
                List<int> requestnumlist = new List<int>();
                String RequestName = "Requirements" + O.ToString();
                var d = random.Next(1, 10);
                for (var p = 0; p < d; p++)
                {
                    requestnumlist.Add(p);
                    testtpyelist.Add((p % 2 == 0) ? "" : "Low");
                }
                var k = new DDL();
                k.TestType = testtpyelist;
                k.RequestNumList = requestnumlist;
                k.RequestName = RequestName;

                var l ="University NO." + O.ToString();
                var t = new List<DDL>();
                t.Add(k);
                GetDataLists.Add(new DataList() { SchoolName = l, List = t });
            }
        }
    }

}
