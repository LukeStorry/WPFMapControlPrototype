using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Maps.MapControl.WPF;

namespace WPFMapControlPrototype
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            myMap.CredentialsProvider =
                new ApplicationIdCredentialsProvider(Environment.GetEnvironmentVariable("BING_MAPS_KEY"));
            myMap.Center = new Location(51.444, -2.604);
            myMap.ZoomLevel = 14;
            // myMap.MouseLeftButtonDown += (sender, e) =>
            // {
            //     myMap.ScreenToGeoPoint(e.GetPosition(this.mapControl1))
            //     Location l = new Location();
            //     myMap.mouTryPixelToLocation(e.GetCurrentPoint(myMap).Position, out l);
            //     Console.WriteLine(sender.GetType());
            // };


            var mapItems = new[]
            {
                new MapItem
                {
                    Location = new Location(51.45, -2.59),
                    Title = "Location 1",
                    Details = "First Location Details   \n     would \n      go \n       here.         ",
                    Uri = new Uri("https://example.com"),
                    Color = Colors.Green
                },
                new MapItem
                {
                    Location = new Location(51.44, -2.595),
                    Title = "Second Location",
                    Details = "Details   \n     would \n      go \n       here.         ",
                    Uri = new Uri("https://google.com"),
                    Color = Colors.Blue
                }
            };

            foreach (var mapItem in mapItems)
            {
                var poly = new MapPolygon
                {
                    Stroke = new SolidColorBrush(mapItem.Color),
                    StrokeThickness = 20,
                    Locations = new LocationCollection {mapItem.Location, mapItem.Location},
                    Tag = mapItem,
                    ToolTip = mapItem.Title
                };
                poly.MouseLeftButtonDown += DetailsPopup;
                poly.MouseRightButtonDown += SetIframeSrc;
                myMap.Children.Add(poly);
            }
        }

        private class MapItem
        {
            public Location Location;
            public string Title;
            public string Details;
            public Uri Uri;
            public Color Color;
        }

        private void SetIframeSrc(object sender, RoutedEventArgs e)
        {
            if (sender is MapPolygon poly)
            {
                if (poly.Tag != null && poly.Tag is MapItem item)
                {
                    webBrowser.Source = item.Uri;
                }
            }
        }

        private static void DetailsPopup(object sender, RoutedEventArgs e)
        {
            if (sender is MapPolygon poly)
            {
                if (poly.Tag != null && poly.Tag is MapItem item)
                {
                    MessageBox.Show(item.Details, $"{item.Title} Status");
                }
            }
        }
    }
}
