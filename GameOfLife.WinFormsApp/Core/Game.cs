namespace GameOfLife.WinFormsApp.Core
{
    internal sealed class Game
    {
        private readonly int _rows;
        private readonly int _columns;
        private bool[,] _map;



        #region CTOR

        /// <summary>
        ///     Constructor create instance of class Game;
        /// </summary>
        /// <param name="rows">Rows count in game map</param>
        /// <param name="columns">Columns count in game map</param>
        /// <param name="generationDensity">
        ///     Int32: The value of the generation density of values when the map is first filled. The higher, the more.
        /// </param>
        /// <param name="generationIsRequired">
        ///     Set to "False" to disable filling the map with the first generation. The map will be empty.
        /// </param>
        public Game(int rows, int columns, int generationDensity, bool generationIsRequired)
        {
            _columns = columns;
            _rows = rows;

            _map = new bool[_columns, _rows];

            if (generationIsRequired)
                GenerateGridUsingRandomDensityFactor(generationDensity);

            TimerTickLength = 66;
        }

        /// <summary>
        ///     Constructor create instance of class Game;
        /// </summary>
        /// <param name="rows">Rows count in game map</param>
        /// <param name="columns">Columns count in game map</param>
        /// <param name="generationDensity">
        ///     Int32: The value of the generation density of values when the map is first filled. The higher, the more.
        /// </param>
        /// <param name="generationIsRequired">
        ///     Set to "False" to disable filling the map with the first generation. The map will be empty.
        /// </param>
        /// <param name="timerTickLength">Int32: Lifespan of one generation in milliseconds (2 - 1000).</param>
        public Game(int rows, int columns, int generationDensity, bool generationIsRequired, int timerTickLength) 
            : this(rows, columns, generationDensity, generationIsRequired)
        {
            TimerTickLength = timerTickLength;
        }

        #endregion



        #region internal PROPS


        internal uint CurrentGeneration { get; private set; }

        internal int TimerTickLength { get; private set; }

        internal int CurrentPopulation => GetCurrentAliveCellsCount();

        internal int GridSize => _rows * _columns;


        #endregion


        #region private METHODS

        /// <summary>
        ///     Gets the number of live cells on the map that have the status "true".
        /// </summary>
        /// <returns>Int32: the number of cells on the map with the value "true".</returns>
        private int GetCurrentAliveCellsCount()
        {
            var result = 0;

            for (var column = 0; column < _map.GetLength(0); column++)
                for (var row = 0; row <  _map.GetLength(1); row++)
                    if (_map[column, row]) 
                        result++;

            return result;
        }


        private void GenerateGridUsingRandomDensityFactor(int generationDensity)
        {
            throw new NotImplementedException();
        }


        // TODO: Документація, рефакторінг та розбір алгоритму
        private int GetCellNeighborsCount(int column, int row)
        {
            var result = 0;

            for (var i = -1; i < 2; i++)
            {
                for (var j = -1; j < 2; j++)
                {
                    var xCol = (column + i + _columns) % _columns;
                    var yRow = (row + j + _rows) % _rows;

                    var isSelfChecked = xCol == column && yRow == row;
                    var hasLifeAround = _map[xCol, yRow];

                    if (hasLifeAround && !isSelfChecked)
                        result++;
                }
            }

            return result;
        }

        private bool[,] GetCleanMap()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Inserts the selected boolean value by coordinates.
        /// </summary>
        /// <param name="column">Int32: Column number or "X" coordinate value.</param>
        /// <param name="row">Int32: Row number or "Y" coordinate value.</param>
        /// <param name="state">Bool: The bool value to insert.</param>
        private void UpdateCell(int column, int row, bool state)
            =>
                _map[column, row] = state;


        private bool ValidateCellPosition(int column, int row)
            => 
                column >= 0 && 
                row >= 0 && 
                column < _columns && 
                row < _rows;


        #endregion



        #region public METHODS


        /// <summary>
        ///     Gets an array of boolean values of the current map generation.
        ///     Returns bool[columns, rows] of the current map generation.
        /// </summary>
        /// <returns></returns>
        internal bool[,] GetCurrentGeneration()
        {
            var resultBoolArray = new bool[_columns, _rows];

            for (var columns = 0; columns < _columns; columns++)
                for (var rows = 0; rows < _rows; rows++)
                    resultBoolArray[columns, rows] = _map[columns, rows];

            return resultBoolArray;
        }

        /// <summary>
        ///     Alternately changes the value of each cell depending on the number and condition of neighboring ones.
        ///     If 3 of them are “live” the value will be “true”, otherwise it will be “false”.
        /// </summary>
        internal void GetNextGeneration()
        {
            var newMap = new bool[_columns, _rows];

            for (var column = 0; column < _columns; column++)
                for (var row = 0; row < _rows; row++)
                {
                    var neighborsCount = GetCellNeighborsCount(column, row);

                    newMap[column, row] = _map[column, row] switch
                    {
                        false when neighborsCount == 3 
                            => true,
                        true when neighborsCount is < 2 or > 3 
                            => false,
                        _ 
                            => _map[column, row]
                    };
                }

            _map = newMap;
            CurrentGeneration++;
        }


        /// <summary>
        ///     Allows the coordinates specified in the parameters to indicate the value on the map "true".
        /// </summary>
        /// <param name="xCol">Column number or "X" coordinate value.</param>
        /// <param name="yRow">Row number or "Y" coordinate value.</param>
        internal void AddCell(int xCol, int yRow) 
            => 
                UpdateCell(xCol, yRow, state:true);

        /// <summary>
        ///     Allows the coordinates specified in the parameters to indicate the value on the map "false".
        /// </summary>
        /// <param name="xCol">Column number or "X" coordinate value.</param>
        /// <param name="yRow">Row number or "Y" coordinate value.</param>
        internal void RemoveCell(int xCol, int yRow) 
            => 
                UpdateCell(xCol, yRow, state: false);

        /// <summary>
        ///     Clears the map by setting all values to false.
        /// </summary>
        internal void CleanMap() 
            => 
                _map = GetCleanMap();


        #endregion
    }
}
