using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace iSTMC.PageView
{
   [ValueConversion(typeof(int), typeof(string))]
   public class TimeoutToStringConverter : IValueConverter
   {
      public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
      {
         int val = int.Parse(value.ToString());

         TimeSpan ts = TimeSpan.FromSeconds(val);

         return string.Format("{0}:{1:D2}", (int)ts.TotalMinutes, ts.Seconds);
      }

      public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
      {
         throw new Exception("The method or operation is not implemented.");
      }
   }

   [ValueConversion(typeof(bool), typeof(Visibility))]
   public class BoolToVisibilityConverter : IValueConverter
   {
      public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
      {
         bool val = (bool)value;

         if (val)
            return Visibility.Visible;
         else
            return Visibility.Collapsed;
      }

      public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
      {
         throw new Exception("The method or operation is not implemented.");
      }
   }

   [ValueConversion(typeof(bool), typeof(Visibility))]
   public class BoolToHiddenConverter : IValueConverter
   {
      public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
      {
         bool val = (bool)value;

         if (val)
            return Visibility.Visible;
         else
            return Visibility.Hidden;
      }

      public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
      {
         throw new Exception("The method or operation is not implemented.");
      }
   }

   [ValueConversion(typeof(bool), typeof(string))]
   public class BooleanToStringValueConverter : IValueConverter
   {
      public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
      {
         if (value == null || parameter == null)
            return false;
         string checkvalue = value.ToString();
         string targetvalue = parameter.ToString();
         bool r = checkvalue.Equals(targetvalue,
             StringComparison.InvariantCultureIgnoreCase);
         return r;
      }

      public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
      {
         if (value == null || parameter == null)
            return null;
         bool usevalue = (bool)value;

         if (usevalue)
            return parameter.ToString();

         return null;
      }
   }

   [ValueConversion(typeof(int), typeof(string))]
   public class IntToCurrencyStrConverter : IValueConverter
   {
      public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
      {
         if (value == null)
            return "";

         if (string.IsNullOrEmpty(value.ToString()))
            return "";

         int val = (int)value;

         if (val > 0)
            return string.Format("$ {0:n0}", val);
         else
            return "";
      }

      public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
      {
         throw new Exception("The method or operation is not implemented.");
      }
   }

   [ValueConversion(typeof(int), typeof(Visibility))]
   public class IntToVisibilityConverter : IValueConverter
   {
      public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
      {
         int val = (int)value;

         if (val > 0)
            return Visibility.Visible;
         else
            return Visibility.Collapsed;
      }

      public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
      {
         throw new Exception("The method or operation is not implemented.");
      }
   }

   [ValueConversion(typeof(bool), typeof(Brush))]
   public class BoolToBrushConverter : IValueConverter
   {
      public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
      {
         bool val = (bool)value;

         if (val)
            return (SolidColorBrush)(new BrushConverter().ConvertFrom("#26FFFFFF"));
         else
            return Brushes.Transparent;
      }

      public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
      {
         throw new Exception("The method or operation is not implemented.");
      }
   }

   [ValueConversion(typeof(string), typeof(string))]
   public class MobileDashConverter : IValueConverter
   {
      public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
      {
         string val = value.ToString();

         if (val.Length != 10 || val.IndexOf("-") >= 0)
            return val;
         else
            return val.Substring(0, 4) + "-" + val.Substring(4);
      }

      public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
      {
         throw new Exception("The method or operation is not implemented.");
      }
   }
}
