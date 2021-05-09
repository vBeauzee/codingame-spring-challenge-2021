class Tree 
{
    public int cellIndex; // location of this tree
    public int size; //size of this tree: 0-3
    public bool isMine; // 1 if this is your tree
    public bool isDormant; // 1 if this tree is dormant

    public Tree() 
    {
        string[] inputs = Console.ReadLine().Split(' ');
        cellIndex = int.Parse(inputs[0]); // location of this tree
        size = int.Parse(inputs[1]); // size of this tree: 0-3
        isMine = inputs[2] != "0"; // 1 if this is your tree
        isDormant = inputs[3] != "0"; // 1 if this tree is dormant
        
    }
    
    public override string ToString() 
    {
        return "cellIndex : " + cellIndex + " | size : " + size + " | isMine : " + isMine +" | isDormant : " + isDormant;
    }
}