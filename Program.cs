using System.Linq;

namespace wave_function_collapse;

class Program {
    static void Main(string[] args) {
        Random rng = new();
        // CardinalState[,] board = new CardinalState[10, 10];

        WaveMap<CardinalState> map = new((10, 10), getValidStates);
        var board = map.Board;

        map.Collapse();

        // // initialize board with random values
        // for (int i = 0; i < board.Length; i++) {
        //     for (int j = 0; j < board.Length; j++) {
        //         CardinalState tile = new() {
        //             North = rng.Next(2) == 0,
        //             East = rng.Next(2) == 0,
        //             South = rng.Next(2) == 0,
        //             West = rng.Next(2) == 0
        //         };
        //         board[i, j] = tile;
        //     }
        // }

        for (int i = 0; i < board.GetLength(0); i++) {
            for (int j = 0; j < board.GetLength(1); j++) {
                CardinalState tile = board[i, j][0]; // access is safe because the board has been collapsed here
                Console.Write(tile.GetChar());
            }
            Console.Write('\n');
        }
    }

    static List<CardinalState> getValidStates(List<CardinalState>[,] board, (int x, int y) pos) {
        List<CardinalState> result = new();

        // generate all possible CardinalStates
        foreach (var north in new bool[] {true, false}) {    
            foreach (var east in new bool[] {true, false}) {    
                foreach (var south in new bool[] {true, false}) {    
                    foreach (var west in new bool[] {true, false}) {    
                        result.Add(new CardinalState {
                            North = north,
                            East = east,
                            South = south,
                            West = west
                        });
                    }
                }
            }
        }

        if (!(pos.x > 0 && board[pos.x - 1, pos.y].Count == 1)) {
            if (board[pos.x - 1, pos.y][0].South) {
                foreach (var invalidState in 
                    from state in result 
                        where !state.North 
                        select state
                ) 
                {
                    result.Remove(invalidState);
                }
            }
        }

        return result;
    }

    struct CardinalState {
        public bool North;
        public bool East;
        public bool South;
        public bool West;

        public char GetChar() {
            char[] chars = {
                '·', // 0000
                '╵', // 0001
                '╶', // 0010
                '└', // 0011
                '╷', // 0100
                '│', // 0101
                '┌', // 0110
                '├', // 0111
                '╴', // 1000
                '┘', // 1001
                '─', // 1010
                '┴', // 1011
                '┐', // 1100
                '┤', // 1101
                '┬', // 1110
                '┼', // 1111
                //    ^ WSEN
            };

            int index = (North ? 1 : 0) | (East ? 1 : 0) << 1 | (South ? 1 : 0) << 2 | (West ? 1 : 0) << 3;

            return chars[index];
        }
    }
}


// enum RoomType {
//     Bridge,
//     Reactor,
//     Hall,

// }