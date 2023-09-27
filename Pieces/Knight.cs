using Chess.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Pieces
{
    public class Knight : Piece
    {
        public Knight(int y, int x, bool color) : base(y, x, color)
        {
            Value = 3;
        }

        protected override void _GetControlledPositions()
        {
            // reset available positions
            ControlledPositions.Clear();

            // check if knight is too close to upper border to move up Y
            if (Y + 2 <= 7)
            {
                // too close to right border 
                if (X + 1 <= 7)
                    ControlledPositions.Add(ChessBoard.GetPosition(Y + 2, X + 1));

                // too close to left border
                if (X - 1 >= 0)
                    ControlledPositions.Add(ChessBoard.GetPosition(Y + 2, X - 1));
            }
            // check if knight is too close to lower border to move down Y
            if(Y - 2 >= 0)
            {
                // too close to right border
                if (X + 1 <= 7)
                    ControlledPositions.Add(ChessBoard.GetPosition(Y - 2, X + 1));

                // too close to left border
                if (X - 1 >= 0)
                    ControlledPositions.Add(ChessBoard.GetPosition(Y - 2, X - 1));
            }
            // check if knight is too close to right border to move right Y
            if(X + 2 <= 7)
            {
                // too close to upper border
                if (Y + 1 <= 7)
                    ControlledPositions.Add(ChessBoard.GetPosition(Y + 1, X + 2));

                // too close to lower border
                if (Y - 1 >= 0)
                    ControlledPositions.Add(ChessBoard.GetPosition(Y - 1, X + 2));
            }
            // check if knight is too close to left border to move left Y
            if(X - 2 >= 0)
            {
                // too close to upper border
                if (Y + 1 <= 7)
                    ControlledPositions.Add(ChessBoard.GetPosition(Y + 1, X - 2));

                // too close to lower border
                if (Y - 1 >= 0)
                    ControlledPositions.Add(ChessBoard.GetPosition(Y - 1, X - 2));
            }
        }
    }
}
