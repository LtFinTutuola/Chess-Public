using Chess.Game;
using Chess.Pieces;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Chess.Resources.Converters
{
    internal class SquareHighlightConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var position = values[0] as Position;
            var piece = values[1] as Piece;

            if (position == null || piece == null) return Visibility.Hidden;

            return piece.AvailablePositions.Contains(position) ? Visibility.Visible : Visibility.Hidden;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
