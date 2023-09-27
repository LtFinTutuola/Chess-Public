using Chess.Pieces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace Chess.Resources.Converters
{
    public class DisplayPieceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var piece = value as Piece;
            if(piece == null)
                return null;

            if (piece.GetType() == typeof(Bishop))
                return piece.Color ? "Resources/Images/WhiteBishop.png" : "Resources/Images/BlackBishop.png";

            else if (piece.GetType() == typeof(King))
                return piece.Color ? "Resources/Images/WhiteKing.png" : "Resources/Images/BlackKing.png";

            else if (piece.GetType() == typeof(Knight))
                return piece.Color ? "Resources/Images/WhiteKnight.png" : "Resources/Images/BlackKnight.png";

            else if (piece.GetType() == typeof(Pawn))
                return piece.Color ? "Resources/Images/WhitePawn.png" : "Resources/Images/BlackPawn.png";

            else if (piece.GetType() == typeof(Queen))
                return piece.Color ? "Resources/Images/WhiteQueen.png" : "Resources/Images/BlackQueen.png";

            else
                return piece.Color ? "Resources/Images/WhiteRook.png" : "Resources/Images/BlackRook.png";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
