namespace wave_function_collapse;

class WaveMap<State> where State : WaveMap<State>.Tile {
    public Tile[,] Board;

    public interface Tile {}

    private struct UncollapsedTile : Tile {
        public required List<State> ValidStates;
    }

    private bool hasCollapsed() {
        foreach (Tile space in Board) {
            if (space is UncollapsedTile) {
                return false;
            }
        }
        return true;
    }

    public void Collapse(Func<Tile[,], (int, int), List<State>> getValidStates, Func<Tile[,], bool> isBoardValid) {

        while (!hasCollapsed()) {
            for (int i = 0; i < Board.Length; i++) {
                int stride = Board.GetLength(0);
                (int x, int y) pos = (i % stride, i / stride);

                List<State> result = getValidStates(Board, pos);

                if (result.Count > 1) {
                    Board[pos.x, pos.y] = result[0];
                } else {
                    Board[pos.x, pos.y] = new UncollapsedTile() {ValidStates = result};
                }
            }
        }
    }

}