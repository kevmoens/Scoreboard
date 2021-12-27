using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;

namespace Scoreboard.UWP
{
    static public class HelmetHelper
    {
        public static ImageSource HelmetUri(string teamNum)
        {
            return new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri($"ms-appx:///Helmets/{teamNum}.png"));
        }
    }
}
