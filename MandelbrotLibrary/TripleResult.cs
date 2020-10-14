namespace MandelbrotLibrary
{
    public class TripleResult
    {
        public int X { get; set; }

        public int Y { get; set; }

        public int Iteration { get; set; }
    }

    public class TripleResultNew
    {
        public (int, int, int) Result { get; set; }
    }
}
