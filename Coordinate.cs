class Coordinate 
{
    public int cellIndex;
    public int targetIndex;

    public Coordinate(string cell, string target){
        cellIndex = int.Parse(cell);
        targetIndex = int.Parse(target);
    }

    public override string ToString()
    {
        return cellIndex + " " + targetIndex;
    }
}