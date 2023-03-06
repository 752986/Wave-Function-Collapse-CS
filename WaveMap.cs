namespace wave_function_collapse;

class WaveMap<State> {
    public List<State>[,] Board;

    private Func<List<State>[,], (int, int), List<State>> getValidStates;
    private Func<List<State>[,], bool> isBoardValid;

    public WaveMap(
        (int width, int height) dimensions, 
        Func<List<State>[,], (int, int), List<State>> getValidStates, 
        Func<List<State>[,], bool> isBoardValid = null
    )
    {
        this.getValidStates = getValidStates;
        this.isBoardValid = isBoardValid;

        Board = new List<State>[dimensions.width, dimensions.height];

        for (int x = 0; x < dimensions.width; x++) {
            for (int y = 0; y < dimensions.width; y++) {
                Board[x, y] = new List<State>();
            }
        }

        for (int x = 0; x < dimensions.width; x++) {
            for (int y = 0; y < dimensions.width; y++) {
                Board[x, y] = getValidStates(Board, (x, y));
            }
        }

        Random rng = new();
        (int x, int y) seedPoint = (rng.Next(dimensions.width), rng.Next(dimensions.height));
        List<State> slot = (Board[seedPoint.x, seedPoint.y]);
        Board[seedPoint.x, seedPoint.y] = new List<State>() {slot[rng.Next(slot.Count)]};
    }

    private bool hasCollapsed() {
        foreach (var space in Board) {
            if (space.Count != 1) {
                return false;
            }
        }
        return true;
    }

    public void Collapse(bool printDebug = false) {
        int iterations = 0;
        do {
            if (printDebug) {
                Console.WriteLine("Beginning attempt ", iterations);
            }

            while (!hasCollapsed()) {
                int stride = Board.GetLength(0);
                for (int i = 0; i < Board.Length; i++) {
                    (int x, int y) pos = (i % stride, i / stride);

                    Board[pos.x, pos.y] = getValidStates(Board, pos);
                }
            }

            if (isBoardValid == null) {return;}

            iterations++;
        } while (!isBoardValid(Board));
    }

}