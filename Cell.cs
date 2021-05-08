class Cell 
{
    public int index;
    public int richness;
    public List<int> neighboors = new List<int>();

    public Cell(string[] inputs) 
    {
        index = int.Parse(inputs[0]); // 0 is the center cell, the next cells spiral outwards
        richness = int.Parse(inputs[1]); // 0 if the cell is unusable, 1-3 for usable cells
        // the index of the neighbouring cell for each direction
        for (int i = 2 ; i < 8 ; i++) {
            int index = int.Parse(inputs[i]);
            if (index >= 0) {
                neighboors.Add(int.Parse(inputs[i]));
            }
        }
    }

    public override string ToString() 
    {
        return 
        "Index : " + index + "\n" +
        "Richness : " + richness + "\n" +
        "Neighboors : " + NeighBoorsListStr() + "\n";
    }

    private string  NeighBoorsListStr()
    {
        string formattedStr = "[ ";
        for (int i = 0; i < neighboors.Count; i++) {
            formattedStr += neighboors[i];
            if( i < neighboors.Count - 1) {
                formattedStr += " , ";
            }
        }
        return formattedStr += " ]";
    }
}
