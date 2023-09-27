using Chess.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Chess.Pieces
{
    /// <summary> Indicate squares to add or remove to get trajectory for each direction </summary>
    public class DirectionAttribute : Attribute
    {
        public int IncreaseX { get; set; }
        public int IncreaseY { get; set; }
        public DirectionAttribute(int increaseX, int increaseY)
        {
            IncreaseX = increaseX;
            IncreaseY = increaseY;
        }

        public static DirectionAttribute GetDirectionAttribute(MoveDirection direction)
        {
            MemberInfo memberInfo = typeof(MoveDirection).GetMember(direction.ToString()).FirstOrDefault();
            return memberInfo != null ? (DirectionAttribute)memberInfo.GetCustomAttributes(typeof(DirectionAttribute), false).FirstOrDefault() : null;
        }
    }

    public enum TrajectoryType
    {
        ControlledPositions,
        CheckTrajectory,
        XRayAttacks,
        AllPositions
    }

    public enum MoveDirection
    {
        [Direction(-1, 0)]
        Left,
        [Direction(1, 0)]
        Right,
        [Direction(0, 1)]
        Up,
        [Direction(0, -1)]
        Down,
        [Direction(-1, 1)]
        UpperLeft,
        [Direction(-1, -1)]
        LowerLeft,
        [Direction(1, 1)]
        UpperRight,
        [Direction(1, -1)]
        LowerRight,
        All
    }

    public abstract class LongshotPiece : Piece
    {
        protected List<MoveDirection> _availableDirections;
        private List<Position> _kingXRayAttack = new();
        public List<Position> KingXRayAttack 
        {
            get { return _kingXRayAttack; }
            set 
            {
                _kingXRayAttack = value;
                EvaluatePinnings(_kingXRayAttack);
            }
        }

        protected LongshotPiece(int y, int x, bool color) : base(y, x, color) { }

        public override void SetIsAlive(bool value)
        {
            base.SetIsAlive(value);
            if (!value) KingXRayAttack = new();//_xRayAttacks = new();
        }

        protected override void _GetControlledPositions()
        {
            ControlledPositions.Clear();
            ControlledPositions.AddRange(GetLongshotTrajectory(MoveDirection.All, TrajectoryType.ControlledPositions));
        }

        public override void GetLegalMoves(MoveStatus status)
        {
            SetXRayAttacks();
            base.GetLegalMoves(status);
        }

        /// <summary> Return all available XRay attacks, if any of them contains king, evaluate if any opponent's piece is pinned </summary>
        public void SetXRayAttacks()
        {
            if(GetLongshotTrajectory(MoveDirection.All, TrajectoryType.XRayAttacks).Select(p => p.Piece).Contains(ChessBoard.GetKing(!Color)))
                KingXRayAttack = GetLongshotTrajectory(GetTrajectoryDirection(ChessBoard.GetKing(!Color)), TrajectoryType.XRayAttacks);

            else KingXRayAttack = new();
        }

        /// <summary> Get longshot piece trajectory, based on specified TrajectoryType </summary>
        /// <param name="direction">Represent direction of trajectory to return, in case of MoveDirection.All, method will be called recursively to return all available directions</param>
        /// <param name="trajectory">Type of trajectory to return, it could be ControlledPositions, CheckTrajectory, XRayAttacks or AllPositions</param>
        public List<Position> GetLongshotTrajectory(MoveDirection direction, TrajectoryType trajectory)
        {
            var positions = new List<Position>();
            if (trajectory == TrajectoryType.XRayAttacks) positions.Add(ChessBoard.GetPosition(Y, X));

            // call the method recursively for each piece's available direction 
            if (direction == MoveDirection.All)
                foreach (var dir in _availableDirections) positions.AddRange(GetLongshotTrajectory(dir, trajectory));
            
            else
            {
                // get square progression by direction's attribute => it indicate if it's required adding or substracting i value from x or y
                var squareProgression = DirectionAttribute.GetDirectionAttribute(direction);
                if (squareProgression == null) return positions;

                for (int i = 1; i <= 7; i++)
                {
                    // if counter has reached chessboard border, stop the count and return positions alredy evaluated
                    if (_HasReachedBorder(direction, i, X, Y)) break;                    

                    var position = ChessBoard.GetPosition(Y + (squareProgression.IncreaseY * i), X + (squareProgression.IncreaseX * i));

                    // if there are no pieces, add position
                    if (position.Piece == null) positions.Add(position);

                    // else, method behavior changes by required TrajectoryType
                    else
                    {
                        switch (trajectory)
                        {
                            case TrajectoryType.ControlledPositions:
                                positions.Add(position);
                                return positions;

                            case TrajectoryType.CheckTrajectory: 
                                return positions;

                            case TrajectoryType.XRayAttacks:
                                positions.Add(position);
                                if (position.Piece == ChessBoard.GetKing(!Color)) return positions;
                                else break;

                            case TrajectoryType.AllPositions:
                                positions.Add(position);
                                break;
                        }
                    }
                }
            }
            return positions;
        }

        public override List<Position> GetCheckTrajectory(Piece oppKing)
        {
            var trajectory = base.GetCheckTrajectory(oppKing);
            trajectory.AddRange(GetLongshotTrajectory(GetTrajectoryDirection(oppKing), TrajectoryType.CheckTrajectory));
            return trajectory;
        }

        public MoveDirection GetTrajectoryDirection(Piece refPiece)
        {
            // reference piece is in lower position on the same column
            if (refPiece.X == X && refPiece.Y < Y) return MoveDirection.Down;
            // reference piece is in upper position on the same column
            else if (refPiece.X == X && refPiece.Y > Y) return MoveDirection.Up;
            // reference piece is in lower position on the same row
            else if (refPiece.Y == Y && refPiece.X < X) return MoveDirection.Left;
            // reference piece is in upper position on the same row
            else if (refPiece.Y == Y && refPiece.X > X) return MoveDirection.Right;
            // reference piece is in upper right diagonal position
            else if (refPiece.X > X && refPiece.Y > Y) return MoveDirection.UpperRight;
            // reference piece is in upper left diagonal position
            else if (refPiece.X < X && refPiece.Y > Y) return MoveDirection.UpperLeft;
            // reference piece is in lower right diagonal position
            else if (refPiece.X > X && refPiece.Y < Y) return MoveDirection.LowerRight;
            // reference piece is in lower left diagonal position
            else /*if (refPiece.X < X && refPiece.Y < Y)*/ return MoveDirection.LowerLeft;
        }

        private bool _HasReachedBorder(MoveDirection direction, int i, int x, int y)
        {
            switch (direction)
            {
                default:
                case MoveDirection.Left: return x - i < 0;
                case MoveDirection.Right: return x + i > 7;
                case MoveDirection.Up: return y + i > 7;
                case MoveDirection.Down: return y - i < 0;
                case MoveDirection.UpperLeft: return y + i > 7 || x - i < 0;
                case MoveDirection.LowerLeft: return y - i < 0 || x - i < 0;
                case MoveDirection.UpperRight: return y + i > 7 || x + i > 7;
                case MoveDirection.LowerRight:  return y - i < 0 || x + i > 7;
            }
        }

        public void EvaluatePinnings(List<Position> xRayAttack)
        {
            if(!xRayAttack.Any()) foreach(var piece in ChessBoard.GetPieces(!Color).Where(p => p.PinnerPiece == this)) piece.PinnerPiece = null;                
            else
            {
                var pieces = xRayAttack.Select(p => p.Piece).Where(p => p != null && p.GetType() != typeof(King)).Except(new List<Piece> { this}).ToList();
                if (pieces.Count == 1 && pieces.FirstOrDefault().Color != Color) pieces.FirstOrDefault().PinnerPiece = this;

                else foreach (var piece in pieces) piece.PinnerPiece = null;
            }
        } 
    }
}
