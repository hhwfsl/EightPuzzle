namespace EightPuzzle
{
    internal class Record
    {
        public int Id { get; set; }
        public DateTime startTime { get; }
        public DateTime endTime { get; }
        public double duringTime { get; }
        public int steps { get; }

        public Record(int ID, DateTime startTime, DateTime endTime, double duringTime, int steps)
        {
            this.Id = ID;
            this.startTime = startTime;
            this.endTime = endTime;
            this.duringTime = duringTime;
            this.steps = steps;
        }
    }
}
