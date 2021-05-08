class Day
{
    public int index; // the game lasts 24 days: 0-23
    public int nutrients; // the base score you gain from the next COMPLETE action
    public int sun; // your sun points
    public int score; // your current score
    public int oppSun; // opponent's sun points
    public int oppScore; // opponent's score
    public bool oppIsWaiting; // whether your opponent is asleep until the next day
    public int numberOfTrees;  // the current amount of trees

    public Day() 
    {
        index = int.Parse(Console.ReadLine()); // the game lasts 24 days: 0-23
        nutrients = int.Parse(Console.ReadLine()); // the base score you gain from the next COMPLETE action
        string[] inputs = Console.ReadLine().Split(' ');
        sun = int.Parse(inputs[0]); // your sun points
        score = int.Parse(inputs[1]); // your current score
        inputs = Console.ReadLine().Split(' ');
        oppSun = int.Parse(inputs[0]); // opponent's sun points
        oppScore = int.Parse(inputs[1]); // opponent's score
        oppIsWaiting = inputs[2] != "0"; // whether your opponent is asleep until the next day
        numberOfTrees = int.Parse(Console.ReadLine()); // the current amount of trees
    }

    public override String ToString()
    {
        return 
        "Day : " + index + "\n"+
        "Nutrients : " + nutrients + "\n" +
        "Sun : " + sun + "\n" +
        "Score : " + score + "\n" +
        "OppSun : " + oppSun + "\n" +
        "OppScore : " + oppScore + "\n" +
        "OppIsWaiting : " + oppIsWaiting + "\n" +
        "NumberOfTrees : " + numberOfTrees + "\n";

    }
}
